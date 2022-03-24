using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Chat
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignInContentDialog : ContentDialog
    {
        string strConn = Classes.connection.strConn;
        public string Result { get; private set; }

        private async void SignInCheck()
        {
            
            if (string.IsNullOrEmpty(userNameTextBox.Text))
            {
                userNameTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                Flyout.ShowAttachedFlyout(userNameTextBox);
            }
            if (string.IsNullOrEmpty(passwordTextBox.Password))
            {
                passwordTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                Flyout.ShowAttachedFlyout(passwordTextBox);
            }
            if ((!string.IsNullOrEmpty(userNameTextBox.Text))&&(!string.IsNullOrEmpty(passwordTextBox.Password)))
            {
                await using (SqlConnection connection = new SqlConnection(strConn))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT UserLogin, UserPassword from [LoginUser] WHERE (UserLogin = @login)", connection);
                    SqlParameter logPr = new SqlParameter("@login", userNameTextBox.Text);
                    command.Parameters.Add(logPr);
                    SqlDataReader sqlReader = command.ExecuteReader();
                    sqlReader.Read();
                    if ((sqlReader.HasRows) && (sqlReader.GetString(1) == passwordTextBox.Password))
                    {
                        this.Result = sqlReader.GetString(0);

                        this.Hide();
                    }
                    else
                        Flyout.ShowAttachedFlyout(loginButton);
                }
            }
        }

        public SignInContentDialog()
        {
            this.InitializeComponent();
        }
        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            SignInCheck();
        }
        private void GuestButtonClick(object sender, RoutedEventArgs e)
        {
            this.Result = "Guest";
            this.Hide();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(strConn))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO [LoginUser] (UserLogin, UserPassword) VALUES  (@login, @pass)", connection);
                SqlParameter logPr = new SqlParameter("@login", userNameTextBox.Text);
                command.Parameters.Add(logPr);
                SqlParameter PassPr = new SqlParameter("@pass", passwordTextBox.Password);
                command.Parameters.Add(PassPr);
                command.ExecuteNonQuery();
            }
            this.Result = userNameTextBox.Text;
            this.Hide();
        }
    }
}
