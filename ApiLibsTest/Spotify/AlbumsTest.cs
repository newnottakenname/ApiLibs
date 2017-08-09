using System.IO;
using System.Threading.Tasks;
using ApiLibs.General;
using ApiLibs.Spotify;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApiLibsTest.Spotify
{
    class AlbumsTest
    {
        private AlbumService albumService;
        private SpotifyService spotify;

        [TestInitialize]
        public void SetUp()
        {
            Passwords passwords = Passwords.ReadPasswords(Memory.ApplicationPath + "Laurentia" + Path.DirectorySeparatorChar);
            spotify = new SpotifyService(passwords.SpotifyRefreshToken, passwords.SpotifyClientId, passwords.SpotifySecret);
            albumService = spotify.AlbumService;
        }

        [TestMethod]
        public async Task GetAlbumTest()
        {
            Assert.IsNotNull(await albumService.GetAlbum("1qnHtqA3ZLLxFnUid3VseY"));
        }

        [TestMethod]
        public async Task GetNewReleasesTest()
        {
            Assert.IsNotNull(await albumService.GetNewReleases());
        }

        [TestMethod]
        public async Task GetTracksTest()
        {
            Album alb = await albumService.GetAlbum("1qnHtqA3ZLLxFnUid3VseY");
            Assert.IsNotNull(await albumService.GetTracks(alb));
        }
    }
}
