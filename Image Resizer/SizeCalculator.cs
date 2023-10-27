using System.Diagnostics;
using ImageSharpImage = SixLabors.ImageSharp.Image;
using System.Text;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Webp;

namespace Image_Resizer
{
    internal class SizeCalculator
    {
        private ImageLoader _imageLoader;

        private double _loadedImageSize = 0;
        private double _processedImageSize = 0;


        public SizeCalculator(ImageLoader imageLoader)
        {
            _imageLoader = imageLoader;
        }


        public void CalculateLoadedSize()
        {
            foreach (string image in _imageLoader._loadedImagePaths.Values)
            {
                using (Image<Rgba32> img = ImageSharpImage.Load<Rgba32>(image))
                {
                    _loadedImageSize += Math.Round((double)new FileInfo(image).Length / (1024 * 1024), 2);
                }
            }
        }

        public void CalculateProcessedSize()
        {
            foreach (string image in _imageLoader._processedImagePaths.Values)
            {
                using (Image<Rgba32> img = ImageSharpImage.Load<Rgba32>(image))
                {
                    _loadedImageSize += Math.Round((double)new FileInfo(image).Length / (1024 * 1024), 2);
                }
            }
        }

        




    }
}
