using Microsoft.IdentityModel.Tokens;
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
using Microsoft.Win32;
using System.IO;
using MusicPlayer.DAL.Models;
using MusicPlayer.BLL.Services;
using System.Security.Policy;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for DetailWindow.xaml
    /// </summary>
    public partial class DetailWindow : Window
    {
        public DetailWindow()
        {
            InitializeComponent();
        }

        private SongService _service = new();
        public Song EditSong { get; set; }

        private string _uploadFilePath;

        private bool CheckVar()
        {
            if (SongNameTextBox.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Song name must not be emptied!", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (SongNameTextBox.Text.Length > 40)
            {
                MessageBox.Show("Song name length must be <= 40!", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (ArtistTextBox.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Artist name must not be emptied", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (ArtistTextBox.Text.Length > 30)
            {
                MessageBox.Show("Artist name length must be <= 30", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (_uploadFilePath == null && EditSong == null)
            {
                MessageBox.Show("Please upload a file first", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (_service.checkExistSongName(SongNameTextBox.Text) && _service.checkExistSongArtist(ArtistTextBox.Text))
            {
                MessageBox.Show("Song is existed", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;

        }
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            // ở explorer để chọn music file
            OpenFileDialog ofd = new()
            {
                Filter = "Music Files (*.mp3;*.mp4)|*.mp3;*.mp4",
                Title = "Select a Music File"
            };

            if (ofd.ShowDialog() == true)
            {
                // lưu trữ file path đã chọn
                _uploadFilePath = ofd.FileName;
                // cập nhật UI uploadButton
                string fileName = System.IO.Path.GetFileNameWithoutExtension(_uploadFilePath);
                UploadButton.Content = fileName;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckVar())
            {
                return;
            }
            Song x = new();
            x.SongName = SongNameTextBox.Text;
            x.Artist = ArtistTextBox.Text;
            x.FilePath = _uploadFilePath;

            if (x.FilePath == null)
            {
                x.FilePath = FilePathTextBox.Text;
            }
            if (EditSong == null)
            {
                //x.SongId = int.Parse(SongIdTextBox.Text);
                _service.AddSong(x);
            }
            else
            {
                x.SongId = int.Parse(SongIdTextBox.Text);
                _service.UpdateSong(x);
            }
            MessageBox.Show("Saved successfully!", "Sucess", MessageBoxButton.OK);
            this.Close();
        }

        private void FillElements()
        {
            SongIdTextBox.Text = EditSong.SongId.ToString();
            SongIdTextBox.IsEnabled = false;
            SongNameTextBox.Text = EditSong.SongName;
            ArtistTextBox.Text = EditSong.Artist;
            UploadButton.Content = EditSong.SongName;
            UploadButton.Tag = EditSong.FilePath;
            FilePathTextBox.Text = EditSong.FilePath;   

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (EditSong != null)
            {
                FillElements();
                DetailWindowMode.Content = "Update Song";
            }
            else
            {
                SongIdTextBox.IsEnabled = false;
                DetailWindowMode.Content = "Add Song";
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
