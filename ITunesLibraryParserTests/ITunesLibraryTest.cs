using System;
using System.Linq;
using ITunesLibraryParser.Tests.TestObjects;
using Moq;
using Xunit;

namespace ITunesLibraryParser.Tests {
        public class ITunesLibraryTest {

        private Mock<IFileSystem> fileSystem;
        private ITunesLibrary subject;
        private const string Filepath = "itunes-library-file-location";
        private const string SongWithBadData = "Stairway To The Stars";
        private const string AlbumWithMultipleArtists = "Bags Meets Wes";
        private const string AlbumNameForMultipleAlbums = "Undercurrent";

        public ITunesLibraryTest() {
            fileSystem = new Mock<IFileSystem>();
            subject = new ITunesLibrary(Filepath, fileSystem.Object);
        }

        [Fact]
        public void Tracks_Parses_All_Tracks_From_Library() {
            fileSystem.Setup(fs => fs.ReadTextFromFile(Filepath)).Returns(TestLibraryData.CreateXml());

            var result = subject.Tracks;
            Assert.Equal(36, result.Count());
        }

        [Fact]
        public void Tracks_Only_Parses_File_Once_For_ITunesLibrary_Lifetime() {
            fileSystem.Setup(fs => fs.ReadTextFromFile(Filepath)).Returns(TestLibraryData.CreateXml());

            var results = subject.Tracks;
            results = subject.Tracks;

            fileSystem.Verify(fs => fs.ReadTextFromFile(Filepath), Times.Once);
            Assert.NotNull(results);
        }

        [Fact]
        public void Tracks_Parses_Fields_On_Track() {
            fileSystem.Setup(fs => fs.ReadTextFromFile(Filepath)).Returns(TestLibraryData.CreateXml());

            var result = subject.Tracks.First();

            Assert.Equal(17714, result.TrackId);
            Assert.Equal("Dream Gypsy", result.Name);
            Assert.Equal("Bill Evans & Jim Hall", result.Artist);
            Assert.Equal("Judith Veevers", result.Composer);
            Assert.Equal(AlbumNameForMultipleAlbums, result.Album);
            Assert.Equal("Jazz", result.Genre);
            Assert.Equal("AAC audio file", result.Kind);
            Assert.Equal(result.Size, 11550486);
            Assert.Equal(result.TrackNumber, 3);
            Assert.Equal(result.Year, 1962);
            Assert.Equal(result.DateModified.Value.Date, new DateTime(2012, 2, 25));
            Assert.Equal(result.DateAdded.Value.Date, new DateTime(2012, 2, 25));
            Assert.Equal(result.BitRate, 320);
            Assert.Equal(result.SampleRate, 44100);
            Assert.Equal(result.PlayCount, 11);
            Assert.Equal(result.PlayDate.Value.Date, new DateTime(2012, 8, 15));
            Assert.False(result.PartOfCompilation);
            Assert.Equal(result.Rating, 80);
            Assert.Equal(result.AlbumRating, 60);
            Assert.True(result.AlbumRatingComputed);
            Assert.Equal("file://localhost/C:/Users/anthony/Music/iTunes/iTunes%20Music/Bill%20Evans%20&%20Jim%20Hall/Undercurrent/03%20Dream%20Gypsy.m4a", result.Location);
            Assert.Equal("C7B8EDD04F93004D", result.PersistentId);
        }

        [Fact]
        public void Tracks_Parses_AlbumArtist_Node_If_Present() {
            fileSystem.Setup(fs => fs.ReadTextFromFile(Filepath)).Returns(TestLibraryData.CreateXml());

            var results = subject.Tracks;

            Assert.Contains(results, t => !string.IsNullOrWhiteSpace(t.AlbumArtist));
        }

        [Fact]
        public void Tracks_Populates_Null_Values_For_Nonexistent_Elements() {
            fileSystem.Setup(fs => fs.ReadTextFromFile(Filepath)).Returns(TestLibraryData.CreateXml());

            var result = subject.Tracks.First();
            Assert.True(string.IsNullOrEmpty(result.AlbumArtist));
        }

        [Fact]
        public void Tracks_Sets_Boolean_Properties_To_True_For_Existing_Boolean_Nodes() {
            fileSystem.Setup(fs => fs.ReadTextFromFile(Filepath)).Returns(TestLibraryData.CreateXml());

            Assert.Equal(1, subject.Tracks.Count(t => t.PartOfCompilation));
        }

        [Fact]
        public void Tracks_Converts_Milliseconds_TotalTime_To_String_Playing_Time_Minutes_And_Seconds() {
            fileSystem.Setup(fs => fs.ReadTextFromFile(Filepath)).Returns(TestLibraryData.CreateXml());

            var result = subject.Tracks.First();
            
            Assert.Equal("4:35", result.PlayingTime);
        }

        [Fact]
        public void Tracks_Handles_Missing_Size_Values_In_Library() {
            fileSystem.Setup(fs => fs.ReadTextFromFile(Filepath)).Returns(TestLibraryData.CreateXml());

            var result = subject.Tracks.First(t => t.Name == SongWithBadData);

            Assert.Null(result.Size);
        }

        [Fact]
        public void Tracks_Handles_Missing_TotalTime_Values_In_Library() {
            fileSystem.Setup(fs => fs.ReadTextFromFile(Filepath)).Returns(TestLibraryData.CreateXml());

            var result = subject.Tracks.First(t => t.Name == SongWithBadData);

            Assert.True(string.IsNullOrEmpty(result.PlayingTime));
        }

        [Fact]
        public void Playlists_Parses_And_Returns_All_Playlists() {
            fileSystem.Setup(fs => fs.ReadTextFromFile(Filepath)).Returns(TestLibraryData.CreateXml());

            var results = subject.Playlists;

            Assert.Equal(15, results.Count());
        }

        [Fact]
        public void Playlists_Adds_Each_Track_To_Playlist() {
            fileSystem.Setup(fs => fs.ReadTextFromFile(Filepath)).Returns(TestLibraryData.CreateXml());

            var results = subject.Playlists;

            var result = results.First();
            Assert.Equal(29475, result.PlaylistId);
            Assert.Equal("MILES JAZZ - 3/2/06", result.Name);
            var firstTrack = result.Tracks.First();
            Assert.Equal("So What", firstTrack.Name);
        }

        [Fact]
        public void Albums_Groups_Tracks_Into_Albums_From_Library() {
            fileSystem.Setup(fs => fs.ReadTextFromFile(Filepath)).Returns(TestLibraryData.CreateXml());

            var results = subject.Albums;

            var result = results.First();
            Assert.True(results.Count() > 0);
            Assert.Equal("Bill Evans & Jim Hall", result.Artist);
            Assert.Equal("Undercurrent", result.Name);
            Assert.Equal("Jazz", result.Genre);
            Assert.False(result.IsCompilation);
            Assert.Equal(result.Year, 1962);
            Assert.Equal(8, result.Tracks.Count());
        }

        [Fact]
        public void Albums_Uses_AlbumArtist_For_Artist_When_It_Exists() {
            fileSystem.Setup(fs => fs.ReadTextFromFile(Filepath)).Returns(TestLibraryData.CreateXml());

            var results = subject.Albums;
 
            var result = results.First(r => r.Name.StartsWith(AlbumWithMultipleArtists));
            Assert.Equal(result.Artist, result.Tracks.First().AlbumArtist);
        }

        [Fact]
        public void Albums_Uses_VariousArtists_As_Artist_When_Compilation_And_No_AlbumArtist_Defined() {
            fileSystem.Setup(fs => fs.ReadTextFromFile(Filepath)).Returns(TestLibraryData.CreateXml());

            var results = subject.Albums;

            Assert.Contains(results, x => x.Artist == "Various Artists");
        }

        [Fact]
        public void Albums_Groups_Tracks_With_Different_Artists_From_Same_Album() {
            fileSystem.Setup(fs => fs.ReadTextFromFile(Filepath))
                .Returns(TestLibraryData.CreateXml());

            var results = subject.Albums;
           
            Assert.Contains(results, a => a.Tracks.GroupBy(t => t.Artist).Count() > 1);
        }

        [Fact]
        public void Albums_Groups_Tracks_From_Different_Albums_With_Same_Name() {
            fileSystem.Setup(fs => fs.ReadTextFromFile(Filepath)).Returns(TestLibraryData.CreateXml());

            var result = subject.Albums;

            Assert.Equal(2, result.Count(a => a.Name == AlbumNameForMultipleAlbums));
        }
    }
}
