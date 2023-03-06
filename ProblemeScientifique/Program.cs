namespace ProblemeScientifique
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyImage lac = new MyImage("./Iena.bmp");
            //lac.Nuance_De_Gris();
            lac.From_image_to_file("./lena_dd.bmp", lac);
            Console.ReadKey();
        }
    }
}