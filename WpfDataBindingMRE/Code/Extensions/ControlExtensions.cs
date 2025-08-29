using System;
using System.Resources;
using System.Windows;
using System.Windows.Controls;

namespace WpfDataBindingMRE.Code.Extensions;

/// <summary>
///		Extensions for WPF <see cref="Control"/>
///		objects.
/// </summary>
public static class ControlExtensions
{
	/// <summary>
	///		Recursively sets localized menu item texts to the
	///		specified <see cref="Menu"/> object and its child
	///		<see cref="MenuItem"/> objects.
	/// </summary>
	/// <param name="parent">
	///		<see cref="Window"/> or <see cref="UserControl"/>
	///		whose namespace path will be used for retrieving
	///		the corresponding resource file.
	/// </param>
	/// <param name="menu">
	///		Menu control to receive the menu item text
	///		found in the text resources specified by
	///		the <paramref name="resMan"/> parameter.
	/// </param>
	public static void SetMenuItemTexts(this Control parent, Menu menu)
	{
		Type ctrlType = parent.GetType();
		ResourceManager resMan = new ResourceManager(ctrlType.FullName![..^ctrlType.Name.Length] + "Resources.MenuItems", ctrlType.Assembly);

		SetMenuItemTexts(menu, resMan);
	}

	/// <summary>
	///		Recursively sets menu item texts from
	///		the specified <see cref="ResourceManager"/>
	///		to the specified <see cref="MenuItem"/>
	///		controls.
	/// </summary>
	/// <param name="parent">
	///		<see cref="MenuItem"/> control to receive
	///		the menu item text found in the text resources
	///		specified by the <paramref name="resMan"/>
	///		parameter.
	/// </param>
	/// <param name="resMan">
	///		<see cref="ResourceManager"/> object to
	///		retrieve menu item texts from.
	/// </param>
	private static void SetMenuItemTexts(ItemsControl parent, ResourceManager resMan)
	{
		if (parent is MenuItem item && resMan.GetString(item.Name) is string text) item.Header = text;

		foreach (Control child in parent.Items) if (child is MenuItem childItem) SetMenuItemTexts(childItem, resMan);
	}
}
