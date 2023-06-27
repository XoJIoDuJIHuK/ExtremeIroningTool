using ExtremeIroningTool.Interfaces;
using ExtremeIroningTool.MVVM.Models;
using ExtremeIroningTool.MVVM.Views;
using ExtremeIroningTool.Utilitary_classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace ExtremeIroningTool.MVVM.ViewModels
{
    public class ViewModelBottomPartArmyConfigurator : INotifyPropertyChanged
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        public string AddIcon
        {
            get
            {
                return IsFieldmarshalViewActive ? PathsToImages.AddNewArmyGroup : PathsToImages.AddNewArmy;
            }
        }

        #region Headers
        
        /// <summary>
        /// Properties and commands for headers of current army and army group
        /// </summary>

        private Visibility armyGroupHeaderVisibility = Visibility.Hidden;
        private Visibility armyHeaderVisibility = Visibility.Hidden;
        public Visibility ArmyGroupHeaderVisibility
        {
            get { return armyGroupHeaderVisibility; }
            set
            {
                armyGroupHeaderVisibility = value;
                OnPropertyChanged();
            }
        }
        public Visibility ArmyHeaderVisibility
        {
            get { return armyHeaderVisibility; }
            set
            {
                armyHeaderVisibility = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddDivisionCommand { get; set; }

        public ICommand ChangeArmyGroupIcon { get; set; }
        public ICommand ChangeArmyIcon { get; set; }

        public ICommand EditArmyGroupCommand { get; set; }
        public ICommand EditArmyCommand { get; set; }

        public string ArmyGroupIcon
        {
            get
            {
                if (ArmyGroups[CurrentArmyGroupIndex] != null)
                {
                    return ArmyGroups[CurrentArmyGroupIndex].PathToIcon;
                }
                return string.Empty;
            }
        }
        public string ArmyIcon
        {
            get
            {
                if (CurrentArmyGroupIndex != -1 && CurrentArmyIndex != -1)
                {
                    return CurrentArmyGroup[CurrentArmyIndex].PathToIcon;
                }
                return string.Empty;
            }
        }

        public string ArmyGroupName
        {
            get
            {
                if (ArmyGroups[CurrentArmyGroupIndex] != null)
                {
                    return ArmyGroups[CurrentArmyGroupIndex].Name;
                }
                return string.Empty;
            }
            set
            {
                if (ArmyGroups[CurrentArmyGroupIndex] != null)
                {
                    ArmyGroups[CurrentArmyGroupIndex].Name = value;
                }
            }
        }
        public string ArmyName
        {
            get
            {
                if (CurrentArmyGroup[CurrentArmyIndex] != null)
                {
                    return CurrentArmyGroup[CurrentArmyIndex].Name;
                }
                return string.Empty;
            }
            set
            {
                if (CurrentArmyGroup[CurrentArmyIndex] != null)
                {
                    CurrentArmyGroup[CurrentArmyIndex].Name = value;
                }
            }
        }

        private string proxyArmyName;
        public string ProxyArmyName
        {
            get { return proxyArmyName; }
            set
            {
                proxyArmyName = value;
                OnPropertyChanged();
            }
        }

        private string proxyArmyGroupName;
        public string ProxyArmyGroupName
        {
            get { return proxyArmyGroupName; }
            set
            {
                proxyArmyGroupName = value;
                OnPropertyChanged();
            }
        }

        public bool forbidEditArmyName = true;
        public bool ForbidEditArmyName
        {
            get { return forbidEditArmyName; }
            set
            {
                forbidEditArmyName = value;
                OnPropertyChanged();
            }
        }

        public bool forbidEditArmyGroupName = true;
        public bool ForbidEditArmyGroupName
        {
            get { return forbidEditArmyGroupName; }
            set
            {
                forbidEditArmyGroupName = value;
                OnPropertyChanged();
            }
        }

        public string ArmyGroupButtonImageSource { get { return !ForbidEditArmyGroupName ? PathsToImages.ConfirmIcon : PathsToImages.EditIcon; } }
        public string ArmyButtonImageSource { get { return !ForbidEditArmyName ? PathsToImages.ConfirmIcon : PathsToImages.EditIcon; } }

        public void EditArmyGroupClick()
        {
            if (!(ArmyGroupName == ProxyArmyGroupName) && !UtilitaryFunctions.AcceptableArmyGroupName(ProxyArmyGroupName))
            {
                ProxyArmyGroupName = $"{ProxyArmyGroupName} - copy";
                if (!UtilitaryFunctions.AcceptableArmyGroupName(ProxyArmyGroupName))
                {
                    ProxyArmyGroupName = UtilitaryFunctions.GetDefaultName("Army Group");
                }
            }
            LandForces.allArmyGroupNames.Add(ProxyArmyGroupName);
            ArmyGroupName = ProxyArmyGroupName;
            ForbidEditArmyGroupName = !ForbidEditArmyGroupName;
            OnPropertyChanged(nameof(ForbidEditArmyGroupName));

            OnPropertyChanged(nameof(ArmyGroupButtonImageSource));
        }
        public void EditArmyClick()
        {
            if (!(ArmyName == ProxyArmyName) && !UtilitaryFunctions.AcceptableArmyName(ProxyArmyName))
            {
                ProxyArmyName = UtilitaryFunctions.GetDefaultName("Army");
            }
            LandForces.allArmyNames.Add(ProxyArmyName);
            ArmyName = ProxyArmyName;
            ForbidEditArmyName = !ForbidEditArmyName;
            OnPropertyChanged(nameof(ForbidEditArmyName));
            OnPropertyChanged(nameof(ProxyArmyName));

            OnPropertyChanged(nameof(ArmyButtonImageSource));
        }

        public ICommand editArmyIcon { get; set; }
        public ICommand editArmyGroupIcon { get; set; }

        public void EditArmyIcon()
        {
            view.parentView.viewModel.ShowEditIconWindow(UnitType.Army);
        }
        public void EditArmyGroupIcon()
        {
            view.parentView.viewModel.ShowEditIconWindow(UnitType.ArmyGroup);
        }

        public void SetArmyIcon(string path)
        {
            CurrentArmyGroup[CurrentArmyIndex].PathToIcon = path;
            OnPropertyChanged(nameof(ArmyIcon));
        }
        public void SetArmyGroupIcon(string path)
        {
            ArmyGroups[CurrentArmyGroupIndex].PathToIcon = path;
            OnPropertyChanged(nameof(ArmyGroupIcon));
        }

        private SelectDivisionWindow selectDivisionWindow
        {
            get { return view.parentView.viewModel.model.selectDivisionWindow; }
        }
        public void AddDivisionClick()
        {
            if (!IsFieldmarshalViewActive)
            {
                selectDivisionWindow.Owner = view.parentView.mainWindow;
                selectDivisionWindow.ShowDialog();
            }
        }

        public void AddDivisionToCurrentArmy(Division division)
        {
            int index = CurrentArmyDivisions.IndexOf(division);
            if (index != -1)
            {
                CurrentArmyDivisions[index].Value++;
            }
            else
            {
                CurrentArmyDivisions.Add(new(division, 1));
            }
            CurrentArmyGroup[CurrentArmyIndex].UpdateProperties();
            OnPropertyChanged(nameof(CurrentArmyDivisions));
        }
        public void IncrementDivisionCount(Division division)
        {
            CurrentArmyDivisions[CurrentArmyDivisions.IndexOf(division)].Value++;
        }
        public void DecrementDivisionCount(Division division)
        {
            int index = CurrentArmyDivisions.IndexOf(division);
            if (CurrentArmyDivisions[index].Value == 1)
            {
                CurrentArmyDivisions.RemoveAt(index);
                CurrentArmyGroup[CurrentArmyIndex].UpdateProperties();
                return;
            }
            CurrentArmyDivisions[CurrentArmyDivisions.IndexOf(division)].Value--;
        }

        #endregion

        private LandForces currentLandForce;
        public ObservableCollection<ArmyGroup> ArmyGroups
        {
            get
            {
                if (currentLandForce != null) return currentLandForce.ArmyGroups;
                else return new();
            }
        }

        private Country CurrentCountry
        {
            get { return DataBaseInteraction.allCountries[view.parentView.mainWindow.viewModel.model.selectedCountries[view.index]]; }
        }

        private int currentArmyGroupIndex = -1;
        public int CurrentArmyGroupIndex
        {
            get { return currentArmyGroupIndex; }
            set
            {
                currentArmyGroupIndex = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Army> CurrentArmyGroup//List of armies of current army group
        {
            get
            {
                if (CurrentArmyGroupIndex != -1) return ArmyGroups[CurrentArmyGroupIndex].Armies;
                else return null;
            }
        }

        private int currentArmyIndex = -1;
        public int CurrentArmyIndex
        {
            get { return currentArmyIndex; }
            set
            {
                currentArmyIndex = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<UnitDictionaryElement> CurrentArmyDivisions
        {
            get
            {
                if (CurrentArmyGroupIndex != -1)
                {
                    if (CurrentArmyIndex != -1)
                    {
                        return CurrentArmyGroup[CurrentArmyIndex].Divisions;
                    }
                }
                return null;
            }
        }

        public BottomPartArmyConfigurator view;
        public ModelBottomPartArmyConfigurator model;

        public ICommand showFieldmarshals { get; set; }
        public ICommand addClick { get; set; }

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private bool isFieldmarshalViewActive = false;
        public bool IsFieldmarshalViewActive
        {
            get { return isFieldmarshalViewActive; }
            set
            {
                isFieldmarshalViewActive = value;
                OnPropertyChanged(nameof(IsFieldmarshalViewActive));
                OnPropertyChanged(nameof(AddIcon));
            }
        }
        public int armyGroupToAdd = 0;

        public ViewModelBottomPartArmyConfigurator(BottomPartArmyConfigurator view, ref LandForces landForce)
        {
            IsFieldmarshalViewActive = true;
            currentLandForce = landForce;

            this.view = view;
            this.view.DataContext = this;
            model = new();
            InitializeSelectGeneralWindow();

            showFieldmarshals = new RelayCommand(ShowArmyGroups);
            addClick = new RelayCommand(AddClick);

            SaveCommand = new RelayCommand(SaveClick);
            DeleteCommand = new RelayCommand(DeleteClick);

            EditArmyGroupCommand = new RelayCommand(EditArmyGroupClick);
            EditArmyCommand = new RelayCommand(EditArmyClick);
            AddDivisionCommand = new RelayCommand(AddDivisionClick);

            editArmyIcon = new RelayCommand(EditArmyIcon);
            editArmyGroupIcon = new RelayCommand(EditArmyGroupIcon);

            HideSelectGeneralWindowCommand = new RelayCommand(HideSelectGeneralWindowClick);
            UnpinCommanderCommand = new RelayCommand(UnpinCommanderClick);

            StatusBarType = StatusBarMessageType.None;
            StatusBarText = string.Empty;
        }

        private void InitializeSelectGeneralWindow()
        {
            model.selectGeneralWindow = new();
            model.selectGeneralWindow.DataContext = this;

            model.selectGeneralWindow.Hide();
        }

        public void ShowArmyGroups()
        {
            if (!IsFieldmarshalViewActive)
            {
                view.Generals.Visibility = Visibility.Collapsed;
                view.Fieldmarshals.Visibility = Visibility.Visible;

                ArmyGroupHeaderVisibility = Visibility.Hidden;
                OnPropertyChanged(nameof(ArmyGroupHeaderVisibility));
                ArmyHeaderVisibility = Visibility.Hidden;
                OnPropertyChanged(nameof(ArmyHeaderVisibility));
            }
            IsFieldmarshalViewActive = true;

            CurrentArmyGroupIndex = -1;
            CurrentArmyIndex = -1;
            OnPropertyChanged(nameof(CurrentArmyDivisions));
        }
        public void ShowArmies(ILandForce armyGroup)
        {
            if (armyGroup is ArmyGroup)
            {
                for (int i = 0; i < ArmyGroups.Count; i++)
                {
                    if ((ArmyGroup)armyGroup == ArmyGroups[i])
                    {
                        CurrentArmyGroupIndex = i;
                        OnPropertyChanged(nameof(CurrentArmyGroup));
                        OnPropertyChanged(nameof(ArmyGroupIcon));

                        ProxyArmyGroupName = armyGroup.Name;
                        OnPropertyChanged(nameof(ProxyArmyGroupName));

                        OnPropertyChanged(nameof(ArmyGroupButtonImageSource));

                        ArmyGroupHeaderVisibility = Visibility.Visible;
                        OnPropertyChanged(nameof(ArmyGroupHeaderVisibility));

                        break;
                    }
                    if (i == ArmyGroups.Count - 1) throw new Exception("No such army group");//should never be thrown
                }

                ((RelayCommand)showFieldmarshals).canExecute = true;
                view.Generals.Visibility = Visibility.Visible;
                view.Fieldmarshals.Visibility = Visibility.Collapsed;

                IsFieldmarshalViewActive = false;
            }
            else
            {
                var army = (Army)armyGroup;

                int i = 0;
                foreach (Army a in CurrentArmyGroup)
                {
                    if (a == army)
                    {
                        CurrentArmyIndex = i;
                        OnPropertyChanged(nameof(ArmyIcon));

                        ProxyArmyName = army.Name;
                        OnPropertyChanged(nameof(ProxyArmyName));

                        OnPropertyChanged(nameof(ArmyButtonImageSource));

                        ArmyHeaderVisibility = Visibility.Visible;
                        OnPropertyChanged(nameof(ArmyHeaderVisibility));

                        break;
                    }
                    i++;
                }

                OnPropertyChanged(nameof(CurrentArmyDivisions));
            }
        }

        public void AddClick()
        {
            if (IsFieldmarshalViewActive)
            {
                view.parentView.viewModel.addWindow.Owner = view.parentView.mainWindow;
                view.parentView.viewModel.addWindow.calledCountry = view;
                view.parentView.viewModel.addWindow.armyGroupCards.SelectedItem = null;
                view.parentView.viewModel.addWindow.ShowDialog();
            }
            else
            {
                AddArmy();
            }
        }
        public void AddArmy()
        {
            CurrentArmyGroup.Add(new());
        }
        public void AddGroup(ArmyGroup armyGroup)
        {
            ArmyGroups.Add(armyGroup.Clone());
            OnPropertyChanged(nameof(ArmyGroups));
        }

        public void DeleteLandForce(ILandForce force)
        {
            if (IsFieldmarshalViewActive)
            {
                var armyGroup = (ArmyGroup)force;
                armyGroup.Commander.pinnedLandForce = null;
                ArmyGroups.Remove(armyGroup);
                OnPropertyChanged(nameof(ArmyGroups));
            }
            else
            {
                var army = force as Army;
                army.Commander.pinnedLandForce = null;
                if (CurrentArmyGroup[CurrentArmyIndex] == army)
                {
                    CurrentArmyIndex = -1;
                    armyHeaderVisibility = Visibility.Collapsed;
                    OnPropertyChanged(nameof(CurrentArmyDivisions));
                    OnPropertyChanged(nameof(ArmyHeaderVisibility));
                }
                ArmyGroups[CurrentArmyGroupIndex].Armies.Remove(army);
            }
        }
        public void DeleteDivision(Division division)
        {
            if (CurrentArmyGroupIndex != -1)
            {
                if (CurrentArmyIndex != -1)
                {
                    foreach (var d in CurrentArmyGroup[CurrentArmyIndex].Divisions)
                    {
                        if (division == d.Key)
                        {
                            CurrentArmyGroup[CurrentArmyIndex].Divisions.Remove(d);
                            break;
                        }
                    }
                }
            }
            OnPropertyChanged(nameof(CurrentArmyDivisions));
        }

        public void SaveClick()
        {
            StatusBarMessageType type = StatusBarMessageType.None;
            string message = string.Empty;
            if (CurrentArmyGroupIndex != -1)
            {
                if (CurrentArmyIndex != -1)
                {
                    Army army = CurrentArmyGroup[CurrentArmyIndex];
                    int result = DataBaseInteraction.Save(ArmyGroups[CurrentArmyGroupIndex], army);
                    
                    switch (result)
                    {
                        case -1:
                            {
                                type = StatusBarMessageType.Error;
                                message = $"Could not save army {army.Name}";
                                break;
                            }
                        case 0:
                            {
                                type = StatusBarMessageType.Success;
                                message = $"Army {army.Name} succesfully saved";
                                break;
                            }
                        default:
                            {
                                type = StatusBarMessageType.Success;
                                message = $"Army {army.Name} successfully overwritten";
                                break;
                            }
                    }
                }
                else
                {
                    ArmyGroup armyGroup = ArmyGroups[CurrentArmyGroupIndex];

                    int result = DataBaseInteraction.Save(armyGroup);

                    switch (result)
                    {
                        case -1:
                            {
                                type = StatusBarMessageType.Error;
                                message = $"Could not save army group {armyGroup.Name}";
                                break;
                            }
                        case 0:
                            {
                                type = StatusBarMessageType.Success;
                                message = $"Army group {armyGroup.Name} succesfully saved";
                                break;
                            }
                        default:
                            {
                                type = StatusBarMessageType.Success;
                                message = $"Army group {armyGroup.Name} successfully overwritten";
                                break;
                            }
                    }
                }
            }
            SetStatusBar(type, message);
            OnPropertyChanged(nameof(ArmyGroups));
            OnPropertyChanged(nameof(CurrentArmyGroup));

            showFieldmarshals.Execute(null);
        }
        public void DeleteClick()
        {
            if (CurrentArmyGroupIndex != -1)
            {
                if (CurrentArmyIndex != -1)
                {
                    var army = CurrentArmyGroup[CurrentArmyIndex];
                    army.Commander.pinnedLandForce = null;
                    if (DataBaseInteraction.Delete(army))
                    {
                        CurrentArmyGroup.RemoveAt(CurrentArmyIndex);
                        CurrentArmyIndex = -1;
                        SetStatusBar(StatusBarMessageType.Success, $"Army {army.Name} successfully deleted");
                    }
                    else
                    {
                        SetStatusBar(StatusBarMessageType.Error, $"Could not delete army {army.Name}");
                    }
                }
                else
                {
                    var armyGroup = ArmyGroups[CurrentArmyGroupIndex];
                    armyGroup.Commander.pinnedLandForce = null;
                    if (DataBaseInteraction.Delete(armyGroup))
                    {
                        ArmyGroups.RemoveAt(CurrentArmyGroupIndex);
                        CurrentArmyGroupIndex = -1;
                        SetStatusBar(StatusBarMessageType.Success, $"Army group {armyGroup.Name} successfully deleted");
                    }
                    else
                    {
                        SetStatusBar(StatusBarMessageType.Error, $"Could not delete army group {armyGroup.Name}");
                    }
                }
            }
            OnPropertyChanged(nameof(ArmyGroups));
            OnPropertyChanged(nameof(CurrentArmyGroup));
            
            showFieldmarshals.Execute(null);
        }

        #region Status bar

        public string StatusBarText { get; set; }
        public string StatusBarImageSource
        {
            get
            {
                switch (StatusBarType)
                {
                    case StatusBarMessageType.Error:
                        {
                            return PathsToImages.ErrorIcon;
                        }
                    case StatusBarMessageType.None:
                        {
                            return PathsToImages.SuccessIcon;
                        }
                    default:
                        {
                            return PathsToImages.SuccessIcon;
                        }
                }
            }
        }

        private StatusBarMessageType StatusBarType { get; set; }
        public Brush StatusBarBackground
        {
            get
            {
                switch (StatusBarType)
                {
                    case StatusBarMessageType.Error:
                        {
                            return new SolidColorBrush(Colors.IndianRed);
                        }
                    case StatusBarMessageType.None:
                        {
                            return new SolidColorBrush(Colors.Transparent);
                        }
                    default:
                        {
                            return new SolidColorBrush(Colors.ForestGreen);
                        }
                }
            }
        }

        private class StatusBarParameter
        {
            public StatusBarMessageType type;
            public string text;

            public StatusBarParameter(StatusBarMessageType type, string text)
            {
                this.type = type;
                this.text = text;
            }
        }
        public void SetStatusBar(StatusBarMessageType type, string text)
        {
            StatusBarUpdate(type, text);
        }

        private object statusBarLocker = new();
        private async void StatusBarUpdate(StatusBarMessageType type, string text)
        {
            lock (statusBarLocker)
            {
                StatusBarType = type;
                StatusBarText = text;

                OnPropertyChanged(nameof(StatusBarText));
                OnPropertyChanged(nameof(StatusBarImageSource));
                OnPropertyChanged(nameof(StatusBarBackground));
            }

            await Task.Delay(5000);

            lock (statusBarLocker)
            {
                if (StatusBarText == text)
                {
                    StatusBarType = StatusBarMessageType.None;
                    StatusBarText = string.Empty;

                    OnPropertyChanged(nameof(StatusBarText));
                    OnPropertyChanged(nameof(StatusBarImageSource));
                    OnPropertyChanged(nameof(StatusBarBackground));
                }
            }
        }

        #endregion

        #region General Selection

        public ObservableCollection<General> FilteredGenerals
        {
            get
            {
                var ret = new ObservableCollection<General>();

                if (IsFieldmarshalViewActive)
                {
                    foreach (var g in DataBaseInteraction.DBFieldMarshals)
                    {
                        if (g.pinnedLandForce == null && g.Country == CurrentCountry)
                        {
                            ret.Add(g);
                        }
                    }
                }
                else
                {
                    foreach (var g in DataBaseInteraction.DBGenerals)
                    {
                        if (g.pinnedLandForce == null && g.Country == CurrentCountry)
                        {
                            ret.Add(g);
                        }
                    }
                }

                return ret;
            }
        }

        private ILandForce? SelectedLandForce;
        private SelectGeneralWindow selectGeneralWindow
        {
            get { return model.selectGeneralWindow; }
        }

        public void GeneralClick(ILandForce landForce)
        {
            OnPropertyChanged(nameof(FilteredGenerals));

            SelectedLandForce = landForce;
            selectGeneralWindow.Owner = view.parentView.mainWindow;
            selectGeneralWindow.landForce = landForce;
            selectGeneralWindow.ShowDialog();
        }

        public ICommand HideSelectGeneralWindowCommand { get; set; }
        public void HideSelectGeneralWindowClick()
        {
            selectGeneralWindow.landForce = null;
            selectGeneralWindow.Hide();
        }

        public ICommand UnpinCommanderCommand { get; set; }
        public void UnpinCommanderClick()
        {
            if (SelectedLandForce != null)
            {
                SelectedLandForce.Commander.pinnedLandForce = null;
                SelectedLandForce.Commander = SelectedLandForce is Army ? General.nullGeneral : General.nullFieldMarshal;

                HideSelectGeneralWindowCommand.Execute(null);
            }
            else
            {
                MessageBox.Show("selected land force is null");
            }
        }

        public void SetGeneral(General general)
        {
            if (SelectedLandForce != null)
            {
                UnpinCommanderCommand.Execute(null);
                SelectedLandForce.Commander = general;
                general.pinnedLandForce = SelectedLandForce;
            }
            else
            {
                HideSelectGeneralWindowCommand.Execute(null);
            }
            OnPropertyChanged(nameof(FilteredGenerals));
        }

        #endregion

    }
}
