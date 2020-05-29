using System.Threading.Tasks;

namespace SpotifyEnhancer.BuisnessLogic.Interfaces
{
    /// <summary>
    /// A service for moving library songs from Apple music to spotify
    /// </summary>
    public interface IMigrationService
    {
        /// <summary>
        /// Goes through the entire users apple music library and attempts to add to the spotify account
        /// </summary>
        Task<bool> MigrateLibrary(string userToken);
    }
}