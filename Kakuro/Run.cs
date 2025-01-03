using System.Collections.Generic;

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
