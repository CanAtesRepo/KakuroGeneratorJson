using System;

public class Program
{
    public static int Main(string[] args)
    {
        // 10x10'luk bir Kakuro oluşturmak için rastgele sayı üreteci
        Random random = new Random();
        var kakuro = new Kakuro(10, 10, random);

        Console.WriteLine("Oluşturulan Kakuro Bulmacası:");
        kakuro.Print();

        Console.WriteLine("\nBulmaca Çözülüyor...");
        bool isSolved = kakuro.Solve();

        if (isSolved && kakuro.Solved())
        {
            Console.WriteLine("\nÇözüm:");
            kakuro.Print();
            return 0;
        }
        else
        {
            Console.WriteLine("\nBulmaca çözülemedi.");
            return 1;
        }
    }
}
