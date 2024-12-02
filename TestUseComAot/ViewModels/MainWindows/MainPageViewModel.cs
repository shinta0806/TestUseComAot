using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TestUseComAot.Models;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;

namespace TestUseComAot.ViewModels.MainWindows;

public partial class MainPageViewModel : ObservableRecipient
{
	public MainPageViewModel()
	{
		ButtonConvertClickedCommand = new RelayCommand(ButtonConvertClicked);
	}

	private String? _kanji = "夕飯は唐揚げにしよう";
	public String? Kanji
	{
		get => _kanji;
		set => SetProperty(ref _kanji, value);
	}

	private String? _hiragana;
	public String? Hiragana
	{
		get => _hiragana;
		set => SetProperty(ref _hiragana, value);
	}

	#region 逆変換
	public RelayCommand ButtonConvertClickedCommand
	{
		get;
	}

	private async void ButtonConvertClicked()
	{
		try
		{
			unsafe
			{
				if (String.IsNullOrEmpty(Kanji))
				{
					throw new Exception("漢字が入力されていません。");
				}

				// "MSIME.Japan"
				Guid clsId = new("6a91029e-aa49-471b-aee7-7d332785660d");

				// COM オブジェクト作成
				HRESULT result = PInvoke.CoCreateInstance(clsId, null, CLSCTX.CLSCTX_INPROC_SERVER | CLSCTX.CLSCTX_INPROC_HANDLER | CLSCTX.CLSCTX_LOCAL_SERVER,
					typeof(IFELanguage2).GUID, out void* ppv);
				if (result.Failed)
				{
					throw new Exception("COM オブジェクトの作成に失敗：" + result);
				}

				ComWrappers comWrappers = new StrategyBasedComWrappers();
				IFELanguage2 lang = (IFELanguage2)comWrappers.GetOrCreateObjectForComInstance((nint)ppv, CreateObjectFlags.None);

				// IME 初期化
				result = (HRESULT)lang.Open();
				if (result.Failed)
				{
					throw new Exception("IME の初期化に失敗：" + result);
				}

				// 逆変換
				result = (HRESULT)lang.GetPhonetic(Kanji, 1, -1, out String hiragana);
				if (result.Failed)
				{
					throw new Exception("逆変換に失敗：" + result);
				}
				Hiragana = hiragana;

				// IME 後始末
				result = (HRESULT)lang.Close();
				if (result.Failed)
				{
					throw new Exception("IME の後始末に失敗：" + result);
				}

				// ToDo: COM の解放はどうするのか
			}
		}
		catch (Exception ex)
		{
			await App.MainWindow.ShowMessageDialogAsync(ex.Message);
		}
	}
	#endregion
}
