using System;
using System.Threading.Tasks;

using PC_Componentes_UWP.Helpers;
using PC_Componentes_UWP.Views;

using Windows.ApplicationModel;

namespace PC_Componentes_UWP.Services
{
    public static class FirstRunDisplayService
    {
        internal static async Task ShowIfAppropriateAsync()
        {
            bool hasShownFirstRun = false;
            hasShownFirstRun = await Windows.Storage.ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(hasShownFirstRun));

            if (!hasShownFirstRun)
            {
                await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(nameof(hasShownFirstRun), true);
                var dialog = new FirstRunDialog();
                await dialog.ShowAsync();
            }
        }
    }
}
