using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace ProblemeScientifique
{
    internal class MyImage
    {
        private int file_size;
        private int largeur;
        private int hauteur;
        private int bitsParCouleur = 24;
        private Pixel [,] pixels;


        /// <summary>
        /// Constructeur en fonctions d'une matrice de pixel
        /// </summary>
        /// <param name="pixels"></param>
        public MyImage(Pixel[,] pixels)
        {
            this.pixels = pixels;
            this.hauteur = pixels.GetLength(0);
            this.largeur= pixels.GetLength(1);
            this.file_size= pixels.GetLength(0) * pixels.GetLength(1) * this.bitsParCouleur;
        }

        /// <summary>
        /// Constructeur en fonction d'un fichier
        /// </summary>
        /// <param name="myfile"></param>
        public MyImage(string myfile)
        {
            //bytes est un vecteur composé d'octets représentant les métadonnées et les données de l'image
            byte[] bytes = File.ReadAllBytes(myfile);

            this.file_size = bytes[2];

            this.largeur = bytes[18];

            this.hauteur = bytes[22];

            this.bitsParCouleur = bytes[28];

            //On rempli la matrice de pixel avec les informations :
            //    - hauteur et largeur pour les dimensions
            //    - 3 bytes consécutifs représentent 1 pixels
            
            this.pixels = new Pixel [this.hauteur * 3, this.largeur * 3];

            // i percours les bytes de la 54ème position jusqu'au dernier, soit toute l'image
            for(int i = 54; i < myfile.Length; i = i + this.pixels.GetLength(1))
            {
                // j allant de 54 jusqu'à la fin de la largeur
                for(int j = i + 3; j < i + this.pixels.GetLength(1); j = j + 3)
                {
                    this.pixels[i - 54, j - 54] = new Pixel(bytes[j - 3], bytes[j - 2], bytes[j - 1]);
                }
            }


        }

        /// <summary>
        /// Propriété en lecture et écriture de file_size
        /// </summary>
        public int File_Size
        {
            get { return this.file_size; }
            set { this.file_size = value;}
        }

        /// <summary>
        /// Propriété en lecture et écriture de la largeur 
        /// </summary>
        public int Largeur
        {
            get { return this.largeur; }
            set { this.largeur = value;}
        }

        /// <summary>
        /// Propriété en lecture et écriture de la hauteur
        /// </summary>
        public int Hauteur
        {
            get { return this.hauteur; }
            set { this.hauteur = value;}
        }

        /// <summary>
        /// Propriété en écriture et en lecture de la matrice de pixel
        /// </summary>
        public Pixel[,] MatPix
        {
            get { return this.pixels; }
            set { this.pixels = value; }
        } 

        /// <summary>
        /// Propriété en lecture et écriture de BitsParCouleur
        /// </summary>
        public int BitsParCouleur
        {
            get { return this.bitsParCouleur; }
            set { this.bitsParCouleur = value; }
        }


        /// <summary>
        /// prend une instance de MyImage et la transforme en fichier binaire respectant la structure du fichier.bmp
        /// </summary>
        /// <param name="file"></param>
        public void From_image_to_file(string file, MyImage image)
        {
            List <byte> byts_list = null;
            byte[] byts_tab = null;

            //Header

            //Type
            byts_list.Add(66);
            byts_list.Add(77);

            //Size
            byts_list.Add((byte)image.File_Size);
            byts_list.Add(4);
            byts_list.Add(0);
            byts_list.Add(0);

            //champ réservé
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);

            //taille de l'offset
            byts_list.Add(54);
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);

            //Headder Info

            //Taille du Header
            byts_list.Add(40);
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);

            //Largeur
            byts_list.Add((byte)image.Largeur);
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);

            //Hauteur
            byts_list.Add((byte)image.Hauteur);
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);

            //Nombre de plan par image
            byts_list.Add(1);
            byts_list.Add(0);

            //Nombre de bits par pixel
            byts_list.Add((byte)image.BitsParCouleur);
            byts_list.Add(0);

            //Compression
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);

            //Taille de l'image incluant le remplissage
            byts_list.Add((byte)image.File_Size);
            byts_list.Add(4);
            byts_list.Add(0);
            byts_list.Add(0);


            //Résolution horizontale
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);
            
            //Résolution verticale
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);
            
            //Nombre de coueur de la palette
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);

            //Nombre de couleurs importantes de la palette
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);


            //Image

            for(int i = 0; i < image.MatPix.GetLength(0) * 3; i++)
            {
                for(int j = 0; j < image.MatPix.GetLength(1); j++)
                {
                    
                }
            }






            byts_tab = new byte[byts_list.Count];
            for (int i = 0; i < byts_list.Count; i++)
            {
                byts_tab[i] = byts_list[i];
            }


            File.WriteAllBytes(file, byts_tab);
            
        }

        public int Convertir_Endian_To_Int(byte[] tab)
        {
            int res = 0;

            //byte[] = Math.Pow(256,);


            return res;

        
        }
    }
}
