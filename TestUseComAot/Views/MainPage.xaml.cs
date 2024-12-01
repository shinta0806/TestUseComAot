using Microsoft.UI.Xaml.Controls;

using TestUseComAot.ViewModels;

namespace TestUseComAot.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
