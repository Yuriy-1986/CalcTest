using System.Globalization;
using System.IO;
using System.Text;

namespace CalcTest
{
  public class Tokenizer
  {
    TextReader _reader;
    char _currentChar;
    Token _currentToken;
    double _number;

    public Tokenizer(TextReader reader)
    {
      _reader = reader;
      NextChar();
      NextToken();
    }

    public Token Token
    {
      get { return _currentToken; }
    }

    public double Number
    {
      get { return _number; }
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
          _currentToken = Token.EOF;
          return;

        case '+':
          NextChar();
          _currentToken = Token.Add;
          return;

        case '-':
          NextChar();
          _currentToken = Token.Subtract;
          return;
        
        case '*':
          NextChar();
          _currentToken = Token.Multiply;
          return;

        case '/':
          NextChar();
          _currentToken = Token.Divide;
          return;

        case '(':
          NextChar();
          _currentToken = Token.OpenParens;
          return;

        case ')':
          NextChar();
          _currentToken = Token.CloseParens;
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

        _number = double.Parse(sb.ToString(), CultureInfo.InvariantCulture);
        _currentToken = Token.Number;
        return;
      }

      throw new InvalidDataException($"Unexpected character: {_currentChar}");
    }
  }
}