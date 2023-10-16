namespace Trellol.Util
{
    // Utility class for string manipulation extensions
    public static class StringExtensions
    {
        // Extension method to capitalize the first character of a string
        public static string FirstCharToUpper(this string input)
        {
            // Check if the input string is null or empty
            if (string.IsNullOrEmpty(input))
            {
                // Return the input as is
                return input;
            }

            // Capitalize the first character and concatenate the rest of the string
            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}
