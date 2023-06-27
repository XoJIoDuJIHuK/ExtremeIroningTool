using ExtremeIroningTool.MVVM.Views;
using ExtremeIroningTool.Utilitary_classes;
using ExtremeIroningTool.Utilitary_classes.DataBaseClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExtremeIroningTool.MVVM.ViewModels
{
    public class ViewModelBattalionEditor
    {
        public BattalionEditor view;

        public ICommand BackCommand
        {
            get { return view.mainWindow.viewModel.BackBattalionEditorCommand; }
        }

        public ViewModelBattalionEditor(BattalionEditor view)
        {
            this.view = view;
            view.DataContext = this;
        }

        public ObservableCollection<Unit> AllBattalions
        {
            get
            {
                var ret = new ObservableCollection<Unit>();

                foreach (var b in DataBaseInteraction.DBBattalions)
                {
                    ret.Add(b);
                }
                foreach (var s in DataBaseInteraction.DBSupportCompanies)
                {
                    ret.Add(s);
                }

                return ret;
            }
        }

        public bool UpdateBattalion(string BattalionName, string Property, string NewValue)
        {
            if (Property == "PathToIcon" && NewValue == string.Empty) return false;

            else if (Property == "FrontWidth" && (!byte.TryParse(NewValue, out byte ByteNewValue) || ByteNewValue > 255)) return false;

            else if (!float.TryParse(NewValue, out float FloatNewValue) || FloatNewValue < 0) return false;

            else
            {
                DataBaseInteraction.UpdateBattalion(BattalionName, Property, NewValue);
            }

            return true;
        }

        public void Hide()
        {
            view.Visibility = Visibility.Collapsed;
        }
        public void Show()
        {
            view.Visibility = Visibility.Visible;
        }
    }
}
