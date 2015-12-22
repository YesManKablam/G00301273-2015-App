using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CoolType
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TypingPage : Page
    {
        public string[] separator = { " ", "\t", "\n", "\r", "\r\n" };
        public string[] arr = new string[] { };
        List<string> myCollection = new List<string>();
        public int i = 0;
        int score = 0;
        StorageFile file;
        String curWord = "";

        public TypingPage()
        {
            this.InitializeComponent();
        }

        private async void listBtn_Click(object sender, RoutedEventArgs e)
        {
            listBlock.Text = "";

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add(".txt");
            file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                listBlock.Text = "Loaded File";
                listBtn.Visibility = Visibility.Collapsed;
                boxBtn.Visibility = Visibility.Visible;
                listBox.Visibility = Visibility.Visible;
            }
            else
            {
                listBlock.Text = "Operation Canceled";
            }

        }

        private async void boxBtn_Click(object sender, RoutedEventArgs e)
        {
            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                using (StreamReader reader = new StreamReader(stream.AsStream()))
                {
                    string[] words = reader.ReadToEnd().Split(' ', '\t', '\n', '\r', '\r');

                    if (listBox.Text == curWord)
                    {
                        test.Text = "Correct";
                        score += 1;
                    }

                    listBox.Text = "";
                    boxBtn.Content = "Check Word";

                    if (words.Length > i)
                    {
                        listBlock.Text = words[i]; //+ " " + words[j];
                        curWord = words[i];
                        int findThis = words.Length;
                        i++;
                    }
                    else
                    {
                        Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                        Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync("sample.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);

                        await Windows.Storage.FileIO.WriteTextAsync(sampleFile, score.ToString());
                        this.Frame.Navigate(typeof(ScorePage));
                    }

                }
            }
        }
    }
}
