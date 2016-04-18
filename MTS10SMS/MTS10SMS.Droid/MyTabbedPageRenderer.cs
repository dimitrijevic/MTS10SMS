// http://forums.xamarin.com/discussion/comment/80381/#Comment_80381       

using System.ComponentModel;
using Android.App;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedPageRenderer))]
public class TabbedPageRenderer : TabbedRenderer
{
    private Activity activity;
    private bool isFirstDesign = true;

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);
        activity = this.Context as Activity;
    }

    protected override void OnWindowVisibilityChanged(ViewStates visibility)
    {
        base.OnWindowVisibilityChanged(visibility);
        if (isFirstDesign)
        {
            ActionBar actionBar = activity.ActionBar;

            ColorDrawable colorDrawable = new ColorDrawable(Color.White);
            actionBar.SetStackedBackgroundDrawable(colorDrawable);
            //actionBar.SetHomeAsUpIndicator(new ColorDrawable(Color.SlateGray));
            // TODO: set tab text color programmatically
            for (int i = 0; i < actionBar.TabCount; i++)
            {

                var tab = actionBar.GetTabAt(i);
                var textView = new TextView(Context)
                {
                    TextFormatted = tab.TextFormatted,
                    Gravity = Android.Views.GravityFlags.Center,
                    LayoutParameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.MatchParent)
                };

                var tabbed = Element as TabbedPage;
                textView.SetTextColor(Color.Argb(255, 237, 22, 57));

                tab.SetCustomView(textView);
            }

            isFirstDesign = false;
        }
    }
}