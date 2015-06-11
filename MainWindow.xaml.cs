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
        #region Design Specific
        public MainWindow()
        {
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


        private async void ButtonGetAsClick(object sender, RoutedEventArgs e)
        {
            PhantomJs.FileType fileType = PhantomJs.FileType.Pdf;

            if (_radioButtonGif.IsChecked == true)
            {
                fileType = PhantomJs.FileType.Gif;
            }
            else if (_radioButtonJpeg.IsChecked == true)
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

        private void ButtonDonateClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=mail%2esomeone%40gmail%2ecom&lc=FR&item_name=I%2dTechnology%2eNET&currency_code=EUR&bn=PP%2dDonationsBF%3abtn_donate_SM%2egif%3aNonHosted");
        }
    }
}
