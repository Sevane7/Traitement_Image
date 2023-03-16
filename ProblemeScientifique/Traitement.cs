using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProblemeScientifique
{
    internal class Traitement
    {
        private MyImage image;
        public Traitement(MyImage image)
        {
            this.image = image;
        }
        public MyImage Image
        {
            get { return this.image; }
            set { this.image = value; }
        }

    }
}
