using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.media;

namespace PaintWPF.Models
{
    public abstract class Shape : INotifyPropertyChanged
    {
        private Brush _strokeColor = Brushes.Black;
        public Brush StrokeColor
        {
            get => _strokeColor;
            set
            {
                _strokeColor = value;
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
