using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace CalcTest
{
  public class Tokenizer : ITokenizer
  {
    TextReader _reader;
    char _currentChar;
    string _currentToken;
    TokenType _currentTokenType;
    double _number;
    HashSet<string> _tokens;

    public string Token { get { return _currentToken; } }

    public TokenType TokenType { get { return _currentTokenType; } }

    public double Number { get { return _number; } }

    public Tokenizer(TextReader reader, HashSet<string> tokens)
    {
      _reader = reader;
      _tokens = tokens;
      NextChar();
      NextToken();
    }

    void NextChar()
    {
      int ch = _reader.Read();
      _currentChar = ch < 0 ? '\0' : (char)ch;
    }

    public void NextToken()
    {
      while (char.IsWhiteSpace(_currentChar))
      {
        NextChar();
      }

      switch (_currentChar)
      {
        case '\0':
          _currentTokenType = TokenType.EOF;
          return;
        case '(':
          NextChar();
          _currentToken = "(";
          _currentTokenType = TokenType.OpenParens;
          return;
        case ')':
          NextChar();
          _currentToken = ")";
          _currentTokenType = TokenType.CloseParens;
          return;
      }

      if (char.IsDigit(_currentChar) || _currentChar == '.')
      {
        StringBuilder sb = new StringBuilder();
        bool haveDecimalPoint = false;
        while (char.IsDigit(_currentChar) || (!haveDecimalPoint && _currentChar == '.'))
        {
          sb.Append(_currentChar);
          haveDecimalPoint = _currentChar == '.';
          NextChar();
        }
        _currentToken = sb.ToString();
        _number = double.Parse(sb.ToString(), CultureInfo.InvariantCulture);
        _currentTokenType = TokenType.Number;
        return;
      }

      //this tokenizer implementation selects shortest token found, not a greedy one
      if (_tokens.Any(s => s.StartsWith(_currentChar)))
      {
        string possibleToken = "";
        bool tokenFound = false;
        do
        {
          possibleToken += _currentChar;
          NextChar();
          if (_tokens.Any(s => s.Equals(possibleToken))) tokenFound = true;
        } while (!tokenFound && char.IsWhiteSpace(_currentChar));
        
        if (tokenFound)
        {
          _currentToken = possibleToken;
          _currentTokenType = TokenType.Operator;
          return;
        }
      }

      throw new InvalidDataException($"Unexpected character: {_currentChar}");
    }
  }
}