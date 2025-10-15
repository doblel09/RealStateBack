namespace RealStateApp.Core.Application.Helpers
{
    public class CodeGenerator
    {
        private static readonly HashSet<int> generatedCodes = new HashSet<int>();

        public static int GenerateUniqueCode()
        {
            Random random = new Random();
            int newCode;

            do
            {
                newCode = random.Next(100000, 1000000);
            }
            while (generatedCodes.Contains(newCode));

            generatedCodes.Add(newCode);
            return newCode;
        }
    }
}