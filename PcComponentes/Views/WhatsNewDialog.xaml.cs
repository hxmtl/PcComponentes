﻿using System;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace PC_Componentes_UWP.Views
{
    public sealed partial class WhatsNewDialog : ContentDialog
    {
        public WhatsNewDialog()
        {
            // TODO WTS: Update the contents of this dialog every time you release a new version of the app
            InitializeComponent();
        }

        private async void DonateButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var link = new Uri("https://www.paypal.me/PabloJmnz");
            await Launcher.LaunchUriAsync(link);
        }
    }
}
