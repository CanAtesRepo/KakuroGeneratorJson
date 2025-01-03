using System;
using System.Collections.Generic;
using System.Linq;

namespace KakuroSolver
{
    public class Run
    {
        public int Sum { get; set; }
        public List<(int Row, int Col)> Cells { get; set; }

        public Run(int sum)
        {
            Sum = sum;
            Cells = new List<(int Row, int Col)>();
        }
    }

    public class Cell
    {
        public int Value { get; set; } = 0; // 0 = boş hücre
        public List<int> PossibleValues { get; set; } = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public Run HorizontalRun { get; set; }
        public Run VerticalRun { get; set; }

        public bool IsClue { get; set; } = false; // Klip hücre olup olmadığını belirtir
        public int ClueHorizontal { get; set; } = 0;
        public int ClueVertical { get; set; } = 0;
    }

    public class Kakuro
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Cell[,] Grid { get; private set; }
        public List<Run> HorizontalRuns { get; private set; }
        public List<Run> VerticalRuns { get; private set; }

        private Random random;
        private bool hasUniqueSolution = true;

        public Kakuro(int width, int height, Random random)
        {
            Width = width;
            Height = height;
            this.random = random;
            Grid = new Cell[Height, Width];
            HorizontalRuns = new List<Run>();
            VerticalRuns = new List<Run>();
            InitializeGrid();
            GenerateRuns();
            AssignClues();

            // Çözümü oluştur ve kontrol et
            GenerateSolution();

            // Eğer çözüm yoksa yeni bulmaca oluştur
            while (!SolveRecursive())
            {
                ResetGrid();
                GenerateRuns();
                AssignClues();
                GenerateSolution();
            }

            // Benzersiz çözüm kontrolü
            hasUniqueSolution = CheckUniqueSolution();
        }

        // Mevcut hücreleri başlatır
        private void InitializeGrid()
        {
            for (int r = 0; r < Height; r++)
                for (int c = 0; c < Width; c++)
                    Grid[r, c] = new Cell();
        }

        // Bulmaca oluşturulurken grid'i sıfırlar
        private void ResetGrid()
        {
            for (int r = 0; r < Height; r++)
                for (int c = 0; c < Width; c++)
                {
                    Grid[r, c].Value = 0;
                    Grid[r, c].PossibleValues = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                    Grid[r, c].IsClue = false;
                    Grid[r, c].ClueHorizontal = 0;
                    Grid[r, c].ClueVertical = 0;
                    Grid[r, c].HorizontalRun = null;
                    Grid[r, c].VerticalRun = null;
                }

            HorizontalRuns.Clear();
            VerticalRuns.Clear();
        }

        // Rastgele run'lar oluşturur
        private void GenerateRuns()
        {
            // Yatay run'lar oluşturma
            for (int r = 0; r < Height; r++)
            {
                int c = 0;
                while (c < Width)
                {
                    // Rastgele hücreye klip koyma
                    if (random.NextDouble() < 0.15 || c == 0) // %15 olasılıkla veya satırın başında klip
                    {
                        // Klip hücreyi işaretleme
                        Grid[r, c].IsClue = true;

                        // Yatay run oluşturma
                        Run horizontalRun = new Run(0); // Toplam başlangıçta 0
                        HorizontalRuns.Add(horizontalRun);

                        // Run'un hücrelerini ekleme
                        int remainingWidth = Width - c - 1; // Klip hücresi hariç kalan hücre sayısı
                        if (remainingWidth < 1)
                        {
                            c++;
                            continue;
                        }

                        int runLength;
                        if (remainingWidth == 1)
                        {
                            runLength = 1;
                        }
                        else
                        {
                            runLength = random.Next(2, Math.Min(5, remainingWidth) + 1); // 2 ile min(5, remainingWidth) arasında
                        }

                        bool validRun = true;

                        for (int i = 1; i <= runLength; i++)
                        {
                            if (c + i >= Width)
                            {
                                validRun = false;
                                break;
                            }

                            // Klip hücrelerin çakışmamasını sağlama
                            if (Grid[r, c + i].IsClue)
                            {
                                validRun = false;
                                break;
                            }

                            horizontalRun.Cells.Add((r, c + i));
                            Grid[r, c + i].HorizontalRun = horizontalRun;
                        }

                        if (validRun)
                            c += runLength + 1;
                        else
                            c++;
                    }
                    else
                    {
                        c++;
                    }
                }
            }

            // Dikey run'lar oluşturma
            for (int c = 0; c < Width; c++)
            {
                int r = 0;
                while (r < Height)
                {
                    // Rastgele hücreye klip koyma
                    if (random.NextDouble() < 0.15 || r == 0) // %15 olasılıkla veya sütunun başında klip
                    {
                        // Klip hücreyi işaretleme
                        Grid[r, c].IsClue = true;

                        // Dikey run oluşturma
                        Run verticalRun = new Run(0); // Toplam başlangıçta 0
                        VerticalRuns.Add(verticalRun);

                        // Run'un hücrelerini ekleme
                        int remainingHeight = Height - r - 1; // Klip hücresi hariç kalan hücre sayısı
                        if (remainingHeight < 1)
                        {
                            r++;
                            continue;
                        }

                        int runLength;
                        if (remainingHeight == 1)
                        {
                            runLength = 1;
                        }
                        else
                        {
                            runLength = random.Next(2, Math.Min(5, remainingHeight) + 1); // 2 ile min(5, remainingHeight) arasında
                        }

                        bool validRun = true;

                        for (int i = 1; i <= runLength; i++)
                        {
                            if (r + i >= Height)
                            {
                                validRun = false;
                                break;
                            }

                            // Klip hücrelerin çakışmamasını sağlama
                            if (Grid[r + i, c].IsClue)
                            {
                                validRun = false;
                                break;
                            }

                            verticalRun.Cells.Add((r + i, c));
                            Grid[r + i, c].VerticalRun = verticalRun;
                        }

                        if (validRun)
                            r += runLength + 1;
                        else
                            r++;
                    }
                    else
                    {
                        r++;
                    }
                }
            }
        }

        // Clue hücrelerine toplamları atar
        private void AssignClues()
        {
            // Yatay run'lar için toplamları atama
            foreach (var run in HorizontalRuns)
            {
                if (run.Cells.Count > 0)
                {
                    // Run'un toplamını belirleme (run uzunluğuna bağlı olarak)
                    run.Sum = CalculateRunSum(run.Cells.Count);
                    var firstCell = run.Cells.First();
                    if (firstCell.Col - 1 >= 0)
                    {
                        Grid[firstCell.Row, firstCell.Col - 1].ClueHorizontal = run.Sum;
                    }
                }
            }

            // Dikey run'lar için toplamları atama
            foreach (var run in VerticalRuns)
            {
                if (run.Cells.Count > 0)
                {
                    // Run'un toplamını belirleme (run uzunluğuna bağlı olarak)
                    run.Sum = CalculateRunSum(run.Cells.Count);
                    var firstCell = run.Cells.First();
                    if (firstCell.Row - 1 >= 0)
                    {
                        Grid[firstCell.Row - 1, firstCell.Col].ClueVertical = run.Sum;
                    }
                }
            }
        }

        // Run uzunluğuna bağlı olarak toplam hesaplama
        private int CalculateRunSum(int runLength)
        {
            // 1'den 9'a kadar benzersiz sayılarla oluşturulabilecek en küçük ve en büyük toplamları kullanarak rastgele bir toplam belirleyin
            int minSum = Enumerable.Range(1, runLength).Sum();
            int maxSum = Enumerable.Range(9 - runLength + 1, runLength).Sum();
            return random.Next(minSum, maxSum + 1);
        }

        // Bulmacanın çözümünü oluşturur
        private void GenerateSolution()
        {
            // Çözümleme algoritması zaten SolveRecursive ile yapılmaktadır
            // Bu metod boş bırakılabilir veya çözüme yönelik ek adımlar eklenebilir
        }

        // Bulmacayı çözer
        public bool Solve()
        {
            return SolveRecursive();
        }

        private bool SolveRecursive()
        {
            // Hücreleri sol üstten sağ alta doğru tarar
            for (int r = 0; r < Height; r++)
            {
                for (int c = 0; c < Width; c++)
                {
                    if (Grid[r, c].IsClue)
                        continue;

                    if (Grid[r, c].Value == 0)
                    {
                        foreach (var num in Grid[r, c].PossibleValues.OrderBy(x => random.Next()))
                        {
                            if (IsSafe(r, c, num))
                            {
                                Grid[r, c].Value = num;

                                if (SolveRecursive())
                                    return true;

                                Grid[r, c].Value = 0;
                            }
                        }
                        return false; // Hiçbir numara uygun değilse geri dön
                    }
                }
            }
            return true; // Tüm hücreler doldurulmuş
        }

        // Belirli bir hücreye numara koymanın güvenli olup olmadığını kontrol eder
        private bool IsSafe(int row, int col, int num)
        {
            // Yatay run kontrolü
            if (Grid[row, col].HorizontalRun != null)
            {
                var run = Grid[row, col].HorizontalRun;
                int currentSum = num;
                HashSet<int> usedNumbers = new HashSet<int> { num };

                foreach (var cell in run.Cells)
                {
                    int val = Grid[cell.Row, cell.Col].Value;
                    if (val != 0)
                    {
                        if (usedNumbers.Contains(val))
                            return false;
                        usedNumbers.Add(val);
                        currentSum += val;
                    }
                }

                if (currentSum > run.Sum)
                    return false;

                // Eğer tüm hücreler doluysa toplamın eşit olup olmadığını kontrol et
                if (run.Cells.All(cell => Grid[cell.Row, cell.Col].Value != 0))
                {
                    if (currentSum != run.Sum)
                        return false;
                }
            }

            // Dikey run kontrolü
            if (Grid[row, col].VerticalRun != null)
            {
                var run = Grid[row, col].VerticalRun;
                int currentSum = num;
                HashSet<int> usedNumbers = new HashSet<int> { num };

                foreach (var cell in run.Cells)
                {
                    int val = Grid[cell.Row, cell.Col].Value;
                    if (val != 0)
                    {
                        if (usedNumbers.Contains(val))
                            return false;
                        usedNumbers.Add(val);
                        currentSum += val;
                    }
                }

                if (currentSum > run.Sum)
                    return false;

                // Eğer tüm hücreler doluysa toplamın eşit olup olmadığını kontrol et
                if (run.Cells.All(cell => Grid[cell.Row, cell.Col].Value != 0))
                {
                    if (currentSum != run.Sum)
                        return false;
                }
            }

            return true;
        }

        // Bulmacanın çözüldüğünü kontrol eder
        public bool Solved()
        {
            for (int r = 0; r < Height; r++)
                for (int c = 0; c < Width; c++)
                    if (!Grid[r, c].IsClue && Grid[r, c].Value == 0)
                        return false;

            // Run'ların toplamlarını ve benzersizliğini kontrol eder
            foreach (var run in HorizontalRuns)
            {
                int sum = 0;
                HashSet<int> unique = new HashSet<int>();
                foreach (var cell in run.Cells)
                {
                    sum += Grid[cell.Row, cell.Col].Value;
                    if (!unique.Add(Grid[cell.Row, cell.Col].Value))
                        return false;
                }
                if (sum != run.Sum)
                    return false;
            }

            foreach (var run in VerticalRuns)
            {
                int sum = 0;
                HashSet<int> unique = new HashSet<int>();
                foreach (var cell in run.Cells)
                {
                    sum += Grid[cell.Row, cell.Col].Value;
                    if (!unique.Add(Grid[cell.Row, cell.Col].Value))
                        return false;
                }
                if (sum != run.Sum)
                    return false;
            }

            return true;
        }

        // Bulmacanın benzersiz bir çözümü olup olmadığını kontrol eder
        private bool CheckUniqueSolution()
        {
            int solutionCount = 0;
            UniqueSolutionRecursive(ref solutionCount);

            return solutionCount == 1;
        }

        private void UniqueSolutionRecursive(ref int solutionCount)
        {
            if (solutionCount > 1)
                return;

            for (int r = 0; r < Height; r++)
            {
                for (int c = 0; c < Width; c++)
                {
                    if (Grid[r, c].IsClue)
                        continue;

                    if (Grid[r, c].Value == 0)
                    {
                        foreach (var num in Grid[r, c].PossibleValues.OrderBy(x => random.Next()))
                        {
                            if (IsSafe(r, c, num))
                            {
                                Grid[r, c].Value = num;
                                UniqueSolutionRecursive(ref solutionCount);
                                Grid[r, c].Value = 0;
                            }
                        }
                        return;
                    }
                }
            }
            solutionCount++;
        }

        // Bulmacayı ekrana yazdırır
        public void Print()
        {
            // Başlıkları yazdırma
            Console.Write("     ");
            for (int c = 0; c < Width; c++)
                Console.Write($" {c + 1,2} ");
            Console.WriteLine();

            for (int r = 0; r < Height; r++)
            {
                // Satır numarasını yazdırma
                Console.Write($"{r + 1,3} | ");

                for (int c = 0; c < Width; c++)
                {
                    var cell = Grid[r, c];
                    if (cell.IsClue)
                    {
                        string clue = "";
                        if (cell.ClueVertical > 0)
                            clue += $"\\{cell.ClueVertical}";
                        if (cell.ClueHorizontal > 0)
                            clue += $"/{cell.ClueHorizontal}";
                        Console.Write($"{clue,4}");
                    }
                    else
                    {
                        Console.Write(cell.Value > 0 ? $" {cell.Value,2} " : " ?? ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
