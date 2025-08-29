using System;
using System.Resources;
using System.Windows.Input;

namespace WpfDataBindingMRE.Code.Commands;

public abstract class _CommandBase : RoutedCommand
{
	private static string AddString(string text, string addText) => string.IsNullOrWhiteSpace(text) ? addText : text + "+" + addText;

	public static KeyGesture LocalizeKeyGesture(KeyGesture gesture)
	{
		Type cmdType = typeof(_CommandBase);
		ResourceManager resMan = new ResourceManager(cmdType.FullName![..^cmdType.Name.Length] + "Resources.Modifiers", cmdType.Assembly);
		string gestureString = "";

		if ((gesture.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt) gestureString = AddString(gestureString, resMan.GetString(nameof(ModifierKeys.Alt)) ?? "");
		if ((gesture.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) gestureString = AddString(gestureString, resMan.GetString(nameof(ModifierKeys.Shift)) ?? "");
		if ((gesture.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) gestureString = AddString(gestureString, resMan.GetString(nameof(ModifierKeys.Control)) ?? "");

		return new KeyGesture(gesture.Key, gesture.Modifiers, AddString(gestureString, gesture.Key.ToString()));
	}
}
