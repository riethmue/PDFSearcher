using System.Collections.Generic;
using System.ComponentModel;

namespace PDFSearcher
{
    public class File : INotifyPropertyChanged
    {
        private string FilePath_ = string.Empty;
        public string FilePath
        {
            get { return FilePath_; }
            set
            {
                FilePath_ = value;
                NotifyPropertyChanged("FilePath");
            }
        }

        public File(string filePath)
        {
            FilePath = filePath;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
