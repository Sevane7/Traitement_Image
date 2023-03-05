namespace ProblemeScientifique
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyImage test = new MyImage("./test.bmp");
            test.ToString();
            Console.ReadKey();
        }
    }
}