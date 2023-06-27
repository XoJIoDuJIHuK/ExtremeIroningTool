using ExtremeIroningTool.Utilitary_classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ExtremeIroningTool.MVVM.Views;
using ExtremeIroningTool.MVVM.Models;
using ExtremeIroningTool;
using System.Data.SqlClient;
using System.Windows.Media.Imaging;

namespace ExtremeIroningTool.MVVM.ViewModels
{
    public enum BattalionType
    {
        All,
        Tank,
        Mobile,
        Infantry
    }

    public class ViewModelDivisionConstructorWindow : INotifyPropertyChanged
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private DivisionContructorWindow view;
        private ViewModelArmyConfigurator parentViewModel { get { return view.parentView.viewModel; } }
        public bool EditMode = false;

        private BattalionType filterType = BattalionType.All;
        private BattalionType FilterType
        {
            get { return filterType; }
            set
            {
                filterType = value;
                OnPropertyChanged(nameof(FilteredBattalions));
            }
        }

        public ViewModelDivisionConstructorWindow(DivisionContructorWindow view)
        {
            this.view = view;

            CloseDivisionConstructor = new RelayCommand(CloseDivisionConstructorWindow);
            SaveDivisionCommand = new RelayCommand(SaveDivision);

            AddBattalionCommand = new RelayCommand(AddBattalionClick);
            AddSupportCompanyCommand = new RelayCommand(AddSupportCompanyClick);

            HideAddMenu = new RelayCommand(HideAddMenuClick);

            ForbidEditDivisionName = true;

            EditDivisionNameCommand = new RelayCommand(EditDivisionNameClick);
            CloneDivisionCommand = new RelayCommand(CloneDivision);

            ChangeIcon = new RelayCommand(ChangeIconClick);

            DeleteCurrentBattalionCommand = new RelayCommand(DeleteCurrentBattalion);
            DeleteCurrentSupportCompanyCommand = new RelayCommand(DeleteCurrentSupportCompany);
        }

        public void InitConstructorDivision()
        {
            ConstructorDivision = ConstructorDivision;
            OnPropertyChanged(nameof(Battalions));
            OnPropertyChanged(nameof(SupportCompanies));
            UpdateDivisionProperties();
        }

        private ObservableCollection<Division> Divisions
        {
            get { return view.parentView.viewModel.Divisions; }
        }

        public ICommand CloseDivisionConstructor { get; set; }
        public void CloseDivisionConstructorWindow()
        {
            EditMode = false;
            ForbidEditDivisionName = true;
            UpdateDivisionProperties();
            ConstructorDivision = new();
            view.Hide();
        }

        public ICommand ChangeIcon { get; set; }
        public void ChangeIconClick()
        {
            view.parentView.viewModel.ShowEditIconWindow(UnitType.Division);
        }

        public ICommand SaveDivisionCommand { get; set; }
        public void SaveDivision()
        {
            if (ConstructorDivision.Battalions.Count < 1) return;

            if (!EditMode)
            {
                if (ConstructorDivision.Name == "EmptyName") ConstructorDivision.Name = UtilitaryFunctions.GetDefaultName("division");
                Divisions.Add(ConstructorDivision);
            }
            int saveResult = DataBaseInteraction.Save(ConstructorDivision, OriginalDivision);//original division is null, if not edit mode


            if (saveResult == 0)
            {
                parentViewModel.ShowStatusBarMessage(StatusBarMessageType.Success, $"Division {ConstructorDivision.Name} successfully saved");
            }
            else if (saveResult == 1)
            {
                parentViewModel.ShowStatusBarMessage(StatusBarMessageType.Success, $"Division {ConstructorDivision.Name} successfully overwritten");
            }
            else if (saveResult == -1)
            {
                parentViewModel.ShowStatusBarMessage(StatusBarMessageType.Error, $"Could not save division {ConstructorDivision.Name}");
            }

            if (EditMode)
            {
                view.parentView.mainWindow.viewModel.UpdateDivision(ConstructorDivision, OriginalDivision.Name);
            }

            ConstructorDivision = new();

            ProxyDivisionName = DivisionName;
            OnPropertyChanged(nameof(Divisions));
            OnPropertyChanged(nameof(ConstructorDivision));

            OnPropertyChanged(nameof(ProxyDivisionName));
            OnPropertyChanged(nameof(DivisionPathToIcon));

            OnPropertyChanged(nameof(AddSupportCompanyButtonVisibility));
            OnPropertyChanged(nameof(AddBattalionButtonVisibility));

            OnPropertyChanged(nameof(SupportCompanies));
            OnPropertyChanged(nameof(Battalions));

            UpdateStats();
            parentViewModel.UpdateDivisionList();
        }

        public ICommand CloneDivisionCommand { get; set; }
        public void CloneDivision()
        {
            EditMode = false;

            OriginalDivision = null;

            ConstructorDivision = ConstructorDivision.Clone();
            ConstructorDivision.Name = UtilitaryFunctions.GetDefaultName("Division");
            ProxyDivisionName = ConstructorDivision.Name;
        }

        public Division ConstructorDivision
        {
            get { return view.parentView.viewModel.model.ConstructorDivision; }
            set
            {
                view.parentView.viewModel.model.ConstructorDivision = value;

                ProxyDivisionName = value.Name;

                OnPropertyChanged();
                OnPropertyChanged(nameof(AddBattalionButtonVisibility));
                OnPropertyChanged(nameof(Battalions));
                OnPropertyChanged(nameof(SupportCompanies));
                OnPropertyChanged(nameof(TerrainModifiers));
                OnPropertyChanged(nameof(AddSupportCompanyButtonVisibility));
            }
        }
        public Division? OriginalDivision = null;//reference to division in Divisions list that is being edited

        #region StatProperties
        public string Health { get { return Math.Round(ConstructorDivision.health, 2).ToString(); } }
        public string Organisation { get { return Math.Round(ConstructorDivision.organization, 2).ToString(); } }
        public string SoftAttack { get { return Math.Round(ConstructorDivision.softAttack, 2).ToString(); } }
        public string HardAttack { get { return Math.Round(ConstructorDivision.hardAttack, 2).ToString(); } }
        public string Defence { get { return Math.Round(ConstructorDivision.defence, 2).ToString(); } }
        public string Breakthrough { get { return Math.Round(ConstructorDivision.breakthrough, 2).ToString(); } }
        public string Armor { get { return Math.Round(ConstructorDivision.armor, 2).ToString(); } }
        public string Piercing { get { return Math.Round(ConstructorDivision.piercing, 2).ToString(); } }
        public string FrontWidth { get { return ConstructorDivision.frontWidth.ToString(); } }
        public string VehicleRatio { get { return $"{Math.Round(ConstructorDivision.vehicleRatio, 2)}%"; } }
        public List<TerrainModifier> TerrainModifiers { get { return ConstructorDivision.modifiers; } }
        private void UpdateStats()
        {
            OnPropertyChanged(nameof(Health));
            OnPropertyChanged(nameof(Organisation));
            OnPropertyChanged(nameof(SoftAttack));
            OnPropertyChanged(nameof(HardAttack));
            OnPropertyChanged(nameof(Defence));
            OnPropertyChanged(nameof(Breakthrough));
            OnPropertyChanged(nameof(Armor));
            OnPropertyChanged(nameof(Piercing));
            OnPropertyChanged(nameof(FrontWidth));
            OnPropertyChanged(nameof(VehicleRatio));
        }
        #endregion

        public ObservableCollection<UnitDictionaryElement> Battalions { get { return ConstructorDivision.Battalions; } }
        public ObservableCollection<SupportCompany> SupportCompanies { get { return ConstructorDivision.SupportCompanies; } }

        public string DivisionName
        {
            get { return ConstructorDivision.Name; }
            set
            {
                ConstructorDivision.Name = value;
                OnPropertyChanged();
            }
        }
        public string DivisionPathToIcon
        {
            get { return ConstructorDivision.PathToIcon; }
            set
            {
                ConstructorDivision.PathToIcon = value;
                OnPropertyChanged();
            }
        }

        private string proxyDivisionName;
        public string ProxyDivisionName
        {
            get
            {
                return proxyDivisionName;
            }
            set
            {
                proxyDivisionName = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddSupportCompanyCommand { get; set; }
        public void AddSupportCompanyClick()
        {
            view.parentView.viewModel.model.divisionContructorWindow.Battalions.Visibility = Visibility.Collapsed;
            view.parentView.viewModel.model.divisionContructorWindow.SupportCompanies.Visibility = Visibility.Collapsed;
            view.parentView.viewModel.model.divisionContructorWindow.AddSupportCompany.Visibility = Visibility.Visible;
        }

        public ICommand AddBattalionCommand { get; set; }
        public void AddBattalionClick()
        {
            view.parentView.viewModel.model.divisionContructorWindow.Battalions.Visibility = Visibility.Collapsed;
            view.parentView.viewModel.model.divisionContructorWindow.SupportCompanies.Visibility = Visibility.Collapsed;
            view.parentView.viewModel.model.divisionContructorWindow.AddBattalion.Visibility = Visibility.Visible;
        }

        public ICommand HideAddMenu { get; set; }
        public void HideAddMenuClick()
        {
            view.parentView.viewModel.model.divisionContructorWindow.AddBattalion.Visibility = Visibility.Collapsed;
            view.parentView.viewModel.model.divisionContructorWindow.AddSupportCompany.Visibility = Visibility.Collapsed;

            view.parentView.viewModel.model.divisionContructorWindow.Battalions.Visibility = Visibility.Visible;
            view.parentView.viewModel.model.divisionContructorWindow.SupportCompanies.Visibility = Visibility.Visible;

            CurrentBattalion = null;
            OnPropertyChanged(nameof(DeleteCBBVisibility));
            CurrentSupportCompany = null;
            OnPropertyChanged(nameof(DeleteCSCBVisibility));

            OnPropertyChanged(nameof(AddBattalionButtonVisibility));
            OnPropertyChanged(nameof(AddSupportCompanyButtonVisibility));
        }


        public Visibility AddSupportCompanyButtonVisibility
        {
            get
            {
                return SupportCompanies.Count < 5 ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        public Visibility AddBattalionButtonVisibility
        {
            get
            {
                return BattalionsCount < 25 ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        public int BattalionsCount
        {
            get
            {
                int count = 0;
                foreach (var b in Battalions)
                {
                    count += b.Value;
                }
                return count;
            }
        }

        private ObservableCollection<Battalion> AllBattalions { get { return DataBaseInteraction.DBBattalions; } }
        private ObservableCollection<SupportCompany> AllSupportCompanies { get { return DataBaseInteraction.DBSupportCompanies; } }

        public ObservableCollection<Battalion> FilteredBattalions
        {
            get
            {
                var ret = new ObservableCollection<Battalion>();

                foreach (var b in AllBattalions)
                {
                    if ((b.Type == FilterType || FilterType ==
                        BattalionType.All) && Battalions.IndexOf(b) == -1)
                    {
                        ret.Add(b);
                    }
                }

                return ret;
            }
        }
        public ObservableCollection<SupportCompany> FilteredSupportCompanies
        {
            get
            {
                var ret = new ObservableCollection<SupportCompany>();

                foreach (var s in AllSupportCompanies)
                {
                    if (!SupportCompanies.Contains(s))//TODO: write filter conditions here
                    {
                        ret.Add(s);
                    }
                }

                return ret;
            }
        }

        public void AddUnit(Battalion battalion)
        {
            int index = Battalions.IndexOf(battalion);
            if (CurrentBattalion == null) //just add
            {
                if (index != -1)
                {
                    Battalions[index].Value++;
                }
                else
                {
                    Battalions.Add(new(battalion, 1));
                }
                if (BattalionsCount > 24) HideAddMenu.Execute(null);
            }
            else //replace
            {
                ReplaceBattalion(battalion);
                HideAddMenu.Execute(null);
            }
            OnPropertyChanged(nameof(Battalions));
            OnPropertyChanged(nameof(FilteredBattalions));

            ConstructorDivision.SummarizeStats();

            UpdateStats();
        }
        public void AddUnit(SupportCompany supportCompany)
        {
            int index = ConstructorDivision.SupportCompanies.IndexOf(supportCompany);

            if (CurrentSupportCompany == null)
            {
                if (index == -1)
                {
                    SupportCompanies.Add(supportCompany);
                    if (SupportCompanies.Count > 4) HideAddMenu.Execute(null);
                }
            }
            else
            {
                ReplaceSupportCompany(supportCompany);
                HideAddMenu.Execute(null);
            }

            OnPropertyChanged(nameof(SupportCompanies));
            OnPropertyChanged(nameof(FilteredSupportCompanies));

            ConstructorDivision.SummarizeStats();

            UpdateStats();
        }

        public void IncrementBattalion(Battalion battalion)
        {
            if (BattalionsCount < 25)
            {
                Battalions[Battalions.IndexOf(battalion)].Value++;
            }
            OnPropertyChanged(nameof(AddBattalionButtonVisibility));

            ConstructorDivision.SummarizeStats();

            UpdateStats();
        }
        public void DecrementBattalion(Battalion battalion)
        {
            int index = Battalions.IndexOf(battalion);
            if (Battalions[index].Value > 1)
            {
                Battalions[index].Value--;
            }
            else
            {
                Battalions.RemoveAt(index);
                OnPropertyChanged(nameof(FilteredBattalions));
            }
            OnPropertyChanged(nameof(AddBattalionButtonVisibility));

            ConstructorDivision.SummarizeStats();

            UpdateStats();
        }

        private bool forbidEditDivisionName;
        public bool ForbidEditDivisionName
        {
            get { return forbidEditDivisionName; }
            set
            {
                forbidEditDivisionName = value;
                OnPropertyChanged(nameof(EditDivisionNameButtonBackground));
            }
        }

        public string EditDivisionNameButtonBackground
        {
            get
            {
                if (!ForbidEditDivisionName) return PathsToImages.ConfirmIcon;
                else return PathsToImages.EditIcon;
            }
        }
        public ICommand EditDivisionNameCommand { get; set; }
        public void EditDivisionNameClick()
        {
            if (DivisionName == ProxyDivisionName || UtilitaryFunctions.AcceptableDivisionName(ProxyDivisionName))
            {
                DivisionName = ProxyDivisionName;
                ForbidEditDivisionName = !ForbidEditDivisionName;
                OnPropertyChanged(nameof(ForbidEditDivisionName));
            }
            else
            {
                var newName = $"{ProxyDivisionName} - copy";
                if (newName.Length <= 50 && UtilitaryFunctions.AcceptableDivisionName(newName))
                {
                    DivisionName = newName;
                    ProxyDivisionName = DivisionName;
                    ForbidEditDivisionName = !ForbidEditDivisionName;
                    OnPropertyChanged(nameof(ForbidEditDivisionName));
                }
                else
                {
                    DivisionName = UtilitaryFunctions.GetDefaultName("Division");
                    ProxyDivisionName = DivisionName;
                    ForbidEditDivisionName = !ForbidEditDivisionName;
                    OnPropertyChanged(nameof(ForbidEditDivisionName));
                }
            }
        }

        public ICommand DeleteCurrentSupportCompanyCommand { get; set; }
        public void DeleteCurrentSupportCompany()
        {
            if (CurrentSupportCompany != null) SupportCompanies.Remove(CurrentSupportCompany);
            CurrentSupportCompany = null;

            HideAddMenu.Execute(null);
            OnPropertyChanged(nameof(SupportCompanies));
            OnPropertyChanged(nameof(FilteredSupportCompanies));
        }

        private SupportCompany? CurrentSupportCompany = null;
        public Visibility DeleteCSCBVisibility
        {
            get { return CurrentSupportCompany == null ? Visibility.Collapsed : Visibility.Visible; }
        }

        private Battalion? CurrentBattalion = null;
        public Visibility DeleteCBBVisibility
        {
            get { return CurrentBattalion == null ? Visibility.Collapsed : Visibility.Visible; }
        }

        public ICommand DeleteCurrentBattalionCommand { get; set; }
        public void DeleteCurrentBattalion()
        {
            if (CurrentBattalion != null) Battalions.RemoveAt(Battalions.IndexOf(CurrentBattalion));
            CurrentBattalion = null;

            HideAddMenu.Execute(null);
            OnPropertyChanged(nameof(Battalions));
            OnPropertyChanged(nameof(FilteredBattalions));
        }

        public void ReplaceBattalion(Battalion newBattalion)
        {
            int index = Battalions.IndexOf(CurrentBattalion);

            Battalions[index].Key = newBattalion;
            Battalions[index].Value = 1;
        }
        public void ReplaceSupportCompany(SupportCompany newSupportCompany)
        {
            int index = SupportCompanies.IndexOf(CurrentSupportCompany);
            if (index == -1) return;//TODO: убрать при реализации фильтрации

            SupportCompanies[index] = newSupportCompany;
        }

        public void ChangeUnitClick(Battalion oldBattalion)
        {
            CurrentBattalion = oldBattalion;
            AddBattalionCommand.Execute(null);

            OnPropertyChanged(nameof(DeleteCBBVisibility));
        }
        public void ChangeUnitClick(SupportCompany oldSupportCompany)
        {
            CurrentSupportCompany = oldSupportCompany;
            AddSupportCompanyCommand.Execute(null);

            OnPropertyChanged(nameof(DeleteCSCBVisibility));
        }

        public void UpdateDivisionProperties()
        {
            OnPropertyChanged(nameof(ConstructorDivision));
            OnPropertyChanged(nameof(DivisionName));
            OnPropertyChanged(nameof(DivisionPathToIcon));
            OnPropertyChanged(nameof(EditMode));
            OnPropertyChanged(nameof(EditDivisionNameButtonBackground));
            OnPropertyChanged(nameof(ForbidEditDivisionName));
        }
        public void ApplyFilter(string type)
        {
            switch (type)
            {
                case "Tnk":
                    {
                        FilterType = BattalionType.Tank;
                        break;
                    }
                case "Mob":
                    {
                        FilterType = BattalionType.Mobile;
                        break;
                    }
                case "Inf":
                    {
                        FilterType = BattalionType.Infantry;
                        break;
                    }
                default:
                    {
                        FilterType = BattalionType.All;
                        break;
                    }

            }
        }
    }
}