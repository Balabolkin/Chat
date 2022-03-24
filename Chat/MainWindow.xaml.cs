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
using Windows.UI;
using System.Data.SqlClient;
using System.Data;

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
        string strConn = Classes.connection.strConn;
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
            await using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Message VALUES (@send, @rec, @mess, @date)", connection);
                SqlParameter logPr = new SqlParameter("@send", username);
                command.Parameters.Add(logPr);
                logPr = new SqlParameter("@rec", "Guest");
                command.Parameters.Add(logPr);
                logPr = new SqlParameter("@mess", messageBox.Text);
                command.Parameters.Add(logPr);
                logPr = new SqlParameter("@date", DateTime.Now);
                command.Parameters.Add(logPr);
                command.ExecuteNonQuery();
            }
            messageBox.Text="";
        }


        private async void readMessages(object sender, object e)
        {
            await using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT SndUser, Message, MsgDataTime from Message", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                while (messNum!=table.Rows.Count)
                {
                    DataRow dataRow = table.Rows[messNum];
                    messNum++;
                    if (dataRow[0].ToString() == username)
                    {
                        var col = new SolidColorBrush(Colors.Wheat);
                        InvertedListView.Items.Add(new Message(dataRow[0].ToString(), dataRow[1].ToString(), dataRow[2].ToString(), HorizontalAlignment.Right, col));
                    }
                    else
                    {
                        var col = new SolidColorBrush(Color.FromArgb(255, 194, 175, 141));
                        InvertedListView.Items.Add(new Message(dataRow[0].ToString(), dataRow[1].ToString(), dataRow[2].ToString(), HorizontalAlignment.Left, col));
                    }
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
        public SolidColorBrush MsgColor { get; private set; }
        public Message(string user ,string text, string dateTime, HorizontalAlignment align, SolidColorBrush color)
        {
            MsgUser = user;
            MsgText = text;
            MsgDateTime = dateTime;
            MsgAlignment = align;
            MsgColor = color;
        }
    }
}
