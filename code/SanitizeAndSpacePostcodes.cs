/// <summary>
/// We want to trim any start and end whitespace, whilst ensuring we keep the space in the middle
/// </summary>
/// <param name="original"></param>
/// <returns></returns>
private static string SanitizeAndFormatPostcodeWithSpace(string original)
{
    char[] Digits = "0123456789".ToCharArray();
    string noSpaces = original.Replace(" ", "").Trim();
    int lastDigit = noSpaces.LastIndexOfAny(Digits);

    if (lastDigit == -1)
    {
        throw new ArgumentException("No digits!");
    }

    return noSpaces.Insert(lastDigit, " ");
}