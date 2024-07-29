using System.Windows;

namespace Triad_Practice;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        
        var model = new MainModel();
        
        model.TriadsChanged += (sender, args) =>
        {
            CurrentTriad.DataContext = new TriadViewModel(model.CurrentTriad);
            NextTriad.DataContext = new TriadViewModel(model.NextTriad);
        };
        
        _viewModel = new MainViewModel(model);
        DataContext = _viewModel;
    }

    private void StartStopMetronome_OnClick(object sender, RoutedEventArgs e)
    {
        CurrentTriad.DataContext = null;
        NextTriad.DataContext = null;
        _viewModel.StartStopMetronome_OnClick();
    }
}