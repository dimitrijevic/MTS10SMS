using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.Connectivity;
using Plugin.Share;
using Xamarin.Forms;

namespace MTS10SMS
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            if(Device.OS != TargetPlatform.WinPhone)
            {
                EntryFromNumber.HeightRequest = 40; //Device.GetNamedSize(NamedSize.Large, typeof(Label));
                EntryToNumber.HeightRequest = 40; //Device.GetNamedSize(NamedSize.Large, typeof(Label));
                EntryPassword.HeightRequest = 40; //Device.GetNamedSize(NamedSize.Large, typeof(Label));
                PickerFromPrefix.HeightRequest = 40; //Device.GetNamedSize(NamedSize.Large, typeof(Label));
                PickerToPrefix.HeightRequest = 40; //Device.GetNamedSize(NamedSize.Large, typeof(Label));
                ButtonBack.HeightRequest = 40; //Device.GetNamedSize(NamedSize.Large, typeof(Label));
                ButtonLogin.HeightRequest = 40; //Device.GetNamedSize(NamedSize.Large, typeof(Label));
                ButtonSMS.HeightRequest = 40; //Device.GetNamedSize(NamedSize.Large, typeof(Label));
                ButtonSend.HeightRequest = 40; //Device.GetNamedSize(NamedSize.Large, typeof(Label));
            }
        }

        private async void ButtonSMS_OnClicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                //var notificator = DependencyService.Get<IToastNotificator>();
                //bool tapped = await notificator.Notify(ToastNotificationType.Success,
                //    "OK", "SMS number sent, awaiting response, checking...", TimeSpan.FromSeconds(1));
                UserDialogs.Instance.ShowLoading("SMS number sending, awaiting response, checking...", MaskType.Gradient);
                //Toast(new ToastConfig(ToastEvent.Success, "OK", "SMS number sent, awaiting response, checking..."));

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
                //tapped = await notificator.Notify(ToastNotificationType.Success,
                //    "OK", "Login code sent, awaiting response, checking...", TimeSpan.FromSeconds(1));
                UserDialogs.Instance.ShowLoading("Login code sent, awaiting response, checking...", MaskType.Gradient);
                //UserDialogs.Instance.Toast(new ToastConfig(ToastEvent.Success, "OK", "Login code sent, awaiting response, checking..."));

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
                //var notificator = DependencyService.Get<IToastNotificator>();
                //bool tapped = await notificator.Notify(ToastNotificationType.Success,
                //    "OK", "Message text sent, awaiting response, checking...", TimeSpan.FromSeconds(1));
                UserDialogs.Instance.ShowLoading("Message text sent, awaiting response, checking...", MaskType.Gradient);
                //UserDialogs.Instance.Toast(new ToastConfig(ToastEvent.Success, "OK", "Message text sent, awaiting response, checking..."));

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
            this.ParentView.Navigation.PushAsync(new AboutPage());
        }

        private void MenuItem_OnClickedShareApp(object sender, EventArgs e)
        {
            CrossShare.Current.ShareLink("http://smarturl.it/mts10sms", "FREE SMS DAILY!? Use mts10sms! iOS/Android/Win", "mts10sms");
        }
    }
}
