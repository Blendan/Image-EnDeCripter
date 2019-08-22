using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

namespace Image_en_deripter
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String imgUrl;
        private BitImage bitImage = new BitImage();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSelectImgSource_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
                imgUrl = openFileDialog.FileName;

            lblSelectImgSource.Content = imgUrl;

            bitImage.LoadImage(imgUrl);

            lblMaxBits.Content = bitImage.GetUsabelBitCount();
        }

        private void SaveImage()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "Unknown";
            dialog.Filter = "Image files (*.png)|*.png";
            if (dialog.ShowDialog() == true)
            {

                Bitmap bmp = bitImage.CreateBitmap(new TextRange(inputMessage.Document.ContentStart, inputMessage.Document.ContentEnd).Text);
                bmp.Save(dialog.FileName, ImageFormat.Png);
            }
        }

        private void BtnEncode_Click(object sender, RoutedEventArgs e)
        {
            if(imgUrl!=null)
            SaveImage();
        }

        private void InputMessage_KeyUp(object sender, KeyEventArgs e)
        {
            ShwoLeft();
        }

        private void BtnDecode_Click(object sender, RoutedEventArgs e)
        {
            if (imgUrl != null)
            {
                //inputMessage.Text = bitImage.ReadMessage();

                inputMessage.Document.Blocks.Clear();
                inputMessage.Document.Blocks.Add(new Paragraph(new Run(bitImage.ReadMessage())));

                ShwoLeft();
            }
        }

        private void ShwoLeft()
        {
            byte[] array = Encoding.UTF8.GetBytes(new TextRange(inputMessage.Document.ContentStart, inputMessage.Document.ContentEnd).Text);
            BitArray bits = new BitArray(array);

            int bitsL = bits.Length;
            bitsL = bitImage.GetUsabelBitCount()- bitsL;

            lblBitsUsed.Content = bitsL + " b || " + bitsL / 8 + " Letters";
        }
    }
}
