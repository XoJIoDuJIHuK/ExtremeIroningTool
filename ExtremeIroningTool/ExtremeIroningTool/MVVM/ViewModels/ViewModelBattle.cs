using ExtremeIroningTool.MVVM.Views;
using ExtremeIroningTool.Utilitary_classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ExtremeIroningTool.MVVM.ViewModels
{
    public class ViewModelBattle : INotifyPropertyChanged
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        #region Battle constants

        private const double BASE_CHANCE_TO_AVOID_HIT = 0.9;
        private const double CHANCE_TO_AVOID_HIT_AT_NO_DEF = 0.6;
        private const double ARMOR_DEFLECTION_FACTOR = 0.5;
        private const double HEALTH_DAMAGE_MODIFIER = 0.06;
        private const double ORG_DAMAGE_MODIFIER = 0.053;

        #endregion

        public Battle view;
        private Random random = new();
        private bool continueBattle = false;
        public bool ContinueBattle
        {
            get { return continueBattle; }
            set
            {
                continueBattle = value;
                OnPropertyChanged(nameof(ContinueBattle));
            }
        }

        private string logText = string.Empty;
        public string LogText
        {
            get
            {
                lock (logText)
                {
                    return logText;
                }
            }
            set
            {
                lock (logText)
                {
                    logText = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand BackBattleClickCommand { get { return view.mainWindow.viewModel.BackBattleClickCommand; } }
        public ICommand ResetBattleCommand { get; set; }
        public ICommand StartBattleCommand { get; set; }

        public Visibility StartBattleButtonVisibility
        {
            get
            {
                return ContinueBattle ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public Visibility ResetBattleButtonVisibility
        {
            get
            {
                return ContinueBattle ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public ViewModelBattle(Battle view)
        {
            this.view = view;
            view.DataContext = this;

            ResetBattleCommand = new RelayCommand(ResetBattleClick);
            StartBattleCommand = new RelayCommand(StartBattle);

            SpeedUp = new RelayCommand(() =>
            {
                Speed -= 10;
            });
            SlowDown = new RelayCommand(() =>
            {
                Speed += 10;
            });

            SelectedTerrain = DataBaseInteraction.DBTerrainTypes.FirstOrDefault();
            view.TerrainTypesComboBox.SelectedIndex = 0;
        }

        public LandForces Attackers
        {
            get
            {
                return view.mainWindow.viewModel.model.Attackers;
            }
        }
        public LandForces Defenders
        {
            get
            {
                return view.mainWindow.viewModel.model.Defenders;
            }
        }

        public ObservableCollection<ArmyGroup> AttackerArmy { get { return Attackers.ArmyGroups; } }
        public ObservableCollection<ArmyGroup> DefenderArmy { get { return Defenders.ArmyGroups; } }

        public void ResetBattleClick()
        {
            ContinueBattle = false;
            OnPropertyChanged(nameof(StartBattleButtonVisibility));
            OnPropertyChanged(nameof(ResetBattleButtonVisibility));
            Attackers.AllDivisions.Clear();
            Defenders.AllDivisions.Clear();

            foreach (var group in AttackerArmy)
            {
                foreach (var army in group.Armies)
                {
                    foreach (var division in army.BattleDivisions)
                    {
                        Attackers.AllDivisions.Add(division);
                        division.Heal();
                        division.UpdateBattleStats();
                    }
                }
            }
            foreach (var group in DefenderArmy)
            {
                foreach (var army in group.Armies)
                {
                    foreach (var division in army.BattleDivisions)
                    {
                        Defenders.AllDivisions.Add(division);
                        division.Heal();
                        division.UpdateBattleStats();
                    }
                }
            }

            OnPropertyChanged(nameof(AttackerArmy));
            OnPropertyChanged(nameof(DefenderArmy));
        }

        public void Hide()
        {
            view.Visibility = System.Windows.Visibility.Collapsed;
        }
        public void Show()
        {
            view.Visibility = System.Windows.Visibility.Visible;
        }

        private bool Attack(Division division)//if attacking division was destroyed, return true
        {
            void OneAttack(Division attackingDivision, Division defendingDivision, bool first)
            {
                
                int amountOfSoftAttacks = (int)(attackingDivision.softAttack * (1 - defendingDivision.vehicleRatio));
                int amountOfHardAttacks = (int)(attackingDivision.hardAttack * defendingDivision.vehicleRatio);
                int amountOfAttacks = amountOfHardAttacks + amountOfSoftAttacks;
                amountOfAttacks = (int)(amountOfAttacks * SelectedTerrain.attackModifier);

                double divisionAttackMultiplier = 1;
                if (attackingDivision.modifiers.Where(u => u.type == SelectedTerrain.type).Any())
                {
                    divisionAttackMultiplier = attackingDivision.modifiers.Where(u => u.type == SelectedTerrain.type).First().attackModifier;
                }
                amountOfAttacks = (int)(amountOfAttacks * divisionAttackMultiplier * SelectedTerrain.attackModifier);

                int amountOfHits;
                double def = first ? defendingDivision.defence : defendingDivision.breakthrough;

                double divisionDefenseMultiplier = 1;
                if (defendingDivision.modifiers.Where(u => u.type == SelectedTerrain.type).Any())
                {
                    divisionDefenseMultiplier = defendingDivision.modifiers.Where(u => u.type == SelectedTerrain.type).First().attackModifier;
                }
                def = (int)(amountOfAttacks * divisionDefenseMultiplier);

                if (amountOfAttacks > def)
                {
                    amountOfHits = (int)((1 - BASE_CHANCE_TO_AVOID_HIT) * def +
                        (1 - CHANCE_TO_AVOID_HIT_AT_NO_DEF) * (amountOfAttacks - def));
                }
                else
                {
                    amountOfHits = (int)((1 - BASE_CHANCE_TO_AVOID_HIT) * amountOfAttacks);
                }

                int healthDieSize = 2;
                int orgDieSize = attackingDivision.armor > defendingDivision.piercing ? 6 : 4;

                int healthParts = (int)(attackingDivision.health / attackingDivision.MaxHealth * 10);
                double attackHealthFactor = (double)healthParts / 10;

                double armorDamageFactor = defendingDivision.armor > attackingDivision.piercing ? ARMOR_DEFLECTION_FACTOR : 1;

                double mathExpectHealthDamage = (1 + healthDieSize) / 2 * amountOfHits * attackHealthFactor *
                    HEALTH_DAMAGE_MODIFIER * armorDamageFactor;
                double mathExpectOrgDamage = (1 + orgDieSize) / 2 * amountOfHits * ORG_DAMAGE_MODIFIER * armorDamageFactor;

                defendingDivision.health -= mathExpectHealthDamage;
                defendingDivision.organization -= mathExpectOrgDamage;

                WriteToLog($"{attackingDivision.Name} attacks{(!first ? " back" : "")} {defendingDivision.Name}, {Math.Round(mathExpectHealthDamage, 2)} DMG to Health, {Math.Round(mathExpectOrgDamage, 2)} DMG to Organization, terrain: {SelectedTerrain.type}");
            }

            //determine defending division
            Division defendingDivision = Defenders.AllDivisions[random.Next(0, Defenders.AllDivisions.Count)];
            //divisions attack each other
            OneAttack(division, defendingDivision, true);
            OneAttack(defendingDivision, division, false);

            division.UpdateBattleStats();
            defendingDivision.UpdateBattleStats();
            //remove destroyed divisions
            if (defendingDivision.health < 0 || defendingDivision.organization < 0)
                Defenders.AllDivisions.Remove(defendingDivision);
            if (division.health < 0 || division.organization < 0)
                return true;
            else return false;
        }
        public void StartBattle()
        {
            ContinueBattle = true;

            LogText = string.Empty;
            OnPropertyChanged(nameof(LogText));

            OnPropertyChanged(nameof(StartBattleButtonVisibility));
            OnPropertyChanged(nameof(ResetBattleButtonVisibility));

            Battle();
        }

        const string logFilePath = "./logFile";
        private readonly FileStream logFileStream = File.Create(logFilePath);

        public async void Battle()
        {
            logFileStream.Close();
            LogText = string.Empty;

            while (Attackers.AllDivisions.Count > 0 && Defenders.AllDivisions.Count > 0 && ContinueBattle)
            {
                for (int i = 0, frontCapacity = 0; i < Attackers.AllDivisions.Count; i++)
                {
                    if (frontCapacity < SelectedTerrain.combatWidth)
                    {
                        if (Defenders.AllDivisions.Count < 1) break;
                        if (Attack(Attackers.AllDivisions[i])) Attackers.AllDivisions.RemoveAt(i--);//if division was destroyed,
                                                                                                    //remove it
                        else frontCapacity += Attackers.AllDivisions[i].frontWidth;
                    }
                    else
                    {
                        break;
                    }
                }
                await Task.Delay(Speed);
            }
            LogText = File.ReadAllText(logFilePath);
            if (ContinueBattle) LogText += Attackers.AllDivisions.Count > 0 ? "Attackers won\n" : "Defenders won\n";
            ResetBattleCommand.Execute(null);
        }

        private void WriteToLog(string message)
        {
            //LogText += message;
            File.AppendAllLines(logFilePath, new List<string>() { message });
        }

        private Terrain selectedTerrain;
        public Terrain SelectedTerrain
        {
            get { return selectedTerrain; }
            set
            {
                if (!ContinueBattle) selectedTerrain = value;
                else view.TerrainTypesComboBox.SelectedItem = selectedTerrain;
            }
        }
        public string CurrentTerrainImageSource
        {
            get
            {
                return SelectedTerrain != null ? SelectedTerrain.PathToIcon : string.Empty;
            }
        }

        public ObservableCollection<Terrain> TerrainTypes
        {
            get { return DataBaseInteraction.DBTerrainTypes; }
        }

        private int speed = 50;
        public int Speed
        {
            get { return speed; }
            set
            {
                if (value >= 0) speed = value;
                OnPropertyChanged();
            }
        }

        public ICommand SpeedUp { get; set; }
        public ICommand SlowDown { get; set; }
    }
}
