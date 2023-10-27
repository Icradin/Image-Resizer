
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
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Runtime.InteropServices.ObjectiveC;

namespace Image_Resizer
{


    public partial class UIController : Form
    {
        private Settings settings;
        private ImageLoader _imageLoader;
        private ImageProcessor _imageProcessor;

        



        FolderBrowserDialog folderDialog = new FolderBrowserDialog();

        public Dictionary<string, string> imagePathMapping = new Dictionary<string, string>();
        public Dictionary<string, string> originalImagePaths = new Dictionary<string, string>();
        public List<string> lastSelectedImages = new List<string>();

        private StringBuilder tooSmallImages = new StringBuilder();

        public string appDataPath;
        public string loadedImagesPath;
        public string processedImagesPath;
        public string tempDirectoryPath;

        private double orriginalImageSize = 0;
        private double newImageSize = 0;

        private List<string> checkedListBox1Items = new List<string>();
        public IResampler GetImageSharpSampler(SamplerType samplerType)
        {
            switch (samplerType)
            {
                case SamplerType.Bicubic:
                    return KnownResamplers.Bicubic;
                case SamplerType.Bilinear:
                    return KnownResamplers.Triangle;

                // Add cases for other samplers if needed
                default:
                    throw new ArgumentException($"Unsupported sampler type: {samplerType}");
            }
        }

        public UIController()
        {
            InitializeComponent();

            settings = new Settings();
            _imageLoader = new ImageLoader(this, settings);
            _imageProcessor = new ImageProcessor(_imageLoader, settings);

            appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ImageResizer");
            loadedImagesPath = Path.Combine(appDataPath, "LoadedImages");
            processedImagesPath = Path.Combine(appDataPath, "ProcessedImages");
            tempDirectoryPath = Path.Combine(appDataPath, "Temp");  // Step 2: Initialize this variable here

            // Create directories if they don't exist
            if (!Directory.Exists(appDataPath)) Directory.CreateDirectory(appDataPath);
            if (!Directory.Exists(loadedImagesPath)) Directory.CreateDirectory(loadedImagesPath);
            if (!Directory.Exists(processedImagesPath)) Directory.CreateDirectory(processedImagesPath);
            if (!Directory.Exists(tempDirectoryPath)) Directory.CreateDirectory(tempDirectoryPath);  // Create the Temp directory if it doesn't exist

            this.FormClosing += new FormClosingEventHandler(this.Form1_FormClosing);

            propertyGrid1.SelectedObject = settings;
        }

        private void UpdateProgressBar(int value)
        {
           progressBar1.Value = value;
        }

        private void CallLoadImages(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();

            _imageLoader.LoadImages();

            foreach (string imageName in _imageLoader._checkedListBox1Filler)
            {
                checkedListBox1.Items.Add(imageName);
            }

        }

        private void CallProcessImages(int desiredResolution)
        {
            int selectedCount = checkedListBox1.CheckedIndices.Count;
            string imageName = "";
            string imagePath = "";
            string newImagePath = "";
            string fileExtension = "";


            DialogResult result = MessageBox.Show($"You are about to change the resolution of {selectedCount} images. Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return;
            }

            for (int index = 0; index < checkedListBox1.Items.Count; index++)
            {
                if (checkedListBox1.GetItemChecked(index) == true)
                {
                    imageName = _imageLoader._imageNames[index];
                    imagePath = _imageLoader._loadedImagePaths[imageName];
                    
                    
                    _imageProcessor.ProcessImages(desiredResolution, imagePath);

                }

            }
        }




        private void SetImageResolution(int desiredWidth)
        {


            int selectedCount = checkedListBox1.CheckedIndices.Count;
            
            DialogResult result = MessageBox.Show($"You are about to change the resolution of {selectedCount} images. Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return; // Exit the method if the user selects "No"
            }

            progressBar1.Minimum = 0;
            progressBar1.Maximum = selectedCount;
            progressBar1.Value = 0;

            foreach (int index in checkedListBox1.CheckedIndices)
            {
                string selectedItemText = checkedListBox1.Items[index].ToString();
                string imagePath = imagePathMapping[selectedItemText];



                tooSmallImages.AppendLine($"The Following Images are too Small to be resized: ");

                int originalWidth = 0;
                int originalHeight = 0;

                string newFilePath = Path.Combine(tempDirectoryPath, Path.GetFileName(imagePath));

                string fileExtension = Path.GetExtension(imagePath).ToLower();

                if (File.Exists(newFilePath))
                {
                    File.Delete(newFilePath);
                }

                using (Image<Rgba32> img = SixLabors.ImageSharp.Image.Load<Rgba32>(imagePath))
                {
                    originalWidth = img.Width;
                    originalHeight = img.Height;

                    if (originalWidth <= desiredWidth)
                    {
                        tooSmallImages.AppendLine(imagePath);


                        continue;
                    }

                    float scaleFactor = (float)desiredWidth / img.Width;
                    int newHeight = (int)(img.Height * scaleFactor);

                    var resizeOptions = new ResizeOptions
                    {
                        Size = new SixLabors.ImageSharp.Size(desiredWidth, newHeight),
                        Sampler = GetImageSharpSampler(settings.SamplerType)
                    };

                    img.Mutate(x => x.Resize(resizeOptions));

                    switch (fileExtension)
                    {
                        case ".png":
                            var pngEncoder = new PngEncoder
                            {
                                CompressionLevel = (PngCompressionLevel)settings.PngCompressionLevel,
                                BitDepth = settings.PngBitDepth,
                                FilterMethod = settings.PngFilterMethod,
                            };
                            img.Save(newFilePath, pngEncoder);
                            break;
                        case ".jpeg":
                        case ".jpg":
                            var jpegEncoder = new JpegEncoder
                            {
                                Quality = settings.JpegQuality,
                                Interleaved = settings.JpegInterleave,
                            };
                            img.Save(newFilePath, jpegEncoder);
                            break;

                        case ".tiff":
                            var tiffEncoder = new TiffEncoder
                            {
                                Compression = (SixLabors.ImageSharp.Formats.Tiff.Constants.TiffCompression?)settings.TiffCompression,
                                BitsPerPixel = settings.TiffBitsPerPixel,

                            };
                            img.Save(newFilePath, tiffEncoder);
                            break;

                        case ".bmp":
                            var bmpEncoder = new BmpEncoder
                            {
                                BitsPerPixel = settings.BmpBits,
                                SupportTransparency = settings.BmpTransparency,
                            };
                            img.Save(newFilePath, bmpEncoder);
                            break;

                        case ".webp":
                            var webpEncoder = new WebpEncoder
                            {
                                Quality = settings.WebPQuality,
                                Method = settings.WebpEncodingMethod,
                            };
                            img.Save(newFilePath, webpEncoder);
                            break;

                        case ".tga":
                            var tgaEncoder = new TgaEncoder
                            {
                                BitsPerPixel = settings.TgaBitsPerPixel,
                                Compression = settings.TgaCompression,
                            };
                            img.Save(newFilePath, tgaEncoder);
                            break;

                        default:
                            img.Save(newFilePath); // Default save method if no specific encoder is set
                            break;
                    }

                    

                    img.Save(newFilePath);

                    FileInfo fileInfo = new FileInfo(newFilePath);
                    double fileSizeInMB = Math.Round((double)fileInfo.Length / (1024 * 1024), 2);
                    string newItemText = $"{Path.GetFileName(imagePath)} ({img.Width}x{img.Height}, {fileSizeInMB} MB)";
                    UpdateCheckedListBox2(Path.GetFileName(imagePath), newItemText);
                    pictureBox2.Image?.Dispose();
                    pictureBox2.Image = ConvertToBitmap(img); // Assuming ConvertToBitmap is compatible with Six Labors


                }

                progressBar1.Value++;
            }

            string message = tooSmallImages.ToString();

            MessageBox.Show(message);

            CalculateSizeDifference();

            progressBar1.Value = 0;
        }

        private void CalculateLoadedSize()
        {
            string[] loadedImagePaths = Directory.GetFiles(loadedImagesPath);

            foreach (string loadedImageName in loadedImagePaths)
            {
                string fileExtension = Path.GetExtension(loadedImageName).ToLower();
                if (fileExtension == ".tiff")
                {
                    continue; // Skip Tiff images
                }
                using (Image<Rgba32> img = ImageSharpImage.Load<Rgba32>(loadedImageName))
                {
                    orriginalImageSize += Math.Round((double)new FileInfo(loadedImageName).Length / (1024 * 1024), 2);
                }
            }

            StringBuilder loadedImageSize = new StringBuilder();

            loadedImageSize.AppendLine($"Total Loaded Size: {Math.Round(orriginalImageSize, 2)} MB");

            textBox1.Text = loadedImageSize.ToString();
        }

        private void CalculateResizedSize()
        {
            string[] processedImagePaths = Directory.GetFiles(processedImagesPath);

            foreach (string processedImageName in processedImagePaths)
            {
                string fileExtension = Path.GetExtension(processedImageName).ToLower();
                if (fileExtension == ".tiff")
                {
                    continue; // Skip Tiff images
                }
                using (Image<Rgba32> img = ImageSharpImage.Load<Rgba32>(processedImageName))
                {
                    newImageSize += Math.Round((double)new FileInfo(processedImageName).Length / (1024 * 1024), 2);
                }
            }
        }

        private void CalculateSizeDifference()
        {
            CalculateLoadedSize();
            CalculateResizedSize();


            double totalSizeDifferenceMB = orriginalImageSize - newImageSize;
            StringBuilder prependBuilder = new StringBuilder();
            prependBuilder.AppendLine($"Total Original Size: {Math.Round(orriginalImageSize, 2)} MB");
            prependBuilder.AppendLine($"Total New Size: {Math.Round(newImageSize, 2)} MB");
            prependBuilder.AppendLine($"Total Data Saved: {Math.Round(totalSizeDifferenceMB, 2)} MB");
            prependBuilder.AppendLine();

            // Now, you can use prependBuilder.ToString() to get the final string
            textBox1.Text = prependBuilder.ToString();
        }

        private void UpdateCheckedListBox2(string originalFileName, string newItemText)
        {
            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                if (checkedListBox2.Items[i].ToString().StartsWith(originalFileName))
                {
                    checkedListBox2.Items.RemoveAt(i);
                    break;
                }
            }
            checkedListBox2.Items.Add(newItemText);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Dispose of the image in pictureBox2
            pictureBox2.Image?.Dispose();
            pictureBox2.Image = null;
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = null;

            // Delay the deletion
            System.Threading.Thread.Sleep(100);

            // Clear the Loaded Images directory
            ClearDirectory(loadedImagesPath, "Loaded Images");

            // Clear the Temp directory
            ClearDirectory(tempDirectoryPath, "Processed Images");

        }

        private void SelectLargeImages(object sender, EventArgs e)
        {
            int lir = settings.LargeImageResolution;
            List<int> indicesToCheck = new List<int>();
            List<int> indicesToUncheck = new List<int>();

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                string item = checkedListBox1.Items[i].ToString();

                // Attempt to extract resolution from the item string
                string resolutionPart = item.Split(' ')[0].Trim('(', ')');
                string[] dimensions = resolutionPart.Split('x');

                if (dimensions.Length != 2)
                {
                    // Skip if the format is unexpected
                    continue;
                }

                if (!int.TryParse(dimensions[0], out int width) || !int.TryParse(dimensions[1], out int height))
                {
                    // Skip if we can't parse the width or height
                    continue;
                }

                if (width >= lir || height >= lir)
                {
                    indicesToCheck.Add(i);
                }
                else if (width < lir || height < lir)
                {
                    indicesToUncheck.Add(i);
                }
            }

            // Now, set the items as checked outside of the loop
            foreach (int index in indicesToCheck)
            {
                checkedListBox1.SetItemChecked(index, true);
            }

            foreach (int index in indicesToUncheck)
            {
                checkedListBox1.SetItemChecked(index, false);
            }
        }



        private void SelectImageofType(string extension)
        {


            for (int index = 0; index < checkedListBox1.Items.Count; index++)
            {
                if (checkedListBox1.Items[index].ToString().ToLower().Contains(extension.ToLower()))
                {
                    checkedListBox1.SetItemChecked(index, true);
                }
            }


        }

        private void SelectPNG(object sender, EventArgs e)
        {
            SelectImageofType(".png");
        }

        private void SelectTGA(object sender, EventArgs e)
        {
            SelectImageofType(".tga");
        }

        private void OpenTempFolder(object sender, EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", tempDirectoryPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error 03: An error occurred: " + ex.Message);
            }
        }

        private void OpenImages(object sender, EventArgs e)
        {
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                string[] files = Directory.GetFiles(folderDialog.SelectedPath, "*.png", SearchOption.AllDirectories)
                    .Concat(Directory.GetFiles(folderDialog.SelectedPath, "*.jpg", SearchOption.AllDirectories))
                    .Concat(Directory.GetFiles(folderDialog.SelectedPath, "*.tga", SearchOption.AllDirectories))
                    .Concat(Directory.GetFiles(folderDialog.SelectedPath, "*.tiff", SearchOption.AllDirectories))
                    .Concat(Directory.GetFiles(folderDialog.SelectedPath, "*.webm", SearchOption.AllDirectories))
                    .ToArray();

                checkedListBox1.Items.Clear();


                List<string> unsupportedFiles = new List<string>();

                progressBar1.Minimum = 0;
                progressBar1.Maximum = files.Length;
                progressBar1.Value = 0;

                foreach (string file in files)
                {
                    File.Copy(file, Path.Combine(loadedImagesPath, Path.GetFileName(file)), true);

                    // Store the original path
                    originalImagePaths[Path.GetFileName(file)] = file; // Added this line

                    try
                    {
                        string resolution;
                        double fileSizeInMB;
                        using (Image<Rgba32> img = ImageSharpImage.Load<Rgba32>(file))
                        {
                            resolution = $"{img.Width}x{img.Height}";
                            fileSizeInMB = Math.Round((double)new FileInfo(file).Length / (1024 * 1024), 2);
                        }



                        string fileName = Path.GetFileName(file);
                        string parentDirectory = new DirectoryInfo(file).Parent.Name;
                        string displayText = $"({resolution} {fileSizeInMB} MB) {fileName} / {parentDirectory}";
                        checkedListBox1.Items.Add(displayText);
                        imagePathMapping[displayText] = Path.Combine(loadedImagesPath, Path.GetFileName(file));

                        ///rens edited the shit above
                    }
                    catch (Exception)
                    {
                        unsupportedFiles.Add(file);
                    }

                    progressBar1.Value++;
                }

                if (unsupportedFiles.Count > 0)
                {
                    string message = "Error 04: The following files are not supported:\n\n" + string.Join("\n", unsupportedFiles);
                    MessageBox.Show(message);
                }

                progressBar1.Value = 0;
                CalculateLoadedSize();
            }
        }

        private System.Drawing.Bitmap ConvertToBitmap(Image<Rgba32> image)
        {
            using (var memoryStream = new MemoryStream())
            {


                image.Save(memoryStream, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
                memoryStream.Seek(0, SeekOrigin.Begin);
                return new System.Drawing.Bitmap(memoryStream);
            }
        }

        //private void ChangeOrriginalImagePreview(object sender, ItemCheckEventArgs e)
        //{
        //    if (e.NewValue == CheckState.Checked)
        //    {
        //        int lastIndex = checkedListBox1.SelectedIndices.Count > 0 ? checkedListBox1.SelectedIndices[checkedListBox1.SelectedIndices.Count - 1] : -1;

        //        if (e.Index == lastIndex)
        //        {
        //            string selectedItemText = checkedListBox1.Items[e.Index].ToString();
        //            string imagePath = imagePathMapping[selectedItemText];

        //            pictureBox1.Image?.Dispose();

        //            int width = 0;
        //            int height = 0;
        //            if (Path.GetExtension(imagePath).ToLower() == ".tga")
        //            {
        //                try
        //                {
        //                    using (Image<Rgba32> img = ImageSharpImage.Load<Rgba32>(imagePath))
        //                    {
        //                        pictureBox1.Image = ConvertToBitmap(img);
        //                        width = img.Width;
        //                        height = img.Height;
        //                    }
        //                }
        //                catch (FileNotFoundException ex)
        //                {
        //                    MessageBox.Show($"Error 05: Error: {ex.Message}");
        //                }
        //            }
        //            else
        //            {
        //                pictureBox1.Image = System.Drawing.Image.FromFile(imagePath);
        //                width = pictureBox1.Image.Width;
        //                height = pictureBox1.Image.Height;
        //            }

        //            FileInfo fileInfo = new FileInfo(imagePath);
        //            double fileSizeInMB = Math.Round((double)fileInfo.Length / (1024 * 1024), 2);

        //            //label1.Text = $"Dimensions: {width}x{height}\nSize: {fileSizeInMB} MB";
        //        }
        //    }
        //}

        //private void ChangeProcessedImagePreview(object sender, ItemCheckEventArgs e)
        //{
        //    if (e.NewValue == CheckState.Checked)
        //    {
        //        int lastIndex = checkedListBox2.SelectedIndices.Count > 0 ? checkedListBox2.SelectedIndices[checkedListBox2.SelectedIndices.Count - 1] : -1;

        //        if (e.Index == lastIndex)
        //        {
        //            string selectedItemText = checkedListBox2.Items[e.Index].ToString();

        //            // Assuming tempFolderPath is the path to your temp folder
        //            string tempFolderPath = tempDirectoryPath;
        //            string filename = selectedItemText.Split(' ')[0];

        //            // Combine the temp folder path with the extracted filename
        //            string imagePath = Path.Combine(tempFolderPath, filename);


        //            pictureBox2.Image?.Dispose();

        //            int width = 0;
        //            int height = 0;
        //            if (Path.GetExtension(imagePath).ToLower() == ".tga")
        //            {
        //                try
        //                {
        //                    using (Image<Rgba32> img = ImageSharpImage.Load<Rgba32>(imagePath))
        //                    {
        //                        pictureBox2.Image = ConvertToBitmap(img);
        //                        width = img.Width;
        //                        height = img.Height;
        //                    }
        //                }
        //                catch (FileNotFoundException ex)
        //                {
        //                    MessageBox.Show($"Error 05: Error: {ex.Message}");
        //                }
        //            }
        //            else
        //            {
        //                pictureBox2.Image = System.Drawing.Image.FromFile(imagePath);
        //                width = pictureBox2.Image.Width;
        //                height = pictureBox2.Image.Height;
        //            }

        //            FileInfo fileInfo = new FileInfo(imagePath);
        //            double fileSizeInMB = Math.Round((double)fileInfo.Length / (1024 * 1024), 2);

        //            //label1.Text = $"Dimensions: {width}x{height}\nSize: {fileSizeInMB} MB";
        //        }
        //    }
        //}

        private void SelectAllImages(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
        }

        #region Cleanup



        //All the things nesessary to clear the folders when needed
        private void ClearTempFolderButton(object sender, EventArgs e)
        {
            ClearTempFolder();
            if (System.IO.Directory.GetFiles(tempDirectoryPath) == null)
            {
                MessageBox.Show("Temp folder cleared successfully.");
            }
            else if (System.IO.Directory.GetFiles(tempDirectoryPath) != null)
            {
                MessageBox.Show("Error 15: Temp folder not cleared.");
            }

        }

        private void ClearDirectory(string directoryPath, string directoryName)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(directoryPath);

                foreach (FileInfo file in di.GetFiles())
                {
                    int retryCount = 3;
                    while (retryCount > 0)
                    {
                        try
                        {
                            file.Delete();
                            break; // Exit the loop if the file is deleted successfully
                        }
                        catch
                        {
                            if (retryCount == 1) // If it's the last retry and still fails, throw the exception
                            {
                                throw;
                            }
                            System.Threading.Thread.Sleep(100); // Wait for 100ms before retrying
                        }
                        retryCount--;
                    }
                }

                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error 16: An error occurred while clearing the {directoryName} directory: " + ex.Message);
            }
        }

        private void ClearTempFolder()
        {
            try
            {
                pictureBox2.Image?.Dispose();
                pictureBox2.Image = null;
                checkedListBox2.Items.Clear();

                DirectoryInfo di = new DirectoryInfo(tempDirectoryPath);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }

                MessageBox.Show("Temp folder cleared successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error 18: An error occurred while clearing the temp folder: {ex.Message}");
            }
        }


        #endregion

        #region File Writing and Restoration
        private void RestoreLastSelection(object sender, EventArgs e)
        {
            if (lastSelectedImages.Count == 0)
            {
                MessageBox.Show("No images were selected in the last overwrite operation.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show($"You are about to restore the last selected {lastSelectedImages.Count} images to their original state. Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return; // Exit the method if the user selects "No"
            }

            foreach (string item in lastSelectedImages)
            {
                string originalPath = Path.Combine("LoadedImages", Path.GetFileName(item));
                File.Copy(originalPath, item, true); // Overwrite the original with the backup from LoadedImages
            }

            // Clear the last selected images list and the checkedListBox2
            lastSelectedImages.Clear();
            checkedListBox2.Items.Clear();
        }

        private void RestoreAll(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"You are about to restore all images to their original state. Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return; // Exit the method if the user selects "No"
            }

            foreach (string item in checkedListBox2.Items)
            {
                string originalPath = Path.Combine("LoadedImages", Path.GetFileName(item));
                File.Copy(originalPath, item, true); // Overwrite the original with the backup from LoadedImages
            }

            checkedListBox2.Items.Clear();
        }

        private void OverwriteSelected(object sender, EventArgs e)
        {
            int selectedCount = checkedListBox2.CheckedItems.Count;
            DialogResult result = MessageBox.Show($"You are about to overwrite {selectedCount} images. Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return; // Exit the method if the user selects "No"
            }

            lastSelectedImages.Clear();
            foreach (string item in checkedListBox2.CheckedItems)
            {
                lastSelectedImages.Add(item);
            }

            // Create a list to store items that need to be removed after processing
            List<object> itemsToRemove = new List<object>();


            progressBar1.Minimum = 0;
            progressBar1.Maximum = checkedListBox2.CheckedItems.Count;
            progressBar1.Value = 0;

            foreach (string item in checkedListBox2.CheckedItems)
            {
                string fileNameWithResolution = item; // This is the full display text in checkedListBox2

                // Split the string by the opening parenthesis '(' and take the first part
                var parts = fileNameWithResolution.Split('(');




                if (parts.Length < 2)
                {
                    MessageBox.Show($"Error 19: Unexpected format for item: {item}");
                    continue;
                }
                string fileName = parts[0].Trim(); // Trim to remove any trailing spaces

                if (originalImagePaths.ContainsKey(fileName))
                {
                    string originalFullFilePath = originalImagePaths[fileName];

                    // Get original file size and resolution

                    int originalWidth = 0;
                    int originalHeight = 0;
                    using (System.Drawing.Image originalImg = System.Drawing.Image.FromFile(originalFullFilePath))
                    {
                        originalWidth = originalImg.Width;
                        originalHeight = originalImg.Height;
                    }
                    // Extract just the filename from originalFullFilePath
                    string fileNameOnly = Path.GetFileName(originalFullFilePath);

                    // Find the corresponding entry in checkedListBox1 based on the filename
                    var entry = imagePathMapping.FirstOrDefault(x => Path.GetFileName(x.Value) == fileNameOnly);
                    if (entry.Key != null)
                    {
                        checkedListBox1.Items.Remove(entry.Key);

                        // Path to the image in the temp folder
                        string tempFilePath = Path.Combine(tempDirectoryPath, fileNameOnly);




                        // Check if the image exists in the temp folder
                        if (File.Exists(tempFilePath))
                        {
                            // Delete the original file first
                            if (File.Exists(originalFullFilePath))
                            {
                                File.Delete(originalFullFilePath);
                            }

                            // Now, copy the image from the temp folder to the original location
                            File.Copy(tempFilePath, originalFullFilePath);

                            int newWidth = 0;
                            int newHeight = 0;
                            using (System.Drawing.Image newImg = System.Drawing.Image.FromFile(originalFullFilePath))
                            {
                                newWidth = newImg.Width;
                                newHeight = newImg.Height;
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Error 23: Image not found in temp folder: {tempFilePath}");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Error 21: Entry not found for {originalFullFilePath}");
                    }
                    // Add the item to the list of items to be removed from checkedListBox2
                    itemsToRemove.Add(item);
                }
                else
                {
                    MessageBox.Show($"Error 22: Original path not found for {fileName}");
                }

                progressBar1.Value++;
            }

            // Remove the processed items from checkedListBox2
            foreach (string item in itemsToRemove)
            {
                checkedListBox2.Items.Remove(item);
            }



            // Clear the pictureBox1
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            progressBar1.Value = 0;
        }

        private void OverwriteAll(object sender, EventArgs e)
        {
            int totalCount = checkedListBox2.Items.Count;
            DialogResult result = MessageBox.Show($"You are about to overwrite all {totalCount} images. Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return; // Exit the method if the user selects "No"
            }

            lastSelectedImages.Clear();
            foreach (string item in checkedListBox2.Items)
            {
                lastSelectedImages.Add(item);
            }

            // Select all items in checkedListBox2
            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                checkedListBox2.SetItemChecked(i, true);
            }


            // Create a list to store items that need to be removed after processing
            List<object> itemsToRemove = new List<object>();
            StringBuilder overwriteInfoBuilder = new StringBuilder();
            double totalOriginalSizeMB = 0;
            double totalNewSizeMB = 0;

            // Debug: Print out the contents of imagePathMapping

            double totalOriginalSize = 0;
            double totalNewSize = 0;
            StringBuilder label4Text = new StringBuilder();


            progressBar1.Minimum = 0;
            progressBar1.Maximum = checkedListBox2.Items.Count;
            progressBar1.Value = 0;

            foreach (string item in checkedListBox2.CheckedItems)
            {
                string fileNameWithResolution = item; // This is the full display text in checkedListBox2

                // Split the string by the opening parenthesis '(' and take the first part
                var parts = fileNameWithResolution.Split('(');
                if (parts.Length < 2)
                {
                    MessageBox.Show($"Error 19: Unexpected format for item: {item}");
                    continue;
                }
                string fileName = parts[0].Trim(); // Trim to remove any trailing spaces

                if (originalImagePaths.ContainsKey(fileName))
                {
                    string originalFullFilePath = originalImagePaths[fileName];

                    // Get original file size and resolution
                    double originalFileSizeInMB = Math.Round((double)new FileInfo(originalFullFilePath).Length / (1024 * 1024), 2);
                    totalOriginalSize += originalFileSizeInMB;

                    int originalWidth = 0;
                    int originalHeight = 0;
                    using (System.Drawing.Image originalImg = System.Drawing.Image.FromFile(originalFullFilePath))
                    {
                        originalWidth = originalImg.Width;
                        originalHeight = originalImg.Height;
                    }
                    // Extract just the filename from originalFullFilePath
                    string fileNameOnly = Path.GetFileName(originalFullFilePath);

                    // Find the corresponding entry in checkedListBox1 based on the filename
                    var entry = imagePathMapping.FirstOrDefault(x => Path.GetFileName(x.Value) == fileNameOnly);
                    if (entry.Key != null)
                    {
                        checkedListBox1.Items.Remove(entry.Key);

                        // Path to the image in the temp folder
                        string tempFilePath = Path.Combine(tempDirectoryPath, fileNameOnly);

                        // Capture the original file size before overwriting
                        FileInfo originalFileInfo = new FileInfo(originalFullFilePath);
                        totalOriginalSizeMB += (double)originalFileInfo.Length / (1024 * 1024);


                        // Check if the image exists in the temp folder
                        if (File.Exists(tempFilePath))
                        {
                            // Delete the original file first
                            if (File.Exists(originalFullFilePath))
                            {
                                File.Delete(originalFullFilePath);
                            }

                            // Now, copy the image from the temp folder to the original location
                            File.Copy(tempFilePath, originalFullFilePath);
                            double newFileSizeInMB = Math.Round((double)new FileInfo(originalFullFilePath).Length / (1024 * 1024), 2);
                            totalNewSize += newFileSizeInMB;

                            int newWidth = 0;
                            int newHeight = 0;
                            using (System.Drawing.Image newImg = System.Drawing.Image.FromFile(originalFullFilePath))
                            {
                                newWidth = newImg.Width;
                                newHeight = newImg.Height;
                            }


                        }

                        else
                        {
                            MessageBox.Show($"Error 23: Image not found in temp folder: {tempFilePath}");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Error 21: Entry not found for {originalFullFilePath}");
                    }

                    // Add the item to the list of items to be removed from checkedListBox2
                    itemsToRemove.Add(item);
                }
                else
                {
                    MessageBox.Show($"Error 22: Original path not found for {fileName}");
                }

                progressBar1.Value++;
            }

            // Remove the processed items from checkedListBox2
            foreach (string item in itemsToRemove)
            {
                checkedListBox2.Items.Remove(item);
            }



            // Clear the pictureBox1
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            progressBar1.Value = 0;
        }



        #endregion

        private void GridValueLimiter(object s, PropertyValueChangedEventArgs e)
        {
            //if (settings.encoderQuality >= 100 || settings.encoderQuality <= 0)
            //{
            //    MessageBox.Show($"The encoder quality must be between 0 and 100. Resetting Quality to 100 now.");
            //    settings.encoderQuality = 100;
            //    propertyGrid1.Refresh();
            //}
        }


        #region Resolution Buttons
        //Resolution chagning buttons
        private void button2_Click(object sender, EventArgs e)
        {
            CallProcessImages(2048);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CallProcessImages(1024);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            CallProcessImages(512);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            CallProcessImages(256);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            CallProcessImages(128);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CallProcessImages(64);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            CallProcessImages(32);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            CallProcessImages(16);
        }
        #endregion


        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }
    }
}