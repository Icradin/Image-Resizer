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
using YamlDotNet.Serialization;

namespace Image_Resizer
{
    internal class ImageProcessor
    {
        private ImageLoader _imageLoader;
        private Settings _settings;

        public int _desiredWidth = 0;
        public int _originalWidth = 0;

        public string _newFilePath;

        public List<string> _smallFiles = new List<string>();

        public ImageProcessor(ImageLoader loader, Settings settings)
        {
            _settings = settings;
            _imageLoader = loader;
            _newFilePath = _imageLoader._processedImagePath;
        }

        public IResampler GetImageSharpSampler(SamplerType samplerType)
        {
            switch (samplerType)
            {
                case SamplerType.Bicubic:
                    return KnownResamplers.Bicubic;
                case SamplerType.Bilinear:
                    return KnownResamplers.Triangle;
                default:
                    throw new ArgumentException($"Unsupported sampler type: {samplerType}");
            }
        }

        public void ProcessImages(int desiredWidth, string imagePath)
        {
            

            string fileExtension = Path.GetExtension(imagePath);

            //make sure to delete the old image if it exists
            if (File.Exists(Path.Combine(_imageLoader._processedImagePath, Path.GetFileName(imagePath))))
            {
                File.Delete(Path.Combine(_imageLoader._processedImagePath, Path.GetFileName(imagePath)));
            }

            using (Image<Rgba32> img = SixLabors.ImageSharp.Image.Load<Rgba32>(imagePath))
            {
                _originalWidth = img.Width;

                if(_originalWidth < desiredWidth)
                {
                    _smallFiles.Add(imagePath);
                }


                float scaleFactor = (float)desiredWidth / img.Width;
                int newHeight = (int)(img.Height * scaleFactor);

                var resizeOptions = new ResizeOptions
                {
                    Size = new SixLabors.ImageSharp.Size(desiredWidth, newHeight),
                    Sampler = GetImageSharpSampler(_settings.SamplerType)
                };

                img.Mutate(x => x.Resize(resizeOptions));

                switch (fileExtension)
                {
                    case ".png":
                        var pngEncoder = new PngEncoder
                        {
                            CompressionLevel = (PngCompressionLevel)_settings.PngCompressionLevel,
                            BitDepth = _settings.PngBitDepth,
                            FilterMethod = _settings.PngFilterMethod,
                        };
                        img.Save(_newFilePath, pngEncoder);
                        break;
                    case ".jpeg":
                    case ".jpg":
                        var jpegEncoder = new JpegEncoder
                        {
                            Quality = _settings.JpegQuality,
                            Interleaved = _settings.JpegInterleave,
                        };
                        img.Save(_newFilePath, jpegEncoder);
                        break;

                    case ".tiff":
                        var tiffEncoder = new TiffEncoder
                        {
                            Compression = (SixLabors.ImageSharp.Formats.Tiff.Constants.TiffCompression?)_settings.TiffCompression,
                            BitsPerPixel = _settings.TiffBitsPerPixel,

                        };
                        img.Save(_newFilePath, tiffEncoder);
                        break;

                    case ".bmp":
                        var bmpEncoder = new BmpEncoder
                        {
                            BitsPerPixel = _settings.BmpBits,
                            SupportTransparency = _settings.BmpTransparency,
                        };
                        img.Save(_newFilePath, bmpEncoder);
                        break;

                    case ".webp":
                        var webpEncoder = new WebpEncoder
                        {
                            Quality = _settings.WebPQuality,
                            Method = _settings.WebpEncodingMethod,
                        };
                        img.Save(_newFilePath, webpEncoder);
                        break;

                    case ".tga":
                        var tgaEncoder = new TgaEncoder
                        {
                            BitsPerPixel = _settings.TgaBitsPerPixel,
                            Compression = _settings.TgaCompression,
                        };
                        img.Save(_newFilePath, tgaEncoder);
                        break;

                    default:
                        img.Save(_newFilePath); 
                        break;
                }

            }

            StringBuilder smallFileString = new StringBuilder();
            smallFileString.AppendLine("These files are smaller than the desired resolution: ");
            foreach (string smallFile in _smallFiles)
            {
                smallFileString.Append(smallFile + "\n");
            }   
            


        }
    }
}
