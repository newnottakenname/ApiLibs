using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.Spotify;
using NUnit.Framework;

namespace ApiLibsTest.Spotify
{
    class ProfileServiceTest
    {
        private ProfileService profileService;
        private SpotifyService spotify;

        [SetUp]
        public void SetUp()
        {
            Passwords passwords = Passwords.ReadPasswords(Memory.ApplicationPath + "Laurentia" + Path.DirectorySeparatorChar);
            spotify = new SpotifyService(passwords.SpotifyRefreshToken, passwords.SpotifyClientId, passwords.SpotifySecret);
            profileService = spotify.ProfileService;
        }

        [Test]
        public async Task GetMeTest()
        {
            Assert.IsNotNull(await profileService.GetMe());
        }

        [Test]
        [Category("ModifyState")]
        public async Task FollowArtistTest()
        {
            await profileService.Follow(UserType.Artist, "32WkQRZEVKSzVAAYqukAEA");
        }

        [Test]
        [Category("ModifyState")]
        public async Task UnfollowArtistTest()
        {
            await profileService.Unfollow(UserType.Artist, "32WkQRZEVKSzVAAYqukAEA");
        }

        [Test]
        [Category("ModifyState")]
        public async Task FollowUserTest()
        {
            await profileService.Follow(UserType.User, "ohwondermusic");
        }

        [Test]
        [Category("ModifyState")]
        public async Task UnfollowUserTest()
        {
            await profileService.Unfollow(UserType.User, "ohwondermusic");
        }

        [Test]
        [Category("ModifyState")]
        public async Task CheckIfFollowingTest()
        {
            await profileService.CheckIfFollowing(UserType.User, new List<string> { "ohwondermusic" });
        }

        [Test]
        public async Task GetFollowingArtistsTest()
        {
            Assert.IsNotNull(await profileService.GetFollowingArtists());
        }

        [Test]
        public async Task GetTopArtistsTest()
        {
            Assert.IsNotNull(await profileService.GetTopArtists());
        }

        //Tracks

        [Test]
        public async Task GetTopTracksTest()
        {
            Assert.IsNotNull(await profileService.GetTopTracks());
        }

        [Test]
        public async Task GetRecentlyPlayedTest()
        {
            Assert.IsNotNull(await profileService.GetRecentlyPlayed());
        }

        [Test]
        public async Task GetSavedTest()
        {
            Assert.IsNotNull(await profileService.GetSavedTracks());
        }
    }
}
