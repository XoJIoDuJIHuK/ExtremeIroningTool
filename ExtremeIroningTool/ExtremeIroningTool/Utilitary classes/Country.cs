using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtremeIroningTool.Utilitary_classes
{
    public class Country
    {
        public string tag;
        public string name;
        public string pathToIcon;
        public string pathToPoster;
        public string description;

        public Country(string tag, string name, string pathToIcon, string pathToPoster, string description)
        {
            this.tag = tag;
            this.name = name;
            this.pathToIcon = pathToIcon;
            this.pathToPoster = pathToPoster;
            this.description = description;
        }
        public override string ToString()
        {
            return name;
        }
    }
}
