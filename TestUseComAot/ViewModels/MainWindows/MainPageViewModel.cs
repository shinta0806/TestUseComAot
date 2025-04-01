// ============================================================================
// 
// メインページの ViewModel
// 
// ============================================================================

// ----------------------------------------------------------------------------
// 
// ----------------------------------------------------------------------------

using System.Runtime.InteropServices;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;
using Windows.Win32.UI.Input.Ime;

namespace TestUseComAot.ViewModels.MainWindows;

public partial class MainPageViewModel : ObservableRecipient
{
	// ====================================================================
	// コンストラクター
	// ====================================================================

	/// <summary>
	/// メインコンストラクター
	/// </summary>
	public MainPageViewModel()
	{
		ButtonConvertClickedCommand = new RelayCommand(ButtonConvertClicked);
	}

	// ====================================================================
	// public プロパティー
	// ====================================================================

	// --------------------------------------------------------------------
	// View 通信用のプロパティー
	// --------------------------------------------------------------------

	/// <summary>
	/// 逆変換前の漢字
	/// </summary>
	private String? _kanji = "夕飯は唐揚げにしよう";
	public String? Kanji
	{
		get => _kanji;
		set => SetProperty(ref _kanji, value);
	}

	/// <summary>
	/// 逆変換後のひらがな
	/// </summary>
	private String? _hiragana;
	public String? Hiragana
	{
		get => _hiragana;
		set => SetProperty(ref _hiragana, value);
	}

	// --------------------------------------------------------------------
	// コマンド
	// --------------------------------------------------------------------

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
				IFELanguage* ime = null;
				BSTR kanjiBStr = new();
				BSTR hiraganaBStr = new();
				try
				{
					if (String.IsNullOrEmpty(Kanji))
					{
						throw new Exception("漢字が入力されていません。");
					}

					// "MSIME.Japan"
					Guid clsId = new("6a91029e-aa49-471b-aee7-7d332785660d");

					// COM オブジェクト作成
					// ポインタ取得後 IFELanguage ime2 = *ime; のようなコードはビルドできるが正常に動作しない
					HRESULT result = PInvoke.CoCreateInstance(clsId, null, CLSCTX.CLSCTX_INPROC_SERVER | CLSCTX.CLSCTX_INPROC_HANDLER | CLSCTX.CLSCTX_LOCAL_SERVER,
						out ime);
					if (result.Failed)
					{
						throw new Exception("COM オブジェクトの作成に失敗：" + result);
					}

					// IME 初期化
					result = ime->Open();
					if (result.Failed)
					{
						throw new Exception("IME の初期化に失敗：" + result);
					}

					// 逆変換
					// "useSafeHandles": true の時は SysFreeStringSafeHandle を使うのかもしれない
					// BSTR を Char* コンストラクターで作ると文字列の長さが半分になってしまうので StringToBSTR() で作る
					kanjiBStr = (BSTR)Marshal.StringToBSTR(Kanji);
					result = ime->GetPhonetic(kanjiBStr, 1, -1, ref hiraganaBStr);
					if (result.Failed)
					{
						throw new Exception("逆変換に失敗：" + result);
					}
					Hiragana = hiraganaBStr.ToString();

					// IME 後始末
					result = ime->Close();
					if (result.Failed)
					{
						throw new Exception("IME の後始末に失敗：" + result);
					}
				}
				finally
				{
					Marshal.FreeBSTR(hiraganaBStr);
					Marshal.FreeBSTR(kanjiBStr);
					if (ime != null)
					{
						// COM 解放
						ime->Release();
					}
				}
			}
		}
		catch (Exception ex)
		{
			await App.MainWindow.ShowMessageDialogAsync(ex.Message);
		}
	}
	#endregion
}
