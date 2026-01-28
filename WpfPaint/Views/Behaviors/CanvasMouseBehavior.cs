using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfPaint.Views.Behaviors
{
    public static class CanvasMouseBehavior
    {
        public static readonly DependencyProperty MouseDownCommandProperty =
            DependencyProperty.RegisterAttached("MouseDownCommand", typeof(ICommand), typeof(CanvasMouseBehavior), new PropertyMetadata(OnMouseDownCommandChanged));

        public static ICommand GetMouseDownCommand(DependencyObject obj) => (ICommand)obj.GetValue(MouseDownCommandProperty);
        public static void SetMouseDownCommand(DependencyObject obj, ICommand value) => obj.SetValue(MouseDownCommandProperty, value);

        private static void OnMouseDownCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Canvas canvas)
            {
                if (e.NewValue != null)
                {
                    canvas.MouseDown += Canvas_MouseDown;
                }
                else
                {
                    canvas.MouseDown -= Canvas_MouseDown;
                }
            }
        }

        private static void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                var command = GetMouseDownCommand(canvas);
                var position = e.GetPosition(canvas);
                if (command?.CanExecute(position) == true)
                {
                    command.Execute(position);
                }
            }
        }

        public static readonly DependencyProperty MouseMoveCommandProperty =
            DependencyProperty.RegisterAttached("MouseMoveCommand", typeof(ICommand), typeof(CanvasMouseBehavior), new PropertyMetadata(OnMouseMoveCommandChanged));

        public static ICommand GetMouseMoveCommand(DependencyObject obj) => (ICommand)obj.GetValue(MouseMoveCommandProperty);
        public static void SetMouseMoveCommand(DependencyObject obj, ICommand value) => obj.SetValue(MouseMoveCommandProperty, value);

        private static void OnMouseMoveCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Canvas canvas)
            {
                if (e.NewValue != null)
                {
                    canvas.MouseMove += Canvas_MouseMove;
                }
                else
                {
                    canvas.MouseMove -= Canvas_MouseMove;
                }
            }
        }

        private static void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                var command = GetMouseMoveCommand(canvas);
                var position = e.GetPosition(canvas);
                if (command?.CanExecute(position) == true)
                {
                    command.Execute(position);
                }
            }
        }

        public static readonly DependencyProperty MouseUpCommandProperty =
            DependencyProperty.RegisterAttached("MouseUpCommand", typeof(ICommand), typeof(CanvasMouseBehavior), new PropertyMetadata(OnMouseUpCommandChanged));

        public static ICommand GetMouseUpCommand(DependencyObject obj) => (ICommand)obj.GetValue(MouseUpCommandProperty);
        public static void SetMouseUpCommand(DependencyObject obj, ICommand value) => obj.SetValue(MouseUpCommandProperty, value);

        private static void OnMouseUpCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Canvas canvas)
            {
                if (e.NewValue != null)
                {
                    canvas.MouseUp += Canvas_MouseUp;
                }
                else
                {
                    canvas.MouseUp -= Canvas_MouseUp;
                }
            }
        }

        private static void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                var command = GetMouseUpCommand(canvas);
                var position = e.GetPosition(canvas);
                if (command?.CanExecute(position) == true)
                {
                    command.Execute(position);
                }
            }
        }
    }
}
