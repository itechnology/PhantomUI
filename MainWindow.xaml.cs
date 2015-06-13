using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace PhantomUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Init
        public string ApplicationDirectory { get; set; }

        public MainWindow()
        {
            //To get the location the assembly normally resides on disk or the install directory
            string basePath = Assembly.GetExecutingAssembly().Location;
            string baseDir  = Path.GetDirectoryName(basePath);

            ApplicationDirectory = baseDir;
            
            InitializeComponent();            
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {            
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void ButtonExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        private async void ButtonRunClick(object sender, RoutedEventArgs e)
        {
            browseIcon.Icon       = FontAwesome.WPF.FontAwesomeIcon.Spinner;
            browseIcon.Spin       = true;
            _buttonRun.Visibility = Visibility.Hidden;

            PhantomJs.FileType fileType = PhantomJs.FileType.Pdf;

            if (_radioButtonJpeg.IsChecked == true)
            {
                fileType = PhantomJs.FileType.Jpeg;
            }
            else if (_radioButtonPng.IsChecked == true)
            {
                fileType = PhantomJs.FileType.Png;
            }

            var phantom = new PhantomJs();
            var result  = await phantom.GetPdfTask(_textBoxUrl.Text, fileType);

            if (File.Exists(result))
            {
                Process.Start(result);
            }
            else
            {
                MessageBox.Show("Something went wrong ...", "Oops !", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            browseIcon.Icon       = FontAwesome.WPF.FontAwesomeIcon.Folder;
            browseIcon.Spin       = false;
            _buttonRun.Visibility = Visibility.Visible;
        }

        private void ButtonBrowseClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = @"Html Files (.html)|*.html|Htm Files (*.htm)|*.htm"
            };

            var result = dialog.ShowDialog();
            if (result == true)
            {
                _textBoxUrl.Text = dialog.FileName;
            }
        }

        private void ButtonGitHubClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/itechnology/PhantomUI");
        }

        private void ScriptButtonClick(object sender, RoutedEventArgs e)
        {
            Process.Start(string.Format(@"{0}\lib\scripts\run.js", ApplicationDirectory));
        }

        private void ConfigButtonClick(object sender, RoutedEventArgs e)
        {
            Process.Start(string.Format(@"{0}\lib\config.json", ApplicationDirectory));
        }
    }
}
