namespace SimplestUnitTest.Tests
{
    public class SimplestUnitFunctionTests
    {
        public static void SimplestUnitFunction_ReturnsPikachuIfZero_ReturnsString()
        {
            try
            {
                var num = 0;
                var simplest = new SimplestUnitFunction();

                var result = simplest.ReturnsPikachuIfZero(num);

                if (result == "PIKACHU!")
                {
                    Console.WriteLine("PASSED: SimplestUnitFunction_ReturnsPikachuIfZero_ReturnsString");
                }
                else
                {
                    Console.WriteLine("FAILED: SimplestUnitFunction_ReturnsPikachuIfZero_ReturnsString");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
