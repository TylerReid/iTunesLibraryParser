using System.Linq;
using ITunesLibraryParser.Tests.TestObjects;
using Xunit;

namespace ITunesLibraryParser.Tests {

        public class PlaylistTest {

        private Playlist subject;

        public PlaylistTest() {
            subject = PlaylistMother.Create();
        }

        [Fact]
        public void ToString_Playlist() {
            Assert.Equal($"{subject.Name} - {subject.Tracks.Count()} tracks", subject.ToString());
        }

        [Fact]
        public void Copy() {
            var result = subject.Copy();

            Assert.Equal(subject, result);
            Assert.NotSame(subject, result);
        }

        [Fact]
        public void Equals_Returns_False_When_Null() {
            var result = subject.Equals(null);
            Assert.False(result);
        }

        [Fact]
        public void Equals_Returns_False_When_Not_Equal() {
            var other = PlaylistMother.Create();
            other.Name = "New Playlist";

            var result = subject.Equals(other);
            Assert.False(result);
        }

        [Fact]
        public void Equals_Returns_True_When_Equal() {
            var other = PlaylistMother.Create();

            var result = subject.Equals(other);
            Assert.True(result);
        }

        [Fact]
        public void Equals_Returns_True_When_Same_Reference() {
            var result = subject.Equals(subject);
            Assert.True(result);
        }

        [Fact]
        public void Object_Equals_Returns_False_When_Not_SameType() {
            var other = AlbumMother.Create();

            var result = subject.Equals(other);
            Assert.False(result);
        }

        [Fact]
        public void Object_Equals_Returns_False_When_Null() {
            var result = subject.Equals((object)null);
            Assert.False(result);
        }

        [Fact]
        public void Object_Equals_Returns_True_When_Equal() {
            var other = PlaylistMother.Create() as object;

            var result = subject.Equals(other);
            Assert.True(result);
        }

        [Fact]
        public void Object_Equals_Returns_True_When_ReferenceEqual() {
            var result = subject.Equals(subject as object);
            Assert.True(result);
        }

        [Fact]
        public void GetHashCode_Returns_The_Same_HashCode_For_Equal_Playlists() {
            var expectedHashCode = subject.Copy().GetHashCode();

            var result = subject.GetHashCode();
            Assert.Equal(expectedHashCode, result);
        }

        [Fact]
        public void GetHashCode_Returns_A_Different_HashCode_For_Playlists_Not_Equal() {
            var other = subject.Copy();
            other.Name = "Jazz for the Dark Hours";
            var expectedHashCode = other.GetHashCode();

            var result = subject.GetHashCode();
            Assert.NotEqual(expectedHashCode, result);
        }
    }
}
