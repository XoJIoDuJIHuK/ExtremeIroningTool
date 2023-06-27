using ExtremeIroningTool.MVVM.Views;
using ExtremeIroningTool.Utilitary_classes;
using ExtremeIroningTool.Utilitary_classes.DataBaseClasses;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace ExtremeIroningTool.MVVM.ViewModels
{
    public class ViewModelAudioPlayer : INotifyPropertyChanged
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        enum RepeatMode {
            NoRepeat,
            RepeatAll,
            RepeatOne
        }

        private DispatcherTimer _timer = new DispatcherTimer();

        public ObservableCollection<Tracks> Tracks
        {
            get { return DataBaseInteraction.DBTracks; }
        }
        public ObservableCollection<Tracks> SortedTracks { get; set; }

        private MediaPlayer mediaPlayer = new();

        public ICommand RevealCommand { get; set; }
        public ICommand PauseCommand { get; set; }
        public ICommand NextTrackCommand { get; set; }
        public ICommand PreviousTrackCommand { get; set; }
        public ICommand ShuffleCommand { get; set; }
        public ICommand RepeatCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand ConfirmAddCommand { get; set; }
        public ICommand CancelAddCommand { get; set; }

        public AudioPlayer view;
        public ViewModelAudioPlayer(AudioPlayer view)
        {
            this.view = view;
            view.DataContext = this;

            Hidden = true;
            Shuffled = false;
            IsPlaying = false;
            CurrentTrackIndexInSortedList = -1;
            repeatMode = RepeatMode.NoRepeat;

            RevealCommand = new RelayCommand(RevealClick);
            PauseCommand = new RelayCommand(Pause);
            ShuffleCommand = new RelayCommand(SortTracks);
            PreviousTrackCommand = new RelayCommand(PlayPreviousTrack);
            NextTrackCommand = new RelayCommand(PlayNextTrack);
            AddCommand = new RelayCommand(AddTrackClick);
            ConfirmAddCommand = new RelayCommand(ConfirmAddTrack);
            CancelAddCommand = new RelayCommand(CancelAddTrack);
            RepeatCommand = new RelayCommand(RepeatClick);

            void UpdateSliderValue(object sender, EventArgs e)
            {
                view.Progress.Value = mediaPlayer.Position.TotalSeconds;
            }
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += new EventHandler(UpdateSliderValue);
            _timer.Start();

            void MediaPlayer_MediaOpened(object? sender, EventArgs e)
            {
                view.Progress.Value = 0;
                view.Progress.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            }
            void MediaPlayer_MediaEnded(object? sender, EventArgs e)
            {
                if (repeatMode == RepeatMode.RepeatOne)
                {
                    mediaPlayer.Position = TimeSpan.Zero;
                    mediaPlayer.Play();
                    return;
                }
                PlayNextTrack();
            }
            mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;

            InitSortedTracks();

            Crutch1ErrorVisibility = Visibility.Collapsed;
        }

        public void RevealClick()
        {
            int p = Hidden ? -1 : 1;
            view.translateTransform.X = view.translateTransform.X + p * view.Main.ActualWidth;
            Hidden = !Hidden;
            OnPropertyChanged(nameof(RevealButtonSource));
        }

        private void InitSortedTracks()
        {
            SortedTracks = new();
            foreach (var t in Tracks)
            {
                SortedTracks.Add(t);
            }
            Shuffled = true;
            SortTracks();
        }

        private bool Hidden { get; set; }
        public string RevealButtonSource
        {
            get
            {
                return Hidden ? PathsToImages.TriangleToLeft : PathsToImages.TriangleToRight;
            }
        }

        private int CurrentTrackIndexInSortedList { get; set; }

        private bool shuffled = false;
        private bool Shuffled
        {
            get { return shuffled; }
            set
            {
                shuffled = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShuffleButtonSource));
            }
        }

        private bool isPlaying = false;
        public bool IsPlaying
        {
            get { return isPlaying; }
            set
            {
                isPlaying = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PauseButtonSource));
                view.mainWindow.viewModel.MutedByPlayer = value;
            }
        }
        private RepeatMode repeatMode { get; set; }

        public string PauseButtonSource
        {
            get
            {
                return IsPlaying ? PathsToImages.Pause : PathsToImages.Play;
            }
        }
        public string RepeatButtonSource
        {
            get
            {
                switch (repeatMode)
                {
                    case RepeatMode.NoRepeat:
                        {
                            return PathsToImages.NoRepeat;
                        }
                    case RepeatMode.RepeatAll:
                        {
                            return PathsToImages.RepeatAll;
                        }
                    default:
                        {
                            return PathsToImages.RepeatOne;
                        }
                }
            }
        }

        private Visibility listVisibility = Visibility.Visible;
        public Visibility ListVisibility
        {
            get
            {
                return listVisibility;
            }
            set
            {
                listVisibility = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AddMenuVisibility));
            }
        }
        public Visibility AddMenuVisibility
        {
            get
            {
                return ListVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public void ChangeTrackProgress(TimeSpan ts)
        {
            view.Progress.Value = ts.TotalSeconds;
            mediaPlayer.Position = ts;

        }

        public void PlayTrack(Tracks track, int index)
        {
            CurrentTrackIndexInSortedList = index;
            if (!File.Exists(track.TrackPath)) MessageBox.Show($"Track {track.TrackPath} is missing");

            IsPlaying = true;
            OnPropertyChanged(nameof(PauseButtonSource));

            //mediaPlayer.SoundLocation = track.TrackPath;
            mediaPlayer.Open(new(track.TrackPath, UriKind.RelativeOrAbsolute));
            mediaPlayer.Play();

            if (index != -1) view.Tracks.SelectedIndex = index;
        }

        public void Pause()
        {
            if (!IsPlaying)
            {
                if (view.Tracks.SelectedIndex == -1)
                {
                    view.Tracks.SelectedIndex = 0;
                }
                else
                {
                    mediaPlayer.Play();
                    IsPlaying = true;
                }
            }
            else
            {
                mediaPlayer.Pause();
                IsPlaying = false;
            }
            OnPropertyChanged(nameof(PauseButtonSource));
        }

        private List<int> GetRandomTrackOrder()//returns order in which tracks from database will be played
        {
            int count = Tracks.Count;
            var ascendingIntList = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                ascendingIntList.Add(i);
            }//1,2,3,...

            var randomGenerator = new Random();
            var ret = new List<int>(count);

            for (int i = 0; i < count; i++)
            {
                int index = randomGenerator.Next(0, ascendingIntList.Count);
                ret.Add(ascendingIntList[index]);
                ascendingIntList.RemoveAt(index);
            }
            return ret;
        }
        private void SortTracks()
        {
            Shuffled = !Shuffled;
            if (SortedTracks.Count < 2) return;

            CurrentTrackIndexInSortedList = -1;
            
            Tracks? currentTrack = CurrentTrackIndexInSortedList != -1 ? SortedTracks[CurrentTrackIndexInSortedList] : null;


            if (!Shuffled)
            {
                int i = 1;
                if (view.Tracks.SelectedIndex != -1)
                {
                    CurrentTrackIndexInSortedList = 1;
                    view.Tracks.SelectedIndex = 1;
                    SortedTracks[0] = Tracks[0];
                    CurrentTrackIndexInSortedList = 0;
                    view.Tracks.SelectedIndex = 0;
                }
                else
                {
                    i = 0;
                }

                for (; i < Tracks.Count; i++)
                {
                    SortedTracks[i] = Tracks[i];
                }
            }
            else
            {
                var listOrder = GetRandomTrackOrder();

                CurrentTrackIndexInSortedList = 1;
                view.Tracks.SelectedIndex = 1;
                SortedTracks[0] = Tracks[listOrder[0]];
                CurrentTrackIndexInSortedList = 0;
                view.Tracks.SelectedIndex = 0;

                for (int i = 1; i < Tracks.Count; i++)
                {
                    SortedTracks[i] = Tracks[listOrder[i]];
                }
            }

            if (currentTrack != null) for (int i = 0; i < SortedTracks.Count; i++)
                {
                    if (SortedTracks[i] == currentTrack) CurrentTrackIndexInSortedList = i;
                }//find current track

            OnPropertyChanged(nameof(SortedTracks));
        }

        public void RepeatClick()
        {
            repeatMode = repeatMode == RepeatMode.NoRepeat ? RepeatMode.RepeatAll : repeatMode == RepeatMode.RepeatAll ?
                RepeatMode.RepeatOne : RepeatMode.NoRepeat;
            OnPropertyChanged(nameof(RepeatButtonSource));
        }

        public void PlayPreviousTrack()
        {
            if (SortedTracks.Count > 0 && CurrentTrackIndexInSortedList != -1)
            {
                int index;

                if (CurrentTrackIndexInSortedList > 0)
                {
                    index = CurrentTrackIndexInSortedList - 1;
                }
                else
                {
                    index = SortedTracks.Count - 1;
                }

                PlayTrack(SortedTracks[index], index);
            }
        }
        public void PlayNextTrack()
        {
            if (CurrentTrackIndexInSortedList == SortedTracks.Count - 1 && repeatMode == RepeatMode.NoRepeat)
            {
                mediaPlayer.Stop();
                IsPlaying = false;
                OnPropertyChanged(nameof(PauseButtonSource));
                CurrentTrackIndexInSortedList = 0;
                return;
            }
            if (SortedTracks.Count > 0 && CurrentTrackIndexInSortedList != -1)
            {
                int index;

                if (CurrentTrackIndexInSortedList < Tracks.Count - 1)
                {
                    index = CurrentTrackIndexInSortedList + 1;
                }
                else
                {
                    index = 0;
                }

                PlayTrack(SortedTracks[index], index);
            }
        }

        public void AddTrackClick()
        {
            ListVisibility = Visibility.Collapsed;
        }
        public void CancelAddTrack()
        {
            ListVisibility = Visibility.Visible;
            view.TrackName.Text = string.Empty;
            view.TrackPath.Text = string.Empty;
            view.NameErrorTextBlock.Text = string.Empty;
            view.PathErrorTextBlock.Text = string.Empty;
        }
        public void ConfirmAddTrack()
        {
            string Name = view.TrackName.Text;
            string Path = view.TrackPath.Text;
            bool trackValid = true;

            if (DataBaseInteraction.TrackExists(Name))
            {
                view.NameErrorTextBlock.Text = "Name is already taken";
                trackValid = false;
            }
            if (!File.Exists(Path))
            {
                view.PathErrorTextBlock.Text = "File does not exist";
                trackValid = false;
            }
            if (!trackValid)
            {
                return;
            }

            Crutch1ErrorVisibility = Visibility.Collapsed;
            OnPropertyChanged(nameof(Crutch1ErrorVisibility));

            DataBaseInteraction.AddTrack(new()
            {
                TrackName = Name,
                TrackPath = Path
            });

            InitSortedTracks();

            ListVisibility = Visibility.Visible;
        }

        public void DeleteTrack(Tracks track)
        {
            if (SortedTracks.Count() == 1)
            {
                Crutch1ErrorVisibility = Visibility.Visible;
                OnPropertyChanged(nameof(Crutch1ErrorVisibility));
                return;
            }

            Tracks? currentTrack = CurrentTrackIndexInSortedList != -1 ? SortedTracks[CurrentTrackIndexInSortedList] : null;

            int oldIndex = CurrentTrackIndexInSortedList;

            if (currentTrack == track)
            {
                PlayNextTrack();
            }
            if (CurrentTrackIndexInSortedList > oldIndex) CurrentTrackIndexInSortedList--;

            DataBaseInteraction.DeleteTrack(track);
            SortedTracks.Remove(track);
            OnPropertyChanged(nameof(SortedTracks));
        }

        public Visibility Crutch1ErrorVisibility { get; set; }

        public string ShuffleButtonSource { get { return Shuffled ? PathsToImages.ShuffleOn : PathsToImages.ShuffleOff; } }
    }
}
