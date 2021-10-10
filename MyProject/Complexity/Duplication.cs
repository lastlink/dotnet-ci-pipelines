namespace MyProject.Complexity
{
    /// <summary>
    /// code quality checks should flag some things here
    /// </summary>
    public class Duplication
    {
        public static int TimesTwo(int input)
        {
            var result = input * 2;
            return result;
        }

        public static int MultiplyByTwo(int input)
        {
            var result = input * 2;
            return result;
        }
    }
}