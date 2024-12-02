namespace TestUseComAot.Views.MainWindows;

public sealed partial class MainWindow : WindowEx
{
	public MainWindow()
	{
		InitializeComponent();

		AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/WindowIcon.ico"));
		Content = new MainPage();
		Title = "AOT で COM を使うテスト";
	}
}
