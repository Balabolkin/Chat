using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using WinRT.Interop;
using Windows.UI.Popups;
using System.Threading;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Chat
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>

    public interface IInitializeWithWindow
    {
        void Initialize(IntPtr hwnd);
    }
    public sealed partial class MainWindow : Window
    {
        private string username = "Guest";
        private int messNum = 0;
        public MainWindow()
        {
            this.InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += readMessages;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }
        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {

            SignInContentDialog signInDialog = new SignInContentDialog();
            signInDialog.XamlRoot = this.Content.XamlRoot;
            await signInDialog.ShowAsync();
            usernameBlock.Text = signInDialog.Result;
            if (usernameBlock.Text == "Guest")
                loginButton.Content = "Sign In";
            else
                loginButton.Content = "Log out";
            InvertedListView.Items.Clear();
            messNum = 0;
            username=usernameBlock.Text;
        }

        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter writer = new StreamWriter(@"D:\YandexDisk\VKI\System programming\Lomakin\chat.txt", true))
            {
                await writer.WriteLineAsync(username+'/'+DateTime.Now.ToString()+'/'+messageBox.Text);
                messageBox.Text = "";
            }
        }


        private async void readMessages(object sender, object e)
        {
            using (StreamReader sr = new StreamReader(@"D:\YandexDisk\VKI\System programming\Lomakin\chat.txt"))
            {
                string mess = await sr.ReadLineAsync();
                for (int i=0; i<messNum; i++)
                    mess = await sr.ReadLineAsync();
                while (mess != null)
                {
                    string user = mess.Substring(0, mess.IndexOf('/'));
                    string dt = mess.Substring(user.Length + 1, 19);
                    mess = mess.Substring(user.Length + 21);
                    if (user == username)
                        InvertedListView.Items.Add(new Message(user, mess, dt, HorizontalAlignment.Right));
                    else
                        InvertedListView.Items.Add(new Message(user, mess, dt, HorizontalAlignment.Left));
                    mess = await sr.ReadLineAsync();
                    messNum++;
                }
            }
        }
    }

    public class Message
    {
        public string MsgUser { get; private set; }
        public string MsgText { get; private set; }
        public string MsgDateTime { get; private set; }
        public HorizontalAlignment MsgAlignment { get; set; }
        public Message(string user ,string text, string dateTime, HorizontalAlignment align)
        {
            MsgUser = user;
            MsgText = text;
            MsgDateTime = dateTime;
            MsgAlignment = align;
        }
    }
}
