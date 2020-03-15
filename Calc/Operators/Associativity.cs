namespace CalcTest
{
  public enum Associativity
  {
    LEFT, // a + b + c => (a + b) + c
    RIGHT // a^b^c => a^(b^c) (^ as in Math.Pow)
  }
}