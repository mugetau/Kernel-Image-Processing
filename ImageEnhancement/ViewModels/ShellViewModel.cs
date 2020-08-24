using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using Microsoft.Win32;
using System.IO;
using ImageEnhancement.Models;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace ImageEnhancement.ViewModels
{
    public class ShellViewModel: Conductor<Object>
    {
        #region Fields&Properties
        private string _source;
        private string _destination;
        public string Source 
        {
            get { return _source; } 
            set 
            { 
                _source = value;
                NotifyOfPropertyChange(() => Source);
            }
        }
        public string Destination
        {
            get { return _destination; }
            set
            {
                _destination = value;
                NotifyOfPropertyChange(() => Destination);
            }
        }

        public ImagePath ImgPath { get; set; } = new ImagePath();
        #endregion

        public void Browse()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                Source = openFileDialog.FileName;

            ImgPath.Source = Source;
            /*ImagePath imgPath = new ImagePath();
            imgPath.Source = Source;
            JSONManager.JSONExport(imgPath);*/
        }

        public void Select()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Destination = dialog.FileName;
                ImgPath.Destination = Destination;
            }
        }
    }
}
