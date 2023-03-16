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
        public Pixel(byte blue, byte green, byte red)
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
            set { this.green = value; }
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

        /// <summary>
        /// Renvoie une matrice carrée du même pixel de dimmension : zoom
        /// </summary>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public Pixel [,] Zoom(int zoom)
        {
            Pixel[,] pixelZoom = new Pixel[zoom, zoom]; 

            for(int i = 0; i < pixelZoom.GetLength(0); i++)
            {
                for(int j = 0; j < pixelZoom.GetLength(1); j++)
                {
                    pixelZoom[i, j] = new Pixel(this.blue, this.red, this.green);
                }
            }

            return pixelZoom;
        }

        /// <summary>
        /// Multiplie chaque couleur du pixel par une valeur
        /// </summary>
        /// <param name="value"></param>
        public Pixel Mult(double value)
        {
            byte blue_mult = (byte)(this.blue * value);
            byte red_mult = (byte)(this.red * value);
            byte green_mult = (byte)(this.green * value);

            Pixel res = new Pixel(blue_mult, red_mult, green_mult);

            return res;
        }

        /// <summary>
        /// Retourne une instance de pixel
        /// Retourne la somme de tous les pixel
        /// </summary>
        /// <param name="tabpix"></param>
        /// <returns></returns>
        public Pixel SommePix(List <Pixel> tabpix)
        {
            Pixel res = new Pixel(0,0,0);

            for(int i = 0; i < tabpix.Count; i++)
            {
                res.Blue += tabpix[i].Blue;
                res.Red += tabpix[i].Red;
                res.Green += tabpix[i].Green;
            }

            //res.Blue = (byte)(res.Blue / tabpix.Count);
            //res.Red = (byte)(res.Red / tabpix.Count);
            //res.Green = (byte)(res.Green / tabpix.Count);

            return res;
        }

        /// <summary>
        /// Retourne une instance de pixel, prend un pixel en paramètre
        /// Retourne la somme de ces 2 pixels.
        /// </summary>
        /// <param name="pixel"></param>
        /// <returns></returns>
        public Pixel Sum(Pixel a, Pixel b)
        {
            Pixel res = new Pixel(0, 0, 0);

            res.Blue = (byte)Math.Min(a.Blue +  b.Blue, 255);
            res.Green = (byte)Math.Min(a.Green +  b.Green, 255);
            res.Red = (byte)Math.Min(a.Red + b.Red, 255);

            return res;
        }

        /// <summary>
        /// Le pixel devient la norme des deux autres
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Pixel Norme(Pixel a, Pixel b)
        {
            Pixel res = new Pixel(0, 0, 0);

            res.Blue = (byte)Math.Sqrt(a.Blue * a.Blue + b.Blue * b.Blue);
            res.Green = (byte)Math.Sqrt(a.Green * a.Green + b.Green * b.Green);
            res.Red = (byte)Math.Sqrt(a.Red * a.Red + b.Red * b.Red);

            return res;
        }

        /// <summary>
        /// Retourne une instance de pixel
        /// Retourne le log de celui ci pixel
        /// </summary>
        /// <returns></returns>
        public Pixel LogPix()
        {
            Pixel res = new Pixel(0, 0, 0);

            res.Blue = (byte)(255 / Math.Log(255) * Math.Log( this.blue + 1));
            res.Green = (byte)(255 / Math.Log(255) * Math.Log( this.green + 1));
            res.Red = (byte)(255 / Math.Log(255) * Math.Log( this.red + 1));

            return res;
        }

        /// <summary>
        /// Retourne une instance de pixel
        /// Retourne la valeur absolue de celui ci
        /// </summary>
        /// <returns></returns>
        public Pixel AbsolutePix()
        {
            Pixel res = new Pixel(0, 0, 0);

            res.Blue = (byte)(1.50 * this.blue + 50.0);
            res.Green = (byte)(1.50 * this.green + 50.0);
            res.Red = (byte)(1.50 * this.red + 50.0);

            return res;
        }

        /// <summary>
        /// Retourne une instance de pixel
        /// Retourne un pixel de couleur opposée
        /// </summary>
        /// <returns></returns>
        public Pixel Invere_BW()
        {
            Pixel res = new Pixel(0, 0, 0);

            res.Blue = (byte)(255 - this.blue);
            res.Green = (byte)(255 - this.green);
            res.Red = (byte)(255 - this.red);

            return res;
        }

        public void Black()
        {
            this.blue = 0;
            this.green = 0;
            this.red = 0;
        }

        public void White()
        {
            this.blue = 255;
            this.green = 255;
            this.red = 255;
        }

    }
}
