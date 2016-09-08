using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFSearcher
{
    public class Result : File
    {
        private int Frequency_ = 0;
        private List<int> Pages_ = new List<int>();
        public List<int> Pages
        {
            get { return Pages_; }
            set
            {
                Pages_ = value;
                NotifyPropertyChanged("Pages");
            }
        }

        public int Frequency
        {
            get { return Frequency_; }
            set
            {
                Frequency_ = value;
                NotifyPropertyChanged("Frequency");
            }
        }
        public Result(string filePath, int frequency) : base(filePath)
        {

            FilePath = filePath;
            Frequency = frequency;
        }
    }
}
