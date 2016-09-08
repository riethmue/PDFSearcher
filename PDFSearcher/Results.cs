using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace PDFSearcher
{
    public class Results : ObservableCollection<Result>
    {
        public void SearchForWords(ObservableCollection<File> fileList, string word)
        {
            foreach(var file in fileList)
            {
                SearchForWords(file, word);
            }
        }


        private void SearchForWords(File file, string word)
        {
            int frequency = 0;
            Result currentItem = null;
            if(System.IO.File.Exists(file.FilePath))
            {
                var pdfReader = new PdfReader(file.FilePath);
                for(var currentPage=1;currentPage <=pdfReader.NumberOfPages; currentPage++)
                {
                    var textExtractionStrategy = new SimpleTextExtractionStrategy();
                    var currentPageText = PdfTextExtractor.GetTextFromPage(pdfReader,currentPage, textExtractionStrategy);
                    if(currentPageText.Contains(word))
                    {
                        frequency++;
                        if(frequency == 1)
                        {
                            Add(new Result(file.FilePath, 0));
                            currentItem = this[this.Count - 1];
                        }

                        currentItem.Frequency++;
                        currentItem.Pages.Add(currentPage);
                    }
                }
                pdfReader.Close();
            }
        }
    }
}
