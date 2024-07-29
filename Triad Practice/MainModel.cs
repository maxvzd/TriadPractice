using System.Media;
using System.Windows.Threading;

namespace Triad_Practice;

public class MainModel
{
    private readonly List<PossibleTriad> _possibleTriads =
    [
        new PossibleTriad(StringCombinations.DGB, "DGB1"),
        new PossibleTriad(StringCombinations.DGB, "DGB2"),
        new PossibleTriad(StringCombinations.DGB, "DGB3"),
        new PossibleTriad(StringCombinations.GBE, "GBE1"),
        new PossibleTriad(StringCombinations.GBE, "GBE2"),
        new PossibleTriad(StringCombinations.GBE, "GBE3"),
        new PossibleTriad(StringCombinations.ADG, "EADG1"),
        new PossibleTriad(StringCombinations.ADG, "EADG2"),
        new PossibleTriad(StringCombinations.ADG, "EADG3"),
        new PossibleTriad(StringCombinations.EAD, "EADG1"),
        new PossibleTriad(StringCombinations.EAD, "EADG2"),
        new PossibleTriad(StringCombinations.EAD, "EADG3")
    ];
    
    private readonly SoundPlayer _minorTick;
    private readonly SoundPlayer _majorTick;
    private readonly DispatcherTimer _metronome;
    private int _currentBeat = 0;
    private List<Key> _possibleKeys;
    
    public Triad CurrentTriad;
    public Triad NextTriad;
    
    public event EventHandler? TriadsChanged;
    public event EventHandler? MetronomeTicked;
    
    public MainModel()
    {
        _minorTick = new SoundPlayer("Sounds/MinorTick.wav");
        _majorTick = new SoundPlayer("Sounds/MajorTick.wav");

        _metronome = new DispatcherTimer();

        ClickCount = 0;
        ClicksPerTriad = 4;
        
        BeatsPerMinute = 60;
        BeatsPerMeasure = 4;
        
        _metronome.Tick += metronome_tick;
        _possibleKeys = new List<Key>()
        {
            Key.C
        };

        NextTriad = new Triad(_possibleTriads[0].StringCombination, _possibleTriads[0].ImageName, _possibleKeys[0]);
        CurrentTriad = new Triad(_possibleTriads[0].StringCombination, _possibleTriads[0].ImageName, _possibleKeys[0]);
        GenerateNextTriad();
        

    }
    
    public bool ShouldPlayMajorTick { get; set; } = true;

    public int BeatsPerMinute { get; set; }

    public int BeatsPerMeasure { get; set; }

    public int ClicksPerTriad { get; set; }
    
    public int ClickCount { get; private set; }


    private void MetronomeTick()
    {
        if (_currentBeat == 0 && ShouldPlayMajorTick)
        {
            _majorTick.Play();
        }
        else
        {
            _minorTick.Play();
        }

        _currentBeat++;
        if (_currentBeat / BeatsPerMeasure == 1)
        {
            _currentBeat = 0;
        }

        ClickCount++;
        if (ClickCount == ClicksPerTriad)
        {
            GenerateNextTriad();
            ClickCount = 0;
        }
        
        MetronomeTicked?.Invoke(this, EventArgs.Empty);

    }
    
    private void metronome_tick(object? sender, EventArgs eventArgs)
    {
        MetronomeTick();
    }

    public void StartMetronome(List<Key> keysToChooseFrom)
    {
        double ticksPerMs = 60d / BeatsPerMinute * 1000d;
        _metronome.Interval = TimeSpan.FromMilliseconds(ticksPerMs);
        _possibleKeys = keysToChooseFrom;

        NextTriad = GenerateTriad(_possibleTriads, _possibleKeys);
        GenerateNextTriad();
        
        _metronome.Start();

        MetronomeTick();  
    }

    public void StopMetronome()
    {
         _metronome.Stop();
         _currentBeat = 0;
         ClickCount = 0;
    }
    
    private void GenerateNextTriad()
    {
        Triad generatedTriad = GenerateTriad(_possibleTriads, _possibleKeys);

        if (NextTriad.StringCombination == generatedTriad.StringCombination &&
            NextTriad.ImageName == generatedTriad.ImageName && 
            NextTriad.Key == generatedTriad.Key)
        {
            GenerateNextTriad();
        }
        else
        {
            CurrentTriad = NextTriad;
            NextTriad = generatedTriad;
        }
        
        TriadsChanged?.Invoke(this, EventArgs.Empty);
    }

    private static Triad GenerateTriad(List<PossibleTriad> possibleTriads, List<Key> keysToChooseFrom)
    {
        Random random = new Random();
        int triadSelection = random.Next(0, possibleTriads.Count);
        PossibleTriad generatedTriad = possibleTriads[triadSelection];
        
        int keySelect = random.Next(0, keysToChooseFrom.Count);
        Key generatedKey = keysToChooseFrom[keySelect];
        
        return new Triad(generatedTriad.StringCombination, generatedTriad.ImageName, generatedKey);
    }
}