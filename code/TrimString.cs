/// <summary>
/// Trims a string down to a specified length
/// </summary>
/// <param name="input">String to trim</param>
/// <param name="maxLength">Max length</param>
/// <param name="minLength">Optional minimum length</param>
/// <returns></returns>
public static string TrimString(string input, int maxLength, int minLength = 0)
{
    if (!string.IsNullOrEmpty(input))
        return input.Length <= maxLength ? input : input.Substring(0, maxLength);

    return "".PadLeft(minLength, ' ');
}