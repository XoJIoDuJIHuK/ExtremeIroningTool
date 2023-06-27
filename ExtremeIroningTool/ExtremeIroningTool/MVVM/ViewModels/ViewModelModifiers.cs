using ExtremeIroningTool.MVVM.Views;
using ExtremeIroningTool.Utilitary_classes;
using ExtremeIroningTool.Utilitary_classes.DataBaseClasses;
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
using System.Windows.Media.Imaging;

namespace ExtremeIroningTool.MVVM.ViewModels
{
    public class ViewModelModifiers : INotifyPropertyChanged
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        public class ProxyModifier : INotifyPropertyChanged
        {
            #region INPC
            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string name = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
            #endregion

            public Modifiers self { get; set; }

            public ProxyModifier(Modifiers s) { self = s; }

            public string Name { get { return self.Name; } }
            public string BattalionType { get { return self.BattalionType; } }
            public string Property { get { return self.Property; } }
            public float? Modifier { get { return self.Modifier; } }
            public string PathToIcon { get { return self.PathToIcon; } }

            public bool FirstCountry { get; set; }
            public bool SecondCountry { get; set; }

            public void UpdateProperties()
            {
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(BattalionType));
                OnPropertyChanged(nameof(Property));
                OnPropertyChanged(nameof(Modifier));
                OnPropertyChanged(nameof(PathToIcon));
            }
        }

        public ViewModifiers view;
        private SelectImageWindow selectImageWindow = new()
        {
            IsArmyConfiguratorWindow = false,
            Topmost = true,
        };

        public ViewModelModifiers(ViewModifiers view)
        {
            this.view = view;
            view.DataContext = this;

            selectImageWindow.DataContext = this;

            ModifiersList = new();
            foreach (var m in DataBaseInteraction.DBModifiers) ModifiersList.Add(new(m));

            PropertiesList = new()
            {
                "Health",
                "Organization",
                "Soft attack",
                "Hard attack",
                "Defense",
                "Breakthrough",
                "Armor",
                "Piercing",
                "Front width",
            };
            TypesList = new()
            {
                "tnk",
                "mob",
                "inf"
            };

            CancelCommand = new RelayCommand(CancelEditClick);
            SaveCommand = new RelayCommand(SaveClick);

            ChangeIconCommand = new RelayCommand(ChangeIconClick);
            hideEditIconWindow = new RelayCommand(CloseSelectIconWindowClick);

            AddCommand = new RelayCommand(() => Edit(new() { PathToIcon = CurrentPathToIcon}));

            CurrentPathToIcon = "./../../Images/Modifiers/1.png";
        }

        public List<ProxyModifier> ModifiersList
        {
            get; set;
        }
        public List<ProxyModifier> FilteredModifiersList
        {
            get
            {
                var ret = new List<ProxyModifier>();

                if (SearchText == string.Empty)
                {
                    foreach (var m in ModifiersList)
                    {
                        ret.Add(m);
                    }
                }
                else
                {
                    foreach (var m in ModifiersList.Where(u => u.Name.Contains(SearchText)))
                    {
                        ret.Add(m);
                    }
                }

                return ret;
            }
        }

        public ICommand BackCommand { get { return view.mainWindow.viewModel.BackModifiersCommand; } }
        public ICommand ForwardCommand { get { return view.mainWindow.viewModel.ForwardModifiersCommand; } }

        public ICommand CancelCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public ICommand ChangeIconCommand { get; set; }
        public ICommand hideEditIconWindow { get; set; }

        public ICommand AddCommand { get; set; }

        public List<string> PropertiesList { get; set; }
        public List<string> TypesList { get; set; }

        public string FirstStateFlag
        {
            get { return DataBaseInteraction.allCountries[view.mainWindow.viewModel.model.selectedCountries[0]].pathToIcon; }
        }
        public string SecondStateFlag
        {
            get { return DataBaseInteraction.allCountries[view.mainWindow.viewModel.model.selectedCountries[1]].pathToIcon; }
        }

        public void UpdateFlags()
        {
            OnPropertyChanged(nameof(FirstStateFlag));
            OnPropertyChanged(nameof(SecondStateFlag));
        }

        private string searchText = string.Empty;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ModifiersList));
            }
        }

        private Modifiers? OriginalModifier = null;

        public string CurrentPathToIcon { get; set; }

        public void Delete(Modifiers modifier)
        {
            DataBaseInteraction.DeleteModifier(modifier.Name);
            ModifiersList.Remove(ModifiersList.Where(u => u.Name == modifier.Name).First());
            OnPropertyChanged(nameof(FilteredModifiersList));
        }
        public void Edit(Modifiers modifier)
        {
            OriginalModifier = modifier;
            GetSelectedModifiers();

            view.ChangeModifierName.Text = modifier.Name;
            view.ChangeModifierValue.Text = modifier.Modifier.ToString();
            view.BattalionType.Text = modifier.BattalionType;
            view.Property.Text = modifier.Property;

            CurrentPathToIcon = modifier.PathToIcon;

            OnPropertyChanged(nameof(CurrentPathToIcon));
            OnPropertyChanged(nameof(FilteredModifiersList));

            view.Select.Visibility = Visibility.Collapsed;
            view.Edit.Visibility = Visibility.Visible;
        }
        public void CancelEditClick()
        {
            view.ChangeModifierName.Text = string.Empty;
            view.ChangeModifierValue.Text = string.Empty;

            OriginalModifier = null;

            view.Select.Visibility = Visibility.Visible;
            view.Edit.Visibility = Visibility.Collapsed;
        }
        public void SaveClick()
        {
            var name = view.ChangeModifierName.Text;
            var battalionType = view.BattalionType.Text;
            var Property = view.Property.Text;
            var value = view.ChangeModifierValue.Text.Replace('.', ',');

            if (name == string.Empty || value == string.Empty || !float.TryParse(value, out float modifier) || modifier < 0 || !TypesList.Contains(battalionType) ||
                !PropertiesList.Contains(Property))
            {
                MessageBox.Show("Invalid input");
                return;
            }

            if (OriginalModifier != null && name == OriginalModifier.Name)
            {
                DataBaseInteraction.UpdateModifier(OriginalModifier.Name, new Modifiers()
                {
                    Name = name,
                    BattalionType = battalionType,
                    Property = Property,
                    Modifier = modifier,
                    PathToIcon = CurrentPathToIcon
                });

                OriginalModifier.Name = name;
                OriginalModifier.BattalionType = battalionType;
                OriginalModifier.Property = Property;
                OriginalModifier.Modifier = modifier;
                OriginalModifier.PathToIcon = CurrentPathToIcon;

                OriginalModifier.UpdateProperties();

                ModifiersList.Where(u => u.Name == OriginalModifier.Name).First().UpdateProperties();
            }
            else
            {
                if (DataBaseInteraction.DBModifiers.Where(u => u.Name == name).Any())
                {
                    name = UtilitaryFunctions.GetModifierDefaultName();
                    if (name == string.Empty) return;
                }
                var m = new Modifiers()
                {
                    Name = name,
                    BattalionType = battalionType,
                    Property = Property,
                    Modifier = modifier,
                    PathToIcon = CurrentPathToIcon
                };
                DataBaseInteraction.Save(m);

                ModifiersList.Add(new(m));

                OnPropertyChanged(nameof(FilteredModifiersList));
            }

            OriginalModifier = null;

            view.ChangeModifierName.Text = string.Empty;
            view.ChangeModifierValue.Text = string.Empty;
            view.BattalionType.Text = TypesList[0];
            view.Property.Text = PropertiesList[0];

            view.Select.Visibility = Visibility.Visible;
            view.Edit.Visibility = Visibility.Collapsed;
        }

        public void ChangeIcon(BitmapImage image)
        {
            CurrentPathToIcon = image.UriSource.OriginalString;
            OnPropertyChanged(nameof(CurrentPathToIcon));
            hideEditIconWindow.Execute(null);
        }
        public List<BitmapImage> CurrentListOfIcons
        {
            get
            {
                return DataBaseInteraction.DBModifiersIcons;
            }
        }

        public void ChangeIconClick()
        {
            selectImageWindow.ShowDialog();
        }
        public void CloseSelectIconWindowClick()
        {
            selectImageWindow.Hide();
        }

        public void Hide()
        {
            view.Visibility = Visibility.Collapsed;
        }
        public void Show()
        {
            view.Visibility = Visibility.Visible;
        }

        public void CheckFirstCheckbox(ProxyModifier modifier)
        {
            var m = ModifiersList.Where(u => u.Name == modifier.Name).First();
            m.FirstCountry = !m.FirstCountry;
        }
        public void CheckSecondCheckbox(ProxyModifier modifier)
        {
            var m = ModifiersList.Where(u => u.Name == modifier.Name).First();
            m.SecondCountry = !m.SecondCountry;
        }

        public List<List<Modifiers>> GetSelectedModifiers()
        {
            var ret = new List<List<Modifiers>>()
            {
                new(),
                new()
            };

            SearchText = string.Empty;

            foreach (var m in ModifiersList)
            {
                if (m.FirstCountry) ret[0].Add(m.self);
                if (m.SecondCountry) ret[1].Add(m.self);
            }

            return ret;
        }
    }
}
