public class Kakuro { // Kakuro puzzle solver
    private int[] horz, vert;
    private bool[,,] forbidden; // note: [v,h,n] is true if n+1 is forbidden

    public Kakuro(int[] horz, int[] vert) {
        this.horz = horz;
        this.vert = vert;
        forbidden = new bool[vert.Length, horz.Length, 9];
    }

    public void Solve() {
        for (int pass = 0; pass < 100; pass++) {
            bool didSomething = false;
            for (int v = 0; v < vert.Length; v++)
                for (int h = 0; h < horz.Length; h++)
                    if (GetValue(v, h) == 0)
                        for (int n = 1; n <= 9; n++)
                            if (!forbidden[v, h, n - 1] && !IsPossible(v, h, n))
                                didSomething = forbidden[v, h, n - 1] = true;
            if (!didSomething) break;
        }
    }

    private bool IsPossible(int v, int h, int n) {
        int vRemaining = vert[v] - n, hRemaining = horz[h] - n;
        List<List<int>> vOthers = new(), hOthers = new();
        for (int hh = 0; hh < horz.Length; hh++)
            if (hh != h) {
                if (GetValue(v, hh) == n) return false;
                vOthers.Add(Enumerable.Range(1, 9).Where(nn => n != nn && !forbidden[v, hh, nn - 1]).ToList());
            }
        for (int vv = 0; vv < vert.Length; vv++)
            if (vv != v) {
                if (GetValue(vv, h) == n) return false;
                hOthers.Add(Enumerable.Range(1, 9).Where(nn => n != nn && !forbidden[vv, h, nn - 1]).ToList());
            }
        return IsPossible(vRemaining, vOthers) && IsPossible(hRemaining, hOthers);
    }

    private bool IsPossible(int remaining, List<List<int>> others) {
        List<int> selector = others.Select(_ => 0).ToList(), othersCount = others.Select(o => o.Count).ToList();
        do {
            var sel = others.Select((o, i) => o[selector[i]]).ToList();
            if (new HashSet<int>(sel).Count == sel.Count && sel.Sum() == remaining) return true;
        } while (NextSelector(selector, othersCount));
        return false;
    }

    private bool NextSelector(List<int> selector, List<int> othersCount) {
        for (int s = 0; s < selector.Count; s++) {
            if (++selector[s] < othersCount[s]) return true;
            selector[s] = 0;
        }
        return false;
    }

    public bool Print() { // returns true if solved
        bool isSolved = true;
        Console.Write("  ");
        for (int h = 0; h < horz.Length; h++)
            Console.Write(" " + horz[h].ToString().PadLeft(2));
        Console.WriteLine();
        for (int v = 0; v < vert.Length; v++) {
            Console.Write(vert[v].ToString().PadLeft(2));
            for (int h = 0; h < horz.Length; h++) {
                var n = GetValue(v, h);
                isSolved &= n > 0;
                Console.Write(n > 0 ? n.ToString().PadLeft(3) : " ??");
            }
            Console.WriteLine();
        }
        return isSolved;
    }

    private int GetValue(int v, int h) {
        int value = 0;
        for (int n = 1; n <= 9; n++)
            if (!forbidden[v, h, n - 1]) {
                if (value > 0) return 0;
                value = n;
            }
        return value;
    }
}