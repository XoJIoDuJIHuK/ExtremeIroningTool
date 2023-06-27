using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ExtremeIroningTool.MVVM.Models;
using ExtremeIroningTool.MVVM.Views;
using ExtremeIroningTool.Utilitary_classes;

namespace ExtremeIroningTool.MVVM.ViewModels
{
    public class ViewModelMainMenu : INotifyPropertyChanged
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private bool LoggedAsAdmin = false;

        public ICommand StartCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand EditBattalionsCommand
        {
            get { return view.mainWindow.viewModel.EditBattalionsCommand; }
        }

        public ICommand LogIn { get; set; }
        public ICommand CancelLogIn { get; set; }
        public ICommand ShowLoginMenu { get; set; }

        public ViewMainMenu view;
        public ViewModelMainMenu(ViewMainMenu view)
        {
            StartCommand = new RelayCommand(Start);
            ExitCommand = new RelayCommand(Exit);

            LogIn = new RelayCommand(LogInClick);
            CancelLogIn = new RelayCommand(CancelLoginClick);
            ShowLoginMenu = new RelayCommand(ShowLoginMenuClick);

            PlayNextBackgroundCommand = new RelayCommand(() => view.mainWindow.viewModel.PlayNextBackground());
            PlayPreviousBackgroundCommand = new RelayCommand(() => view.mainWindow.viewModel.PlayPreviousBackground());
            MuteCommand = new RelayCommand(MuteClick);

            this.view = view;
            view.DataContext = this;
        }

        public void Hide()
        {
            view.mainWindow.viewModel.MutedByScreen = true;
            view.mainWindow.viewModel.PauseBackground();
            view.Visibility = Visibility.Collapsed;
        }
        public void Show()
        {
            view.Visibility = Visibility.Visible;
            if (!view.mainWindow.Player.viewModel.IsPlaying) view.mainWindow.viewModel.MutedByScreen = false;
            if (!view.mainWindow.Player.viewModel.IsPlaying) view.mainWindow.viewModel.PlayBackground();
        }

        public void Start()
        {
            view.mainWindow.viewModel.StartMenuButtonClickCommand.Execute(null);
        }
        public void Exit()
        {
            view.mainWindow.viewModel.ExitApplicationCommand.Execute(null);
        }

        public void ShowLoginMenuClick()
        {
            if (!LoggedAsAdmin)
            {
                view.Buttons.Visibility = Visibility.Collapsed;
                view.Login.Visibility = Visibility.Visible;
            }
            else
            {
                view.LoginButton.Content = "Log in";
                LoggedAsAdmin = false;
                view.EditBattalionsButton.Visibility = Visibility.Collapsed;
            }
        }
        public void CancelLoginClick()
        {
            view.LoginTextBox.Text = string.Empty;
            view.PasswordTextBox.Password = string.Empty;

            view.Buttons.Visibility = Visibility.Visible;
            view.Login.Visibility = Visibility.Collapsed;
            view.ErrorTextBlock.Visibility = Visibility.Collapsed;
        }
        public void LogInClick()
        {
            string name = view.LoginTextBox.Text;
            string password = view.PasswordTextBox.Password;

            if (DataBaseInteraction.IsAdmin(name, password))
            {
                LoggedAsAdmin = true;
                view.ErrorTextBlock.Visibility = Visibility.Collapsed;

                view.LoginButton.Content = "Log out";
                view.EditBattalionsButton.Visibility = Visibility.Visible;

                CancelLogIn.Execute(null);
            }
            else
            {
                view.ErrorTextBlock.Visibility = Visibility.Visible;
            }
        }

        public string MutedIconSource
        {
            get { return view.mainWindow.viewModel.MutedByUser ? PathsToImages.Muted : PathsToImages.Unmuted; }
        }

        public ICommand PlayPreviousBackgroundCommand { get; set; }
        public ICommand PlayNextBackgroundCommand { get; set; }
        public ICommand MuteCommand { get; set; }

        public void MuteClick()
        {
            view.mainWindow.viewModel.MutedByUser = !view.mainWindow.viewModel.MutedByUser;
            OnPropertyChanged(nameof(MutedIconSource));
        }
    }
}
