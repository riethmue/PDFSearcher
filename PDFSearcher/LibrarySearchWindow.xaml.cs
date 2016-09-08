using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PDFSearcher
{
    /// <summary>
    /// Interaktionslogik für DialogWindow.xaml
    /// </summary>
    public partial class LibrarySearchWindow : Window, INotifyPropertyChanged
    {
        #region binding fields
        private string LibraryPath_ = System.Configuration.ConfigurationManager.AppSettings.Get("libraryPath");
        private string SearchWords_ = string.Empty;
        private ICollectionView ResultListView_;
        private int SelectedIndex_ = 0;
        private BackgroundWorker BackgroundWorker = new BackgroundWorker();
        private int ProgressbarValue_ = 0;
        #endregion
        public int ProgressbarValue
        {
            get { return ProgressbarValue_; }
            set
            {
                ProgressbarValue_ = value;
                NotifyPropertyChanged("ProgressbarValue");
            }
        }
        public int SelectedIndex
        {
            get { return SelectedIndex_; }
            set
            {
                SelectedIndex_ = value;
                NotifyPropertyChanged("SelectedIndex");
            }
        }
        public ICollectionView ResultListView
        {
            get { return ResultListView_; }
            set
            {
                ResultListView_ = value;
                NotifyPropertyChanged("ResultListView");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private Results ResultList_ = new Results();
        private ObservableCollection<File> FileList_ = new ObservableCollection<File>();
        private Visibility ProgressbarVisibility_ = Visibility.Hidden;
        public Visibility ProgressbarVisibility
        {
            get { return ProgressbarVisibility_; }
            set
            {
                ProgressbarVisibility_ = value;
                NotifyPropertyChanged("ProgressbarVisibility");
            }
        }
        public ICommand SearchWordsEnterCommand { get;}
        public Results ResultList
        {
            get { return ResultList_; }
            set
            {
                ResultList_ = value;
                NotifyPropertyChanged("ResultList");
            }

        }
        public ObservableCollection<File> FileList
        {
            get { return FileList_; }
            set { FileList_ = value; }
        }
        public string SearchWords
        {
            get { return SearchWords_; }
            set
            {
                SearchWords_ = value;
                NotifyPropertyChanged("SearchWords");
            }
        }
        public string LibraryPath
        {
            get { return LibraryPath_; }
            set
            {
                LibraryPath_ = value;
                System.Configuration.ConfigurationManager.AppSettings.Set("libraryPath", value);
                NotifyPropertyChanged("LibraryPath");
            }
        }

        public LibrarySearchWindow()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            var resultDirectory = folderBrowserDialog.ShowDialog();
            
            if(resultDirectory == System.Windows.Forms.DialogResult.OK)
            {
                LibraryPath = folderBrowserDialog.SelectedPath;
            }
        }

        private void SearchWords_Click(object sender, RoutedEventArgs e)
        {
            ResultList.Clear();
            SearchingLibrary();
        }
        
        private void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName ="")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void SearchingLibrary()
        {
            if(Directory.Exists(LibraryPath) && SearchWords != string.Empty)
            {
                CollectFiles(LibraryPath);

                //// Show progressbar.
                //ProgressbarVisibility = Visibility.Visible;

                // For not blocking UI thread.

                //var newAsyncTask = new Task(() =>
                //{
                //    ResultList.SearchForWords(FileList, SearchWords);
                //});
                //newAsyncTask.Start();
                //newAsyncTask.Wait();
                

                ResultList.SearchForWords(FileList, SearchWords);
                ResultListView = CollectionViewSource.GetDefaultView(ResultList);
                SortResultListView();


                //ProgressbarVisibility = Visibility.Hidden;
            }
            else
            {
                // Show error message and reset textbox
            }
        }

        private void SortResultListView()
        {
            ResultListView.SortDescriptions.Add(new SortDescription("Frequency", ListSortDirection.Descending));
        }

        private void CollectFiles(string directoryPath)
        {
            var allFilesInDirectory = Directory.GetFiles(directoryPath);
            foreach (var item in allFilesInDirectory)
            {
                if (Directory.Exists(item))
                {
                    CollectFiles(item);
                }
                else
                {
                    if (IsPdfFile(item))
                    {
                        FileList.Add(new File(item));
                    }
                }
            }
        }

        private bool IsPdfFile(string filePath)
        {
            if (Path.GetExtension(filePath) == ".PDF" || Path.GetExtension(filePath) == ".pdf")
            {
                return true;
            }
            return false;
        }

        private void ListBoxItem_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            // Open file in default program.
            var item = (ListBoxItem)sender;
            var resultFile = (Result)item.Content;
            System.Diagnostics.Process.Start(resultFile.FilePath.ToString());
        }

        private void OpenDirectory_Click(object sender,RoutedEventArgs e)
        {
            var filePath = ResultList[SelectedIndex].FilePath;
            var lastDirectoryName = Path.GetDirectoryName(filePath);
            System.Diagnostics.Process.Start(lastDirectoryName);
        }

        private void Test()
        {
            BackgroundWorker = new BackgroundWorker();
            BackgroundWorker.WorkerReportsProgress = true;
            BackgroundWorker.DoWork += Backgroundworker_DoWork;
            BackgroundWorker.ProgressChanged += Backgroundworker_ProgressChanged;
        }

        private void Backgroundworker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressbarValue = e.ProgressPercentage;
        }

        private void Backgroundworker_DoWork(object sender, DoWorkEventArgs e)
        {
            var counter = 1;
            foreach (var file in FileList)
            {
                ResultList.SearchForWords(file, SearchWords);
                BackgroundWorker.ReportProgress(counter);
                counter++;
            }


        }


        // TODO:
        // - upload to github
        // - backgroundworker with notification
        // - enter search
        // - progressbar
        // - keine treffer notification
        // - install wizard

    }
}
