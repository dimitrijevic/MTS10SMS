using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
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
            this.Style = App.Current.Resources["PivotStyle1"] as System.Windows.Style;
            //this.FontFamily = new FontFamily(@"\Assets\NexaBook.ttf#Nexa Book");
            this.TitleTemplate = GetStyledTitleTemplate();
            this.HeaderTemplate = GetStyledHeaderTemplate();
        }

        private System.Windows.DataTemplate GetStyledTitleTemplate()
        {
            string dataTemplateXaml =
            @"<DataTemplate
                xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""> 
                <TextBlock
                Text=""{Binding}""
                FontFamily=""/Assets/NexaBook.ttf#Nexa Book""
                FontWeight=""Light""
                Foreground=""#FFED1639"" />
              </DataTemplate>"; 
        
            return (System.Windows.DataTemplate)XamlReader.Load(dataTemplateXaml);
        }

        private System.Windows.DataTemplate GetStyledHeaderTemplate()
        {
            string dataTemplateXaml =
            @"<DataTemplate
                xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""> 
                <TextBlock
                Text=""{Binding Title}""
                FontFamily=""/Assets/NexaBook.ttf#Nexa Book""
                FontWeight=""Light""
                Foreground=""#FFED1639"" />
              </DataTemplate>";

            return (System.Windows.DataTemplate)XamlReader.Load(dataTemplateXaml);
        }

    }
}