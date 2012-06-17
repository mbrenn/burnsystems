namespace BurnSystems.UnitTests.ObjectActivation
{
	/// <summary>
	/// Just a test class implementing ICalculator
	/// </summary>
	public class Calculator : ICalculator
	{
		public int InternalAddOffset
		{
			get;
			set;
		}
		
		public int Add(int a, int b)
		{
			return a + b + this.InternalAddOffset;
		}

		public int Subtract(int a, int b)
		{
			return a - b;
		}

		public int Multiply(int a, int b)
		{
			return a * b;
		}
	}
}
