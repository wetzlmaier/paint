using System.Windows;
using Microsoft.Toolkit.Mvvm.Messaging;
using WpfPaint.ViewModels.Messages;
using System.Windows.Media.Imaging;
using System.IO;

namespace WpfPaint.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<SaveCanvasAsImageMessage>(this, (r, m) =>
            {
                var renderBitmap = new RenderTargetBitmap((int)PaintCanvas.ActualWidth, (int)PaintCanvas.ActualHeight, 96d, 96d, System.Windows.Media.PixelFormats.Default);
                renderBitmap.Render(PaintCanvas);

                BitmapEncoder encoder;
                switch (Path.GetExtension(m.Value).ToLower())
                {
                    case ".jpg":
                        encoder = new JpegBitmapEncoder();
                        break;
                    case ".bmp":
                        encoder = new BmpBitmapEncoder();
                        break;
                    default:
                        encoder = new PngBitmapEncoder();
                        break;
                }

                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

                using (var fileStream = new FileStream(m.Value, FileMode.Create))
                {
                    encoder.Save(fileStream);
                }
            });
        }
    }
}
