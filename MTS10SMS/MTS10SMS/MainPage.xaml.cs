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
    }
}
