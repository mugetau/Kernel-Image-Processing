using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEnhancement.Models
{
    public class LowPassFilter : FilterBase
    {
        private double[,] _kernel = new double[,] { { 1, 1, 1, 1, 1 },
                                                    { 1, 1, 1, 1, 1 },
                                                    { 1, 1, 1, 1, 1 },
                                                    { 1, 1, 1, 1, 1 },
                                                    { 1, 1, 1, 1, 1 }, };
        public override double[,] Kernel
        {
            get { return _kernel; }
        }

        private double _scale = 25.0;
        public override double Scale
        {
            get { return _scale; }
        }


        private double _bias = 0.0;
        public override double Bias
        {
            get { return _bias; }
        }

        public override string Name
        {
            get { return "Low Pass"; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
