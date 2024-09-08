using MusicPlayer.BLL.Services;
using MusicPlayer.DAL.Models;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TagLib;
using System.IO;
using System.Windows.Media.Animation;
using MaterialDesignThemes.Wpf;
using MahApps.Metro.IconPacks;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SongService _service = new SongService();
        private MediaPlayer _mediaPlayer = new MediaPlayer();
        private Brush _originalButtonColor;
        private DispatcherTimer _timer = new DispatcherTimer();
        private Song _currentSong;
        private bool _replay = false; // To track if replay is enabled
        private bool _isRandomPlay = false; // Flag to track if random play is active

        public Account UserAccount { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            _originalButtonColor = PlayPauseButton.Background;
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += UpdateProgressBar;
            _mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
        }

        //ĐỔ DATA KHI MỞ CỬA SỔ
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
            UserNameLable.Content = UserAccount.UserName + "'s Music Player";
        }

        //ĐỔ BÀI HÁT TỪ DATABASE
        public void FillDataGrid()
        {
            var previouslyPlayingSong = _currentSong;
            var allSongs = _service.GetAllSongs();
            SongDataGrid.ItemsSource = allSongs;

            if (previouslyPlayingSong != null && allSongs.Contains(previouslyPlayingSong))
            {
                // Highlight the currently playing song using the LoadingRow event
                SongDataGrid.SelectedItem = previouslyPlayingSong;
                SongDataGrid.ScrollIntoView(previouslyPlayingSong);
            }
        }

        //HÀM XÁC ĐỊNH KHI PHÁT HẾT BÀI HÁT SẼ PHÁT TIẾP NHƯ NÀO
        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            if (_isRandomPlay)
            {
                PlayRandomSong(); // Play a random song when the current song ends
            }
            else if (_replay)
            {
                ReplaySong(); // Replay the current song if replay is enabled
            }
            else
            {
                PlayNextSong(); // Play the next song if replay is not enabled and random play is off
            }
        }

        //TỰ ĐỘNG QUA BÀI KẾ TIẾP KHI PHÁT XONG BÀI
        private void PlayNextSong()
        {
            int currentIndex = SongDataGrid.SelectedIndex;
            if (currentIndex < SongDataGrid.Items.Count - 1)
                //phát bài kế tiếp xong khi end bài
                SongDataGrid.SelectedIndex = currentIndex + 1;
            else
                //bài cuối trong list thì loop lại bài đầu tiên
                SongDataGrid.SelectedIndex = 0;

            SongDataGrid.ScrollIntoView(SongDataGrid.SelectedItem);//cuộn bảng xuống vị trí bài hát tiếp theo đc chọn, đề phòng trường hợp 
            var nextSong = SongDataGrid.SelectedItem as Song;
            if (nextSong != null)
                PlaySong(nextSong);
        }

        //HIỂN THỊ PROGRESS CỦA BÀI HÁT VÀ THỜI GIAN 
        private void UpdateProgressBar(object sender, EventArgs e)
        {
            if (_mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                // Get the current position and total duration in seconds
                var currentPosition = _mediaPlayer.Position.TotalSeconds;
                var totalDuration = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;

                // Update the progress bar value
                p_bar.Value = (currentPosition / totalDuration) * 100;

                // Update the labels
                CurrentTimeLabel.Content = TimeSpan.FromSeconds(currentPosition).ToString(@"mm\:ss");
                TotalTimeLabel.Content = TimeSpan.FromSeconds(totalDuration).ToString(@"mm\:ss");
            }
        }


        //BẤM NÚT PLAY PAUSE
        private bool isPlaying = false;
        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            isPlaying = !isPlaying;
            var selectedSong = SongDataGrid.SelectedItem as Song;
            var playPauseButton = sender as Button;
            var icon = (PackIconMaterial)PlayPauseButton.Content;
            if (_currentSong != null)
            {
                if (isPlaying)
                {
                    icon.Kind = PackIconMaterialKind.Pause;
                    _mediaPlayer.Play();
                    _timer.Start();
                    CreateMusicBars(true);
                }
                else
                {
                    icon.Kind = PackIconMaterialKind.Play;
                    _mediaPlayer.Pause();
                    _timer.Stop();
                    CreateMusicBars(false);
                }
            }
            else
            {
                // ko có current song thì play bài đầu
                SongDataGrid.SelectedIndex = 0;
                _currentSong = SongDataGrid.SelectedItem as Song;

                if (_currentSong != null)
                {
                    PlaySong(_currentSong);
                }
            }
        }


        //HÀM PLAY
        private void PlaySong(Song selectedSong)
        {
            if (selectedSong == null)
                return;

            // If the selected song is already the current song and is playing, do nothing.
            if (_currentSong != null && _currentSong.SongId == selectedSong.SongId && isPlaying)
                return;

            _currentSong = selectedSong;

            var song = _service.GetSongById((int)selectedSong.SongId);

            _mediaPlayer.Open(new Uri(selectedSong.FilePath));
            _mediaPlayer.MediaFailed += (s, ev) => MessageBox.Show("Media Failed to Open");

            // Extract and display album art
            DisplayAlbumArt(selectedSong.FilePath);
            var icon = (PackIconMaterial)PlayPauseButton.Content;
            icon.Kind = PackIconMaterialKind.Pause;
            isPlaying = true;
            _mediaPlayer.Play();
            _timer.Start();
            CreateMusicBars(true);
            SongNameLabel.Content = selectedSong.SongName;
            ArtistNameLabel.Content = selectedSong.Artist;
        }


        //lấy ảnh có sẵn từ file mp3
        private void DisplayAlbumArt(string filePath)
        {
            try
            {
                var file = TagLib.File.Create(filePath);
                var tag = file.Tag;

                if (tag.Pictures.Length > 0)
                {
                    var bin = tag.Pictures[0].Data.Data;
                    using (var ms = new MemoryStream(bin))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = ms;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.EndInit();
                        song_img.Source = image;
                    }
                }
                else
                {
                    // ko có ảnh thì để ảnh default
                    var defaultImage = new BitmapImage(new Uri("/default_img.jpg", UriKind.Relative));
                    song_img.Source = defaultImage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load album art: " + ex.Message);

                // Fallback to default image in case of an exception
                var defaultImage = new BitmapImage(new Uri("/default_img.jpg", UriKind.Relative));
                song_img.Source = defaultImage;
            }
        }

        private Random _random = new Random();
        private List<Rectangle> _musicBars = new List<Rectangle>();
        private List<DoubleAnimation> _barAnimations = new List<DoubleAnimation>();
        private void CreateMusicBars(Boolean x)
        {
            foreach (var bar in _musicBars)
            {
                bar.BeginAnimation(Rectangle.HeightProperty, null);
                BarsCanvas.Children.Remove(bar);
            }
            _musicBars.Clear();
            _barAnimations.Clear();

            int numberOfBars = 31;
            double barWidth = 20;
            double barSpacing = 1;

            if (!x)
            {
                foreach (var bar in _musicBars)
                {
                    bar.BeginAnimation(Rectangle.HeightProperty, null); 
                    BarsCanvas.Children.Remove(bar);
                }
                _musicBars.Clear();
                _barAnimations.Clear();
                return;
            }

            _musicBars.Clear(); 
            _barAnimations.Clear();

            for (int i = 0; i < numberOfBars; i++)
            {
                Rectangle bar = new Rectangle
                {
                    Width = barWidth,
                    Height = _random.Next(20, 50),
                    Fill = new SolidColorBrush(Colors.Green),
                    Margin = new Thickness(i * (barWidth + barSpacing), 0, 0, 0),
                    VerticalAlignment = VerticalAlignment.Bottom
                };

                Canvas.SetBottom(bar, 0);

                BarsCanvas.Children.Add(bar);
                _musicBars.Add(bar);

                DoubleAnimation animation = new DoubleAnimation
                {
                    From = _random.Next(10, 20),
                    To = _random.Next(40, 50),
                    Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                    RepeatBehavior = RepeatBehavior.Forever,
                    AutoReverse = true,
                    SpeedRatio = _random.NextDouble() + 0.3
                };

                bar.BeginAnimation(Rectangle.HeightProperty, animation);
                _barAnimations.Add(animation);
            }
        }


        //BẤM NÚT NEXT
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongDataGrid.SelectedIndex < SongDataGrid.Items.Count - 1)
                SongDataGrid.SelectedIndex = SongDataGrid.SelectedIndex + 1;

            else
                SongDataGrid.SelectedIndex = 0;
        }

        //KHI BẤM VÀO 1 MỤC TRONG DATAGRID
        private void SongDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PlaySong(SongDataGrid.SelectedItem as Song);
        }


        //BẤM NÚT PREVIOUS
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongDataGrid.SelectedIndex > 0)
                SongDataGrid.SelectedIndex = SongDataGrid.SelectedIndex - 1;

            else
                SongDataGrid.SelectedIndex = SongDataGrid.Items.Count - 1;
        }

        //KHI BẤM VÀO 1 VỊ TRÍ TRONG PROGRESS BAR
        private void p_bar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                // lấy vị trí mà ng dùng ấn vào trong progress bar
                double positionClicked = e.GetPosition(p_bar).X;

                // tỷ lệ phần trăm vị trí ấn so với bài hát
                double ratio = positionClicked / p_bar.ActualWidth;

                // vị trí mới = tỷ lệ phần trăm * tổng thời lượng bài hát
                double newPositionInSeconds = _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds * ratio;

                // vị trí với của bài hát
                _mediaPlayer.Position = TimeSpan.FromSeconds(newPositionInSeconds);

                // Update lại giá trị progress bar
                p_bar.Value = ratio * 100;
            }
        }

        //ẤN NÚT SEARCH
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchValue = SearchTextBox.Text;

            if (searchValue == "Search song's name or artist")
            {
                // nếu là default text -> ko nhập -> full list và típ tục phát
                var allSongs = _service.GetAllSongs();
                // tắt SelectionChanged tạm để ko bị tắt nhạc
                SongDataGrid.SelectionChanged -= SongDataGrid_SelectionChanged;

                SongDataGrid.ItemsSource = null;
                SongDataGrid.ItemsSource = allSongs;

                // mở lại SelectionChanged
                SongDataGrid.SelectionChanged += SongDataGrid_SelectionChanged;

                return;//xong thì return luôn
            }

            if (_currentSong != null) // nếu có bài đang phát mà search j đó thì tắt nhạc reset ui
            {
                _mediaPlayer.Close();

                PlayPauseButton.Content = "Play ▶️";

                var defaultImage = new BitmapImage(new Uri("/default_img.jpg", UriKind.Relative));
                song_img.Source = defaultImage;
                p_bar.Value = 0;
                _timer.Stop();
                _currentSong = null;
            }

            SongDataGrid.SelectionChanged -= SongDataGrid_SelectionChanged;

            // hiện kết quả tìm kiếm
            var result = _service.SearchSongNameOrArtist(searchValue);
            SongDataGrid.ItemsSource = null;
            SongDataGrid.ItemsSource = result;

            SongDataGrid.SelectionChanged += SongDataGrid_SelectionChanged;
        }

        //hàm tự động xóa và hiện default text
        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "Search song's name or artist")
                SearchTextBox.Text = string.Empty;//xóa văn bản có sẵn khi người dùng ấn vào text box
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text)) // Nếu TextBox trống
                SearchTextBox.Text = "Search song's name or artist"; // Khôi phục văn bản mặc định
        }

        //CHỌN REPLAY
        private void ReplaySong()
        {
            var selectedSong = SongDataGrid.SelectedItem as Song; // Get the currently selected song
            if (selectedSong != null)
            {
                _mediaPlayer.Position = TimeSpan.Zero; // Reset the song position to the beginning
                _mediaPlayer.Play(); // Play the song again
            }
        }

        private void ReplayButton_Click(object sender, RoutedEventArgs e)
        {
            _replay = !_replay;
            if( _replay )
            {
                ReplayButton.Background = new SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                ReplayButton.Background = new SolidColorBrush(Colors.White);
            }
        }
            

        //CHỌN RANDOM
        private void PlayRandomSong()
        {
            if (SongDataGrid.Items.Count > 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(0, SongDataGrid.Items.Count);

                SongDataGrid.SelectedIndex = randomIndex;
                var selectedSong = SongDataGrid.SelectedItem as Song;

                if (selectedSong != null)
                {
                    PlaySong(selectedSong);
                }
            }
        }
        private void RandomPlayButton_Click(object sender, RoutedEventArgs e)
        {
            _isRandomPlay = !_isRandomPlay; // Activate random play mode
            if (_isRandomPlay) {
                PlayRandomSong(); // Play a random song immediately when checked
                RandomPlayButton.Background = new SolidColorBrush(Colors.LightGreen);
            } else
            {
                RandomPlayButton.Background = new SolidColorBrush(Colors.White);
            }
            }
        

        //ẤN NÚT DELETE
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Song selected = SongDataGrid.SelectedItem as Song;
            if (selected == null)
            {
                MessageBox.Show("Choose a song to delete", "Choose a song", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBoxResult answer = MessageBox.Show("Are you sure want to delete this song?", "Confirm?", MessageBoxButton.YesNo);
            if (answer == MessageBoxResult.No)
                return;

            _service.RemoveSong(selected);
            FillDataGrid();
        }

        //BẤM NUT IMPORT
        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            Song temp = _currentSong;
            DetailWindow d = new();
            d.ShowDialog();
            _currentSong = temp;
            PlaySong(_currentSong);
            FillDataGrid();
        }

        //BẤM NÚT UPDATE
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Song selected = SongDataGrid.SelectedItem as Song;
            if (selected == null)
            {
                MessageBox.Show("Please select a song before editing!", "Select?", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Song temp = _currentSong;
            DetailWindow detail = new();
            detail.EditSong = selected;
            detail.ShowDialog();
            PlaySong(temp);
            FillDataGrid();
        }


        //FAVOURATE CHECKBOX
        private void favouriteCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RemoveFavouriveButton.Visibility = Visibility.Visible;
            RemoveFavouriveButton.IsEnabled = true;

            // Save the currently playing song (if any)
            var previouslyPlayingSong = _currentSong;

            // Update ItemsSource with favorite songs
            var favoriteSongs = _service.GetSongsFromFavoritePlaylist(UserAccount.AccountId);
            SongDataGrid.ItemsSource = null;
            SongDataGrid.ItemsSource = favoriteSongs;

            if (previouslyPlayingSong != null && favoriteSongs.Contains(previouslyPlayingSong))
            {
                // Highlight the currently playing song using the LoadingRow event
                SongDataGrid.SelectedItem = previouslyPlayingSong;
                SongDataGrid.ScrollIntoView(previouslyPlayingSong);
            }
        }

        private void favouriteCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            RemoveFavouriveButton.IsEnabled = false;
            RemoveFavouriveButton.Visibility = Visibility.Hidden;
            SongDataGrid.SelectionChanged -= SongDataGrid_SelectionChanged;
            FillDataGrid();
            SongDataGrid.SelectionChanged += SongDataGrid_SelectionChanged;
        }

        private void AddFavouriteButton_Click(object sender, RoutedEventArgs e)
        {
            Song selected = SongDataGrid.SelectedItem as Song;
            if (selected == null)
            {
                MessageBox.Show("Choose a song to add to favourite", "Choose a song", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            List<Song> favList = _service.GetSongsFromFavoritePlaylist(UserAccount.AccountId);
            if (favList != null)
            {
                if (checkExist(selected, favList))
                {
                    MessageBox.Show("The song is already in your favourite list", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            // Add the selected song to the favorites
            _service.AddSongToFavourite(selected.SongId, UserAccount.AccountId);
            //inform
            MessageBox.Show("Add to favourite successfully", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
            // Select the song that was just added to favorites
            SongDataGrid.SelectedItem = selected;
            _currentSong = selected;
           
            // scroll to the selected song
            SongDataGrid.ScrollIntoView(selected);

            // Update the currently playing song if necessary
            if (_currentSong != null && _service.GetSongsFromFavoritePlaylist(UserAccount.AccountId).Contains(_currentSong))
            {
                PlaySong(_currentSong);
            }
        }

        public bool checkExist(Song song, List<Song> l)
        {
            foreach (Song x in l)
            {
                if (x.SongId == song.SongId)
                    return true;
            }
            return false;
        }

        private void RemoveFavouriveButton_Click(object sender, RoutedEventArgs e)
        {
            Song selected = SongDataGrid.SelectedItem as Song;
            if (selected == null)
            {
                MessageBox.Show("Choose a song to remove from favourite", "Choose a song", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            MessageBoxResult answer = MessageBox.Show("Are you sure want to remove this song from your favourite playlist?", "Confirm?", MessageBoxButton.YesNo);
            if (answer == MessageBoxResult.No)
                return;

            _service.RemoveSongFromPlaylist(selected.SongId, UserAccount.AccountId);
            _currentSong = selected;
            //f5 favourite playlist
            SongDataGrid.ItemsSource = null;
            SongDataGrid.ItemsSource = _service.GetSongsFromFavoritePlaylist(UserAccount.AccountId);            
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult answer = MessageBox.Show("Are you sure to exit?", "Confirm?", MessageBoxButton.YesNo);
            if (answer == MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Volume = VolumeSlider.Value;
            }
        }
    }
}
