namespace Triad_Practice;

public class Triad(StringCombinations stringCombination, string imageName, Key key)
{
    public Key Key { get; } = key;
    public string ImageName { get; } = imageName;
    public StringCombinations StringCombination { get; } = stringCombination;
}