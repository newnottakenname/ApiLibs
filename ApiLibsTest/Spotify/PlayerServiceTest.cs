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
    class PlayerServiceTest
    {
        private PlayerService playerService;
        private SpotifyService spotify;

        [ClassInitialize]
        public void SetUp()
        {
            Passwords passwords = Passwords.ReadPasswords(Memory.ApplicationPath + "Laurentia" + Path.DirectorySeparatorChar);
            spotify = new SpotifyService(passwords.SpotifyRefreshToken, passwords.SpotifyClientId, passwords.SpotifySecret);
            playerService = spotify.PlayerService;
        }

        [TestMethod]
        public async Task GetDevicesTest()
        {
            Assert.IsNotNull(await playerService.GetDevices());
        }

        [TestMethod]
        public async Task GetPlayerTest()
        {
            Assert.IsNotNull(await playerService.GetPlayer());
        }

        [TestMethod]
        public async Task GetCurrentlyPlayingTest()
        {
            Assert.IsNotNull(await playerService.GetCurrentPlaying());
        }

        [TestMethod]
        [TestCategory("ModifyState")]
        public async Task TransferPlayback()
        {
            Device d = (await playerService.GetDevices()).devices[1];
            await playerService.TransferPlayback(d, false);
        }

        [TestMethod]
        [TestCategory("ModifyState")]
        public async Task PlayTest()
        {
            Track t = await spotify.TrackService.GetTrack("4uLU6hMCjMI75M1A2tKUQC");
            Device d = (await playerService.GetPlayer()).device;
            await playerService.Play(t, d);
        }

        [TestMethod]
        [TestCategory("ModifyState")]
        public async Task PlayAlbumTest()
        {
            Album album = await spotify.AlbumService.GetAlbum("30SqWqmSU9ww0Btb1j4rpU");
            await playerService.Play(album);
        }

        [TestMethod]
        [TestCategory("ModifyState")]
        public async Task PlayArtistTest()
        {
            await playerService.Play(new Artist { id= "5Pwc4xIPtQLFEnJriah9YJ" });
        }

        [TestMethod]
        [TestCategory("ModifyState")]
        public async Task PlayPlaylistTest()
        {
            await playerService.Play(new Playlist { id= "37i9dQZEVXcGwXcYmYDANi", owner = new Owner {  id= "onerepublicofficial" }  });
        }

        [TestMethod]
        [TestCategory("ModifyState")]
        public async Task PauseTest()
        {
            await playerService.Pause();
        }

        [TestMethod]
        [TestCategory("ModifyState")]
        public async Task NextTest()
        {
            await playerService.Next();
        }

        [TestMethod]
        [TestCategory("ModifyState")]
        public async Task NextWithIdTest()
        {
            var device = (await playerService.GetPlayer()).device;
            await playerService.Next(device.id);
        }

        [TestMethod]
        [TestCategory("ModifyState")]
        public async Task PreviousTest()
        {
            await playerService.Previous();
        }

        [TestMethod]
        [TestCategory("ModifyState")]
        public async Task PreviousWithIdTest()
        {
            var device = (await playerService.GetPlayer()).device;
            await playerService.Previous(device.id);
        }

        [TestMethod]
        [TestCategory("ModifyState")]
        public async Task SeekTest()
        {
            await playerService.Seek(0);
        }

        [TestMethod]
        [TestCategory("ModifyState")]
        public async Task SeekWithIdTest()
        {
            var device = (await playerService.GetPlayer()).device;
            await playerService.Seek(0, device.id);
        }

        [TestMethod]
        [TestCategory("ModifyState")]
        public async Task RepeatTest()
        {
            await playerService.Repeat(RepeatState.Off);
        }

        [TestMethod]
        [TestCategory("ModifyState")]
        public async Task RepeatAllTest()
        {
            await playerService.Repeat(RepeatState.Context);
            await playerService.Repeat(RepeatState.Track);
            await playerService.Repeat(RepeatState.Off);
        }

        [TestMethod][TestCategory("ModifyState")]
        public async Task RepeatWithIdTest()
        {
            var device = (await playerService.GetPlayer()).device;
            await playerService.Repeat(RepeatState.Context, device.id);
        }

        [TestCategory("ModifyState")]
        [TestMethod]
        public async Task ShuffleTest()
        {
            await playerService.Shuffle(false);
        }

        [TestCategory("ModifyState")]
        [TestMethod]
        public async Task StartShuffleTest()
        {
            await playerService.Shuffle(true);
        }

        [TestMethod]
        [TestCategory("ModifyState")]
        public async Task StartShuffleWithIdTest()
        {
            var device = (await playerService.GetPlayer()).device;
            await playerService.Shuffle(true, device.id);
        }
    }
}
