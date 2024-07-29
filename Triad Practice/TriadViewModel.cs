namespace Triad_Practice;

public class TriadViewModel(Triad triadToPresent) : NotifyPropertyChanged
{
    public Key Key => triadToPresent.Key;

    public string ImageName => "\\Images\\" + triadToPresent.ImageName + ".png";

    public StringCombinations StringCombination => triadToPresent.StringCombination;
}