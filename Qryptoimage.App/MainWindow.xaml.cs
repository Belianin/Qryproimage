using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Brushes = System.Windows.Media.Brushes;
using Image = System.Drawing.Image;

namespace Qryptoimage.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bitmap bitmap;
        
        private const string FileFilter = "Image Files|*.bmp;*.png;*.jpg)";
        
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = FileFilter
            };
            
            if (openFileDialog.ShowDialog() == true)
            {
                bitmap = (Bitmap) Image.FromFile(openFileDialog.FileName);
                MainImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));

                WatermarkLabel.Text = LSB.CheckWatermark(bitmap) 
                    ? "✔ Image may has encrypted text"
                    : "❌ Image has no encrypted text";

                EncryptButton.IsEnabled = true;
                DecryptButton.IsEnabled = true;
                SaveButton.IsEnabled = false;
            }
        }

        private void Decrypt(object sender, RoutedEventArgs routedEventArgs)
        {
            
            if (bitmap != null)
            {
                TextInput.Text = LSB.Decode(bitmap);
            }
            //var text = Crypter.Encrypt("hello", Guid.Parse("7ab9bc21-227c-4207-86bc-32f6e95388fe"));

            //TextInput.Text = Crypter.Decrypt(text, Guid.Parse("7ab9bc21-227c-4207-86bc-32f6e95388fe")); //7ab9bc21-227c-4207-86bc-32f6e95388fe
        }

        private void GenerateKey(object sender, RoutedEventArgs e)
        {
            KeyInput.Text = Guid.NewGuid().ToString();
        }
        
        public BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        private void Encrypt(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(KeyInput.Text))
            {
                Encode(KeyInput.Text);
                
                StatusLabel.Content = "Encrypted without key";
            }
            else if (Guid.TryParse(KeyInput.Text.Trim(), out var guid))
            {
                var text = Crypter.Encrypt(KeyInput.Text, guid);
                
                Encode(text);
                
                StatusLabel.Content = "Encrypted with given key";
            }
            else
            {
                StatusLabel.Content = "Invalid key";
            }
        }

        private void Encode(string text)
        {
            LSB.Encode(bitmap, text);
            LSB.SetWatermark(bitmap);
                
            MainImage.Source = Convert(bitmap);
            SaveButton.IsEnabled = true;
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = FileFilter
            };;
            if(saveFileDialog.ShowDialog() == true)
                bitmap.Save(saveFileDialog.FileName, ImageFormat.Png);
        }
    }
}