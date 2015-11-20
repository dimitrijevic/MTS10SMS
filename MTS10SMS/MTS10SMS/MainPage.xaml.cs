using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Connectivity.Plugin;
using Share.Plugin;
using Xamarin.Forms;

namespace MTS10SMS
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            ButtonBack.MinimumHeightRequest = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            ButtonLogin.MinimumHeightRequest = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            ButtonSMS.MinimumHeightRequest = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            ButtonSend.MinimumHeightRequest = Device.GetNamedSize(NamedSize.Large, typeof(Label));
        }

        private async void ButtonSMS_OnClicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await MondoSMS.SMSPostAsync(PickerFromPrefix.SelectedIndex.ToString(), EntryFromNumber.Text);
                CurrentPage = Children[1];
            }
            else
            {
                UserDialogs.Instance.Alert("Network unavailable", "Alert!");
            }
        }

        private async void ButtonLogin_OnClicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await
                    MondoSMS.LoginPostAsync(EntryPassword.Text, PickerFromPrefix.SelectedIndex.ToString(),
                        EntryFromNumber.Text);
                CurrentPage = Children[2];
            }
            else
            {
                UserDialogs.Instance.Alert("Network unavailable", "Alert!");
            }
        }

        private async void ButtonSend_OnClicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await
                    MondoSMS.SendPostAsync(EntryMessage.Text, PickerToPrefix.SelectedIndex.ToString(),
                        EntryToNumber.Text);
            }
            else
            {
                UserDialogs.Instance.Alert("Network unavailable", "Alert!");
            }
        }

        private void ButtonBack_OnClicked(object sender, EventArgs e)
        {
            CurrentPage = Children[0];
        }

        private void MenuItem_OnClickedAboutApp(object sender, EventArgs e)
        {
            this.Navigation.PushModalAsync(new AboutPage());
        }

        private void MenuItem_OnClickedShareApp(object sender, EventArgs e)
        {
            CrossShare.Current.Share("10 free SMS! Use mts10sms. StoreURL?", "Share");
        }
    }
}
