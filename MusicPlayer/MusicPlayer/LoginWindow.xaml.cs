using MusicPlayer.BLL.Services;
using MusicPlayer.DAL.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private AccountService _accountService = new();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Account? acc = _accountService.Authenticate(UserNameTextBox.Text, PasswordTextBox.Password);
            if (acc == null)
            {
                MessageBox.Show("Invalid user name or password!", "Authenticate", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MainWindow m = new();
            m.UserAccount = acc;
            m.Show();
            this.Hide();
        }


        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Are you sure to exit?", "Confirm?", MessageBoxButton.YesNo);
            if (answer == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow m = new();
            m.Show();
        }

    }
}
