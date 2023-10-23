if (args.Length != 2)
{
    Console.WriteLine("Usage: Kakuro <row> <column>");
    Console.WriteLine("Example: Kakuro 20,19,8 22,18,7"); // solution: 985,792,421
    return 1;
}

var horz = args[0].Split(',').Select(int.Parse).ToArray();
var vert = args[1].Split(',').Select(int.Parse).ToArray();

var kakuro = new Kakuro(horz, vert);
kakuro.Solve();
return kakuro.Print() ? 0 : 1;