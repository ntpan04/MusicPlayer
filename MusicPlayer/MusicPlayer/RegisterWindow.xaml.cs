using Microsoft.Identity.Client.NativeInterop;
using Microsoft.IdentityModel.Tokens;
using MusicPlayer.BLL.Services;
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
using MusicPlayer.DAL.Models;
namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private AccountService _service = new();
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private bool CheckVar()
        {
            if (UserNameTextBox.Text.IsNullOrEmpty())
            {
                MessageBox.Show("User name must not be emptied!", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (_service.GetAccountByUserName(UserNameTextBox.Text) != null)
            {
                MessageBox.Show("User name is already exist!", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (UserNameTextBox.Text.Length > 40)
            {
                MessageBox.Show("User name length must be <= 40", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (PasswordTextBox.Password.IsNullOrEmpty())
            {
                MessageBox.Show("Password must not be emptied!", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (PasswordTextBox.Password.Length > 40)
            {
                MessageBox.Show("Password length must be <= 40", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (ConfirmTextBox.Password.IsNullOrEmpty() || !ConfirmTextBox.Password.Equals(PasswordTextBox.Password))
            {
                MessageBox.Show("Confirm must match the password", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckVar() == false)
            {
                return;
            }
            string userName = UserNameTextBox.Text;
            string password = PasswordTextBox.Password;
            _service.AddAccount(userName, password);
            MessageBox.Show("Registed successfull!", "Success", MessageBoxButton.OK);
            this.Close();
        }

        private void LoginButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
