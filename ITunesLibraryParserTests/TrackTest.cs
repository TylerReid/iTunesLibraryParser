using ITunesLibraryParser.Tests.TestObjects;
using Xunit;

namespace ITunesLibraryParser.Tests {
        public class TrackTest {
        private Track subject;
        public TrackTest() {
            subject = TrackMother.Create();
        }

        [Fact]
        public void ToString_Track() {
            Assert.Equal(subject.ToString(), 
                $"Artist: {subject.Artist} - Track: {subject.Name} - Album: {subject.Album}");
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
            var other = TrackMother.Create();
            other.Name = "Maiden Voyage";

            var result = subject.Equals(other);

            Assert.False(result);
        }

        [Fact]
        public void Equals_Returns_True_When_Equal() {
            var other = TrackMother.Create();

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
            var other = TrackMother.Create() as object;

            var result = subject.Equals(other);

            Assert.True(result);
        }

        [Fact]
        public void Object_Equals_Returns_True_When_ReferenceEqual() {
            var result = subject.Equals(subject as object);

            Assert.True(result);
        }

        [Fact]
        public void GetHashCode_Returns_The_Same_HashCode_For_Equal_Tracks() {
            var expectedHashCode = subject.Copy().GetHashCode();
            
            var result = subject.GetHashCode();

            Assert.Equal(expectedHashCode, result);
        }

        [Fact]
        public void GetHashCode_Returns_A_Different_HashCode_For_Tracks_Not_Equal() {
            var other = subject.Copy();
            other.AlbumArtist = "John Coltrane";
            var expectedHashCode = other.GetHashCode();

            var result = subject.GetHashCode();

            Assert.NotEqual(expectedHashCode, result);
        }
    }
}
