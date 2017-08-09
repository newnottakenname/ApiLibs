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
    class ArtistServiceTest
    {
        private ArtistService artistService;
        private SpotifyService spotify;

        [ClassInitialize]
        public void SetUp()
        {
            Passwords passwords = Passwords.ReadPasswords(Memory.ApplicationPath + "Laurentia" + Path.DirectorySeparatorChar);
            spotify = new SpotifyService(passwords.SpotifyRefreshToken, passwords.SpotifyClientId, passwords.SpotifySecret);
            artistService = spotify.ArtistService;
        }

        [TestMethod]
        public async Task GetArtistTest()
        {
            Assert.IsNotNull(await artistService.GetArtist("6MDME20pz9RveH9rEXvrOM"));
        }

        [TestMethod]
        public async Task GetArtistsTest()
        {
            Assert.IsNotNull(await artistService.GetArtists(new List<string>
            {
                "6MDME20pz9RveH9rEXvrOM",
                "1Xylc3o4UrD53lo9CvFvVg"
            }, new System.Globalization.RegionInfo("nl")));
        }

        [TestMethod]
        public async Task GetAlbumsFromArtistTest()
        {
            Assert.IsNotNull(await artistService.GetAlbumFromArtist("5cIc3SBFuBLVxJz58W2tU9"));
        }

        [TestMethod]
        public async Task GetTopTracksTest()
        {
            Assert.IsNotNull(await artistService.GetTopTracks("5cIc3SBFuBLVxJz58W2tU9"));
        }

        [TestMethod]
        public async Task GetRelatedArtistsTest()
        {
            Assert.IsNotNull(await artistService.GetRelatedArtists("5cIc3SBFuBLVxJz58W2tU9"));
        }


    }
}
