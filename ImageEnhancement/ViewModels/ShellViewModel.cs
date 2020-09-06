using Caliburn.Micro;
using System;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using ImageEnhancement.Models;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace ImageEnhancement.ViewModels
{
    public class ShellViewModel: Conductor<Object>
    {
        #region Fields&Properties
        private string _imgSource;
        private string _textSource;
        private string _imgDestination;
        private string _textDestination;
        private string _error = "";
        private BindableCollection<FilterBase> _filters = new BindableCollection<FilterBase> { new LowPassFilter() };
        private BindableCollection<Intensity> _intensities = new BindableCollection<Intensity> { new Intensity(1), new Intensity(2), new Intensity(4) };
        private Intensity _selectedIntensity;
        private FilterBase _selectedFilter;
        public string ImgSource
        {
            get { return _imgSource; }
            set
            {
                _imgSource = value;
                NotifyOfPropertyChange(() => ImgSource);
            }
        }

        public string TextSource
        {
            get { return _textSource; }
            set
            {
                _textSource = value;
                NotifyOfPropertyChange(() => TextSource);
            }
        }
        public string ImgDestination
        {
            get { return _imgDestination; }
            set
            {
                _imgDestination = value;
                NotifyOfPropertyChange(() => ImgDestination);
            }
        }
        public string TextDestination
        {
            get { return _textDestination; }
            set
            {
                _textDestination = value;
                NotifyOfPropertyChange(() => TextDestination);
            }
        }

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                NotifyOfPropertyChange(() => Error);
            }
        }

        public BindableCollection<FilterBase> Filters
        {
            get { return _filters; }
            set
            {
                _filters = value;
                NotifyOfPropertyChange(() => Filters);
            }
        }

        public BindableCollection<Intensity> Intensities
        {
            get { return _intensities; }
            set
            {
                _intensities = value;
                NotifyOfPropertyChange(() => Intensities);
            }
        }
        public Intensity SelectedIntensity
        {
            get { return _selectedIntensity; }
            set
            {
                _selectedIntensity = value;
                NotifyOfPropertyChange(() => SelectedIntensity);
            }
        }

        public FilterBase SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                _selectedFilter = value;
                NotifyOfPropertyChange(() => SelectedFilter);
            }
        }
        #endregion

        public void Browse()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                ImgSource = openFileDialog.FileName;
                TextSource = ImgSource;
            }
        }

        public void Select()
        {

            using(CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.InitialDirectory = "C:\\Users";
                dialog.IsFolderPicker = true;

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    ImgDestination = dialog.FileName;
                    TextDestination = ImgDestination;
                }
            };
        }

        public void Process()
        {
            if (string.IsNullOrWhiteSpace(TextSource) || string.IsNullOrWhiteSpace(TextDestination))
            {
                Error = "Both 'Source' and 'Destination' are necessary!";
                return;
            }

            Bitmap result = LoadFrom(TextSource);

            if (SelectedFilter is FilterBase)
            {
                for (int i = 0; i < SelectedIntensity.Value; i++)
                    result = Calculate.Convolution(result, SelectedFilter);
            }

            string filePath = GetFilePath();

            result.Save(filePath, ImageFormat.Jpeg);

            ImgDestination = filePath;
            result.Dispose();

            Error = "";
        }

        private Bitmap LoadFrom(string path)
        {
            var bytes = File.ReadAllBytes(path);
            var ms = new MemoryStream(bytes);
            return (Bitmap)Image.FromStream(ms);
        }

        private string GetFilePath()
        {
            string[] pathSplitArray = TextSource.Split('\\');
            // Get "Name" and "Extension"
            // Ex. "Astesia" ".jpg"
            string[] newFileNameArray = pathSplitArray[pathSplitArray.Length - 1].Split('.');
            // Creating unique name: "Name" + "(Current Date)" + "Extension"
            // Ex. "Astesia" "(06_09_2020 03_32_34)" + ".jpg"
            string newFileName = newFileNameArray[0] + '_' + $"({Format(DateTime.Now)})" + '.' + newFileNameArray[1];
            return (TextDestination + '\\' + newFileName);
        }

        private string Format(DateTime date)
        {
            string toReturn = "";

            toReturn = Convert.ToString(date);
            toReturn = toReturn.Replace('/', '_');
            toReturn = toReturn.Replace(':', '_');

            return toReturn;
        }
    }
}
