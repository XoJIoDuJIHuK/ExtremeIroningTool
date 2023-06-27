using ExtremeIroningTool.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtremeIroningTool.Utilitary_classes
{
    public class Battalion : Unit
    {
        public BattalionType Type { get; set; }
        //public string PathToIcon { get { return pathToIcon; } }
        public Battalion(string name, double health, double organization, double softAttack, double hardAttack,
            double defence, double breakthrough, double armor, double piercing, int frontWidth,
            string pathToIcon, double vehicleRatio, List<TerrainModifier> modifiers, BattalionType type) :
                base(name, health,
                organization, softAttack, hardAttack, defence, breakthrough, armor, piercing, frontWidth,
                vehicleRatio, pathToIcon, modifiers)
        {
            Type = type;
        }
    }
}
