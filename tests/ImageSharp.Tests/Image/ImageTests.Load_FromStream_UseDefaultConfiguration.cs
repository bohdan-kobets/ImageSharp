// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Tests.TestUtilities;

namespace SixLabors.ImageSharp.Tests;

public partial class ImageTests
{
    public class Load_FromStream_UseDefaultConfiguration : IDisposable
    {
        private static readonly byte[] Data = TestFile.Create(TestImages.Bmp.Bit8).Bytes;

        private MemoryStream BaseStream { get; }

        private AsyncStreamWrapper Stream { get; }

        private bool AllowSynchronousIO { get; set; } = true;

        public Load_FromStream_UseDefaultConfiguration()
        {
            this.BaseStream = new MemoryStream(Data);
            this.Stream = new AsyncStreamWrapper(this.BaseStream, () => this.AllowSynchronousIO);
        }

        private static void VerifyDecodedImage(Image img)
            => Assert.Equal(new Size(127, 64), img.Size());

        [Fact]
        public void Stream_Specific()
        {
            using var img = Image.Load<Rgba32>(this.Stream);
            VerifyDecodedImage(img);
        }

        [Fact]
        public void Stream_Agnostic()
        {
            using var img = Image.Load(this.Stream);
            VerifyDecodedImage(img);
        }

        [Fact]
        public void Stream_OutFormat_Specific()
        {
            using var img = Image.Load<Rgba32>(this.Stream, out IImageFormat format);
            VerifyDecodedImage(img);
            Assert.IsType<BmpFormat>(format);
        }

        [Fact]
        public void Stream_OutFormat_Agnostic()
        {
            using var img = Image.Load(this.Stream, out IImageFormat format);
            VerifyDecodedImage(img);
            Assert.IsType<BmpFormat>(format);
        }

        [Fact]
        public async Task Async_Stream_OutFormat_Agnostic()
        {
            this.AllowSynchronousIO = false;
            (Image Image, IImageFormat Format) formattedImage = await Image.LoadWithFormatAsync(this.Stream);
            using (formattedImage.Image)
            {
                VerifyDecodedImage(formattedImage.Image);
                Assert.IsType<BmpFormat>(formattedImage.Format);
            }
        }

        [Fact]
        public async Task Async_Stream_Specific()
        {
            this.AllowSynchronousIO = false;
            using Image<Rgba32> img = await Image.LoadAsync<Rgba32>(this.Stream);
            VerifyDecodedImage(img);
        }

        [Fact]
        public async Task Async_Stream_Agnostic()
        {
            this.AllowSynchronousIO = false;
            using Image img = await Image.LoadAsync(this.Stream);
            VerifyDecodedImage(img);
        }

        [Fact]
        public async Task Async_Stream_OutFormat_Specific()
        {
            this.AllowSynchronousIO = false;
            (Image<Rgba32> Image, IImageFormat Format) formattedImage = await Image.LoadWithFormatAsync<Rgba32>(this.Stream);
            using (formattedImage.Image)
            {
                VerifyDecodedImage(formattedImage.Image);
                Assert.IsType<BmpFormat>(formattedImage.Format);
            }
        }

        public void Dispose() => this.BaseStream?.Dispose();
    }
}
