
namespace AppSettings.Shared.Settings
{
    /// <summary>
    /// A helper class that stores the name of the appsettings file and the database connection string keys.
    /// </summary>
    public static class AppSettingsHelper
    {
        #region Properties

        /// <summary>
        /// The file name for the appsettings file.
        /// </summary>
        public static string AppSettingsFileName => "appsettings.json";

        /// <summary>
        /// The key for the developmment database connection string.
        /// </summary>
        public static string DevDbConnectionStringKey => "DevConnection";

        #endregion
    }
}
