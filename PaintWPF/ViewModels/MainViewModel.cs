using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using PaintWPF.Models;

namespace PaintWPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Shape> Shapes { get; } = new ObservableCollection<Shape>();

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

        private Brush _selectedColor = Brushes.Black;
        public Brush SelectedColor
        {
            get => _selectedColor;
            set
            {
                _selectedColor = value;
                OnPropertyChanged();
            }
        }

        private double _strokeThickness = 1;
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
        public ICommand NewCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand SaveCommand { get; }

        private Shape _currentShape;
        private Point _startPoint;

        public MainViewModel()
        {
            SelectToolCommand = new RelayCommand(SelectTool);
            NewCommand = new RelayCommand(NewCanvas);
            OpenCommand = new RelayCommand(OpenImage);
            SaveCommand = new RelayCommand(SaveImage);
        }

        private void SelectTool(object tool)
        {
            SelectedTool = tool.ToString();
        }

        private void NewCanvas(object obj)
        {
            Shapes.Clear();
        }

        private void OpenImage(object obj)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // In a real application, you would load the image data into your model.
                // For this example, we'll just clear the canvas.
                Shapes.Clear();
            }
        }

        private void SaveImage(object obj)
        {
            var canvas = obj as Canvas;
            if (canvas == null) return;

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG Image (*.png)|*.png"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var rtb = new RenderTargetBitmap((int)canvas.RenderSize.Width, (int)canvas.RenderSize.Height, 96d, 96d, PixelFormats.Default);
                rtb.Render(canvas);

                var pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

                using (var fs = File.OpenWrite(saveFileDialog.FileName))
                {
                    pngEncoder.Save(fs);
                }
            }
        }

        public void StartDrawing(Point startPoint)
        {
            _startPoint = startPoint;
            switch (SelectedTool)
            {
                case "Pencil":
                    _currentShape = new PolylineShape
                    {
                        Points = new PointCollection { startPoint },
                        StrokeColor = SelectedColor,
                        StrokeThickness = StrokeThickness
                    };
                    break;
                case "Line":
                    _currentShape = new Line
                    {
                        StartPoint = _startPoint,
                        EndPoint = _startPoint,
                        StrokeColor = SelectedColor,
                        StrokeThickness = StrokeThickness
                    };
                    break;
                case "Rectangle":
                    _currentShape = new Models.Rectangle
                    {
                        TopLeft = _startPoint,
                        Width = 0,
                        Height = 0,
                        StrokeColor = SelectedColor,
                        StrokeThickness = StrokeThickness
                    };
                    break;
                case "Ellipse":
                    _currentShape = new Ellipse
                    {
                        Center = _startPoint,
                        RadiusX = 0,
                        RadiusY = 0,
                        StrokeColor = SelectedColor,
                        StrokeThickness = StrokeThickness
                    };
                    break;
            }

            if (_currentShape != null)
            {
                Shapes.Add(_currentShape);
            }
        }

        public void UpdateDrawing(Point currentPoint)
        {
            if (_currentShape == null) return;

            if (_currentShape is PolylineShape polyline)
            {
                polyline.Points.Add(currentPoint);
            }
            else if (_currentShape is Line line)
            {
                line.EndPoint = currentPoint;
            }
            else if (_currentShape is Models.Rectangle rect)
            {
                var topLeft = new Point(System.Math.Min(_startPoint.X, currentPoint.X), System.Math.Min(_startPoint.Y, currentPoint.Y));
                var bottomRight = new Point(System.Math.Max(_startPoint.X, currentPoint.X), System.Math.Max(_startPoint.Y, currentPoint.Y));
                rect.TopLeft = topLeft;
                rect.Width = bottomRight.X - topLeft.X;
                rect.Height = bottomRight.Y - topLeft.Y;
            }
            else if (_currentShape is Ellipse ellipse)
            {
                ellipse.Center = new Point((_startPoint.X + currentPoint.X) / 2, (_startPoint.Y + currentPoint.Y) / 2);
                ellipse.RadiusX = System.Math.Abs((currentPoint.X - _startPoint.X) / 2);
                ellipse.RadiusY = System.Math.Abs((currentPoint.Y - _startPoint.Y) / 2);
            }
        }

        public void EndDrawing()
        {
            _currentShape = null;
        }
    }
}
