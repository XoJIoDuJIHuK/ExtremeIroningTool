using ExtremeIroningTool.Utilitary_classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtremeIroningTool.Interfaces
{
    public interface ILandForce
    {
        string Name { get; set; }
        string PathToIcon { get; set; }
        General Commander { get; set; }
    }
}
