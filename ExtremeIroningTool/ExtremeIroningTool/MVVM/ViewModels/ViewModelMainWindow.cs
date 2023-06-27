using ExtremeIroningTool.MVVM.Models;
using ExtremeIroningTool.MVVM.Views;
using ExtremeIroningTool.Utilitary_classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace ExtremeIroningTool.MVVM.ViewModels
{
    public class ViewModelMainWindow : INotifyPropertyChanged
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        public MainWindow view;
        public ModelMainWindow model;

        public ICommand StartMenuButtonClickCommand { get; set; }
        public ICommand ExitApplicationCommand { get; set; }
        public ICommand BackArmyConfiguratorClickCommand { get; set; }
        public ICommand ForwardArmyConfiguratorClickCommand { get; set; }
        public ICommand BackBattleClickCommand { get; set; }

        public ICommand BackModifiersCommand { get; set; }
        public ICommand ForwardModifiersCommand { get; set; }

        public ICommand EditBattalionsCommand { get; set; }
        public ICommand BackBattalionEditorCommand { get; set; }

        public ViewModelMainWindow(MainWindow mainWindow)
        {
            view = mainWindow;
            model = new ModelMainWindow();
            view.DataContext = this;

            #region Views
            view.MainMenu.mainWindow = view;
            view.CountrySelect.mainWindow = view;
            view.ArmyConfigurator.mainWindow = view;
            view.Modifiers.mainWindow = view;
            view.Battle.mainWindow = view;
            view.BattalionEditor.mainWindow = view;
            view.Player.mainWindow = view;
            #endregion

            DataBaseInteraction.sqlConnection.Open();// открываем базу данных

            #region DB
            DataBaseInteraction.GetCountries();
            DataBaseInteraction.GetIcons();
            DataBaseInteraction.GetDBUnits();
            DataBaseInteraction.GetDivisions();
            DataBaseInteraction.GetArmyGroups();
            DataBaseInteraction.GetGeneralStaff();
            DataBaseInteraction.GetTracks();
            DataBaseInteraction.GetModifiers();
            DataBaseInteraction.GetTerrainTypes();
            #endregion

            StartMenuButtonClickCommand = new RelayCommand(StartMenuButtonClick);
            ExitApplicationCommand = new RelayCommand(Application.Current.Shutdown);

            BackArmyConfiguratorClickCommand = new RelayCommand(BackArmyConfiguratorClick);
            ForwardArmyConfiguratorClickCommand = new RelayCommand(ForwardArmyConfiguratorClick);

            BackModifiersCommand = new RelayCommand(BackModifiersClick);
            ForwardModifiersCommand = new RelayCommand(ForwardModifiersClick);

            BackBattleClickCommand = new RelayCommand(BackBattleClick);

            EditBattalionsCommand = new RelayCommand(EditBattalionsClick);
            BackBattalionEditorCommand = new RelayCommand(BackBattalionEditorClick);
        }   

        public void InitializeVMS()//ViewModelCountrySelect
        {
            view.MainMenu.viewModel = new(view.MainMenu);
            view.CountrySelect.viewModel = new(view.CountrySelect);
            view.ArmyConfigurator.viewModel = new(view.ArmyConfigurator);
            view.ArmyConfigurator.viewModel.InitializeVMS();
            view.Modifiers.viewModel = new(view.Modifiers);
            view.Battle.viewModel = new(view.Battle);
            view.Player.viewModel = new(view.Player);
            view.BattalionEditor.viewModel = new(view.BattalionEditor);
        }

        #region Navigation Buttons
        public void StartMenuButtonClick()
        {
            view.MainMenu.viewModel.Hide();
            view.CountrySelect.viewModel.Show();
        }
        public void BackCountrySelectClick()
        {
            view.CountrySelect.viewModel.Hide();
            view.MainMenu.viewModel.Show();
        }
        public void ForwardCountrySelectClick()
        {
            if (model.selectedCountries.IndexOf(-1) == -1)
            {
                view.CountrySelect.viewModel.Hide();
                view.ArmyConfigurator.viewModel.Show();
            }
            else
            {
                MessageBox.Show("Select two countries");
            }
        }
        public void BackArmyConfiguratorClick()
        {
            view.ArmyConfigurator.viewModel.Hide();
            view.CountrySelect.viewModel.Show();
        }
        public void ForwardArmyConfiguratorClick()
        {

            view.ArmyConfigurator.viewModel.Hide();
            view.Modifiers.viewModel.Show();
        }
        public void BackBattleClick()
        {
            view.Battle.viewModel.Hide();
            view.Modifiers.viewModel.Show();
        }
        public void BackModifiersClick()
        {
            view.Modifiers.viewModel.Hide();
            view.ArmyConfigurator.viewModel.Show();
        }
        public void ForwardModifiersClick()
        {
            SetUpArmies(view.ArmyConfigurator.firstCountry.viewModel.ArmyGroups,
                        view.ArmyConfigurator.secondCountry.viewModel.ArmyGroups);

            view.Modifiers.viewModel.Hide();
            view.Battle.viewModel.Show();

            model.Attackers.UpdateDivisionsBattleStats();
            model.Defenders.UpdateDivisionsBattleStats();
        }

        public void EditBattalionsClick()
        {
            view.MainMenu.viewModel.Hide();
            view.BattalionEditor.viewModel.Show();
        }
        public void BackBattalionEditorClick()
        {
            view.MainMenu.viewModel.Show();
            view.BattalionEditor.viewModel.Hide();
        }
        #endregion

        public void SetUpArmies(ObservableCollection<ArmyGroup> AttackArmyGroups,
                                ObservableCollection<ArmyGroup> DefenseArmyGroups)
        {
            foreach (var ag in AttackArmyGroups)
            {
                foreach (var a in ag.Armies)
                {
                    a.SetUpBattleArmies();
                    foreach (var d in a.BattleDivisions)
                    {
                        model.Attackers.AllDivisions.Add(d);
                    }
                }
            }
            foreach (var ag in DefenseArmyGroups)
            {
                foreach (var a in ag.Armies)
                {
                    a.SetUpBattleArmies();
                    foreach (var d in a.BattleDivisions)
                    {
                        model.Defenders.AllDivisions.Add(d);
                    }
                }
            }

            ApplyModifiers();

            foreach (var d in model.Attackers.AllDivisions)
            {
                d.MaxHealth = d.health;
                d.MaxOrganisation = d.organization;
            }
            foreach (var d in model.Defenders.AllDivisions)
            {
                d.MaxHealth = d.health;
                d.MaxOrganisation = d.organization;
            }
        }

        public void ApplyModifiers()
        {
            var modifiers = view.Modifiers.viewModel.GetSelectedModifiers();

            foreach (var d in model.Attackers.AllDivisions)
            {
                foreach (var m in modifiers[0])
                {
                    double battalionTypePart = d.GetBattalionTypePart(m.BattalionType);

                    switch (m.Property)
                    {
                        case "Health":
                            {
                                d.health *= (double)m.Modifier * battalionTypePart;
                                d.MaxHealth = d.health;
                                break;
                            }
                        case "Organization":
                            {
                                d.organization *= (double)m.Modifier * battalionTypePart;
                                d.MaxOrganisation = d.organization;
                                break;
                            }
                        case "Soft attack":
                            {
                                d.softAttack *= (double)m.Modifier * battalionTypePart;
                                break;
                            }
                        case "Hard attack":
                            {
                                d.hardAttack *= (double)m.Modifier * battalionTypePart;
                                break;
                            }
                        case "Defense":
                            {
                                d.defence *= (double)m.Modifier * battalionTypePart;
                                break;
                            }
                        case "Breakthrough":
                            {
                                d.breakthrough *= (double)m.Modifier * battalionTypePart;
                                break;
                            }
                        case "Armor":
                            {
                                d.armor *= (double)m.Modifier * battalionTypePart;
                                break;
                            }
                        case "Piercing":
                            {
                                d.piercing *= (double)m.Modifier * battalionTypePart;
                                break;
                            }
                        case "Front width":
                            {
                                d.frontWidth *= (byte)m.Modifier;
                                break;
                            }
                        default:
                            {
                                d.vehicleRatio *= (double)m.Modifier * battalionTypePart;
                                break;
                            }
                    }
                }
            }
            foreach (var d in model.Defenders.AllDivisions)
            {
                foreach (var m in modifiers[1])
                {
                    switch (m.Property)
                    {
                        case "Health":
                            {
                                d.health *= (double)m.Modifier;
                                d.MaxHealth = d.health;
                                break;
                            }
                        case "Organization":
                            {
                                d.organization *= (double)m.Modifier;
                                d.MaxOrganisation = d.organization;
                                break;
                            }
                        case "SoftAttack":
                            {
                                d.softAttack *= (double)m.Modifier;
                                break;
                            }
                        case "HardAttack":
                            {
                                d.hardAttack *= (double)m.Modifier;
                                break;
                            }
                        case "Defense":
                            {
                                d.defence *= (double)m.Modifier;
                                break;
                            }
                        case "Breakthrough":
                            {
                                d.breakthrough *= (double)m.Modifier;
                                break;
                            }
                        case "Armor":
                            {
                                d.armor *= (double)m.Modifier;
                                break;
                            }
                        case "Piercing":
                            {
                                d.piercing *= (double)m.Modifier;
                                break;
                            }
                        case "FrontWidth":
                            {
                                d.frontWidth *= (byte)m.Modifier;
                                break;
                            }
                        default:
                            {
                                d.vehicleRatio *= (double)m.Modifier;
                                break;
                            }
                    }
                }
            }
        }

        public void UpdateDivision(Division newDivision, string s)
        {
            model.Attackers.UpdateDivision(newDivision, s);
            model.Defenders.UpdateDivision(newDivision, s);
        }
        public void ClearArmiesFromCommanders()
        {
            foreach (var ag in model.Attackers.ArmyGroups)
            {
                ag.Commander.pinnedLandForce = null;
                ag.Commander = General.nullFieldMarshal;
                foreach (var a in ag.Armies)
                {
                    a.Commander.pinnedLandForce = null;
                    a.Commander = General.nullGeneral;
                }
            }
            foreach (var ag in model.Defenders.ArmyGroups)
            {
                ag.Commander.pinnedLandForce = null;
                ag.Commander = General.nullFieldMarshal;
                foreach (var a in ag.Armies)
                {
                    a.Commander.pinnedLandForce = null;
                    a.Commander = General.nullGeneral;
                }
            }
        }

        #region Background

        private int CurrentVideoIndex = -1;

        public bool CurrentMutedState
        {
            get { return MutedByUser || MutedByScreen || MutedByPlayer; }
        }

        private bool mutedByUser = false;
        public bool MutedByUser
        {
            get { return mutedByUser; }
            set
            {
                mutedByUser = value;
                OnPropertyChanged(nameof(CurrentMutedState));
            }
        }

        private bool mutedByScreen = false;
        public bool MutedByScreen
        {
            get { return mutedByScreen; }
            set
            {
                mutedByScreen = value;
                OnPropertyChanged(nameof(CurrentMutedState));
            }
        }

        private bool mutedByPlayer = false;
        public bool MutedByPlayer
        {
            get { return mutedByPlayer; }
            set
            {
                mutedByPlayer = value;
                OnPropertyChanged(nameof(CurrentMutedState));
            }
        }

        private List<string> MWBackgrundSources = new List<string>()
        {
            "./Images/sov main menu.mp4",
            "./Images/ger main menu.mp4",
            "./Images/gigamenu.mp4",
        };

        public void PauseBackground()
        {
            view.MWBackground.Pause();
        }
        public void PlayBackground()
        {
            view.MWBackground.Play();
        }

        public void PlayNextBackground()
        {
            if (CurrentVideoIndex < MWBackgrundSources.Count - 1) InitMainWindowBackground(CurrentVideoIndex + 1);
            else InitMainWindowBackground(0);
        }
        public void PlayPreviousBackground()
        {
            if (CurrentVideoIndex > 0) InitMainWindowBackground(CurrentVideoIndex - 1);
            else InitMainWindowBackground(MWBackgrundSources.Count - 1);
        }

        public void InitMainWindowBackground(int index = -1)
        {
            var myVisualBrush = view.Background as VisualBrush;
            var random = new Random();

            if (myVisualBrush != null)
            {
                if (myVisualBrush.Visual is MediaElement myMediaElement)
                {
                    if (index == -1) CurrentVideoIndex = random.Next(MWBackgrundSources.Count);
                    else CurrentVideoIndex = index;

                    myMediaElement.Source = new Uri(MWBackgrundSources[CurrentVideoIndex], UriKind.RelativeOrAbsolute);

                    myMediaElement.Position = TimeSpan.Zero;
                    myMediaElement.Play();

                    myMediaElement.SpeedRatio = 1;
                }
            }
        }

        #endregion
    }
}
