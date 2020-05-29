using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpotifyEnhancer.BuisnessLogic.Interfaces;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Enums;
using SpotifyEnhancer.Config;
using SpotifyEnhancer.DataAccess.Interfaces;

//Models for the JSON-responses


namespace SpotifyEnhancer.BuisnessLogic
{
    public class MigrationService : IMigrationService
    {
        
        private readonly ILogger<MigrationService> _logger;
        private readonly IAppleMusicHttpClient _appleMusicClient;
        private readonly SpotifyWebAPI _spotifyClient;
        private readonly SpotifyConfig _spotifyConfig;

        public MigrationService(ILogger<MigrationService> logger, IAppleMusicHttpClient appleMusicClient,
            IOptions<SpotifyConfig> spotifyConfig)
        {
            _logger = logger;
            _appleMusicClient = appleMusicClient;
            _spotifyClient = new SpotifyWebAPI();
            _spotifyClient.AccessToken = spotifyConfig.Value.Token;
        }

        public async Task<bool> MigrateLibrary(string userToken)
        {
            // Lets handle the pagination of the response by calling it as a while loop
            var offsetValue = "";
            
            do { 
                var appleLibraryResponse = await _appleMusicClient.GetUserLibrary(userToken, offsetValue);
                offsetValue = appleLibraryResponse.Next;
                List<string> songsToSave = new List<string>();
                foreach (var song in appleLibraryResponse.Data)
                {
                    // Attributes to search
                    var songTitle = song.Attributes.Name;
                    var artistTitle = song.Attributes.ArtistName;

                    // Search result
                    var spotifySong = await _spotifyClient.SearchItemsAsync($"{songTitle},{artistTitle}"
                        , SearchType.Track);

                    // Search result was empty continue to next song
                    if (!spotifySong.Tracks.Items.Any())
                    {
                        // Create a json file here to write all songs not found
                        continue;
                    }
                    
                    // It's okay to use the first item in the list since we're going one at a time
                    var spotifySongId = spotifySong.Tracks.Items[0].Id;
                    
                    var userAlreadyHasSong = _spotifyClient.CheckSavedTracks(new List<string>() {spotifySongId}).List[0];
                    if (!userAlreadyHasSong)
                    {
                        songsToSave.Add(spotifySong.Tracks.Items[0].Id);
                    }
                }

                await _spotifyClient.SaveTracksAsync(songsToSave);

                Console.WriteLine(appleLibraryResponse);
            } while (!String.IsNullOrEmpty(offsetValue));

            return true;
        }
    }
}