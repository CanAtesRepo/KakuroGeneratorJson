namespace KakuroTests;

[TestClass]
public class Tests {
    [TestMethod]
    public void Test() {
        var kakuro = new Kakuro(new int[] { 20, 19, 8 }, new int[] { 22, 18, 7 });
        kakuro.Solve(); // 985,792,421
        Assert.IsTrue(kakuro.Solved());
        Assert.AreEqual(9, kakuro.GetValue(0, 0));
        Assert.AreEqual(8, kakuro.GetValue(0, 1));
        Assert.AreEqual(5, kakuro.GetValue(0, 2));
        
        Assert.AreEqual(7, kakuro.GetValue(1, 0));
        Assert.AreEqual(9, kakuro.GetValue(1, 1));
        Assert.AreEqual(2, kakuro.GetValue(1, 2));
        
        Assert.AreEqual(4, kakuro.GetValue(2, 0));
        Assert.AreEqual(2, kakuro.GetValue(2, 1));
        Assert.AreEqual(1, kakuro.GetValue(2, 2));
    }
}