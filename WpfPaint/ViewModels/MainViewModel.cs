using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using WpfPaint.Models;
using WpfPaint.ViewModels.Commands;
using Microsoft.Win32;
using System.Xml.Serialization;
using System.IO;

namespace WpfPaint.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Shape> _shapes = new ObservableCollection<Shape>();
        public ObservableCollection<Shape> Shapes
        {
            get => _shapes;
            set
            {
                _shapes = value;
                OnPropertyChanged();
            }
        }

        private string _selectedTool = "Pencil";
        public string SelectedTool
        {
            get => _selectedTool;
            set
            {
                _selectedTool = value;
                OnPropertyChanged();
            }
        }

        private Color _foregroundColor = Colors.Black;
        public Color ForegroundColor
        {
            get => _foregroundColor;
            set
            {
                _foregroundColor = value;
                OnPropertyChanged();
            }
        }

        private Color _backgroundColor = Colors.White;
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                OnPropertyChanged();
            }
        }

        private double _strokeThickness = 2.0;
        public double StrokeThickness
        {
            get => _strokeThickness;
            set
            {
                _strokeThickness = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectToolCommand { get; }
        public ICommand MouseDownCommand { get; }
        public ICommand MouseMoveCommand { get; }
        public ICommand MouseUpCommand { get; }
        public ICommand SelectColorCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        private Shape _currentShape;
        private Point _startPoint;
        private bool _isForegroundColorSelected = true;
        private readonly ObservableCollection<Shape> _history = new ObservableCollection<Shape>();
        private int _historyIndex = -1;
        private readonly ObservableCollection<Shape> _redoStack = new ObservableCollection<Shape>();
        public bool IsForegroundColorSelected
        {
            get => _isForegroundColorSelected;
            set
            {
                _isForegroundColorSelected = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            SelectToolCommand = new RelayCommand(SelectTool);
            MouseDownCommand = new RelayCommand(MouseDown);
            MouseMoveCommand = new RelayCommand(MouseMove);
            MouseUpCommand = new RelayCommand(MouseUp);
            SelectColorCommand = new RelayCommand(SelectColor);
            SaveCommand = new RelayCommand(Save);
            OpenCommand = new RelayCommand(Open);
            UndoCommand = new RelayCommand(Undo, CanUndo);
            RedoCommand = new RelayCommand(Redo, CanRedo);
        }

        private void Save(object parameter)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml|All files (*.*)|*.*",
                DefaultExt = ".xml"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                var serializer = new XmlSerializer(typeof(ObservableCollection<Shape>));
                using (var writer = new StreamWriter(saveFileDialog.FileName))
                {
                    serializer.Serialize(writer, Shapes);
                }
            }
        }

        private void Open(object parameter)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                var serializer = new XmlSerializer(typeof(ObservableCollection<Shape>));
                using (var reader = new StreamReader(openFileDialog.FileName))
                {
                    var shapes = (ObservableCollection<Shape>)serializer.Deserialize(reader);
                    Shapes = shapes;
                }
            }
        }

        private void Undo(object parameter)
        {
            if (CanUndo(parameter))
            {
                var shape = _history[_historyIndex];
                _history.RemoveAt(_historyIndex);
                _historyIndex--;
                _redoStack.Add(shape);
                Shapes.Remove(shape);
            }
        }

        private bool CanUndo(object parameter)
        {
            return _history.Count > 0;
        }

        private void Redo(object parameter)
        {
            if (CanRedo(parameter))
            {
                var shape = _redoStack[_redoStack.Count - 1];
                _redoStack.RemoveAt(_redoStack.Count - 1);
                _history.Add(shape);
                _historyIndex++;
                Shapes.Add(shape);
            }
        }

        private bool CanRedo(object parameter)
        {
            return _redoStack.Count > 0;
        }

        private void SelectTool(object tool)
        {
            SelectedTool = tool.ToString();
        }

        private void SelectColor(object parameter)
        {
            if (parameter is string colorString)
            {
                var color = (Color)ColorConverter.ConvertFromString(colorString);
                if (IsForegroundColorSelected)
                {
                    ForegroundColor = color;
                }
                else
                {
                    BackgroundColor = color;
                }
            }
        }

        private void MouseDown(object parameter)
        {
            _startPoint = (Point)parameter;
            _currentShape = CreateShape();
            if (_currentShape != null)
            {
                Shapes.Add(_currentShape);
            }
        }

        private void MouseMove(object parameter)
        {
            if (_currentShape != null)
            {
                var currentPoint = (Point)parameter;
                _currentShape.EndPoint = currentPoint;

                if (_currentShape is RectangleShape rectangle)
                {
                    var startPoint = new Point(Math.Min(_startPoint.X, currentPoint.X), Math.Min(_startPoint.Y, currentPoint.Y));
                    var endPoint = new Point(Math.Max(_startPoint.X, currentPoint.X), Math.Max(_startPoint.Y, currentPoint.Y));
                    rectangle.StartPoint = startPoint;
                    rectangle.Width = endPoint.X - startPoint.X;
                    rectangle.Height = endPoint.Y - startPoint.Y;
                }
                else if (_currentShape is EllipseShape ellipse)
                {
                    var startPoint = new Point(Math.Min(_startPoint.X, currentPoint.X), Math.Min(_startPoint.Y, currentPoint.Y));
                    var endPoint = new Point(Math.Max(_startPoint.X, currentPoint.X), Math.Max(_startPoint.Y, currentPoint.Y));
                    ellipse.StartPoint = startPoint;
                    ellipse.Width = endPoint.X - startPoint.X;
                    ellipse.Height = endPoint.Y - startPoint.Y;
                }
                else if (_currentShape is PolylineShape polyline)
                {
                    polyline.Points.Add(currentPoint);
                }
            }
        }

        private void MouseUp(object parameter)
        {
            if (_currentShape != null)
            {
                _history.Add(_currentShape);
                _historyIndex++;
                _redoStack.Clear();
                _currentShape = null;
            }
        }

        private Shape CreateShape()
        {
            switch (SelectedTool)
            {
                case "Pencil":
                    return new PolylineShape { Color = ForegroundColor, StrokeColor = ForegroundColor, StrokeThickness = StrokeThickness };
                case "Line":
                    return new LineShape { StartPoint = _startPoint, EndPoint = _startPoint, Color = ForegroundColor, StrokeColor = ForegroundColor, StrokeThickness = StrokeThickness };
                case "Rectangle":
                    return new RectangleShape { StartPoint = _startPoint, EndPoint = _startPoint, Color = BackgroundColor, StrokeColor = ForegroundColor, StrokeThickness = StrokeThickness };
                case "Ellipse":
                    return new EllipseShape { StartPoint = _startPoint, EndPoint = _startPoint, Color = BackgroundColor, StrokeColor = ForegroundColor, StrokeThickness = StrokeThickness };
                default:
                    return null;
            }
        }
    }
}
