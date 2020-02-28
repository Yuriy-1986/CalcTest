using CalcTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UnitTests
{
  [TestClass]
  public class UnitTests
  {
    [TestMethod]
    public void TokenizerTest()
    {
      string testStr = "1+2 - 3.5";
      Tokenizer t = new Tokenizer(new StringReader(testStr));

      Assert.AreEqual(t.Token, Token.Number);
      Assert.AreEqual(t.Number, 1);
      t.NextToken();

      Assert.AreEqual(t.Token, Token.Add);
      t.NextToken();

      Assert.AreEqual(t.Token, Token.Number);
      Assert.AreEqual(t.Number, 2);
      t.NextToken();

      Assert.AreEqual(t.Token, Token.Subtract);
      t.NextToken();

      Assert.AreEqual(t.Token, Token.Number);
      Assert.AreEqual(t.Number, 3.5);
      t.NextToken();
    }

    [TestMethod]
    public void AddSubtractTest()
    {
      Assert.AreEqual(Parser.Parse("1 + 2").Eval(), 3);

      Assert.AreEqual(Parser.Parse("1 - 2").Eval(), -1);

      Assert.AreEqual(Parser.Parse("1 + 2 - 3").Eval(), 0);
    }

    [TestMethod]
    public void UnaryTest()
    {
      Assert.AreEqual(Parser.Parse("-1").Eval(), -1);
      Assert.AreEqual(Parser.Parse("+2").Eval(), 2);
      Assert.AreEqual(Parser.Parse("--3").Eval(), 3);
    }
    [TestMethod]
    public void MultiplyDivideTest()
    {
      Assert.AreEqual(Parser.Parse("3 * 4").Eval(), 12);
      Assert.AreEqual(Parser.Parse("5 / 6").Eval(), 5.0 / 6.0);
    }

    [TestMethod]
    public void OrderTest()
    {
      Assert.AreEqual(Parser.Parse("1 + 2 * 3").Eval(), 7);
      Assert.AreEqual(Parser.Parse("(1 + 2) * 3").Eval(), 9);
      Assert.AreEqual(Parser.Parse("-(1 + 2) * 3").Eval(), -9);
      Assert.AreEqual(Parser.Parse("-((1 + 2) * 3) * 4").Eval(), -36);
    }
  }
}