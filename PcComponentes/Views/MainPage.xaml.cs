using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace PC_Componentes_UWP.Views
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {

        ApplicationDataContainer AjustesLocales = ApplicationData.Current.LocalSettings;


        public MainPage()
        {
            InitializeComponent();

            // Modificar interfaz en función de los ajustes
            if (AjustesLocales.Values.ContainsKey("MenuNativo"))
            {
                if ((bool)AjustesLocales.Values["MenuNativo"] && BarraNavegacion.DefaultLabelPosition != CommandBarDefaultLabelPosition.Right)
                {
                    BarraSuperiorMovil.Visibility = Visibility.Visible;
                }
                else
                {
                    BarraSuperiorMovil.Visibility = Visibility.Collapsed;
                }
            }
            if (AjustesLocales.Values.ContainsKey("BotonInferior"))
            {
                if ((bool)AjustesLocales.Values["BotonInferior"] && BarraNavegacion.DefaultLabelPosition != CommandBarDefaultLabelPosition.Right)
                {
                    BotonCategoriasInferior.Visibility = Visibility.Visible;
                }
                else
                {
                    BotonCategoriasInferior.Visibility = Visibility.Collapsed;
                }
            }
            if (AjustesLocales.Values.ContainsKey("StatusBarVisibility"))
            {
                if ((bool)AjustesLocales.Values["StatusBarVisibility"])
                {
                    if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
                    {
                        StatusBar.GetForCurrentView().ShowAsync();
                    }
                }
                else
                {
                    if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
                    {
                        StatusBar.GetForCurrentView().HideAsync();
                    }
                }
            }
            if (AjustesLocales.Values.ContainsKey("PantallaCompleta"))
            {
                if ((bool)AjustesLocales.Values["PantallaCompleta"])
                {
                    ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
                }
                else
                {
                    ApplicationView.GetForCurrentView().ExitFullScreenMode();
                }
            }
            // Color de la TitleBar
            /// Colores personalizados
            Color ColorPrincipal = Color.FromArgb(255, 255, 96, 0);
            Color ColorSecundario = Color.FromArgb(255, 255, 75, 0);
            /// Movil
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar.GetForCurrentView().BackgroundColor = ColorPrincipal;
                StatusBar.GetForCurrentView().BackgroundOpacity = 1;
                StatusBar.GetForCurrentView().ForegroundColor = Colors.White;
            }
            /// PC
            var TitleBar = ApplicationView.GetForCurrentView().TitleBar;
            /// Title Bar Content Area
            TitleBar.BackgroundColor = ColorPrincipal;
            TitleBar.ForegroundColor = Colors.White;
            /// Title Bar Buttons Area
            TitleBar.ButtonBackgroundColor = ColorPrincipal;
            TitleBar.ButtonForegroundColor = Colors.White;
            TitleBar.ButtonHoverBackgroundColor = ColorSecundario;
            TitleBar.ButtonHoverForegroundColor = Colors.White;

            // Método por si el WebView necesita pantalla completa
            Vista.ContainsFullScreenElementChanged += Vista_ContainsFullScreenElementChanged;
            // ProgRing
            Vista.ContentLoading += Vista_ContentLoading;
            Vista.Loading += Vista_Loading;
            Vista.NavigationCompleted += Vista_NavigationCompleted;
            Vista.Loaded += Vista_Loaded;
            // ErrorMessage
            Vista.NavigationFailed += Vista_NavigationFailed;
            // Método que comprueba si se puede navegar para atrás o para adelante y activa o desactiva los botones de navegación
            ComprobarNavegacion();
        }
        // Método que se ejecuta si hay elementos que se reproducen en pantalla completa en el WebView
        private void Vista_ContainsFullScreenElementChanged(WebView sender, object args)
        {
            var applicationView = ApplicationView.GetForCurrentView();

            if (sender.ContainsFullScreenElement)
            {
                applicationView.TryEnterFullScreenMode();
                BarraSuperiorMovil.Visibility = Visibility.Collapsed;
                BarraNavegacion.Visibility = Visibility.Collapsed;
                Vista.Margin = new Thickness(0);
            }
            else if (applicationView.IsFullScreenMode)
            {
                applicationView.ExitFullScreenMode();
                BarraSuperiorMovil.Visibility = Visibility.Visible;
                BarraNavegacion.Visibility = Visibility.Visible;
            }
        }

        // Método que comprueba si se puede navegar para atrás o para adelante y activa o desactiva los botones de navegación
        private void ComprobarNavegacion()
        {
            if (Vista.CanGoBack)
            {
                BackButton.IsEnabled = true;
            }
            else
            {
                BackButton.IsEnabled = false;
            }
            if (Vista.CanGoForward)
            {
                FowardButton.IsEnabled = true;
            }
            else
            {
                FowardButton.IsEnabled = false;
            }
        }

        // Visibilidad del ErrorMessage
        private void Vista_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            Vista.Visibility = Visibility.Collapsed;
            ProgRing.Visibility = Visibility.Collapsed;
            ErrorMessage.Visibility = Visibility.Visible;
            ToTopButton.Visibility = Visibility.Collapsed;
            // Método que comprueba si se puede navegar para atrás o para adelante y activa o desactiva los botones de navegación
            ComprobarNavegacion();
        }
        // Funcionalidad del ErrorMessage
        private void Hyperlink_Click(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            Vista.Refresh();
        }
        // Visibilidad del Proggres Ring
        private void Vista_Loading(FrameworkElement sender, object args)
        {
            ProgRing.Visibility = Visibility.Visible;
            ErrorMessage.Visibility = Visibility.Collapsed;
            ToTopButton.Visibility = Visibility.Collapsed;
            // Método que comprueba si se puede navegar para atrás o para adelante y activa o desactiva los botones de navegación
            ComprobarNavegacion();
        }
        private void Vista_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
            ProgRing.Visibility = Visibility.Visible;
            ErrorMessage.Visibility = Visibility.Collapsed;
            ToTopButton.Visibility = Visibility.Collapsed;
            // Método que comprueba si se puede navegar para atrás o para adelante y activa o desactiva los botones de navegación
            ComprobarNavegacion();
        }
        private async void Vista_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            // Visibilidad "To top button"
            string addlistener = @"window.onscroll = function() {myFunction()};
                          function myFunction()
                          {if( window.pageYOffset === 0 )  window.external.notify('true'); else window.external.notify('false');} ";
            await Vista.InvokeScriptAsync("eval", new string[] { addlistener });
            //first time check
            var firstnotify = "if( window.pageYOffset === 0 )  window.external.notify('true'); else window.external.notify('false')";
            await Vista.InvokeScriptAsync("eval", new string[] { firstnotify });

            Vista.Visibility = Visibility.Visible;
            ProgRing.Visibility = Visibility.Collapsed;
            ErrorMessage.Visibility = Visibility.Collapsed;
            // Método que comprueba si se puede navegar para atrás o para adelante y activa o desactiva los botones de navegación
            ComprobarNavegacion();
        }
        private void Vista_Loaded(object sender, RoutedEventArgs e)
        {
            Vista.Visibility = Visibility.Visible;
            ProgRing.Visibility = Visibility.Collapsed;
            ErrorMessage.Visibility = Visibility.Collapsed;
            // Método que comprueba si se puede navegar para atrás o para adelante y activa o desactiva los botones de navegación
            ComprobarNavegacion();
        }
        // Abrir el menú de categorias
        private void CategoriasButton_Click(object sender, RoutedEventArgs e)
        {
            MenuCategorias.IsPaneOpen = !MenuCategorias.IsPaneOpen;
        }
        // Botones de la CommandBar inferior
        /// Botón Home
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            Uri Inicio = new Uri("http://www.pccomponentes.com/");
            Vista.Navigate(Inicio);
        }
        /// Botón atrás
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (Vista.CanGoBack)
            {
                Vista.GoBack();
            }
        }
        /// Botón adelante
        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (Vista.CanGoForward)
            {
                Vista.GoForward();
            }
        }
        /// Botón refrescar
        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            Vista.Refresh();
        }
        /// Botón Configuración
        private void GoToAbout_Click(object sender, RoutedEventArgs e)
        {
            /// Navegar a Configuración
            Frame.Navigate(typeof(SettingsPage));
        }
        /// Botón abrir en navegador
        private async void AppBarButton_Click_4(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(Vista.Source);
        }
        /// Botón compartir
        private void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
            // Register for the share event
            DataTransferManager.GetForCurrentView().DataRequested += DataTransferManager_DataRequested;

        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Unregister for the share event
            DataTransferManager.GetForCurrentView().DataRequested -= DataTransferManager_DataRequested;
        }
        /// <summary>
        /// Raised when the user initiates a Share operation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            // We are going to use an async API to talk to the webview, so get a deferral to give
            // us time to generate the results.
            DataRequestDeferral deferral = request.GetDeferral();
            DataPackage dp = await Vista.CaptureSelectedContentToDataPackageAsync();

            // Determine whether there was any selected content.
            bool hasSelection = false;
            try
            {
                hasSelection = (dp != null) && (dp.GetView().AvailableFormats.Count > 0);
            }
            catch (Exception ex)
            {
                switch (ex.HResult)
                {
                    case unchecked((int)0x80070490):
                        // Mobile does not generate a data package with AvailableFormats
                        // and results in an exception. Sorry. Handle the case by acting as
                        // if we had no selected content.
                        hasSelection = false;
                        break;

                    default:
                        // All other exceptions are unexpected. Let them propagate.
                        throw;
                }
            }

            if (hasSelection)
            {
                // Webview has a selection, so we'll share its data package.
                dp.Properties.Title = "Echa un vistazo a este artículo en PC Componentes:";
            }
            else
            {
                // No selection, so we'll share the url of the webview.
                dp = new DataPackage();
                dp.SetWebLink(Vista.Source);
                dp.Properties.Title = "Echa un vistazo a este artículo en PC Componentes:";
            }
            dp.Properties.Description = Vista.Source.ToString();
            request.Data = dp;

            deferral.Complete();
        }

        // Navegación botones superiores
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            CategoriasButton.Visibility = Visibility.Collapsed;
            ImagenMovil.Visibility = Visibility.Collapsed;
            SearchButton.Visibility = Visibility.Collapsed;
            UserButton.Visibility = Visibility.Collapsed;
            CartButton.Visibility = Visibility.Collapsed;
            InterfazBusqueda.Visibility = Visibility.Visible;
            CuadroBusqueda.Focus(FocusState.Programmatic);
        }
        private void CerrarBusqueda_Click(object sender, RoutedEventArgs e)
        {
            CerrarInterfazBusqueda();
        }
        private void CerrarInterfazBusqueda()
        {
            CategoriasButton.Visibility = Visibility.Visible;
            ImagenMovil.Visibility = Visibility.Visible;
            SearchButton.Visibility = Visibility.Visible;
            UserButton.Visibility = Visibility.Visible;
            CartButton.Visibility = Visibility.Visible;
            InterfazBusqueda.Visibility = Visibility.Collapsed;
            Vista.Focus(FocusState.Programmatic);
            CuadroBusqueda.Text = "";
        }

        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            Uri Direccion = new Uri("https://www.pccomponentes.com/usuarios/panel/mis-datos");
            Vista.Navigate(Direccion);
        }
        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            Uri Direccion = new Uri("https://www.pccomponentes.com/cart/");
            Vista.Navigate(Direccion);
        }

        // Navegación con botones del Hamburguer Menú
        private void Componentes_Click(object sender, RoutedEventArgs e)
        {
            Uri componentes = new Uri("https://www.pccomponentes.com/componentes");
            Vista.Navigate(componentes);
            MenuCategorias.IsPaneOpen = false;
        }

        private void Perifericos_Click(object sender, RoutedEventArgs e)
        {
            Uri perifericos = new Uri("https://www.pccomponentes.com/perifericos");
            Vista.Navigate(perifericos);
            MenuCategorias.IsPaneOpen = false;
        }

        private void Cables_Click(object sender, RoutedEventArgs e)
        {
            Uri cables = new Uri("https://www.pccomponentes.com/cables");
            Vista.Navigate(cables);
            MenuCategorias.IsPaneOpen = false;
        }

        private void Redes_Click(object sender, RoutedEventArgs e)
        {
            Uri redes = new Uri("https://www.pccomponentes.com/redes");
            Vista.Navigate(redes);
            MenuCategorias.IsPaneOpen = false;
        }

        private void Ordenadores_Click(object sender, RoutedEventArgs e)
        {
            Uri ordenadores = new Uri("https://www.pccomponentes.com/ordenadores");
            Vista.Navigate(ordenadores);
            MenuCategorias.IsPaneOpen = false;
        }

        private void Multimedia_Click(object sender, RoutedEventArgs e)
        {
            Uri multimedia = new Uri("https://www.pccomponentes.com/audio-foto-video");
            Vista.Navigate(multimedia);
            MenuCategorias.IsPaneOpen = false;
        }

        private void Smartphones_Click(object sender, RoutedEventArgs e)
        {
            Uri smartphones = new Uri("https://www.pccomponentes.com/smartphones-gps");
            Vista.Navigate(smartphones);
            MenuCategorias.IsPaneOpen = false;
        }

        private void Tablets_Click(object sender, RoutedEventArgs e)
        {
            Uri tablets = new Uri("https://www.pccomponentes.com/tablets-pc-ebook");
            Vista.Navigate(tablets);
            MenuCategorias.IsPaneOpen = false;
        }

        private void Gaming_Click(object sender, RoutedEventArgs e)
        {
            Uri gaming = new Uri("https://www.pccomponentes.com/consolas-gaming");
            Vista.Navigate(gaming);
            MenuCategorias.IsPaneOpen = false;
        }

        private void Consumibles_Click(object sender, RoutedEventArgs e)
        {
            Uri consumibles = new Uri("https://www.pccomponentes.com/consumibles");
            Vista.Navigate(consumibles);
            MenuCategorias.IsPaneOpen = false;
        }

        private void Hogar_Click(object sender, RoutedEventArgs e)
        {
            Uri hogar = new Uri("https://www.pccomponentes.com/electro-hogar");
            Vista.Navigate(hogar);
            MenuCategorias.IsPaneOpen = false;
        }

        private void Ocio_Click(object sender, RoutedEventArgs e)
        {
            Uri ocio = new Uri("https://www.pccomponentes.com/ocio-y-tiempo-libre");
            Vista.Navigate(ocio);
            MenuCategorias.IsPaneOpen = false;
        }
        // Botón borrar cache CommandBar
        private async void ClearCacheButton_Click(object sender, RoutedEventArgs e)
        {
            await WebView.ClearTemporaryWebDataAsync();
        }

        private void ImagenMovil_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Uri Inicio = new Uri("https://www.pccomponentes.com/");
            Vista.Navigate(Inicio);
        }
        // Funcionalidad "To top button"
        private async void ToTopButton_Click(object sender, RoutedEventArgs e)
        {
            var ScrollToTopString = @"window.scrollTo(0,0);";
            await Vista.InvokeScriptAsync("eval", new string[] { ScrollToTopString });
        }
        // Visibilidad "To top button
        private void Vista_ScriptNotify(object sender, NotifyEventArgs e)
        {
            var result = e.Value;
            if (result == "true")
            {
                ToTopButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                ToTopButton.Visibility = Visibility.Visible;
            }
        }

        private void CuadroBusqueda_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            Uri busqueda = new Uri("https://www.pccomponentes.com/buscar/?query=" + sender.Text);
            Vista.Navigate(busqueda);
            CerrarInterfazBusqueda();
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
