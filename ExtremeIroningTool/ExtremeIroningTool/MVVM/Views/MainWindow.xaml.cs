using ExtremeIroningTool.MVVM.ViewModels;
using System.Windows;

namespace ExtremeIroningTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModelMainWindow viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new(this);
            viewModel.InitializeVMS();
        }

        private void BackgroundMediaEnded(object sender, RoutedEventArgs e)
        {
            viewModel.PlayNextBackground();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.InitMainWindowBackground(2);
        }
    }
}
