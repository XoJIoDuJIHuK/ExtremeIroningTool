using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using ExtremeIroningTool.Utilitary_classes.DataBaseClasses;
using System.Windows;
using ExtremeIroningTool.MVVM.ViewModels;
using System.IO;
using BCrypt.Net;
using Windows.UI.WebUI;

namespace ExtremeIroningTool.Utilitary_classes
{
    public class DataBaseInteraction
    {
        private static ExtremeIroningDatabaseContext databaseContext = new();

        public static ObservableCollection<ArmyGroup> DBArmyGroups = new();
        public static ObservableCollection<Division> DBDivisions = new();
        public static ObservableCollection<Battalion> DBBattalions = new();
        public static ObservableCollection<SupportCompany> DBSupportCompanies = new();
        public static ObservableCollection<General> DBGenerals = new();
        public static ObservableCollection<General> DBFieldMarshals = new();
        public static ObservableCollection<Modifiers> DBModifiers = new();
        public static ObservableCollection<Terrain> DBTerrainTypes = new();

        public static List<BitmapImage> DBArmyGroupIcons = new();
        public static List<BitmapImage> DBArmyIcons = new();
        public static List<BitmapImage> DBDivisionIcons = new();
        public static List<BitmapImage> DBModifiersIcons = new();

        public static ObservableCollection<Tracks> DBTracks = new();

        public static List<Country> allCountries = new();

        public static List<TerrainType> allTerrainTypes = Enum.GetValues(typeof(TerrainType)).OfType<TerrainType>().ToList();

        public static SqlConnection sqlConnection = new SqlConnection("server=XOJIODUJIHUK\\MSQL_SERVER_V1;" +
                "Trusted_Connection=Yes;" +
                "DataBase=ExtremeIroningDatabase;" +
                "Encrypt=False");

        public static bool GetDBUnits()
        {
            DBBattalions = new();
            DBSupportCompanies = new();

            var file = File.Create("battalions.json");
            file.Close();

            file = File.Create("imports.json");
            file.Close();

            foreach (var u in databaseContext.Battalions.ToList())
            {
                string path = u.PathToIcon.Substring(u.PathToIcon.LastIndexOf('/') + 1);
                string name = u.BattalionName;
                while (name.Contains(' ')) name = name.Remove(name.IndexOf(' '), 1);
                while (name.Contains('-')) name = name.Remove(name.IndexOf('-'), 1);
                var importName = name + "Icon";

                File.AppendAllText("battalions.json", "{\n" + $"name:'{u.BattalionName}',\nicon:{importName}\n" + "},\n");
                File.AppendAllText("imports.json", $"import {importName} from './images/Battalion Icons/{path}'\n");

                UnitSpecificAttackAdjusters dbAttackModifiers;
                UnitSpecificDefenseAdjusters dbDefenseModifiers;

                dbAttackModifiers = databaseContext.UnitSpecificAttackAdjusters.Where(unit => unit.
                    BattalionName == u.BattalionName).First();
                dbDefenseModifiers = databaseContext.UnitSpecificDefenseAdjusters.Where(unit => unit.
                    BattalionName == u.BattalionName).First();

                var modifiers = new List<TerrainModifier>()
                {
                    new(TerrainType.forest, (double)dbAttackModifiers.Forest, (double)dbDefenseModifiers.Forest),
                    new(TerrainType.hill, (double)dbAttackModifiers.Hill, (double)dbDefenseModifiers.Hill),
                    new(TerrainType.mountains, (double)dbAttackModifiers.Mountains, (double)dbDefenseModifiers.Mountains),
                    new(TerrainType.plains, (double)dbAttackModifiers.Plains, (double)dbDefenseModifiers.Plains),
                    new(TerrainType.urban, (double)dbAttackModifiers.Urban, (double)dbDefenseModifiers.Urban),
                    new(TerrainType.jungle, (double)dbAttackModifiers.Jungle, (double)dbDefenseModifiers.Jungle),
                    new(TerrainType.marsh, (double)dbAttackModifiers.Marsh, (double)dbDefenseModifiers.Marsh),
                    new(TerrainType.desert, (double)dbAttackModifiers.Desert, (double)dbDefenseModifiers.Desert),
                    new(TerrainType.river, (double)dbAttackModifiers.River, (double)dbDefenseModifiers.River),
                    new(TerrainType.amphibious, (double)dbAttackModifiers.Amphibious, (double)dbDefenseModifiers.Amphibious),
                    new(TerrainType.fort, (double)dbAttackModifiers.Fort, (double)dbDefenseModifiers.Fort),
                };

                if (u.Type == "sup")
                {
                    DBSupportCompanies.Add(new SupportCompany(u.BattalionName, u.Health, u.Organization,
                        u.SoftAttack, u.HardAttack, u.Defence, u.Breakthrough,
                        u.Armor, u.Piercing, u.FrontWidth, u.PathToIcon, u.VehicleRatio,
                        modifiers));
                }
                else
                {
                    DBBattalions.Add(new Battalion(u.BattalionName, u.Health, u.Organization,
                        u.SoftAttack, u.HardAttack, u.Defence, u.Breakthrough,
                        u.Armor, u.Piercing, u.FrontWidth, u.PathToIcon, u.VehicleRatio,
                        modifiers, u.Type == "inf" ? BattalionType.Infantry : u.Type == "tnk" ?
                        BattalionType.Tank : BattalionType.Mobile));
                }
            }

            return true;
        }
        public static bool GetGeneralStaff()
        {
            DBGenerals = new();
            DBFieldMarshals = new();

            foreach (var g in databaseContext.Generals)
            {
                if (g.Rank == "General")
                {
                    DBGenerals.Add(new(g.GeneralName, g.AttackBonus, g.DefenseBonus, g.PathToIcon,
                        CommanderRank.General, g.Country));
                }
                else
                {
                    DBFieldMarshals.Add(new(g.GeneralName, g.AttackBonus, g.DefenseBonus, g.PathToIcon,
                        CommanderRank.Fieldmarshall, g.Country));
                }
            }

            return true;
        }
        public static bool GetArmyGroups()
        {
            DBArmyGroups = new();

            LandForces.allDivisionNames.Clear();
            LandForces.allDivisionNames.Add("EmptyName");
            LandForces.allArmyNames.Clear();
            LandForces.allArmyGroupNames.Clear();

            foreach (var ag in databaseContext.ArmyGroups.ToList())
            {
                var armies = new ObservableCollection<Army>();

                var dbArmies = databaseContext.ContentsOfArmyGroups.Where(u => u.ArmyGroup == ag.ArmyGroupName).ToList();

                foreach (var a in dbArmies)
                {
                    var army = databaseContext.Armies.Where(u => u.ArmyName == a.ArmyName).First();

                    var divisions = new ObservableCollection<UnitDictionaryElement>();

                    var dbDivisions = databaseContext.ContentsOfArmies.Where(u => u.ArmyName == a.ArmyName).ToList();

                    foreach (var d in dbDivisions)
                    {
                        divisions.Add(new(DBDivisions.Where(u => u.Name == d.DivisionName).First(), d.DivisionCount));

                        LandForces.allDivisionNames.Add(d.DivisionName);
                    }
                    LandForces.allArmyNames.Add(a.ArmyName);

                    armies.Add(new(divisions, army.ArmyName, General.nullGeneral, army.PathToIcon));
                }

                LandForces.allArmyGroupNames.Add(ag.ArmyGroupName);

                DBArmyGroups.Add(new(General.nullFieldMarshal, armies, ag.ArmyGroupName, ag.PathToIcon));
            }

            return true;
        }
        public static bool GetDivisions()
        {
            DBDivisions = new();

            foreach (var d in databaseContext.Divisions.ToList())
            {
                var battalions = new ObservableCollection<UnitDictionaryElement>();
                var supportCompanies = new ObservableCollection<SupportCompany>();

                var unitNames = databaseContext.ContentsOfDivisions.Where(u => u.DivisionName == d.DivisionName).ToList();

                foreach (var name in unitNames)
                {
                    Battalions unit = databaseContext.Battalions.Where(u => u.BattalionName == name.BattalionName).First();

                    if (unit.Type != "sup")
                    {
                        battalions.Add(new(DBBattalions.Where(b => b.name == unit.BattalionName).First(),
                            name.BattalionCount));
                    }
                    else
                    {
                        supportCompanies.Add(DBSupportCompanies.Where(u => u.name == unit.BattalionName).First());
                    }
                }

                DBDivisions.Add(new(d.DivisionName, d.PathToIcon, battalions, supportCompanies));
            }

            return true;
        }
        public static bool GetCountries()
        {
            foreach (var c in databaseContext.Countries)
            {
                allCountries.Add(new(c.CountryTag, c.CountryName, c.PathToIcon, c.PathToPoster, c.Description));
            }

            return true;
        }
        public static bool GetIcons()
        {
            //Divisions
            foreach (var p in databaseContext.PathsToDivisionIcons)
            {
                DBDivisionIcons.Add(UtilitaryFunctions.ToBitmapImage(p.Path));
            }
            //Armies
            foreach (var p in databaseContext.PathsToArmyIcons)
            {
                DBArmyIcons.Add(UtilitaryFunctions.ToBitmapImage(p.Path));
            }
            //Army groups
            foreach (var p in databaseContext.PathsToArmyGroupIcons)
            {
                DBArmyGroupIcons.Add(UtilitaryFunctions.ToBitmapImage(p.Path));
            }
            //Modifiers
            foreach (var p in databaseContext.PathsToModifiersIcons)
            {
                DBModifiersIcons.Add(UtilitaryFunctions.ToBitmapImage(p.Path));
            }

            return true;
        }
        public static void GetTracks()
        {
            DBTracks = new();
            foreach (var t in databaseContext.Tracks)
            {
                DBTracks.Add(t);
            }
        }
        public static void GetModifiers()
        {
            DBModifiers.Clear();

            foreach (var m in databaseContext.Modifiers)
            {
                DBModifiers.Add(new()
                {
                    Name = m.Name,
                    Property = m.Property,
                    Modifier = m.Modifier,
                    PathToIcon = m.PathToIcon,
                    BattalionType = m.BattalionType
                });
            }
        }
        public static void GetTerrainTypes()
        {
            foreach (var tt in databaseContext.TerrainTypes)
            {
                DBTerrainTypes.Add(new(tt.TerrainType, (double)tt.AttackModifier, (byte)tt.CombatWidth));
            }
        }
        
        public static int Save(ArmyGroup ag)
        {
            int ret = 0;

            if (databaseContext.ArmyGroups.Where(u => u.ArmyGroupName == ag.Name).Count() != 0)
            {
                ret = 1;

                foreach (var cag in databaseContext.ContentsOfArmyGroups.Where(u => u.ArmyGroup == ag.Name))
                {
                    databaseContext.ContentsOfArmyGroups.Remove(cag);
                }
                databaseContext.ArmyGroups.Where(u => u.ArmyGroupName == ag.Name).First().PathToIcon = ag.PathToIcon;
            }
            else
            {
                databaseContext.ArmyGroups.Add(new()
                {
                    ArmyGroupName =  ag.Name,
                    PathToIcon = ag.PathToIcon
                });
            }
            databaseContext.SaveChanges();
            foreach (var a in ag.Armies)
            {
                Save(ag, a);
                databaseContext.SaveChanges();
                //var coag = new ContentsOfArmyGroups()
                //{
                //    ArmyGroup = ag.Name,
                //    ArmyName = a.Name
                //};
                //databaseContext.ContentsOfArmyGroups.Add(coag);
            }
            databaseContext.SaveChanges();
            GetArmyGroups();

            return ret;
        }
        public static int Save(ArmyGroup ag, Army a)
        {
            int ret = 0;

            if (databaseContext.Armies.Where(u => u.ArmyName == a.Name).Count() != 0)//if exists, rewrite
            {
                ret = 1;

                foreach (var ca in databaseContext.ContentsOfArmies.Where(u => u.ArmyName == a.Name))
                {
                    databaseContext.ContentsOfArmies.Remove(ca);
                }
                databaseContext.Armies.Where(u => u.ArmyName == a.Name).First().PathToIcon = a.PathToIcon;
            }
            else //add
            {
                databaseContext.Armies.Add(new Armies() { ArmyName = a.Name, PathToIcon = a.PathToIcon });
            }

            if (databaseContext.ContentsOfArmyGroups.Where(dbag => ag.Name == dbag.ArmyGroup && a.Name == dbag.ArmyName).Count() == 0)
            {
                databaseContext.ContentsOfArmyGroups.Add(new()
                {
                    ArmyName = a.Name,
                    ArmyGroup = ag.Name
                });
            }

            databaseContext.SaveChanges();
            foreach (var d in a.Divisions)
            {
                databaseContext.ContentsOfArmies.Add(new ContentsOfArmies()
                {
                    ArmyName = a.Name,
                    DivisionName = d.Key.name,
                    DivisionCount = d.Value
                });
            }
            databaseContext.SaveChanges();
            GetArmyGroups();

            return ret;
        }
        public static int Save(Division newDivision, Division? oldDivision = null)
        {
            int ret = 0;

            if (oldDivision != null)//if overwrite
            {
                if (databaseContext.Divisions.Where(u => u.DivisionName == oldDivision.Name).Count() == 0) return -1;

                ret = 1;

                if (newDivision.Name == oldDivision.Name)//if name wasn't changed
                {
                    //remove old lines
                    foreach (var c in databaseContext.ContentsOfDivisions.Where(u => u.DivisionName == oldDivision.Name))
                    {
                        databaseContext.ContentsOfDivisions.Remove(c);
                    }
                    //Add battalions
                    foreach (var b in newDivision.Battalions)
                    {
                        databaseContext.ContentsOfDivisions.Add(new()
                        {
                            DivisionName = newDivision.Name,
                            BattalionName = b.Key.name,
                            BattalionCount = b.Value < 255 ? (byte)b.Value : (byte)255
                        });
                    }
                    //Add support companies
                    foreach (var s in newDivision.SupportCompanies)
                    {
                        databaseContext.ContentsOfDivisions.Add(new()
                        {
                            DivisionName = newDivision.Name,
                            BattalionName = s.name,
                            BattalionCount = 1
                        });
                    }
                    databaseContext.Divisions.Where(u => u.DivisionName == newDivision.Name).First().PathToIcon = newDivision.PathToIcon;
                }
                else //if division was renamed
                {
                    //add new division
                    databaseContext.Divisions.Add(new()
                    {
                        DivisionName = newDivision.Name,
                        PathToIcon = newDivision.PathToIcon
                    });
                    //remove old lines
                    foreach (var c in databaseContext.ContentsOfDivisions.Where(u => u.DivisionName == oldDivision.Name))
                    {
                        databaseContext.ContentsOfDivisions.Remove(c);
                    }
                    foreach (var d in databaseContext.ContentsOfArmies.Where(u => u.DivisionName == oldDivision.Name))
                    {
                        d.DivisionName = newDivision.Name;
                    }
                    //delete old division
                    databaseContext.Divisions.Remove(databaseContext.Divisions.Where(u => u.DivisionName == oldDivision.Name).First());
                    //Add battalions
                    foreach (var b in newDivision.Battalions)
                    {
                        databaseContext.ContentsOfDivisions.Add(new()
                        {
                            DivisionName = newDivision.Name,
                            BattalionName = b.Key.name,
                            BattalionCount = b.Value < 255 ? (byte)b.Value : (byte)255
                        });
                    }
                    //Add support companies
                    foreach (var s in newDivision.SupportCompanies)
                    {
                        databaseContext.ContentsOfDivisions.Add(new()
                        {
                            DivisionName = newDivision.Name,
                            BattalionName = s.name,
                            BattalionCount = 1
                        });
                    }
                }
                //overwrite old division in DBDivisions with new division
                for (int i = 0; i < DBDivisions.Count; i++)
                {
                    if (DBDivisions[i] == oldDivision)
                    {
                        DBDivisions[i] = newDivision;
                        newDivision.UpdateProperties();
                        break;
                    }
                }
            }
            else//if add
            {
                databaseContext.Divisions.Add(new()
                {
                    DivisionName = newDivision.Name,
                    PathToIcon = newDivision.PathToIcon
                });
                //Add battalions
                foreach (var b in newDivision.Battalions)
                {
                    databaseContext.ContentsOfDivisions.Add(new()
                    {
                        DivisionName = newDivision.Name,
                        BattalionName = b.Key.name,
                        BattalionCount = b.Value < 255 ? (byte)b.Value : (byte)255
                    });
                }
                //Add support companies
                foreach (var s in newDivision.SupportCompanies)
                {
                    databaseContext.ContentsOfDivisions.Add(new()
                    {
                        DivisionName = newDivision.Name,
                        BattalionName = s.name,
                        BattalionCount = 1
                    });
                }
            }
            databaseContext.SaveChanges();

            return ret;
        }

        public static bool Delete(ArmyGroup ag)
        {
            if (databaseContext.ArmyGroups.Where(u => u.ArmyGroupName == ag.Name).Count() == 0)
            {
                return false;
            }

            foreach (var c in databaseContext.ContentsOfArmyGroups.Where(u => u.ArmyGroup == ag.Name))
            {
                databaseContext.ContentsOfArmyGroups.Remove(c);
            }
            foreach (var c in databaseContext.ArmyGroups.Where(u => u.ArmyGroupName == ag.Name))
            {
                databaseContext.ArmyGroups.Remove(c);
            }
            databaseContext.SaveChanges();
            LandForces.allArmyGroupNames.Remove(ag.Name);
            GetArmyGroups();

            return true;
        }
        public static bool Delete(Army a)
        {
            if (databaseContext.Armies.Where(u => u.ArmyName == a.Name).Count() == 0)
            {
                return false;
            }

            foreach (var c in databaseContext.ContentsOfArmyGroups.Where(u => u.ArmyName == a.Name))
            {
                databaseContext.ContentsOfArmyGroups.Remove(c);
            }
            foreach (var c in databaseContext.ContentsOfArmies.Where(u => u.ArmyName == a.Name))
            {
                databaseContext.ContentsOfArmies.Remove(c);
            }
            foreach (var c in databaseContext.Armies.Where(u => u.ArmyName == a.Name))
            {
                databaseContext.Armies.Remove(c);
            }
            databaseContext.SaveChanges();
            LandForces.allArmyNames.Remove(a.Name);
            GetArmyGroups();

            return true;
        }
        public static bool Delete(Division d)
        {
            if (databaseContext.Divisions.Where(u => u.DivisionName ==  d.Name).Count() == 0)
            {
                return false;
            }

            foreach (var ca in databaseContext.ContentsOfArmies.Where(u => u.DivisionName == d.Name))
            {
                databaseContext.ContentsOfArmies.Remove(ca);
            }
            foreach (var cd in databaseContext.ContentsOfDivisions.Where(u => u.DivisionName == d.Name))
            {
                databaseContext.ContentsOfDivisions.Remove(cd);
            }
            foreach (var div in databaseContext.Divisions.Where(u => u.DivisionName == d.Name))
            {
                databaseContext.Divisions.Remove(div);
            }
            databaseContext.SaveChanges();
            LandForces.allDivisionNames.Remove(d.Name);
            GetDivisions();
            GetArmyGroups();

            return true;
        }

        public static void Save(Modifiers modifier)
        {
            databaseContext.Modifiers.Add(new()
            {
                Name = modifier.Name,
                BattalionType = modifier.BattalionType,
                Property = modifier.Property,
                Modifier = modifier.Modifier,
                PathToIcon = modifier.PathToIcon
            });
            databaseContext.SaveChanges();
            DBModifiers.Add(databaseContext.Modifiers.Where(u => u.Name == modifier.Name).First());
        }
        public static bool DeleteModifier(string name)
        {
            if (databaseContext.Modifiers.Where(u => u.Name == name).Any())
            {
                var a = databaseContext.Modifiers.Where(u => u.Name == name).First();
                databaseContext.Modifiers.Remove(a);
                databaseContext.SaveChanges();
                DBModifiers.Remove(DBModifiers.Where(u => u.Name == name).First());
                return true;
            }
            return false;
        }
        public static bool UpdateModifier(string name, Modifiers modifier)
        {
            if (databaseContext.Modifiers.Where(u => u.Name == name).Any())
            {
                var mod = databaseContext.Modifiers.Where(u => u.Name == name).First();
                mod.Name = modifier.Name;
                mod.PathToIcon = modifier.PathToIcon;
                mod.Property = modifier.Property;
                mod.Modifier = modifier.Modifier;
                mod.BattalionType = modifier.BattalionType;
                databaseContext.SaveChanges();

                mod = DBModifiers.Where(u => u.Name == name).First();
                mod.Name = modifier.Name;
                mod.PathToIcon = modifier.PathToIcon;
                mod.Property = modifier.Property;
                mod.Modifier = modifier.Modifier;
                mod.BattalionType = modifier.BattalionType;

                return true;
            }
            return false;
        }

        public static Division? GetDivision(string name)
        {
            if (databaseContext.Divisions.Where(u => u.DivisionName == name).Count() == 0)
            {
                return null;
            }

            var dbdiv = databaseContext.Divisions.Where(u => u.DivisionName == name).First();

            var battalions = new ObservableCollection<UnitDictionaryElement>();
            var supportCompanies = new ObservableCollection<SupportCompany>();

            foreach (var b in databaseContext.ContentsOfDivisions.Where(u => u.DivisionName == name))
            {
                if (databaseContext.Battalions.Where(u => u.BattalionName == b.BattalionName).First().Type == "sup")
                {
                    battalions.Add(new(DBBattalions.Where(u => u.name == b.BattalionName).First(), b.BattalionCount));
                }
                else
                {
                    supportCompanies.Add(DBSupportCompanies.Where(u => u.name == b.BattalionName).First());
                }
            }

            return new Division()
            {
                Name = dbdiv.DivisionName,
                PathToIcon = dbdiv.PathToIcon,
                Battalions = battalions,
                SupportCompanies = supportCompanies
            };
        }

        public Battalion? GetBattalion(string name)
        {
            if (databaseContext.Battalions.Where(u => u.BattalionName == name).Count() == 0 ||
                databaseContext.Battalions.Where(u => u.BattalionName == name).First().Type == "sup")
            {
                return null;
            }

            var dbb = databaseContext.Battalions.Where(u => u.BattalionName == name).First();

            var dbAttackModifiers = databaseContext.UnitSpecificAttackAdjusters.Where(unit => unit.
                    BattalionName == dbb.BattalionName).First();
            var dbDefenseModifiers = databaseContext.UnitSpecificDefenseAdjusters.Where(unit => unit.
                BattalionName == dbb.BattalionName).First();

            var modifiers = new List<TerrainModifier>()
                {
                    new("Forest", (double)dbAttackModifiers.Forest, (double)dbDefenseModifiers.Forest),
                    new("Hill", (double)dbAttackModifiers.Hill, (double)dbDefenseModifiers.Hill),
                    new("Mountains", (double)dbAttackModifiers.Mountains, (double)dbDefenseModifiers.Mountains),
                    new("Plains", (double)dbAttackModifiers.Plains, (double)dbDefenseModifiers.Plains),
                    new("Urban", (double)dbAttackModifiers.Urban, (double)dbDefenseModifiers.Urban),
                    new("Jungle", (double)dbAttackModifiers.Jungle, (double)dbDefenseModifiers.Jungle),
                    new("Marsh", (double)dbAttackModifiers.Marsh, (double)dbDefenseModifiers.Marsh),
                    new("Desert", (double)dbAttackModifiers.Desert, (double)dbDefenseModifiers.Desert),
                    new("River", (double)dbAttackModifiers.River, (double)dbDefenseModifiers.River),
                    new("Amphibious", (double)dbAttackModifiers.Amphibious, (double)dbDefenseModifiers.Amphibious),
                    new("Fort", (double)dbAttackModifiers.Fort, (double)dbDefenseModifiers.Fort),
                };

            return new Battalion(dbb.BattalionName, dbb.Health, dbb.Organization, dbb.SoftAttack, dbb.HardAttack,
                dbb.Defence, dbb.Breakthrough, dbb.Armor, dbb.Piercing, dbb.FrontWidth, dbb.PathToIcon, dbb.VehicleRatio,
                modifiers, dbb.Type == "inf" ? BattalionType.Infantry : dbb.Type == "tnk" ? BattalionType.Tank :
                BattalionType.Mobile);
        }
        public SupportCompany? GetSupportCompany(string name)
        {
            if (databaseContext.Battalions.Where(u => u.BattalionName == name).Count() == 0 ||
                databaseContext.Battalions.Where(u => u.BattalionName == name).First().Type != "sup")
            {
                return null;
            }
            var dbsc = databaseContext.Battalions.Where(u => u.BattalionName == name).First();

            var dbAttackModifiers = databaseContext.UnitSpecificAttackAdjusters.Where(unit => unit.
                    BattalionName == dbsc.BattalionName).First();
            var dbDefenseModifiers = databaseContext.UnitSpecificDefenseAdjusters.Where(unit => unit.
                BattalionName == dbsc.BattalionName).First();

            var modifiers = new List<TerrainModifier>()
                {
                    new("Forest", (double)dbAttackModifiers.Forest, (double)dbDefenseModifiers.Forest),
                    new("Hill", (double)dbAttackModifiers.Hill, (double)dbDefenseModifiers.Hill),
                    new("Mountains", (double)dbAttackModifiers.Mountains, (double)dbDefenseModifiers.Mountains),
                    new("Plains", (double)dbAttackModifiers.Plains, (double)dbDefenseModifiers.Plains),
                    new("Urban", (double)dbAttackModifiers.Urban, (double)dbDefenseModifiers.Urban),
                    new("Jungle", (double)dbAttackModifiers.Jungle, (double)dbDefenseModifiers.Jungle),
                    new("Marsh", (double)dbAttackModifiers.Marsh, (double)dbDefenseModifiers.Marsh),
                    new("Desert", (double)dbAttackModifiers.Desert, (double)dbDefenseModifiers.Desert),
                    new("River", (double)dbAttackModifiers.River, (double)dbDefenseModifiers.River),
                    new("Amphibious", (double)dbAttackModifiers.Amphibious, (double)dbDefenseModifiers.Amphibious),
                    new("Fort", (double)dbAttackModifiers.Fort, (double)dbDefenseModifiers.Fort),
                };

            return new SupportCompany(dbsc.BattalionName, dbsc.Health, dbsc.Organization, dbsc.SoftAttack, dbsc.HardAttack,
                dbsc.Defence, dbsc.Breakthrough, dbsc.Armor, dbsc.Piercing, dbsc.FrontWidth, dbsc.PathToIcon, dbsc.VehicleRatio,
                modifiers);
        }

        public static bool TrackExists(string name)
        {
            return databaseContext.Tracks.Where(u => u.TrackName == name).Count() > 0;
        }
        public static void AddTrack(Tracks track)
        {
            databaseContext.Tracks.Add(track);
            databaseContext.SaveChanges();
            GetTracks();
        }
        public static void DeleteTrack(Tracks track)
        {
            databaseContext.Tracks.Remove(databaseContext.Tracks.Where(u => u.TrackName == track.TrackName).First());
            databaseContext.SaveChanges();
            GetTracks();
        }

        public static void UpdateBattalion(string BattalionName, string Property, string NewValue)
        {
            Unit dbBat;
            if (DBBattalions.Where(u => u.name == BattalionName).Any()) dbBat = DBBattalions.Where(u => u.name == BattalionName).First();
            else dbBat = DBSupportCompanies.Where(u => u.name == BattalionName).First();

            if (databaseContext.Battalions.Where(u => u.BattalionName == BattalionName).Any())
            {
                var b = databaseContext.Battalions.Where(u => u.BattalionName == BattalionName).First();

                switch (Property)
                {
                    case "Health":
                        {
                            b.Health = float.Parse(NewValue);
                            dbBat.health = b.Health;
                            break;
                        }
                    case "Organization":
                        {
                            b.Organization = float.Parse(NewValue);
                            dbBat.organization = b.Organization;
                            break;
                        }
                    case "SoftAttack":
                        {
                            b.SoftAttack = float.Parse(NewValue);
                            dbBat.softAttack = b.SoftAttack;
                            break;
                        }
                    case "HardAttack":
                        {
                            b.HardAttack = float.Parse(NewValue);
                            dbBat.hardAttack = b.HardAttack;
                            break;
                        }
                    case "Defense":
                        {
                            b.Defence = float.Parse(NewValue);
                            dbBat.defence = b.Defence;
                            break;
                        }
                    case "Breakthrough":
                        {
                            b.Breakthrough = float.Parse(NewValue);
                            dbBat.breakthrough = b.Breakthrough;
                            break;
                        }
                    case "Armor":
                        {
                            b.Armor = float.Parse(NewValue);
                            dbBat.armor = b.Armor;
                            break;
                        }
                    case "Piercing":
                        {
                            b.Piercing = float.Parse(NewValue);
                            dbBat.piercing = b.Piercing;
                            break;
                        }
                    case "FrontWidth":
                        {
                            b.FrontWidth = byte.Parse(NewValue);
                            dbBat.frontWidth = b.FrontWidth;
                            break;
                        }
                    case "PathToIcon":
                        {
                            b.PathToIcon = NewValue;
                            dbBat.pathToIcon = b.PathToIcon;
                            break;
                        }
                    default:
                        {
                            b.VehicleRatio = float.Parse(NewValue);
                            dbBat.vehicleRatio = b.VehicleRatio;
                            break;
                        }
                }

                databaseContext.SaveChanges();
            }
        }

        public static bool IsAdmin(string Name, string Password)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(Password);

            if (databaseContext.Admins.Where(u => u.UserName == Name).Any())
            {
                return BCrypt.Net.BCrypt.Verify(Password, databaseContext.Admins.Where(u => u.UserName == Name).First().Password);
            }

            return false;
        }

        public static bool IsBattalionOfType(string name, string type)
        {
            if (databaseContext.Battalions.Where(u => u.BattalionName == name).Any())
            {
                return databaseContext.Battalions.Where(u => u.BattalionName == name).First().Type == type;
            }
            return false;
        }
    }
}
