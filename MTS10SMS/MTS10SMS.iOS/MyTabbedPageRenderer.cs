using MTS10SMS.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedPageRenderer))]
namespace MTS10SMS.iOS
{
    public class TabbedPageRenderer : TabbedRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            TabBar.TintColor = UIColor.FromRGBA(237, 22, 57, 255);
            TabBar.BarTintColor = UIColor.White;
            TabBar.BackgroundColor = UIColor.White;
        }
    }
}