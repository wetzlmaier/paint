using System.Windows;

namespace PaintWPF.Models
{
    public class Ellipse : Shape
    {
        private Point _center;
        public Point Center
        {
            get => _center;
            set
            {
                _center = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Top));
                OnPropertyChanged(nameof(Left));
            }
        }

        private double _radiusX;
        public double RadiusX
        {
            get => _radiusX;
            set
            {
                _radiusX = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Left));
            }
        }

        private double _radiusY;
        public double RadiusY
        {
            get => _radiusY;
            set
            {
                _radiusY = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Top));
            }
        }

        public double Top => Center.Y - RadiusY;
        public double Left => Center.X - RadiusX;
    }
}
