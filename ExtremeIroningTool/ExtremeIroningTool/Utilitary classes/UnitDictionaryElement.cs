using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ExtremeIroningTool.Utilitary_classes
{
    /// <summary>
    /// Needed for replacement of dictionary <Division> and <Battalion>
    /// </summary>
    public class UnitDictionaryElement : INotifyPropertyChanged
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        private Unit key;
        public Unit Key { get { return key; } set { key = value; OnPropertyChanged(); } }

        private int value;
        public int Value { get { return value; } set { this.value = value; OnPropertyChanged(); } }

        public string Name { get { return Key.name; } }
        public string PathToIcon { get { return Key.pathToIcon; } }

        public UnitDictionaryElement(Unit key, int value)
        {
            Key = key;
            Value = value;
        }

        public void UpdateDivision()
        {
            if (Key is Division d)
            {
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(PathToIcon));
            }
        }
    }
}
