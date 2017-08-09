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
    class TrackServiceTest
    {
        private TrackService trackService;

        [ClassInitialize]
        public void SetUp()
        {
            Passwords passwords = Passwords.ReadPasswords(Memory.ApplicationPath + "Laurentia" + Path.DirectorySeparatorChar);
            SpotifyService spotify = new SpotifyService(passwords.SpotifyRefreshToken, passwords.SpotifyClientId, passwords.SpotifySecret);
            trackService = spotify.TrackService;
        }

        [TestMethod]
        public async Task GetTrackTest()
        {
            Assert.AreEqual("Never Gonna Give You Up", (await trackService.GetTrack("4uLU6hMCjMI75M1A2tKUQC")).name);
        }

        [TestMethod]
        public async Task GetAudioAnalysisTest()
        {
            Assert.IsNotNull((await trackService.GetAudioAnalysis("4uLU6hMCjMI75M1A2tKUQC")).track);
        }

        [TestMethod]
        public async Task GetAudioFeaturesTest()
        {
            Assert.IsNotNull((await trackService.GetAudioFeatures("4uLU6hMCjMI75M1A2tKUQC")).acousticness);
        }
    }
}
