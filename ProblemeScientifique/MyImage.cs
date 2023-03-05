using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
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

            byte[] size = new byte[] { bytes[2], bytes[3], bytes[4], bytes[5] };
            this.file_size = Convertir_Endian_To_Int(size);

            byte[] larg = new byte[] { bytes[18], bytes[19], bytes[20], bytes[21] };
            this.largeur = Convertir_Endian_To_Int(larg);

            byte[] haut = new byte[] { bytes[22], bytes[23], bytes[24], bytes[25] };
            this.hauteur = Convertir_Endian_To_Int(haut);

            byte[] bitsparcoul = new byte[] { bytes[28], bytes[29] };
            this.bitsParCouleur = Convertir_Endian_To_Int(bitsparcoul);

            //On rempli la matrice de pixel avec les informations :
            //    - hauteur et largeur pour les dimensions
            //    - 3 bytes consécutifs représentent 1 pixels
            
            this.pixels = new Pixel [this.hauteur, this.largeur];

            // i percours les bytes de la 54ème position jusqu'au dernier, soit toute l'image
            for(int i = 54; i < myfile.Length; i = i + this.pixels.GetLength(1) * 3)
            {
                // j allant de 54 jusqu'à la fin de la largeur
                for(int j = i + 3; j < i + this.pixels.GetLength(1) * 3; j = j + 3)
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
            byte[] size = image.Convertir_Int_To_Endian(image.File_Size, 4);
            for(int i = 0; i < size.Length; i++) { byts_list.Add(size[i]); }

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
            byte[] larg = image.Convertir_Int_To_Endian(image.Largeur, 4);
            for (int i = 0; i < larg.Length; i++) { byts_list.Add(larg[i]); }

            //Hauteur
            byte[] haut = image.Convertir_Int_To_Endian(image.Hauteur, 4);
            for (int i = 0; i < haut.Length; i++) { byts_list.Add(haut[i]); }

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
            for (int i = 0; i < size.Length; i++) 
            {
                if (i == 0) byts_list.Add((byte)(size[i] - 54));
                else byts_list.Add(size[i]);

                byts_list.Add((byte)( i == 0 ? (size[i] - 54) : (size[i])));
            }


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

            //Parcours la matrice de pixel de image
            //ajoute les bites correspondants dans la liste
            for(int i = 0; i < image.MatPix.GetLength(0); i++)
            {
                for(int j = 0; j < image.MatPix.GetLength(1); j++)
                {
                    byts_list.Add(image.MatPix[i,j].Blue);
                    byts_list.Add(image.MatPix[i,j].Red);
                    byts_list.Add(image.MatPix[i,j].Green);
                }
            }

            byts_tab = new byte[byts_list.Count];
            for (int i = 0; i < byts_list.Count; i++)
            {
                byts_tab[i] = byts_list[i];
            }

            File.WriteAllBytes(file, byts_tab);
            
        }

        /// <summary>
        /// Converti un little endian en décimale
        /// Parours le tableau dans le sens inverse, car little endian
        /// Multiplie sa valeur par la puissance de 256 correspondante, en fonction de la position de l'élément dans le tableau
        /// Retourn la somme de toutes ces valeurs
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public int Convertir_Endian_To_Int(byte[] tab)
        {
            int result = 0;

            for (int i = 0; i < tab.Length; i++)
                result += tab[i] * (int)Math.Pow(256, i);

            return result;

        }

        /// <summary>
        /// Converti un int en little endian
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size_endian"></param>
        /// <returns></returns>
        public byte[] Convertir_Int_To_Endian(int value, int size_endian)
        {
            byte[] result = new byte[size_endian];

            result = BitConverter.GetBytes(value);

            if(!BitConverter.IsLittleEndian)
            {
                Array.Reverse(result);
            }

            return result;
        }

        /// <summary>
        /// Retourne l'image en nuance de Gris
        /// Prend une image en paramètre
        /// Utilise la méthode Grey() de la classe pixel
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public MyImage Nuance_De_Gris(MyImage image)
        {
            MyImage res = new MyImage(image.MatPix);

            for(int i = 0; i < image.MatPix.GetLength(0); i++)
            {
                for(int j = 0; j < image.MatPix.GetLength(1); j++)
                {
                    res.MatPix[i, j] = image.MatPix[i, j].Grey();
                }
            }

            return res;
        }

        /// <summary>
        /// Affiche la taille, la largeur et la hauteur dans une console.
        /// </summary>
        public void ToString()
        {
            Console.WriteLine($" taille : {this.file_size}\n largeur : {this.largeur}\n hauteur : {this.hauteur}\n Bits par couleur : {this.bitsParCouleur}");
        }
    }
}
