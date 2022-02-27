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
        public MainWindow()
        {
            this.InitializeComponent();
            
        }
        public enum SignInResult
        {
            SignInOK,
            SignInFail,
            SignInCancel,
            Nothing
        }
        public sealed partial class signInDialog : ContentDialog
        {
            public SignInResult Result { get; set; }

            private void ContentDialog_PrimaryButtonClick(object sender, RoutedEventArgs e)
            {
                this.Result = SignInResult.SignInOK;
            }

            private void ContentDialog_CloseButtonClick(object sender, RoutedEventArgs e)
            {
                this.Result = SignInResult.SignInCancel;
            }
        }
        private async void syncButton_Click(object sender, RoutedEventArgs e)
        {

            SignInContentDialog signInDialog = new SignInContentDialog();
            signInDialog.XamlRoot = this.Content.XamlRoot;
            await signInDialog.ShowAsync();
            usernameBlock.Text = signInDialog.Result;
        }

        //private async void syncButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        using (StreamReader sr = new StreamReader(@"D:\YandexDisk\VKI\System programming\Lomakin\chat.txt"))
        //        {
        //            chatBox.Text = await sr.ReadToEndAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ContentDialog errDialog = new ContentDialog()
        //        {
        //            Title = "Error",
        //            Content = ex.ToString(),
        //            PrimaryButtonText = "Ok",
        //        };
        //        errDialog.XamlRoot = this.Content.XamlRoot;
        //        await errDialog.ShowAsync();

        //    }
        //}
    }
}
