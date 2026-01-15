using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows;

namespace WpfPaint.Models
{
    public class PolylineShape : Shape
    {
        public ObservableCollection<Point> Points { get; set; } = new ObservableCollection<Point>();
    }
}
