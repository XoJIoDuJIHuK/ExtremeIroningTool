using ExtremeIroningTool.MVVM.ViewModels;
using ExtremeIroningTool.Utilitary_classes.DataBaseClasses;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExtremeIroningTool.MVVM.Views
{
    /// <summary>
    /// Логика взаимодействия для AudioPlayer.xaml
    /// </summary>
    public partial class AudioPlayer : UserControl
    {
        public ViewModelAudioPlayer viewModel;
        public MainWindow mainWindow;
        public AudioPlayer()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.PlayTrack(e.AddedItems[0] as Tracks, (sender as ListBox).SelectedIndex);
        }

        private void Progress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int seconds = Convert.ToInt32(Progress.Value);
            viewModel.ChangeTrackProgress(new TimeSpan(0, 0, 0, Convert.ToInt32(Progress.Value)));
        }

        private void Progress_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int seconds = Convert.ToInt32(Progress.Value);
            viewModel.ChangeTrackProgress(new TimeSpan(0, 0, 0, Convert.ToInt32(Progress.Value)));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var track = (e.Source as Button).DataContext as Tracks;
            viewModel.DeleteTrack(track);
        }
    }
}
