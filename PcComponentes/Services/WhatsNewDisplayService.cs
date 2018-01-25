using System;
using System.Threading.Tasks;

using PC_Componentes_UWP.Helpers;
using PC_Componentes_UWP.Views;

using Windows.ApplicationModel;

namespace PC_Componentes_UWP.Services
{
    // For instructions on testing this service see https://github.com/Microsoft/WindowsTemplateStudio/tree/master/docs/features/whats-new-prompt.md
    public static class WhatsNewDisplayService
    {
        internal static async Task ShowIfAppropriateAsync()
        {
            var currentVersion = PackageVersionToReadableString(Package.Current.Id.Version);

            var lastVersion = await Windows.Storage.ApplicationData.Current.LocalSettings.ReadAsync<string>(nameof(currentVersion));

            if (lastVersion == null)
            {
                await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(nameof(currentVersion), currentVersion);
            }
            else
            {
                if (currentVersion != lastVersion)
                {
                    await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(nameof(currentVersion), currentVersion);

                    var dialog = new WhatsNewDialog();
                    await dialog.ShowAsync();
                }
            }
        }

        private static string PackageVersionToReadableString(PackageVersion packageVersion)
        {
            return $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}.{packageVersion.Revision}";
        }
    }
}
