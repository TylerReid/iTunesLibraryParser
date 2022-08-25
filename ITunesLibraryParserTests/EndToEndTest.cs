using System;
using System.IO;
using System.Linq;
using Xunit;

namespace ITunesLibraryParser.Tests {
        public class EndToEndTest {
        private readonly string fileLocation = Path.Combine(Environment.CurrentDirectory,
            "sampleiTunesLibrary.xml");
        private ITunesLibrary subject;

        public EndToEndTest() {
            subject = new ITunesLibrary(fileLocation);
        }

        [Fact]
        public void Tracks_Reads_Library_From_Filesystem_And_Parses() {
            var results = subject.Tracks;

            var result = results.First();
            Assert.Equal(17714, result.TrackId);
            Assert.Equal("Dream Gypsy", result.Name);
            Assert.Equal("Bill Evans & Jim Hall", result.Artist);
            Assert.Equal(80, result.Rating);
            Assert.Equal(60, result.AlbumRating);
            Assert.True(result.AlbumRatingComputed);
        }
    }
}

