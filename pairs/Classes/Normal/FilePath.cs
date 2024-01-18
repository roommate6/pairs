using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace pairs.Classes
{
    [Serializable]
    internal class FilePath : INotifyPropertyChanged
    {
        // Class members:
        public static readonly string PATH_RESOURCES =
            Directory.GetParent(Environment.CurrentDirectory).Parent.FullName.ToString() +
            @"\Resources\";
        private string m_path;
        private string m_fileName;

        // Constructors:
        public FilePath(string path, string fileName)
        {
            Path = path;
            FileName = fileName;
        }
        public FilePath()
        {
            Path = null;
            FileName = null;
        }

        // Properties:
        public string Path
        {
            get
            {
                return m_path;
            }
            set
            {
                m_path = value;
                NotifyPropertyChanged("Path");
                NotifyPropertyChanged("FullPath");
                NotifyPropertyChanged("Initialized");
            }
        }
        public string FileName
        {
            get
            {
                return m_fileName;
            }
            set
            {
                m_fileName = value;
                NotifyPropertyChanged("FileName");
                NotifyPropertyChanged("FullPath");
                NotifyPropertyChanged("Initialized");
            }
        }
        [JsonIgnore]
        public string FullPath
        {
            get
            {
                return ToString();
            }
        }
        [JsonIgnore]
        public bool Initialized
        {
            get
            {
                return Path != null && FileName != null;
            }
        }

        // Functionality:

        // Outside implementations:
        public override string ToString()
        {
            return PATH_RESOURCES + m_path + "\\" + m_fileName;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
