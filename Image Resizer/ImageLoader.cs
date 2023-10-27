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
using SixLabors.ImageSharp.Memory;

namespace Image_Resizer
{
    internal class ImageLoader
    {


        private UIController _uiController;
        private Settings _settings;

        //private variables
        private FolderBrowserDialog _folderBrowserDialog = new FolderBrowserDialog();

        private string _appDataPath;

        public string _orriginalImagePath;
        public string _loadedImagePath;
        public string _processedImagePath;

        public Dictionary<string, string> _orriginalImagePaths = new Dictionary<string, string>();
        public Dictionary<string, string> _loadedImagePaths = new Dictionary<string, string>();
        public Dictionary<string, string> _processedImagePaths = new Dictionary<string, string>();

        public List<string> _checkedListBox1Filler = new List<string>();
        public List<string> _imageNames = new List<string>();
        
        private int _progressBarValue = 0;
        public int _progressBarMaxValue = 0;

        public event EventHandler ProgressBarValueChanged;

        public ImageLoader(UIController controller, Settings settings)
        {
            _uiController = controller;
            _settings = settings;   

            _appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ImageResizer");

            _loadedImagePath = Path.Combine(_appDataPath, "LoadedImages");
            _processedImagePath = Path.Combine(_appDataPath, "ProcessedImages");

            if (!Directory.Exists(_appDataPath)) Directory.CreateDirectory(_appDataPath);
            if (!Directory.Exists(_loadedImagePath)) Directory.CreateDirectory(_loadedImagePath);
            if (!Directory.Exists(_processedImagePath)) Directory.CreateDirectory(_processedImagePath);

        }

        
        public void LoadImages()
        {
            if(_folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string[] files = Directory.GetFiles(_folderBrowserDialog.SelectedPath, "*.png", SearchOption.AllDirectories)
                    .Concat(Directory.GetFiles(_folderBrowserDialog.SelectedPath, "*.jpg", SearchOption.AllDirectories))
                    .Concat(Directory.GetFiles(_folderBrowserDialog.SelectedPath, "*.tga", SearchOption.AllDirectories))
                    //.Concat(Directory.GetFiles(_folderBrowserDialog.SelectedPath, "*.tiff", SearchOption.AllDirectories))
                    //.Concat(Directory.GetFiles(_folderBrowserDialog.SelectedPath, "*.webm", SearchOption.AllDirectories))
                    .ToArray();

                _progressBarMaxValue = files.Length;

                List<string> unsupportedFiles = new List<string>();

                foreach (string file in files)
                {
                    Debug.WriteLine(Path.GetFileName(file));
                    _imageNames.Add(Path.GetFileName(file));

                    string destinationPath = Path.Combine(_loadedImagePath, Path.GetFileName(file));
                    
                    File.Copy(file, destinationPath, true);
                    
                    _orriginalImagePaths[Path.GetFileName(file)] = file;
                    _loadedImagePaths[Path.GetFileName(file)] = destinationPath;

                    try
                    {
                        string resolution;
                        double fileSize;
                        
                        using (Image<Rgba32> img = ImageSharpImage.Load<Rgba32>(file))
                        {
                            resolution = $"{img.Width}x{img.Height}";
                            fileSize = Math.Round((double)new FileInfo(file).Length / (1024 * 1024), 2);
                        }

                        string fileName = Path.GetFileName(file);
                        string parentDirectory = new DirectoryInfo(file).Parent.Name;
                        string itemName = $"({resolution} {fileSize} MB) {fileName} / {parentDirectory}";
                        _checkedListBox1Filler.Add(itemName);

                    }
                    catch (Exception)
                    {
                        unsupportedFiles.Add(file);
                    }

                    if(unsupportedFiles.Count > 0)
                    {
                        string message = "FormatError: The following files are not supported:\n\n" + string.Join("\n", unsupportedFiles);
                        MessageBox.Show(message);
                    }

                    
                }
            }


        }

    }
}
