using System.Windows.Media;
using System.Xml.Serialization;
using System.Windows;

namespace WpfPaint.Models
{
    [XmlInclude(typeof(LineShape))]
    [XmlInclude(typeof(RectangleShape))]
    [XmlInclude(typeof(EllipseShape))]
    [XmlInclude(typeof(PolylineShape))]
    public abstract class Shape
    {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public Color Color { get; set; }
        public Color StrokeColor { get; set; }
        public double StrokeThickness { get; set; }
    }
}
