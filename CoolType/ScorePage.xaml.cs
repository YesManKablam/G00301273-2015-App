using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.OneDrive;
using Microsoft.OneDrive.Sdk;
using System.Collections.Generic;
using Windows.UI.Xaml.Input;
using Windows.UI.Popups;
using Windows.Storage;
using System.IO;
using Windows.Storage.Pickers;
using System.Threading.Tasks;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CoolType
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScorePage : Page
    {
        IOneDriveClient myOneDrive;
        StorageFolder storageFolder;
        StorageFile sampleFile;
        String test = "test";

        private readonly string[] scopes =
                            new string[]
                            {
                                "onedrive.appfolder",
                                           "onedrive.readwrite",
                                           "wl.offline_access",
                                           "wl.signin",
                                           "wl.skydrive",
                                           "wl.skydrive_update",
                                           "onedrive.appfolder"
                            };

        public ScorePage()
        {
            this.InitializeComponent();

        }
        public Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private async void getScoreBtn_Click(object sender, RoutedEventArgs e)
        {
            storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            sampleFile = await storageFolder.GetFileAsync("sample.txt");
            scoreBlock.Text = "Score = " + await Windows.Storage.FileIO.ReadTextAsync(sampleFile);

            /*using (FileStream fs = new FileStream("./score.txt", FileMode.Open, FileAccess.ReadWrite))
            {
                StreamWriter tw = new StreamWriter(fs);
                tw.Write("AAAAAA");
                tw.Flush();
            }*/

         

            /*using (Stream s = GenerateStreamFromString("./score.txt"))
            {
                using (FileStream fs = new FileStream("./score.txt", FileMode.Open, FileAccess.ReadWrite))
                {
                    StreamWriter tw = new StreamWriter(fs);
                    tw.Write("AAAAAA");
                    tw.Flush();
                };
            }*/




        }

        private async void connectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (myOneDrive == null)
            {
                myOneDrive = OneDriveClientExtensions.GetUniversalClient(scopes);
                await myOneDrive.AuthenticateAsync();
            }

            var drive = await myOneDrive
                                .Drive
                                .Request()
                                .GetAsync();

            Stream stream = await sampleFile.OpenStreamForReadAsync();

          //  using (var fileStream = new FileStream("./score.txt", FileMode.Open, FileAccess.Read))
           // {
               

                var uploadedItem = await myOneDrive
                                             .Drive
                                             .Root
                                             .ItemWithPath(test + " Score.txt")
                                             .Content
                                             .Request()
                                             .PutAsync<Item>(stream);
           // }

            scoreBlock.Text = "Uploaded Score!";
        }

        private void playAgainBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TypingPage));
        }
    }
}
