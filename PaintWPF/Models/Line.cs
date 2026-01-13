using System.Windows;

namespace PaintWPF.Models
{
    public class Line : Shape
    {
        private Point _startPoint;
        public Point StartPoint
        {
            get => _startPoint;
            set
            {
                _startPoint = value;
                OnPropertyChanged();
            }
        }

        private Point _endPoint;
        public Point EndPoint
        {
            get => _endPoint;
            set
            {
                _endPoint = value;
                OnPropertyChanged();
            }
        }
    }
}
