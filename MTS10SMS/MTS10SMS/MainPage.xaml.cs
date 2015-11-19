using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            await MondoSMS.SMSPostAsync(PickerFromPrefix.SelectedIndex.ToString(), EntryFromNumber.Text);
            CurrentPage = Children[1];
        }

        private async void ButtonLogin_OnClicked(object sender, EventArgs e)
        {
            await MondoSMS.LoginPostAsync(EntryPassword.Text, PickerFromPrefix.SelectedIndex.ToString(), EntryFromNumber.Text);
            CurrentPage = Children[2];
        }

        private async void ButtonSend_OnClicked(object sender, EventArgs e)
        {
            await MondoSMS.SendPostAsync(EntryMessage.Text, PickerToPrefix.SelectedIndex.ToString(), EntryToNumber.Text);
        }

        private void ButtonBack_OnClicked(object sender, EventArgs e)
        {
            CurrentPage = Children[0];
        }
    }
}
