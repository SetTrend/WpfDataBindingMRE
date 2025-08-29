using System.Collections.Generic;
using System.Windows.Input;

namespace WpfDataBindingMRE.Code.Extensions;

internal static class CommandExtensions
{
	internal static string GetMnemonicText(this RoutedCommand command, string headerText)
	{
		foreach (InputGesture gesture in command.InputGestures)
			if (gesture is KeyGesture keyGesture && keyGesture.Key != Key.None)
			{
				headerText += $"  ({keyGesture.DisplayString})";
				break;
			}

		return headerText;
	}
}
