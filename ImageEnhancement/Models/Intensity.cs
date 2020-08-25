using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEnhancement.Models
{
    public class Intensity
    {
        public int Value { get; set; }
        public string Name
        {
            get { return Value.ToString() + 'x'; }
        }

        public Intensity(int value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
