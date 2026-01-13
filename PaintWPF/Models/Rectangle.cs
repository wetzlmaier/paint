using System.Windows;

namespace PaintWPF.Models
{
    public class Rectangle : Shape
    {
        private Point _topLeft;
        public Point TopLeft
        {
            get => _topLeft;
            set
            {
                _topLeft = value;
                OnPropertyChanged();
            }
        }

        private double _width;
        public double Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }

        private double _height;
        public double Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }
    }
}
