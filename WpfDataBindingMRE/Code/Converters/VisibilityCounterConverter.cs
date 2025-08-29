using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfDataBindingMRE.Code.Converters;

/// <summary>
///		Converts an <see langword="int"/> value into
///		a <see cref="Visibility"/> value. If the
///		<see langword="int"/> value is larger than zero,
///		<see cref="Visibility.Visible"/> is yielded;
///		otherwise <see cref="Visibility.Collapsed"/>.
/// </summary>
public class VisibilityCounterConverter : IValueConverter
{
	/// <summary>
	///		A static, singleton instance of
	///		the <see cref="VisibilityCounterConverter"/>
	///		class.
	/// </summary>
	public static VisibilityCounterConverter Instance { get; } = new VisibilityCounterConverter();



	/// <inheritdoc/>
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> value is int v && v > 0 ? Visibility.Visible : Visibility.Collapsed;

	/// <inheritdoc/>
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		=> value is Visibility v && v == Visibility.Visible ? 1 : 0;
}
