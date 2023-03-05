using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemeScientifique
{
    internal class Pixel
    {
        private byte blue;
        private byte red;
        private byte green;

        /// <summary>
        /// Pixel en BGR
        /// </summary>
        /// <param name="blue"></param>
        /// <param name="red"></param>
        /// <param name="green"></param>
        public Pixel(byte blue, byte red, byte green)
        {
            this.blue = blue;
            this.green = green;
            this.red = red;
        }

        /// <summary>
        /// Proprété en écriture et en lecture du bleu
        /// </summary>
        public byte Blue
        {
            get { return this.blue; }
            set { this.blue = value; }
        }

        /// <summary>
        /// Proprété en écriture et en lecture du rouge
        /// </summary>
        public byte Red
        {
            get { return this.red; }
            set { this.red = value; }
        }

        /// <summary>
        /// Proprété en écriture et en lecture du vert
        /// </summary>
        public byte Green
        {
            get { return this.green; }
            set { this.blue = value; }
        }

        /// <summary>
        /// Retourne la version gris nuancé du pixel
        /// Chaque byte du pixel transformé devient la moyenne des trois bytes
        /// </summary>
        /// <returns></returns>
        public Pixel Grey()
        {
            byte moy = Convert.ToByte((this.blue + this.green + this.green) / 3);

            Pixel gris = new Pixel(moy, moy, moy);

            return gris;
        }
    }
}
