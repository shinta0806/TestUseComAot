using Microsoft.UI.Xaml.Controls;
using TestUseComAot.ViewModels.MainWindows;

namespace TestUseComAot.Views.MainWindows;

public sealed partial class MainPage : Page
{
	public MainPageViewModel ViewModel
	{
		get;
	}

	public MainPage()
	{
		ViewModel = new MainPageViewModel();
		InitializeComponent();
	}
}
