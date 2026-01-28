using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace WpfPaint.ViewModels.Messages
{
    public class SaveCanvasAsImageMessage : ValueChangedMessage<string>
    {
        public SaveCanvasAsImageMessage(string value) : base(value)
        {
        }
    }
}
