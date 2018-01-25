using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using PC_Componentes_UWP.Services;

using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.System;
using Windows.UI.Xaml.Input;

namespace PC_Componentes_UWP.Views
{
    public sealed partial class SettingsPage : Page, INotifyPropertyChanged
    {
        //// TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings-codebehind.md
        //// TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere

        ApplicationDataContainer AjustesLocales = ApplicationData.Current.LocalSettings;

        private ElementTheme _elementTheme = ThemeSelectorService.Theme;

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }

        public SettingsPage()
        {
            InitializeComponent();

            ApplicationView.GetForCurrentView().ExitFullScreenMode();
            // Obtener el número de versión
            PackageVersion numeroversion = Package.Current.Id.Version;
            // Mostar versión en Acerca de
            TextoVersion.Text += string.Format("Versión {0}.{1}.{2}.{3}", numeroversion.Major, numeroversion.Minor, numeroversion.Build, numeroversion.Revision);

            LoadSavedValue();
        }

        private void LoadSavedValue()
        {
            if (AjustesLocales.Values.ContainsKey("ColorEnfasis"))
            {
                MenuNativo.IsOn = (bool)AjustesLocales.Values["ColorEnfasis"];
            }
            if (AjustesLocales.Values.ContainsKey("MenuNativo"))
            {
                MenuNativo.IsOn = (bool)AjustesLocales.Values["MenuNativo"];
            }
            if (AjustesLocales.Values.ContainsKey("BotonInferior"))
            {
                BotonInferior.IsOn = (bool)AjustesLocales.Values["BotonInferior"];
            }
            if (AjustesLocales.Values.ContainsKey("StatusBarVisibility"))
            {
                StatusBarVisibility.IsOn = (bool)AjustesLocales.Values["StatusBarVisibility"];
            }
            if (AjustesLocales.Values.ContainsKey("PantallaCompleta"))
            {
                PantallaCompleta.IsOn = (bool)AjustesLocales.Values["PantallaCompleta"];
            }
        }

        private void GuardarEstado_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch Interruptor = ((ToggleSwitch)sender);
            bool activado = Interruptor.IsOn;
            string NombreInterruptor = Interruptor.Name;

            if (!AjustesLocales.Values.ContainsKey(NombreInterruptor))
            {
                AjustesLocales.Values.Add(NombreInterruptor, activado); // add it
            }
            else
            {
                AjustesLocales.Values[NombreInterruptor] = activado; // edit it
            }

            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null)
            {
                var cacheSize = ((rootFrame)).CacheSize;
                ((rootFrame)).CacheSize = 0;
                ((rootFrame)).CacheSize = cacheSize;
            }
        }

        // Navegación menu lateral
        private void AjustesItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Pivot.SelectedItem = Ajustes;
        }

        private void AcercaDeItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Pivot.SelectedItem = AcercaDe;
        }

        private void ChangelogItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Pivot.SelectedItem = Changelog;
        }

        private void ProximamenteItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Pivot.SelectedItem = Proximamente;
        }
        // Botón de opiniones
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // The URI to launch
            var Opinar = new Uri("ms-windows-store://review/?ProductId=9NF61T873T5V");
            // Launch the URI
            var success = await Windows.System.Launcher.LaunchUriAsync(Opinar);
            if (success)
            {
                // URI launched
            }
            else
            {
                // URI launch failed
            }
        }
        // Botones de Feedback
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }
        // Botón donar
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Uri direccion = new Uri("https://www.paypal.me/PabloJmnz");
            await Launcher.LaunchUriAsync(direccion);
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Uri direccion = new Uri("https://www.foroinsider.com/threads/aplicaci%C3%B3n-uwp-no-oficial-de-pccomponentes-com.7636/");
            await Launcher.LaunchUriAsync(direccion);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Initialize();
        }

        private void Initialize()
        {
            VersionDescription = GetVersionDescription();
        }

        private string GetVersionDescription()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{package.DisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private async void ThemeChanged_CheckedAsync(object sender, RoutedEventArgs e)
        {
            var param = (sender as RadioButton)?.CommandParameter;

            if (param != null)
            {
                await ThemeSelectorService.SetThemeAsync((ElementTheme)param);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
