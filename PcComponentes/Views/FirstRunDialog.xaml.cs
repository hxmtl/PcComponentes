using System;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace PC_Componentes_UWP.Views
{
    public sealed partial class FirstRunDialog : ContentDialog
    {
        public FirstRunDialog()
        {
            // TODO WTS: Update the contents of this dialog with any important information you want to show when the app is used for the first time.
            InitializeComponent();
        }

        private async void DonateButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var link = new Uri("https://www.paypal.me/PabloJmnz");
            await Launcher.LaunchUriAsync(link);
        }
    }
}
