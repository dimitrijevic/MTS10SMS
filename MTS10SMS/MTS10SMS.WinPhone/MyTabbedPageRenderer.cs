using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Primitives;
using MTS10SMS.WinPhone;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedRenderer))]
namespace MTS10SMS.WinPhone
{
    public class TabbedRenderer : TabbedPageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e); 
            this.Style = MTS10SMS.App.Current.Resources["PivotStyle"] as Style;
        }
    }
}