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
