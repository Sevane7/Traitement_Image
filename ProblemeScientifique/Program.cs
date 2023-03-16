namespace ProblemeScientifique
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyImage coco = new MyImage("./Image/coco.bmp");
            MyImage lena = new MyImage("./Image/lena.bmp");
            MyImage lac = new MyImage("./Image/lac.bmp");
            //coco.Nuance_De_Gris();
            //coco.From_image_to_file("./Image/coco_dd.bmp", coco);
            //MyImage cocozoom = coco.Zoom(3);
            //cocozoom.From_image_to_file("./Image/coco_zoom3.bmp", cocozoom);
            MyImage cocofiltre = coco.Filtre(3);
            MyImage lenafiltre = lena.Filtre(3);
            MyImage lacfiltre = lac.Filtre(3);
            lenafiltre.From_image_to_file("./Image/lena_SommeNoyeauBruitTot.bmp", lenafiltre);
        }
    }
}