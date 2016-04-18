using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using MTS10SMS;
using MTS10SMS.WinPhone;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using Color = System.Windows.Media.Color;

[assembly: ExportRenderer(typeof(Editor), typeof(MyEditorRenderer))]

namespace MTS10SMS.WinPhone
{
    public class MyEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            var nativeControl = (TextBox)Control; // UITextView(iOS), EditText(Android)

            nativeControl.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            nativeControl.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 0)); // Red
            nativeControl.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 255, 255));
        }
    }
}
