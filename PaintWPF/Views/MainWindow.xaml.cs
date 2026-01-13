using System.Windows;
using System.Windows.Input;
using PaintWPF.ViewModels;

namespace PaintWPF.Views
{
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = (MainViewModel)DataContext;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _viewModel.StartDrawing(e.GetPosition((IInputElement)sender));
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _viewModel.UpdateDrawing(e.GetPosition((IInputElement)sender));
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _viewModel.EndDrawing();
        }
    }
}
