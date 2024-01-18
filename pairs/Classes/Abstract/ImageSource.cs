using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace pairs.Classes
{
    internal abstract class ImageSource : INotifyPropertyChanged
    {
        // Constructors:
        protected ImageSource(string pathToImageDirectory, string imageName, string nullImageName)
        {
            InitializeMembers(pathToImageDirectory, imageName, nullImageName);
        }
        protected ImageSource(string pathToImageDirectory, string imageName)
        {
            InitializeMembers(pathToImageDirectory, imageName);
        }

        // Properties:
        [JsonIgnore]
        public virtual string Source
        {
            get
            {
                return ImageFilePath.FullPath;
            }
        }
        public string ImageName
        {
            get
            {
                return ImageFilePath.FileName;
            }
            set
            {
                ImageFilePath.FileName = value;
                NotifyPropertyChanged(nameof(ImageName));
                NotifyPropertyChanged(nameof(Source));
                NotifyPropertyChanged(nameof(Initialized));
            }
        }
        [JsonIgnore]
        public abstract bool Initialized
        {
            get;
        }
        protected FilePath ImageFilePath
        {
            get
            {
                return m_imageFilePath;
            }
            set
            {
                m_imageFilePath = value;
                NotifyPropertyChanged(nameof(Source));
            }
        }

        // Events:
        public event PropertyChangedEventHandler PropertyChanged;

        // Methods:
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private void RemoveNullableArguments(ref string imageName, string nullImageName)
        {
            if (imageName == null)
            {
                imageName = nullImageName;
            }
        }

        // Initializers:
        private void InitializeMembers(string pathToImageDirectory, string imageName, string nullImageName)
        {
            RemoveNullableArguments(ref imageName, nullImageName);
            ImageFilePath = new FilePath(pathToImageDirectory, imageName);
        }
        private void InitializeMembers(string pathToImageDirectory, string imageName)
        {
            ImageFilePath = new FilePath(pathToImageDirectory, imageName);
        }

        // Protected members:
        protected FilePath m_imageFilePath;
    }
}
