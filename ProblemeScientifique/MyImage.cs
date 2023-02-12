using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace ProblemeScientifique
{
    internal class MyImage
    {
        private string type;
        private int file_size;
        private int offset_size;
        private int largeur;
        private int hauteur;
        private int bitsParCouleur;

        public MyImage(string type, int file_size, int Offset_size, int hauteur, int largeur, int bitsParCouleur)
        {
            this.type = type;
            this.file_size = file_size;               
            this.offset_size = Offset_size;
            this.hauteur= hauteur;
            this.largeur= largeur;
            this.bitsParCouleur= bitsParCouleur;
        }

        public string Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public int FIle_Size
        {
            get { return this.file_size; }
            set { this.file_size = value;}
        }

        public int Largeur_Size
        {
            get { return this.file_size; }
            set { this.file_size = value;}
        }

        public int Offset_size
        {
            get { return this.offset_size; }
            set { this.offset_size = value;}
        }

        public int Hauteur
        {
            get { return this.hauteur; }
            set { this.hauteur = value;}
        }

        public int Largeur
        {
            get { return this.largeur; }
            set { this.largeur = value;}
        }

        public int BitsParCouleur
        {
            get { return this.bitsParCouleur; }
            set { this.bitsParCouleur = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myfile"></param>
        public MyImage(string myfile)
        {
            //bytes est un vecteur composé d'octets représentant les métadonnées et les données de l'image
            byte[] bytes = File.ReadAllBytes(myfile);

            //Métadonnées

            //HEADER 
            if (bytes[0] == 66 && bytes[1] == 77) 
                this.type = "BM";

            this.file_size = bytes[2];

            this.offset_size = bytes[10];

            this.largeur = bytes[18];

            this.hauteur = bytes[22];

            this.bitsParCouleur= bytes[28];
        }
    }
}
