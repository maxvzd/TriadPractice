using System.ComponentModel;
using System.Windows;

namespace Triad_Practice;

public class MainViewModel : NotifyPropertyChanged
{
    private readonly MainModel _mainModel;
    private string _selectedKey;

    public bool ShouldPlayMajorTick
    {
        get => _mainModel.ShouldPlayMajorTick;
        set
        {
            _mainModel.ShouldPlayMajorTick = value;
            OnPropertyChanged();
        }
    }

    public string StartStopButtonText
    {
        get => _startStopButtonText;
        set
        {
            _startStopButtonText = value;
            OnPropertyChanged();
        }
    }
    
    public int ClicksPerTriad
    {
        get => _mainModel.ClicksPerTriad;
        set
        {
            _mainModel.ClicksPerTriad = value;
            OnPropertyChanged();
            OnPropertyChanged("ClicksUntilChange");
        }
    }
    
    public int BeatsPerMeasure
    {
        get => _mainModel.BeatsPerMeasure;
        set
        {
            _mainModel.BeatsPerMeasure = value;
            OnPropertyChanged();
        }
    }
    
    public int BeatsPerMinute
    {
        get => _mainModel.BeatsPerMinute;
        set
        {
            _mainModel.BeatsPerMinute = value;
            OnPropertyChanged();
        }
    }
    
    public string SelectedKey
    {
        get => _selectedKey;
        set
        {
            _selectedKey = value;
            OnPropertyChanged();
        }
    }

    public string ClicksUntilChange => (ClicksPerTriad - _mainModel.ClickCount).ToString();

    private bool _isRunning;
    private string _startStopButtonText;

    public MainViewModel(MainModel model)
    {
        _startStopButtonText = "Start";
        _isRunning = false;
        _mainModel = model;

        _mainModel.MetronomeTicked += (sender, args) =>
        {
            OnPropertyChanged("ClicksUntilChange");            
        };
        
        // _currentTriadVm = new TriadViewModel(_mainModel.CurrentTriad);
        // _nextTriadVm = new TriadViewModel(_mainModel.NextTriad);

        _selectedKey = Key.C.ToString();
    }

    public void StartStopMetronome_OnClick()
    {
        if (!_isRunning)
        {
            List<Key> keysToChooseFrom = ParseSelectedKey();
            
            _mainModel.StartMetronome(keysToChooseFrom);
            StartStopButtonText = "Stop";

        }
        else
        {
            _mainModel.StopMetronome();
            StartStopButtonText = "Start";
        }
        _isRunning = !_isRunning;
    }
    
    private List<Key> ParseSelectedKey()
    {
        List<Key> parsedKeys = new List<Key>();

        if (Enum.TryParse(SelectedKey, out Key selectedKey))
        {
            parsedKeys.Add(selectedKey);
        }
        else if (SelectedKey == "All keys no sharps")
        {
            parsedKeys.Add(Key.A);
            parsedKeys.Add(Key.B);
            parsedKeys.Add(Key.C);
            parsedKeys.Add(Key.D);
            parsedKeys.Add(Key.E);
            parsedKeys.Add(Key.F);
            parsedKeys.Add(Key.G);
        }
        else if (SelectedKey == "All keys")
        {
            parsedKeys.Add(Key.Ab);
            parsedKeys.Add(Key.A);
            parsedKeys.Add(Key.Bb);
            parsedKeys.Add(Key.B);
            parsedKeys.Add(Key.C);
            parsedKeys.Add(Key.Db);
            parsedKeys.Add(Key.D);
            parsedKeys.Add(Key.Eb);
            parsedKeys.Add(Key.E);
            parsedKeys.Add(Key.F);
            parsedKeys.Add(Key.G);
            parsedKeys.Add(Key.Gb);
        }

        return parsedKeys;
    }

}