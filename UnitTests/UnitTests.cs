using CalcTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace UnitTests
{
  [TestClass]
  public class UnitTests
  {
    [TestMethod]
    public void TokenizerTest()
    {
      string testStr = "1 + (2 - 3.5 )";
      HashSet<string> tokens = new HashSet<string>(new []{ "+", "-" });
      ITokenizer t = new Tokenizer(new StringReader(testStr), tokens);

      Assert.AreEqual(t.TokenType, TokenType.Number);
      Assert.AreEqual(t.Number, 1);
      t.NextToken();

      Assert.AreEqual(t.TokenType, TokenType.Operator);
      Assert.AreEqual(t.Token, "+");
      t.NextToken();

      Assert.AreEqual(t.TokenType, TokenType.OpenParens);
      t.NextToken();

      Assert.AreEqual(t.TokenType, TokenType.Number);
      Assert.AreEqual(t.Number, 2);
      t.NextToken();

      Assert.AreEqual(t.TokenType, TokenType.Operator);
      Assert.AreEqual(t.Token, "-");
      t.NextToken();

      Assert.AreEqual(t.TokenType, TokenType.Number);
      Assert.AreEqual(t.Number, 3.5);
      t.NextToken();

      Assert.AreEqual(t.TokenType, TokenType.CloseParens);
      t.NextToken();

      Assert.AreEqual(t.TokenType, TokenType.EOF);
    }

    
    [TestMethod]
    public void ParserAddOperatorTest()
    {
      IParser parser = new Parser();
      Operator unaryOp = new OperatorUnary("-", OperatorPriority.High, a => -a);
      parser.AddOperator(unaryOp);
      
      Node node = parser.ParseExpression("-2");
      Assert.AreEqual(true, node is NodeUnary);

      Assert.AreEqual(-2, node.Eval());
    }

    [TestMethod]
    public void BasicOperatorsParserEvalTest()
    {
      IParser parser = Parser.BasicOperatorsParser;

      Assert.AreEqual(parser.ParseExpression("-1").Eval(), -1);
      Assert.AreEqual(parser.ParseExpression("+2").Eval(), 2);
      Assert.AreEqual(parser.ParseExpression("--3").Eval(), 3);
      Assert.AreEqual(parser.ParseExpression("3 * 4").Eval(), 12);
      Assert.AreEqual(parser.ParseExpression("-((1 + 2) * 3) * 4").Eval(), -36);
      Assert.AreEqual(parser.ParseExpression("2^2^2^2").Eval(), 65536);
      Assert.AreEqual(parser.ParseExpression("2*2^3 - 1").Eval(), 15);
    }
  }
}