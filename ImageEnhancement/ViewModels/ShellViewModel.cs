using Caliburn.Micro;
using System;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using ImageEnhancement.Models;
using System.Drawing.Imaging;
using System.Drawing;

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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                ImgSource = openFileDialog.FileName;
                TextSource = ImgSource;
            }
        }

        public void Select()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                ImgDestination = dialog.FileName;
                TextDestination = ImgDestination;
            }
        }

        public void Process()
        {
            if (string.IsNullOrWhiteSpace(TextSource) || string.IsNullOrWhiteSpace(TextDestination))
            {
                Error = "Both 'Source' and 'Destination' are necessary!";
                return;
            }

            LowPassFilter lowp = new LowPassFilter();
            Bitmap result = new Bitmap(TextSource);

            if (SelectedFilter is FilterBase)
            {
                for (int i = 0; i < SelectedIntensity.Value; i++)
                    result = Calculate.Convolution(result, lowp);
            }

            string[] splitArray = TextSource.Split('\\');
            string filePath = TextDestination + '\\' + splitArray[splitArray.Length - 1];

            if (System.IO.File.Exists(filePath))
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                System.IO.File.Delete(filePath);
            }

            result.Save(filePath, ImageFormat.Jpeg);

            ImgDestination = filePath;
            result.Dispose();

            Error = "";
            TextSource = "";
            TextDestination = "";
        }
    }
}
