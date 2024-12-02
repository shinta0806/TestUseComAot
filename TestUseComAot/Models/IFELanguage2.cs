// ============================================================================
// 
// IFELanguage2 インターフェース
// 
// ============================================================================

// ----------------------------------------------------------------------------
// Input Method Editor Reference
// https://learn.microsoft.com/en-us/previous-versions/office/developer/office-2007/ee828920(v=office.12)?redirectedfrom=MSDN
// ----------------------------------------------------------------------------

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace TestUseComAot.Models;

// AOT では [ComImport] [InterfaceType] は使用しない
[GeneratedComInterface]
[Guid("21164102-C24A-11d1-851A-00C04FCC6B14")]
public partial interface IFELanguage2
{
	// --------------------------------------------------------------------
	// IFE Language 1
	// --------------------------------------------------------------------

	/// <summary>
	/// 初期化
	/// </summary>
	/// <returns></returns>
	[PreserveSig]
	Int32 Open();

	/// <summary>
	/// 後始末
	/// </summary>
	/// <returns></returns>
	[PreserveSig]
	Int32 Close();

	// --------------------------------------------------------------------
	// IFE Language 2
	// --------------------------------------------------------------------

	/// <summary>
	/// モーフ解析
	/// </summary>
	/// <param name="request"></param>
	/// <param name="cmode"></param>
	/// <param name="cwchInput"></param>
	/// <param name="pwchInput"></param>
	/// <param name="cinfo"></param>
	/// <param name="result"></param>
	/// <returns></returns>
	[PreserveSig]
	Int32 GetMorphResult(UInt32 request, UInt32 cmode, Int32 cwchInput, [MarshalAs(UnmanagedType.LPWStr)] String pwchInput, IntPtr cinfo, out IntPtr result);

	/// <summary>
	/// 変換モード
	/// </summary>
	/// <param name="caps"></param>
	/// <returns></returns>
	[PreserveSig]
	Int32 GetConversionModeCaps(ref UInt32 caps);

	/// <summary>
	/// 漢字→ひらがな
	/// </summary>
	/// <param name="str"></param>
	/// <param name="start"></param>
	/// <param name="length"></param>
	/// <param name="result"></param>
	/// <returns></returns>
	[PreserveSig]
	Int32 GetPhonetic([MarshalAs(UnmanagedType.BStr)] String str, Int32 start, Int32 length, [MarshalAs(UnmanagedType.BStr)] out String result);

	/// <summary>
	/// ひらがな→漢字
	/// </summary>
	/// <param name="str"></param>
	/// <param name="start"></param>
	/// <param name="length"></param>
	/// <param name="result"></param>
	/// <returns></returns>
	[PreserveSig]
	Int32 GetConversion([MarshalAs(UnmanagedType.BStr)] String str, Int32 start, Int32 length, [MarshalAs(UnmanagedType.BStr)] out String result);
}
