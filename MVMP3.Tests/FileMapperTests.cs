using MVMP3.Mappers;
using Xunit;

namespace MVMP3.Tests
{
    public class FileMapperTests
    {
        [Theory]
        [InlineData("I am...", "I am")]
        [InlineData("...I am", "I am")]
        [InlineData(@"B'z The Best ""Pleasure""", "B'z The Best Pleasure")]
        public void RemoveInvalidChars(string directory, string expected)
        {
            //var mapper = new FileMapper(string.Empty, null);
            //var actual = mapper.RemoveInvalidChars(directory);
            //Assert.Equal(expected, actual);
        }
    }
}
