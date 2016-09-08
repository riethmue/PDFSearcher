using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace PDFSearcher
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon NotifyIcon = null;
        public MainWindow()
        {
            InitializeComponent();
            // OnLoaded.
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Hide the main window.
            Visibility = Visibility.Hidden;

            // Show the tray icon.
            NotifyIcon.Visible = true;

        }

        protected override void OnInitialized(EventArgs e)
        {
            // Tray icon.
            NotifyIcon = new System.Windows.Forms.NotifyIcon();
            NotifyIcon.Click += NotifyIcon_Click;
            NotifyIcon.Icon = new System.Drawing.Icon("../../adobe.ico");

            base.OnInitialized(e);
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            var notifyIconContextMenu = (ContextMenu)FindResource("NotifyIconContextMenu");
            notifyIconContextMenu.IsOpen = true;
        }

        public void Menu_Close(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void Library_Search(object sender, EventArgs e)
        {
            // Open library search window.
            var librarySearchWindow = new LibrarySearchWindow();
            librarySearchWindow.Show();

        }

    }
}
