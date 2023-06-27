using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace ExtremeIroningTool.Utilitary_classes
{
    public static class UtilitaryFunctions
    {
        public static void SetImageSource(Image image, string path)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            bitmap.EndInit();
            image.Source = bitmap;
        }
        public static BitmapImage ToBitmapImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }
        public static bool Contains(this ObservableCollection<Division> collection, string name)
        {
            foreach (Division d in collection)
            {
                if (d.name == name) return true;
            }
            return false;
        }
        public static string GetDefaultName(string type)
        {
            try
            {
                HashSet<string> allNames;
                string placeholder;

                switch (type.ToLower())
                {
                    case "army group":
                        {
                            allNames = LandForces.allArmyGroupNames;
                            placeholder = "army group";
                            break;
                        }
                    case "army":
                        {
                            allNames = LandForces.allArmyNames;
                            placeholder = "army";
                            break;
                        }
                    default:
                        {
                            allNames = LandForces.allDivisionNames;
                            placeholder = "division";
                            break;
                        }
                }

                string name = string.Empty;
                string defaultName = $"{placeholder} ";
                for (int i = 1; i < 999999; i++)
                {
                    if (!allNames.Contains(defaultName + i.ToString()))
                    {
                        allNames.Add(defaultName + i.ToString());
                        name = defaultName + i.ToString();
                        break;
                    }
                    if (i == 999998) throw new Exception($"No more default {placeholder} names?");
                }
                return name;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                //mainWindow.viewModel.ExitApplicationCommand.Execute(null);
                return "Alarm, exit application";
            }//name
        }
        public static bool AcceptableDivisionName(string name)
        {
            return !LandForces.allDivisionNames.Contains(name);
        }
        public static bool AcceptableArmyName(string name)
        {
            return !LandForces.allArmyNames.Contains(name);
        }
        public static bool AcceptableArmyGroupName(string name)
        {
            return !LandForces.allArmyGroupNames.Contains(name);
        }
        public static Dictionary<Battalion, byte> Clone(this Dictionary<Battalion, byte> collection)
        {
            Dictionary<Battalion, byte> ret = new();

            foreach(var d in collection)
            {
                ret.Add(d.Key, d.Value);
            }
            return ret;
        }
        public static List<SupportCompany> Clone(this List<SupportCompany> collection)
        {
            List<SupportCompany> ret = new();

            foreach (var d in collection)
            {
                ret.Add(d);
            }
            return ret;
        }
        public static bool ContainsKey(this ObservableCollection<UnitDictionaryElement> collection, Division division)
        {
            foreach (var d in collection)
            {
                if (division.Equals(d.Key)) return true;
            }
            return false;
        }
        public static int GetAmountOfUnits(this ObservableCollection<UnitDictionaryElement> collection, Division division)
        {
            foreach (var d in collection)
            {
                if (division.Equals(d.Key)) return d.Value;
            }
            return 0;
        }
        public static int IndexOf(this ObservableCollection<UnitDictionaryElement> collection, Unit? unit)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                if (unit.Equals(collection[i].Key)) return i;
            }
            return -1;
        }
        public static void OnPropertyChanged(this ObservableCollection<UnitDictionaryElement> collection)
        {
            foreach (var d in collection)
            {
                d.OnPropertyChanged(nameof(d.Value));
                d.OnPropertyChanged(nameof(d.Key));
            }
        }
        public static string GetModifierDefaultName()
        {
            for (int i = 0; i < 999999; i++)
            {
                var name = $"Modifier {i}";
                if (!DataBaseInteraction.DBModifiers.Where(u => u.Name == name).Any()) return name;
            }
            MessageBox.Show("No default modifier names?");
            return string.Empty;
        }
    }
}
