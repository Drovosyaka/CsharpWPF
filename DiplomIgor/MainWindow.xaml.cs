using DiplomIgor.UserControls;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace DiplomIgor
{
    public partial class MainWindow : Window
    {

        private DispatcherTimer timer = new DispatcherTimer();
        private int _isPlaying = 1;
        private int _isMuting = 1;
        private int _songNumber = 0;
        private int _currentCheck = 0;
        private int _mediaElement = 0;
        private int _timeDetector = 0;
        private int _timeDetectorNext = 0;
        private int _mediaDetector = 0;
        private int _mediaDetectorNext = 0;
        private DispatcherTimer _timer;
        string[] files, path;
        SongItem[] song = new SongItem[10];
        SongItem songNext = new SongItem { };
        MediaElement[] me = new MediaElement[10];

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimeDisplay();

            // Создаем таймер, который будет вызывать функцию каждую минуту
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMinutes(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void StopPlayButton(object sender, RoutedEventArgs e)
        {
            if (_isPlaying == 1)
            {
                Play.Visibility = Visibility.Collapsed;
                Pause.Visibility = Visibility.Visible;
                if (TrackList.SelectedItem != null)
                {
                    string selectedFilePath = TrackList.SelectedItem.ToString();
                    MyMediaElement.Source = new Uri("C:\\Users\\grifk\\source\\repos\\DiplomIgor\\DiplomIgor\\Music\\" + selectedFilePath + ".mp3");
                    MyMediaElement.Play();
                }
                _isPlaying = 0;
            }
            else
            {
                Play.Visibility = Visibility.Visible;
                Pause.Visibility = Visibility.Collapsed;
                if (TrackList.SelectedItem != null)
                {
                    string selectedFilePath = TrackList.SelectedItem.ToString();
                    MyMediaElement.Source = new Uri("C:\\Users\\grifk\\source\\repos\\DiplomIgor\\DiplomIgor\\Music\\" + selectedFilePath + ".mp3");
                    MyMediaElement.Pause();
                }
                _isPlaying = 1;
            }
        }
        private void OpenButton(object sender, RoutedEventArgs e)
        {
            #region MainStyleSelected
            Grid2Category.Visibility = Visibility.Collapsed;
            Grid2Main.Visibility = Visibility.Collapsed;
            Grid2Music.Visibility = Visibility.Visible;

            /*SelectedPlaylist.Visibility = Visibility.Visible;
            MainPlaylist.Background = new SolidColorBrush(Color.FromRgb(2, 190, 104));

            MainMain.Background = Brushes.White;
            SelectedMain.Visibility = Visibility.Collapsed;

            MainCategory.Background = Brushes.White;
            SelectedCategory.Visibility = Visibility.Collapsed;*/
            #endregion
/*            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == true)
            {
                files = ofd.FileNames;
                path = ofd.FileNames;
                for (int x = 0; x < files.Length; x++)
                    TrackList.Items.Add(files[x]);
            }*/
            #region MusicList
            if (files == null)
            {

                string path = @"C:\Users\grifk\source\repos\DiplomIgor\DiplomIgor\Music";
                string[] files = Directory.GetFiles(path, "*.mp3"); // Получаем список файлов в папке Music с расширением .mp3

                foreach (string filePath in files)
                {
                    // Используем регулярное выражение для поиска названия песни и автора
                    Match match1 = Regex.Match(filePath, @"Music\\(.*)\.mp3$");
                    if (match1.Success)
                    {
                        // Убираем путь до файла и расширение файла
                        string title = match1.Groups[1].Value;
                        TrackList.Items.Add($"{title}"); // Добавляем в список название песни и автора
                    }
                }
            }
            #endregion
        }

        private void TrackListt(object sender, MouseButtonEventArgs e)
        {
            if (TrackList.SelectedItem != null)
            {
                string selectedFilePath = TrackList.SelectedItem.ToString();
                MyMediaElement.Source = new Uri("C:\\Users\\grifk\\source\\repos\\DiplomIgor\\DiplomIgor\\Music\\" + selectedFilePath + ".mp3");
                MyMediaElement.Play();
                TitleTrack.Text = selectedFilePath;
            }
        }

        private void SkipNext(object sender, RoutedEventArgs e)
        {
            if (TrackList.SelectedIndex < TrackList.Items.Count - 1)
            {
                TrackList.SelectedIndex = TrackList.SelectedIndex + 1;
            }
        }

        private void SkipPrevious(object sender, RoutedEventArgs e)
        {
            if (TrackList.SelectedIndex > 0)
            {
                TrackList.SelectedIndex = TrackList.SelectedIndex - 1;
            }
        }

        private void MuteUnmute(object sender, RoutedEventArgs e)
        {
            if (_isMuting == 1)
            {
                Mute.Visibility = Visibility.Visible;
                Unmute.Visibility = Visibility.Collapsed;
                MyMediaElement.IsMuted = !MyMediaElement.IsMuted;
                _isMuting = 0;
            }
            else
            {
                Mute.Visibility = Visibility.Collapsed;
                Unmute.Visibility = Visibility.Visible;
                MyMediaElement.IsMuted = false;
                _isMuting = 1;
            }
        }

        private void MainButton(object sender, RoutedEventArgs e)
        {
            #region MainStyleSelected
            Grid2Category.Visibility = Visibility.Collapsed;
            Grid2Main.Visibility = Visibility.Visible;
            Grid2Music.Visibility = Visibility.Collapsed;

            /*SelectedPlaylist.Visibility = Visibility.Collapsed;
            MainPlaylist.Background = Brushes.White;

            SelectedMain.Visibility = Visibility.Visible;
            MainMain.Background = new SolidColorBrush(Color.FromRgb(2, 190, 104));
            
            MainCategory.Background = Brushes.White;
            SelectedCategory.Visibility = Visibility.Collapsed;*/
            #endregion
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MyMediaElement.Position = TimeSpan.FromSeconds(e.NewValue);
        }

        private void OpenCategory(object sender, RoutedEventArgs e)
        {
            Grid2Category.Visibility = Visibility.Visible;
            Grid2Main.Visibility = Visibility.Visible;
            Grid2Music.Visibility = Visibility.Collapsed;
        }

        private void InitializeTimeDisplay()
        {
            // Настройка таймера для обновления каждые 1000 мс (1 секунда)
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            /*string selectedFilePath = TrackList.SelectedItem.ToString();*/
            // Обновление текста TextBlock с текущим временем
            var currentTime = DateTime.Now.ToString("HH:mm");
            TimeDisplay.Text = currentTime;
            if (true)
            {
                await Task.Delay(15000);
                PlayMusic();
            }
        }

        private void SetTimeButton_Click1(object sender, MouseButtonEventArgs e)
        {
            if (TimeWindow1.Visibility != Visibility.Visible)
            {
                TimeWindow1.Visibility = Visibility.Visible;
                TimeWindow2.Visibility = Visibility.Collapsed;
                TimeWindow3.Visibility = Visibility.Collapsed;
                TimeWindow4.Visibility = Visibility.Collapsed;
                TimeWindow5.Visibility = Visibility.Collapsed;
                TimeWindow6.Visibility = Visibility.Collapsed;
            }
            else
            {
                _currentCheck = 1;
                int hours, minutes;

                if (int.TryParse(HoursTextBox1.Text, out hours) &&
                    int.TryParse(MinutesTextBox1.Text, out minutes))
                {
                    if (hours >= 0 && hours < 24 &&
                        minutes >= 0 && minutes < 60)
                    {
                        var time = new TimeSpan(hours, minutes, 0);
                        TimeTextBlock1.Text = time.ToString(@"hh\:mm");
                        if (TimeTextBlock1 != null)
                        {
                            ButtonUpload.Visibility = Visibility.Visible;
                        } else
                        {
                            ButtonUpload.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный формат, повторите попытку.");
                    }
                }
                else
                {
                    MessageBox.Show("Неверный формат, повторите попытку.");
                }
            }
        }

        private void SetTimeButton_Click2(object sender, MouseButtonEventArgs e)
        {
            if (TimeWindow2.Visibility != Visibility.Visible)
            {
                TimeWindow1.Visibility = Visibility.Collapsed;
                TimeWindow2.Visibility = Visibility.Visible;
                TimeWindow3.Visibility = Visibility.Collapsed;
                TimeWindow4.Visibility = Visibility.Collapsed;
                TimeWindow5.Visibility = Visibility.Collapsed;
                TimeWindow6.Visibility = Visibility.Collapsed;
            }
            else
            {
                _currentCheck = 2;
                int hours, minutes;

                if (int.TryParse(HoursTextBox2.Text, out hours) &&
                    int.TryParse(MinutesTextBox2.Text, out minutes))
                {
                    if (hours >= 0 && hours < 24 &&
                        minutes >= 0 && minutes < 60)
                    {
                        var time = new TimeSpan(hours, minutes, 0);
                        TimeTextBlock2.Text = time.ToString(@"hh\:mm");

                        if (TimeTextBlock2 != null)
                        {
                            ButtonUpload.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            ButtonUpload.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный формат, повторите попытку.");
                    }
                }
                else
                {
                    MessageBox.Show("Неверный формат, повторите попытку.");
                }
            }
        }

        private void SetTimeButton_Click3(object sender, MouseButtonEventArgs e)
        {
            if (TimeWindow3.Visibility != Visibility.Visible)
            {
                TimeWindow1.Visibility = Visibility.Collapsed;
                TimeWindow2.Visibility = Visibility.Collapsed;
                TimeWindow3.Visibility = Visibility.Visible;
                TimeWindow4.Visibility = Visibility.Collapsed;
                TimeWindow5.Visibility = Visibility.Collapsed;
                TimeWindow6.Visibility = Visibility.Collapsed;
            }
            else
            {
                _currentCheck = 3;
                int hours, minutes;

                if (int.TryParse(HoursTextBox3.Text, out hours) &&
                    int.TryParse(MinutesTextBox3.Text, out minutes))
                {
                    if (hours >= 0 && hours < 24 &&
                        minutes >= 0 && minutes < 60)
                    {
                        var time = new TimeSpan(hours, minutes, 0);
                        TimeTextBlock3.Text = time.ToString(@"hh\:mm");

                        if (TimeTextBlock3 != null)
                        {
                            ButtonUpload.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            ButtonUpload.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный формат, повторите попытку.");
                    }
                }
                else
                {
                    MessageBox.Show("Неверный формат, повторите попытку.");
                }
            }
        }

        private void SetTimeButton_Click4(object sender, MouseButtonEventArgs e)
        {
            if (TimeWindow4.Visibility != Visibility.Visible)
            {
                TimeWindow1.Visibility = Visibility.Collapsed;
                TimeWindow2.Visibility = Visibility.Collapsed;
                TimeWindow3.Visibility = Visibility.Collapsed;
                TimeWindow4.Visibility = Visibility.Visible;
                TimeWindow5.Visibility = Visibility.Collapsed;
                TimeWindow6.Visibility = Visibility.Collapsed;
            }
            else
            {
                _currentCheck = 4;
                int hours, minutes;

                if (int.TryParse(HoursTextBox4.Text, out hours) &&
                    int.TryParse(MinutesTextBox4.Text, out minutes))
                {
                    if (hours >= 0 && hours < 24 &&
                        minutes >= 0 && minutes < 60)
                    {
                        var time = new TimeSpan(hours, minutes, 0);
                        TimeTextBlock4.Text = time.ToString(@"hh\:mm");

                        if (TimeTextBlock4 != null)
                        {
                            ButtonUpload.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            ButtonUpload.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный формат, повторите попытку.");
                    }
                }
                else
                {
                    MessageBox.Show("Неверный формат, повторите попытку.");
                }
            }
        }

        private void SetTimeButton_Click5(object sender, MouseButtonEventArgs e)
        {
            if (TimeWindow5.Visibility != Visibility.Visible)
            {
                TimeWindow1.Visibility = Visibility.Collapsed;
                TimeWindow2.Visibility = Visibility.Collapsed;
                TimeWindow3.Visibility = Visibility.Collapsed;
                TimeWindow4.Visibility = Visibility.Collapsed;
                TimeWindow5.Visibility = Visibility.Visible;
                TimeWindow6.Visibility = Visibility.Collapsed;
            }
            else
            {
                _currentCheck = 5;
                int hours, minutes;

                if (int.TryParse(HoursTextBox5.Text, out hours) &&
                    int.TryParse(MinutesTextBox5.Text, out minutes))
                {
                    if (hours >= 0 && hours < 24 &&
                        minutes >= 0 && minutes < 60)
                    {
                        var time = new TimeSpan(hours, minutes, 0);
                        TimeTextBlock5.Text = time.ToString(@"hh\:mm");

                        if (TimeTextBlock5 != null)
                        {
                            ButtonUpload.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            ButtonUpload.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный формат, повторите попытку.");
                    }
                }
                else
                {
                    MessageBox.Show("Неверный формат, повторите попытку.");
                }
            }
        }

        private void SetTimeButton_Click6(object sender, MouseButtonEventArgs e)
        {
            if (TimeWindow6.Visibility != Visibility.Visible)
            {
                TimeWindow1.Visibility = Visibility.Collapsed;
                TimeWindow2.Visibility = Visibility.Collapsed;
                TimeWindow3.Visibility = Visibility.Collapsed;
                TimeWindow4.Visibility = Visibility.Collapsed;
                TimeWindow5.Visibility = Visibility.Collapsed;
                TimeWindow6.Visibility = Visibility.Visible;
            }
            else
            {
                _currentCheck = 6;
                int hours, minutes;

                if (int.TryParse(HoursTextBox6.Text, out hours) &&
                    int.TryParse(MinutesTextBox6.Text, out minutes))
                {
                    if (hours >= 0 && hours < 24 &&
                        minutes >= 0 && minutes < 60)
                    {
                        var time = new TimeSpan(hours, minutes, 0);
                        TimeTextBlock6.Text = time.ToString(@"hh\:mm");

                        if (TimeTextBlock6 != null)
                        {
                            ButtonUpload.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            ButtonUpload.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный формат, повторите попытку.");
                    }
                }
                else
                {
                    MessageBox.Show("Неверный формат, повторите попытку.");
                }
            }
        }

        private void AddCat1(object sender, RoutedEventArgs e)
        {
            #region Проигрыватель музыки

            if (_mediaDetector < me.Length)
            {
                string selectedFilePath = TrackList.SelectedItem.ToString();
                me[_mediaDetector] = new MediaElement { Name = "id" + _mediaElement++.ToString(), LoadedBehavior = MediaState.Manual };
                SongStackPanel.Children.Add(me[_mediaDetector]);
                me[_mediaDetector].Source = new Uri("C:\\Users\\grifk\\source\\repos\\DiplomIgor\\DiplomIgor\\Music\\" + selectedFilePath + ".mp3");
                _mediaDetector++;
            }
            #endregion

            #region Список направлений по времени
            string timeText = "";
            if (_currentCheck == 1)
            {
                // Получаем текст из TimeTextBlock
                timeText = TimeTextBlock1.Text;
                // Создаем новый экземпляр SongItem
                if (_timeDetector < song.Length)
                {
                    song[_timeDetector] = new SongItem
                    {
                        Name = "id" + _songNumber++.ToString(),
                        Number = _songNumber.ToString(),
                        Title = "Лекцион. день",
                        Time = timeText,
                        IsActive = false
                    };
                    SongStackPanel.Children.Add(song[_timeDetector]);
                    _timeDetector++;
                }
            } else if (_currentCheck == 2)
            {
                // Получаем текст из TimeTextBlock
                timeText = TimeTextBlock2.Text;
                // Создаем новый экземпляр SongItem
                if (_timeDetector < song.Length)
                {
                    song[_timeDetector] = new SongItem
                    {
                        Name = "id" + _songNumber++.ToString(),
                        Number = _songNumber.ToString(),
                        Title = "Рабочий день",
                        Time = timeText,
                        IsActive = false
                    };
                    SongStackPanel.Children.Add(song[_timeDetector]);
                    _timeDetector++;
                }
            } else if ( _currentCheck == 3)
            {
                // Получаем текст из TimeTextBlock
                timeText = TimeTextBlock3.Text;
                // Создаем новый экземпляр SongItem
                if (_timeDetector < song.Length)
                {
                    song[_timeDetector] = new SongItem
                    {
                        Name = "id" + _songNumber++.ToString(),
                        Number = _songNumber.ToString(),
                        Title = "Пункт выдачи",
                        Time = timeText,
                        IsActive = false
                    };
                    SongStackPanel.Children.Add(song[_timeDetector]);
                    _timeDetector++;
                }
            } else if (_currentCheck == 4)
            {
                // Получаем текст из TimeTextBlock
                timeText = TimeTextBlock4.Text;
                // Создаем новый экземпляр SongItem
                if (_timeDetector < song.Length)
                {
                    song[_timeDetector] = new SongItem
                    {
                        Name = "id" + _songNumber++.ToString(),
                        Number = _songNumber.ToString(),
                        Title = "Реклама",
                        Time = timeText,
                        IsActive = false
                    };
                    SongStackPanel.Children.Add(song[_timeDetector]);
                    _timeDetector++;
                }
            } else if (_currentCheck == 5)
            {
                // Получаем текст из TimeTextBlock
                timeText = TimeTextBlock5.Text;
                // Создаем новый экземпляр SongItem
                if (_timeDetector < song.Length)
                {
                    song[_timeDetector] = new SongItem
                    {
                        Name = "id" + _songNumber++.ToString(),
                        Number = _songNumber.ToString(),
                        Title = "Перерыв",
                        Time = timeText,
                        IsActive = false
                    };
                    SongStackPanel.Children.Add(song[_timeDetector]);
                    _timeDetector++;
                }
            } else
            {
                // Получаем текст из TimeTextBlock
                timeText = TimeTextBlock6.Text;
                // Создаем новый экземпляр SongItem
                if (_timeDetector < song.Length)
                {
                    song[_timeDetector] = new SongItem
                    {
                        Name = "id" + _songNumber++.ToString(),
                        Number = _songNumber.ToString(),
                        Title = "Собеседование",
                        Time = timeText,
                        IsActive = false
                    };
                    SongStackPanel.Children.Add(song[_timeDetector]);
                    _timeDetector++;
                }
            }

            ButtonUpload.Visibility = Visibility.Collapsed;
            #endregion
        }

        private async void PlayMusic()
        {
            if (_timeDetector > _timeDetectorNext)
            {
                if (song[_timeDetectorNext].Time == TimeDisplay.Text)
                {
                    if (_mediaDetector > _mediaDetectorNext)
                    {
                        song[_timeDetectorNext].IsActive = true;
                        if (song[_timeDetectorNext] != null)
                        {
                            if(_timeDetectorNext > 0)
                            {
                                song[_timeDetectorNext - 1].IsActive = false;
                                me[_mediaDetectorNext - 1].Pause();
                            }
                        }
                        me[_mediaDetectorNext].Play();
                        _mediaDetectorNext++;
                        _timeDetectorNext++;
                    }
                }
            }
        }
    }
}