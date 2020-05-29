using System.Threading.Tasks;
using SpotifyEnhancer.DataAccess.Models;

namespace SpotifyEnhancer.DataAccess.Interfaces
{
    public interface IAppleMusicHttpClient
    {
        Task<Library> GetUserLibrary(string userToken, string offsetEndpoint = "");
    }
}