var kakuro = new Kakuro(new[] { 20, 19, 8 }, new[] { 22, 18, 7 }); // solution: 985,792,421
kakuro.Solve();
return kakuro.Print() ? 0 : 1;