using System.Windows.Media;

namespace PaintWPF.Models
{
    public class PolylineShape : Shape
    {
        private PointCollection _points = new PointCollection();
        public PointCollection Points
        {
            get => _points;
            set
            {
                _points = value;
                OnPropertyChanged();
            }
        }
    }
}
