﻿using Xamarin.Forms;

namespace MTS10SMS
{
    public partial class AboutPage : ContentPage //, INotifyPropertyChanged
    {
        public static readonly BindableProperty MainTextProperty =
            BindableProperty.Create<AboutPage, string>(p => p.MainText, string.Empty);

        public AboutPage()
        {
            InitializeComponent();
            BindingContext = this;
            MainText = "mts10sms";
        }

        public string MainText
        {
            get { return (string) GetValue(MainTextProperty); }
            set { SetValue(MainTextProperty, value); }
        }
    }
}