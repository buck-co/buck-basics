namespace Buck
{
    // Extension Methods involving strings
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// Returns the string shortened to the max length. Optionally, you can add an elipsis ("...") to the end. If the string is shorter than the max length it will return itself and not add the elipsis.
        /// </summary>
        /// <param name="value">The original string to truncate.</param>
        /// <param name="maxLength">How many characters the string should be shortened to.</param>
        /// <param name="addEllipsis">If true and the string is shortened, will add "..." to the end.</param>
        public static string Truncate(this string value, int maxLength, bool addEllipsis = false)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + (addEllipsis ? "..." : "");
        }
    }
}
