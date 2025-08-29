using System;
using System.Globalization;
using System.Windows.Data;

using WpfDataBindingMRE.Code.FileMru;

namespace WpfDataBindingMRE.Code.Converters;

/// <summary>
///		Converts a <see cref="MruItem"/> value
///		into a <see langword="string"/> value.
/// </summary>
public class MruItemConverter : IValueConverter
{
	/// <summary>
	///		A static, singleton instance of
	///		the <see cref="MruItemConverter"/>
	///		class.
	/// </summary>
	public static MruItemConverter Instance { get; } = new MruItemConverter();



	/// <inheritdoc/>
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> value is MruItem v ? $"_{v.Index}  {v.FilePath}" : "";

	/// <inheritdoc/>
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> new NotImplementedException();
}
