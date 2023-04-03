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

            int index = 54;
            for(int i = 0; i < this.pixels.GetLength(0); i++)
            {
                // j allant de 54 jusqu'à la fin de la largeur
                for(int j = 0; j < this.pixels.GetLength(1); j++)
                {
                    this.pixels[i, j] = new Pixel(bytes[index], bytes[index + 1], bytes[index + 2]);
                    index += 3;
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
        /// Affiche la taille, la largeur et la hauteur dans une console.
        /// </summary>
        public void ToString()
        {
            Console.WriteLine($" taille : {this.file_size}\n largeur : {this.largeur}\n hauteur : {this.hauteur}\n Bits par couleur : {this.bitsParCouleur}");
        }

        /// <summary>
        /// prend une instance de MyImage et la transforme en fichier binaire respectant la structure du fichier.bmp
        /// </summary>
        /// <param name="file"></param>
        public void From_image_to_file(string file, MyImage image)
        {
            List <byte> byts_list = new List<byte> { };
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

                //byts_list.Add((byte)( i == 0 ? (size[i] - 54) : (size[i])));
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
        /// Retourne un int. Converti un little endian en décimale
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
        /// Retourne un tableau de byte. Converti un int en little endian
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
        /// Transforme l'instance de l'image en nuance de Gris
        /// Utilise la méthode Grey() de la classe pixel
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public void Nuance_De_Gris()
        {
            for(int i = 0; i < MatPix.GetLength(0); i++)
            {
                for(int j = 0; j < MatPix.GetLength(1); j++)
                {
                    MatPix[i, j] = MatPix[i, j].Grey();
                }
            }
        }

        /// <summary>
        /// Retourne une image agrandie de l'instance par un coef
        /// </summary>
        /// <param name="coef"></param>
        /// <returns></returns>
        public MyImage Agrandissement(int coef)
        {
            Pixel[,] pix_ag = new Pixel[MatPix.GetLength(0) * coef, MatPix.GetLength(1) * coef];
            MyImage res_ag = new MyImage(pix_ag);

            /*

            //1) Selectionner la zone conservée de l'image 
            int H_conserv = (int)(MatPix.GetLength(0) / coef);
            int L_conserv = (int)(MatPix.GetLength(1) / coef);

            Pixel[,] Pix_conserv = new Pixel[H_conserv, L_conserv];

            int H_Init = MatPix.GetLength(0);
            int L_Init = MatPix.GetLength(1);

            //2) Parcourir Pix_conserv
            int H_Index_Deb = (H_Init - H_conserv) / coef;
            int H_Index_Fin = H_Init - H_Index_Deb;
            int L_Index_Deb = (L_Init - L_conserv) / coef;
            int L_Index_Fin = L_Init - L_Index_Deb;

            //Remplir Pix_conserv avec les pixels centraux de l'image init
            for (int i = H_Index_Deb; i < H_Index_Fin; i++)
            {
                for (int j = L_Index_Deb; j < L_Index_Fin; j++)
                {
                    Pix_conserv[i - H_Index_Deb, j - L_Index_Deb] = MatPix[i, j];
                }
            }
            //4) Remplir la nouvelle image
            */

            for (int i = 0; i < MatPix.GetLength(0); i++)
            {
                for (int j = 0; j < MatPix.GetLength(1); j++)
                {                   
                    for(int k = 0; k < coef; k++)
                    {
                        for(int l = 0; l < coef; l++)
                        {
                            res_ag.MatPix[i * coef + k,j * coef + l] = MatPix[i,j];
                        }                      
                    }
                    
                }
            }

            return res_ag;
        }

        /// <summary>
        /// Retourne une image zommée au centre par un certain coef
        /// Utilise la méthode Agrandissement
        /// </summary>
        /// <param name="coef"></param>
        /// <returns></returns>
        public MyImage Zoom(int coef)
        {
            MyImage Image_ag = new MyImage(MatPix);
            Image_ag = Image_ag.Agrandissement(coef);

            Console.WriteLine(Image_ag.MatPix.GetLength(0));
            Console.WriteLine(Image_ag.MatPix.GetLength(1));

            Pixel[,] respix = new Pixel[MatPix.GetLength(0), MatPix.GetLength(1)]; 
            MyImage res = new MyImage(respix);

            int H_dep = (Image_ag.MatPix.GetLength(0) - res.MatPix.GetLength(0)) / 2;
            int L_dep = (Image_ag.MatPix.GetLength(1) - res.MatPix.GetLength(1)) / 2;

            for(int i = 0; i < res.MatPix.GetLength(0); i++)
            {
                for(int j = 0; j< res.MatPix.GetLength(1); j++)
                {
                    res.MatPix[i , j] = Image_ag.MatPix[H_dep + i,L_dep + j];
                }
            }
            return res;
        }

        /// <summary>
        /// Retourne une liste de pixel
        /// Effectue la multiplication du voisinage d'un pixel et d'un noyeau (stocke le tout sous forme de liste) 
        /// </summary>
        /// <param name="noyeau"></param>
        /// <returns></returns>
        public List <Pixel> Pixelvoisin_X_noyeau(double[,] noyeau, int pos_H, int pos_L)
        {
            List<Pixel> pixelvoisin = new List<Pixel>();
            
            if (pos_H == 0)
            {
                if (pos_L == 0)
                {
                    //Stratégie Miroir : pos_L - 1 (out of range) est pareil que pos_L + 1
                    //                   pos_H - 1 (out of range) est pareil que pos_H + 1
                    pixelvoisin.Add(MatPix[pos_H + 1, pos_L + 1].Mult(noyeau[0, 0]));
                    pixelvoisin.Add(MatPix[pos_H + 1, pos_L].Mult(noyeau[0, 1]));
                    pixelvoisin.Add(MatPix[pos_H + 1, pos_L + 1].Mult(noyeau[0, 2]));

                    pixelvoisin.Add(MatPix[pos_H, pos_L + 1].Mult(noyeau[1, 0]));
                    pixelvoisin.Add(MatPix[pos_H, pos_L].Mult(noyeau[1, 1]));
                    pixelvoisin.Add(MatPix[pos_H, pos_L + 1].Mult(noyeau[1, 2]));

                    pixelvoisin.Add(MatPix[pos_H + 1, pos_L + 1].Mult(noyeau[2, 0]));
                    pixelvoisin.Add(MatPix[pos_H + 1, pos_L].Mult(noyeau[2, 1]));
                    pixelvoisin.Add(MatPix[pos_H + 1, pos_L + 1].Mult(noyeau[2, 2]));
                    return pixelvoisin;
                }
                if (pos_L == Largeur - 1)
                {
                    //Stratégie Miroir : pos_L + 1 (out of range) est pareil que pos_L - 1
                    //                   pos_H - 1 (out of range) est pareil que pos_H + 1
                    pixelvoisin.Add(MatPix[pos_H + 1, pos_L - 1].Mult(noyeau[0, 0]));
                    pixelvoisin.Add(MatPix[pos_H + 1, pos_L].Mult(noyeau[0, 1]));
                    pixelvoisin.Add(MatPix[pos_H + 1, pos_L - 1].Mult(noyeau[0, 2]));

                    pixelvoisin.Add(MatPix[pos_H, pos_L - 1].Mult(noyeau[1, 0]));
                    pixelvoisin.Add(MatPix[pos_H, pos_L].Mult(noyeau[1, 1]));
                    pixelvoisin.Add(MatPix[pos_H, pos_L - 1].Mult(noyeau[1, 2]));

                    pixelvoisin.Add(MatPix[pos_H + 1, pos_L - 1].Mult(noyeau[2, 0]));
                    pixelvoisin.Add(MatPix[pos_H + 1, pos_L].Mult(noyeau[2, 1]));
                    pixelvoisin.Add(MatPix[pos_H + 1, pos_L - 1].Mult(noyeau[2, 2]));
                    return pixelvoisin;
                }
                else
                {
                    //Stratégie miroir : pos_H - 1 (out of range) est pareil que pos_H + 1
                    pixelvoisin.Add(MatPix[pos_H + 1, pos_L - 1].Mult(noyeau[0, 0]));
                    pixelvoisin.Add(MatPix[pos_H + 1, pos_L].Mult(noyeau[0, 1]));
                    pixelvoisin.Add(MatPix[pos_H + 1, pos_L + 1].Mult(noyeau[0, 2]));

                    pixelvoisin.Add(MatPix[pos_H, pos_L - 1].Mult(noyeau[1, 0]));
                    pixelvoisin.Add(MatPix[pos_H, pos_L].Mult(noyeau[1, 1]));
                    pixelvoisin.Add(MatPix[pos_H, pos_L + 1].Mult(noyeau[1, 2]));

                    pixelvoisin.Add(MatPix[pos_H + 1, pos_L - 1].Mult(noyeau[2, 0]));
                    pixelvoisin.Add(MatPix[pos_H + 1, pos_L].Mult(noyeau[2, 1]));
                    pixelvoisin.Add(MatPix[pos_H + 1, pos_L + 1].Mult(noyeau[2, 2]));
                    return pixelvoisin;
                }
            } //Premiere colonne
            if (pos_H == Hauteur - 1)
            {
                if (pos_L == 0)
                {
                    //Stratégie Miroir : pos_L - 1 (out of range) est pareil que pos_L + 1
                    //                   pos_H + 1 (out of range) est pareil que pos_H - 1
                    pixelvoisin.Add(MatPix[pos_H - 1, pos_L + 1].Mult(noyeau[0, 0]));
                    pixelvoisin.Add(MatPix[pos_H - 1, pos_L].Mult(noyeau[0, 1]));
                    pixelvoisin.Add(MatPix[pos_H - 1, pos_L + 1].Mult(noyeau[0, 2]));

                    pixelvoisin.Add(MatPix[pos_H, pos_L + 1].Mult(noyeau[1, 0]));
                    pixelvoisin.Add(MatPix[pos_H, pos_L].Mult(noyeau[1, 1]));
                    pixelvoisin.Add(MatPix[pos_H, pos_L + 1].Mult(noyeau[1, 2]));

                    pixelvoisin.Add(MatPix[pos_H - 1, pos_L + 1].Mult(noyeau[2, 0]));
                    pixelvoisin.Add(MatPix[pos_H - 1, pos_L].Mult(noyeau[2, 1]));
                    pixelvoisin.Add(MatPix[pos_H - 1, pos_L + 1].Mult(noyeau[2, 2]));
                    return pixelvoisin;
                }
                if (pos_L == Largeur - 1)
                {
                    //Stratégie Miroir : pos_L + 1 (out of range) est pareil que pos_L - 1
                    //                   pos_H + 1 (out of range) est pareil que pos_H - 1
                    pixelvoisin.Add(MatPix[pos_H - 1, pos_L - 1].Mult(noyeau[0, 0]));
                    pixelvoisin.Add(MatPix[pos_H - 1, pos_L].Mult(noyeau[0, 1]));
                    pixelvoisin.Add(MatPix[pos_H - 1, pos_L - 1].Mult(noyeau[0, 2]));

                    pixelvoisin.Add(MatPix[pos_H, pos_L - 1].Mult(noyeau[1, 0]));
                    pixelvoisin.Add(MatPix[pos_H, pos_L].Mult(noyeau[1, 1]));
                    pixelvoisin.Add(MatPix[pos_H, pos_L - 1].Mult(noyeau[1, 2]));

                    pixelvoisin.Add(MatPix[pos_H - 1, pos_L - 1].Mult(noyeau[2, 0]));
                    pixelvoisin.Add(MatPix[pos_H - 1, pos_L].Mult(noyeau[2, 1]));
                    pixelvoisin.Add(MatPix[pos_H - 1, pos_L - 1].Mult(noyeau[2, 2]));
                    return pixelvoisin;
                }
                else
                {
                    //Stratégie miroir : pos_H + 1 (out of range) est pareil que pos_H - 1
                    pixelvoisin.Add(MatPix[pos_H - 1, pos_L - 1].Mult(noyeau[0, 0]));
                    pixelvoisin.Add(MatPix[pos_H - 1, pos_L].Mult(noyeau[0, 1]));
                    pixelvoisin.Add(MatPix[pos_H - 1, pos_L + 1].Mult(noyeau[0, 2]));

                    pixelvoisin.Add(MatPix[pos_H, pos_L - 1].Mult(noyeau[1, 0]));
                    pixelvoisin.Add(MatPix[pos_H, pos_L].Mult(noyeau[1, 1]));
                    pixelvoisin.Add(MatPix[pos_H, pos_L + 1].Mult(noyeau[1, 2]));

                    pixelvoisin.Add(MatPix[pos_H - 1, pos_L - 1].Mult(noyeau[2, 0]));
                    pixelvoisin.Add(MatPix[pos_H - 1, pos_L].Mult(noyeau[2, 1]));
                    pixelvoisin.Add(MatPix[pos_H - 1, pos_L + 1].Mult(noyeau[2, 2]));
                    return pixelvoisin;
                }
            } //Derniere colonne
            if (pos_H != 0 && pos_H != Hauteur - 1 && pos_L == 0)
            {
                //Stratégie Miroir : pos_L - 1 (out of range) est pareil que pos_L + 1
                pixelvoisin.Add(MatPix[pos_H - 1, pos_L + 1].Mult(noyeau[0, 0]));
                pixelvoisin.Add(MatPix[pos_H - 1, pos_L].Mult(noyeau[0, 1]));
                pixelvoisin.Add(MatPix[pos_H - 1, pos_L + 1].Mult(noyeau[0, 2]));

                pixelvoisin.Add(MatPix[pos_H, pos_L + 1].Mult(noyeau[1, 0]));
                pixelvoisin.Add(MatPix[pos_H, pos_L].Mult(noyeau[1, 1]));
                pixelvoisin.Add(MatPix[pos_H, pos_L + 1].Mult(noyeau[1, 2]));

                pixelvoisin.Add(MatPix[pos_H + 1, pos_L + 1].Mult(noyeau[2, 0]));
                pixelvoisin.Add(MatPix[pos_H + 1, pos_L].Mult(noyeau[2, 1]));
                pixelvoisin.Add(MatPix[pos_H + 1, pos_L + 1].Mult(noyeau[2, 2]));
                return pixelvoisin;
            } //Premiere Ligne
            if (pos_H != 0 && pos_H != Hauteur - 1 && pos_L == Largeur - 1)
            {
                //Stratégie Miroir : pos_L + 1 (out of range) est pareil que pos_L - 1
                pixelvoisin.Add(MatPix[pos_H - 1, pos_L - 1].Mult(noyeau[0, 0]));
                pixelvoisin.Add(MatPix[pos_H - 1, pos_L].Mult(noyeau[0, 1]));
                pixelvoisin.Add(MatPix[pos_H - 1, pos_L - 1].Mult(noyeau[0, 2]));

                pixelvoisin.Add(MatPix[pos_H, pos_L - 1].Mult(noyeau[1, 0]));
                pixelvoisin.Add(MatPix[pos_H, pos_L].Mult(noyeau[1, 1]));
                pixelvoisin.Add(MatPix[pos_H, pos_L - 1].Mult(noyeau[1, 2]));

                pixelvoisin.Add(MatPix[pos_H + 1, pos_L - 1].Mult(noyeau[2, 0]));
                pixelvoisin.Add(MatPix[pos_H + 1, pos_L].Mult(noyeau[2, 1]));
                pixelvoisin.Add(MatPix[pos_H + 1, pos_L - 1].Mult(noyeau[2, 2]));
                return pixelvoisin;
            } //Derniere Ligne
            else
            {
                pixelvoisin.Add(MatPix[pos_H - 1, pos_L - 1].Mult(noyeau[0, 0]));
                pixelvoisin.Add(MatPix[pos_H - 1, pos_L].Mult(noyeau[0, 1]));
                pixelvoisin.Add(MatPix[pos_H - 1, pos_L + 1].Mult(noyeau[0, 2]));

                pixelvoisin.Add(MatPix[pos_H, pos_L - 1].Mult(noyeau[1, 0]));
                pixelvoisin.Add(MatPix[pos_H, pos_L].Mult(noyeau[1, 1]));
                pixelvoisin.Add(MatPix[pos_H, pos_L + 1].Mult(noyeau[1, 2]));

                pixelvoisin.Add(MatPix[pos_H + 1, pos_L - 1].Mult(noyeau[2, 0]));
                pixelvoisin.Add(MatPix[pos_H + 1, pos_L].Mult(noyeau[2, 1]));
                pixelvoisin.Add(MatPix[pos_H + 1, pos_L + 1].Mult(noyeau[2, 2]));
                return pixelvoisin;
            }
        }

        /// <summary>
        /// Retourne une image filtrée
        /// </summary>
        /// <param name="type_de_floutage"> 1 : flou uniforme</param 1 >
        /// <returns></returns>
        public MyImage Filtre(int type_de_filtre)
        {
            Pixel[,] pixel_res = new Pixel[Hauteur, Largeur];
            MyImage ImageFiltre = new MyImage(pixel_res);

            switch (type_de_filtre)
            {
                //Flou uniforme
                case 1:
                {
                    //noyau
                    double[,] noyeau = new double[3, 3];
                    for (int i = 0; i < noyeau.GetLength(0); i++)
                    {
                        for (int j = 0; j < noyeau.GetLength(1); j++)
                        {
                            noyeau[i, j] = 1.0 / 9.0;
                        }
                    }

                    //Obtention d'image
                    for (int i = 0; i < Hauteur; i++)
                    {
                        for (int j = 0; j < Largeur; j++)
                        {
                            ImageFiltre.MatPix[i, j] = MatPix[i, j].SommePix(Pixelvoisin_X_noyeau(noyeau, i, j));
                        }
                    }

                    break;
                }
                    
                //Flou de Gauss
                case 2:
                {
                    //noyeau 
                    double[,] noyeau = new double[3, 3];
                    noyeau[0, 0] = 1.0 / 16.0;
                    noyeau[0, 1] = 2.0 / 16.0;
                    noyeau[0, 2] = 1.0 / 16.0;
                    noyeau[1, 0] = 2.0 / 16.0;
                    noyeau[1, 1] = 4.0 / 16.0;
                    noyeau[1, 2] = 2.0 / 16.0;
                    noyeau[2, 0] = 1.0 / 16.0;
                    noyeau[2, 1] = 2.0 / 16.0;
                    noyeau[2, 2] = 1.0 / 16.0;

                    //Obtention d'image
                    for (int i = 0; i < Hauteur; i++)
                    {
                        for (int j = 0; j < Largeur; j++)
                        {
                            ImageFiltre.MatPix[i, j] = MatPix[i, j].SommePix(Pixelvoisin_X_noyeau(noyeau, i, j));
                        }
                    }

                    break;
                }

                //Détection des contour
                case 3:
                {
                        
                        //Filtre de Sobel
                        //noyeau horizontale
                        double[,] noyeau_h = new double[3, 3];
                        noyeau_h[0, 0] = -1.0 ;
                        noyeau_h[0, 1] = 0.0 ;
                        noyeau_h[0, 2] = 1.0;
                        noyeau_h[1, 0] = -2.0;
                        noyeau_h[1, 1] = 0.0;
                        noyeau_h[1, 2] = 2.0;
                        noyeau_h[2, 0] = -1.0;
                        noyeau_h[2, 1] = 0.0;
                        noyeau_h[2, 2] = 1.0;

                        MyImage Avec_noyeau_h = new MyImage(MatPix);

                        //noyeau verticale
                        double[,] noyeau_v = new double[3, 3];
                        noyeau_v[0, 0] = -1.0;
                        noyeau_v[0, 1] = -2.0;
                        noyeau_v[0, 2] = -1.0;
                        noyeau_v[1, 0] = 0.0;
                        noyeau_v[1, 1] = 0.0;
                        noyeau_v[1, 2] = 0.0;
                        noyeau_v[2, 0] = 1.0;
                        noyeau_v[2, 1] = 2.0;
                        noyeau_v[2, 2] = 1.0;

                        MyImage Avec_noyeau_v = new MyImage(MatPix);

                        //Obtention d'image par noyeau horizontale
                        for (int i = 0; i < Hauteur; i++)
                        {
                            for (int j = 0; j < Largeur; j++)
                            {
                                Pixel h = Avec_noyeau_h.MatPix[i, j];
                                Pixel v = Avec_noyeau_v.MatPix[i, j];

                                h = MatPix[i, j].SommePix(Pixelvoisin_X_noyeau(noyeau_h, i, j));
                                v = MatPix[i, j].SommePix(Pixelvoisin_X_noyeau(noyeau_v, i, j));

                                ImageFiltre.MatPix[i, j] = MatPix[i, j].Sum(h, v);
                            }
                        }                       

                        /*
                        for(int i = 0; i < Hauteur; i++)
                        {
                            for(int j = 0; j < Largeur; j++)
                            {
                                ImageFiltre.MatPix[i, j] = MatPix[i, j].Grey();

                                if ((int)ImageFiltre.MatPix[i,j].Green >= 128)
                                {
                                    ImageFiltre.MatPix[i, j].Blue = 0;
                                    ImageFiltre.MatPix[i, j].Green = 0;
                                    ImageFiltre.MatPix[i, j].Red = 0;
                                }
                                else
                                {
                                    ImageFiltre.MatPix[i, j].Blue = 255;
                                    ImageFiltre.MatPix[i, j].Green = 255;
                                    ImageFiltre.MatPix[i, j].Red = 255;
                                }
                            }

                        }
                        */
                        
                        /*
                        //noyeau
                        double[,] noyeau = new double[3, 3];
                        noyeau[0, 0] = 1.0;
                        noyeau[0, 1] = 0.0;
                        noyeau[0, 2] = -1.0;
                        noyeau[1, 0] = 0.0;
                        noyeau[1, 1] = 0.0;
                        noyeau[1, 2] = 0.0;
                        noyeau[2, 0] = -1.0;
                        noyeau[2, 1] = 0.0;
                        noyeau[2, 2] = 1.0;

                        //Obtention de l'image
                        for (int i = 0; i < Hauteur; i++)
                        {
                            for (int j = 0; j < Largeur; j++)
                            {
                                //MatPix[i, j].Grey();

                                ImageFiltre.MatPix[i, j] = MatPix[i, j].SommePix(Pixelvoisin_X_noyeau(noyeau, i, j)).AbsolutePix();
                                
                            }
                        }                       
                        */
                        break;
                }
            }
            return ImageFiltre;
        }

        public void Rotationfloatangle(float angle)
        {
            int centrex = largeur / 2;
            int centrey = hauteur / 2;
            double norme = Math.Sqrt(Math.Pow(centrex, 2) + Math.Pow(centrey, 2));
            int normefinale = (int)((norme * 4) / 2 + 1);
            Convert.ToInt32(norme);
            Pixel[,] Imageretournee = new Pixel[normefinale, normefinale];
            for (int i = 0; i < normefinale; i++)
            {
                for (int j = 0; j < normefinale; j++)
                {
                    Imageretournee[i, j];
                }
            }
        }

    }
}
