using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemeScientifique
{
    internal class Pixel
    {
        private int blue;
        private int red;
        private int green;

        /// <summary>
        /// Pixel en BGR
        /// </summary>
        /// <param name="blue"></param>
        /// <param name="red"></param>
        /// <param name="green"></param>
        public Pixel(int blue, int red, int green)
        {
            this.blue = blue;
            this.green = green;
            this.red = red;
        }

        public int Blue
        {
            get { return blue; }
            set { blue = value; }
        }

        public int Red
        {
            get { return red; }
            set { red = value; }
        }

        public int Green
        {
            get { return green; }
            set { blue = value; }
        }
    }
}
