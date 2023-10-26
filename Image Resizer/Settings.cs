
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using System.ComponentModel;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Webp;

namespace Image_Resizer
{

    public enum SamplerType
    {
        Bicubic,
        Bilinear,
        
        // Add other sampler types as needed
    }

    public enum JpegSubsample
    {
        Ratio444, 
        Ratio420,
    }

    public enum TiffCompression
    {
        None, 
        Lzw, 
        Deflate, 

    }



    class Settings
    {
        public bool AutoBitDepth { get; set; } = true;


        

        public int LargeImageResolution { get; set; } = 1024;
        public SamplerType SamplerType { get; set; } = SamplerType.Bicubic;

        // PNG settings
        public int PngCompressionLevel { get; set; } = 6; // Default
        public PngBitDepth PngBitDepth { get; set; } = PngBitDepth.Bit8;
        public PngFilterMethod PngFilterMethod { get; set; } = PngFilterMethod.None;

        // JPEG settings
        public int JpegQuality { get; set; } = 75; // Default
        public bool JpegInterleave { get; set; } = false;

        public JpegSubsample JpegSubsample { get; set; } = JpegSubsample.Ratio444;

        // TIFF settings
        public TiffBitsPerPixel TiffBitsPerPixel { get; set; } = TiffBitsPerPixel.Bit24;
        public TiffCompression TiffCompression { get; set; } = TiffCompression.None;

        // BMP settings

        public BmpBitsPerPixel BmpBits { get; set; } = BmpBitsPerPixel.Pixel24;
        public bool BmpTransparency { get; set; } = false;

        // WebP settings
        public int WebPQuality { get; set; } = 75; // Default
        public WebpEncodingMethod WebpEncodingMethod { get; set; } = WebpEncodingMethod.Default;

        // TGA settings
        public TgaBitsPerPixel TgaBitsPerPixel { get; set; } = TgaBitsPerPixel.Pixel32;
        public TgaCompression TgaCompression { get;set; } = TgaCompression.None;
    }
}
