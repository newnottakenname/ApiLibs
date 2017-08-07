using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.Spotify;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApiLibsTest.Spotify
{
    class SpotifyTest
    {
        private SpotifyService spotify;

        [ClassInitialize]
        public void SetUp()
        {
            Passwords passwords = Passwords.ReadPasswords(Memory.ApplicationPath + "Laurentia" + Path.DirectorySeparatorChar);
            spotify = new SpotifyService(passwords.SpotifyRefreshToken, passwords.SpotifyClientId, passwords.SpotifySecret);
        }

        [TestMethod]
        public async Task SearchTrackTest()
        {
            var result = await spotify.Search("Never gonna give you up", SpotifyService.SearchType.Track);
            Assert.IsNotNull(result.tracks.items);
        }

        [TestMethod]
        public async Task SearchArtistTest()
        {
            var result = await spotify.Search("Rick Astley", SpotifyService.SearchType.Artist);
            Assert.IsNotNull(result.artists.items);
        }

        [TestMethod]
        public async Task SearchAlbumTest()
        {
            var result = await spotify.Search("Whenever you need somebody", SpotifyService.SearchType.Album);
            Assert.IsNotNull(result.albums.items);
        }

        [TestMethod]
        public async Task SearchPlayListTest()
        {
            var result = await spotify.Search("Rick Astley: Top Tracks", SpotifyService.SearchType.Playlist);
            Assert.IsNotNull(result.playlists.items);
        }
    }
}
