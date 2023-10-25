if (args.Length == 2 && args[0].Contains(',') && args[1].Contains(',')) {
    var horz = args[0].Split(',').Select(int.Parse).ToArray();
    var vert = args[1].Split(',').Select(int.Parse).ToArray();
    var kakuro = new Kakuro(horz, vert);
    kakuro.Solve();
    kakuro.Print();
    return kakuro.Solved() ? 0 : 1;
} else if (args.Length is 2 or 3) {
    Random random = args.Length == 3 ? new Random(int.Parse(args[2])) : new Random();
    var kakuro = new Kakuro(int.Parse(args[0]), int.Parse(args[1]), random);
    kakuro.Print();
    Console.WriteLine("Type 'solve' and press ENTER to show solution");
    if (Console.ReadLine() == "solve") {
        kakuro.Solve();
        kakuro.Print();
    }
    return 0;
} else {
    Console.WriteLine("Usage: Kakuro <row values> <column values> to solve or <rows> <columns> [<seed>] to generate");
    Console.WriteLine("Example: Kakuro 20,19,8 22,18,7 - solves puzzle"); // solution: 985,792,421
    Console.WriteLine("Example: Kakuro 3 3 42 - generate 3x3 puzzle with random seed of 42");
    return 0;
}