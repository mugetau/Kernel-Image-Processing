using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEnhancement.Models
{
    public abstract class FilterBase
    {
        public abstract double[,] Kernel
        {
            get;
        }

        public abstract double Scale
        {
            get;
        }

        public abstract double Bias
        {
            get;
        }

        public abstract string Name
        {
            get;
        }
    }
}
