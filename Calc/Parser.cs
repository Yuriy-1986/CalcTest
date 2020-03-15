using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CalcTest
{
  public class Parser : IParser
  {
    Dictionary<int, List<Operator>> _operationsByPriority = new Dictionary<int, List<Operator>>();
    HashSet<string> _tokens = new HashSet<string>();
    SortedSet<int> _priorities = new SortedSet<int>();
    List<int> _prioritiesList;
    ITokenizer _tokenizer;

    public static Parser BasicOperatorsParser
    {
      get
      {
        Parser basicParser = new Parser();
        Operator unaryMinus = new OperatorUnary("-", OperatorPriority.High, (a) => -a);
        Operator unaryPlus = new OperatorUnary("+", OperatorPriority.High, (a) => a);
        Operator add = new OperatorBinary("+", OperatorPriority.Lowest, Associativity.LEFT, (a, b) => (a + b));
        Operator subtract = new OperatorBinary("-", OperatorPriority.Lowest, Associativity.LEFT, (a, b) => (a - b));
        Operator multiply = new OperatorBinary("*", OperatorPriority.Low, Associativity.LEFT, (a, b) => (a * b));
        Operator divide = new OperatorBinary("/", OperatorPriority.Low, Associativity.LEFT, (a, b) => (a / b));
        Operator power = new OperatorBinary("^", OperatorPriority.Medium, Associativity.RIGHT, (a, b) => Math.Pow(a, b));

        basicParser.AddOperator(unaryMinus);
        basicParser.AddOperator(unaryPlus);
        basicParser.AddOperator(add);
        basicParser.AddOperator(subtract);
        basicParser.AddOperator(multiply);
        basicParser.AddOperator(divide);
        basicParser.AddOperator(power);

        return basicParser;
      }
    }

    public Parser()
    {
      _priorities.Add(OperatorPriority.Highest); //parens and numbers priority
    }

    public void AddOperator(Operator op)
    {
      if (!_operationsByPriority.ContainsKey(op.Priority))
      {
        if (!_priorities.Contains(op.Priority)) _priorities.Add(op.Priority);
        _operationsByPriority.Add(op.Priority, new List<Operator>());
      }

      _operationsByPriority[op.Priority].Add(op);
      if (!_tokens.Contains(op.TokenString)) _tokens.Add(op.TokenString);
    }

    public Node ParseExpression(string exprString)
    {
      _tokenizer = new Tokenizer(new StringReader(exprString), _tokens);
      _prioritiesList = _priorities.ToList();

      Node expr = ParseByPriority(_priorities.Min);
      if (_tokenizer.TokenType != TokenType.EOF) throw new SyntaxException("Unexpected characters at the end of expression");
      return expr;
    }

    Node ParseByPriority(int priority)
    {
      if (priority == OperatorPriority.Highest)
      {
        return ParseLeaf();
      }
      else
      {
        int nextPriority = _prioritiesList[_prioritiesList.IndexOf(priority) + 1];
        List<Operator> operators = _operationsByPriority[priority];
        IEnumerable<OperatorUnary> unaryOps = operators.Where((op) => (op is OperatorUnary)).Cast<OperatorUnary>();
        IEnumerable<OperatorBinary> binaryOps = operators.Where((op) => (op is OperatorBinary)).Cast<OperatorBinary>();

        //unary operators
        foreach (var op in unaryOps)
        {
          if (op.TokenString == _tokenizer.Token)
          {
            _tokenizer.NextToken();
            Node rhs = ParseByPriority(priority);
            return new NodeUnary(rhs, op.Op);
          }
        }

        //binary operators
        Node lhs = ParseByPriority(nextPriority);
        while (true)
        {
          OperatorBinary op = binaryOps.FirstOrDefault(op => op.TokenString == _tokenizer.Token);
          if (op == null) return lhs;
          _tokenizer.NextToken();
          Node rhs = ((op.Associativity == Associativity.LEFT) ? ParseByPriority(nextPriority) : ParseByPriority(priority));
          lhs = new NodeBinary(lhs, rhs, op.Op);
        }
      }
    }

    Node ParseLeaf()
    {
      if (_tokenizer.TokenType == TokenType.Number)
      {
        NodeNumber node = new NodeNumber(_tokenizer.Number);
        _tokenizer.NextToken();
        return node;
      }

      if (_tokenizer.TokenType == TokenType.OpenParens)
      {
        _tokenizer.NextToken();

        Node node = ParseByPriority(_priorities.Min);

        if (_tokenizer.TokenType != TokenType.CloseParens) throw new SyntaxException("Missing close parenthesis");
        _tokenizer.NextToken();

        return node;
      }

      throw new SyntaxException($"Unexpected token: {_tokenizer.TokenType}");
    }
  }
}