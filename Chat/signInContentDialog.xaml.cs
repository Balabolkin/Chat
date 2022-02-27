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
        public string Result { get; private set; }

        private void SignInCheck()
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source = BALABOLKIN-DESK\SQLEXPRESS; Initial Catalog = LOGIN; Integrated Security = True;"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT UserLogin from [LoginUser] WHERE (UserLogin = @login) AND (UserPassword = @pass)", connection);
                SqlParameter logPr = new SqlParameter("@login", userNameTextBox.Text);
                command.Parameters.Add(logPr);
                SqlParameter PassPr = new SqlParameter("@pass", passwordTextBox.Password);
                command.Parameters.Add(PassPr);
                SqlDataReader sqlReader = command.ExecuteReader();
                sqlReader.Read();
                if (sqlReader.HasRows == true)
                {
                    this.Result = sqlReader.GetString(0);
                }
                else
                {
                    FlyoutBase.SetAttachedFlyout(this, (FlyoutBase)this.Resources["SignUpFlyout"]);
                    FlyoutBase.ShowAttachedFlyout(this);
                    this.Result = "Guest";
                }
            }
        }

        public SignInContentDialog()
        {
            this.InitializeComponent();
        }
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Ensure the user name and password fields aren't empty. If a required field
            // is empty, set args.Cancel = true to keep the dialog open.
            if (string.IsNullOrEmpty(userNameTextBox.Text))
            {
                args.Cancel = true;
            }
            else if (string.IsNullOrEmpty(passwordTextBox.Password))
            {
                args.Cancel = true;
            }

            // If you're performing async operations in the button click handler,
            // get a deferral before you await the operation. Then, complete the
            // deferral when the async operation is complete.

            ContentDialogButtonClickDeferral deferral = args.GetDeferral();
            SignInCheck();
            deferral.Complete();
        }
        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Result = "Guest";
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
