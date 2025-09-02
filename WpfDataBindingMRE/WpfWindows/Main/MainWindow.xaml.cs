using System;
using System.Configuration;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

using WpfDataBindingMRE.Code.Extensions;
using WpfDataBindingMRE.Code.FileMru;
using WpfDataBindingMRE.Code.Preferences;

namespace WpfDataBindingMRE.WpfWindows.Main;

public partial class MainWindow : Window
{
	public static readonly DependencyProperty SettingsProperty = DependencyProperty.Register("Settings"
																																																, typeof(FullConfig)
																																																, typeof(MainWindow)
																																																);

	/// <summary>
	///		Gets or sets the applications runtime settings.
	/// </summary>
	public FullConfig Settings
	{
		get => (FullConfig)GetValue(SettingsProperty);
		private init => SetValue(SettingsProperty, value);
	}



	/// <summary>
	///		Gets the current application session's
	///		settings.
	/// </summary>
	private Options Options => Settings.Preferences;



	public FileMruList FileMruList => Options.FileMruList;



	public MainWindow()
	{
		Settings = new FullConfig();

		InitializeComponent();

		this.SetMenuItemTexts(MainMenu);
	}



	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		try
		{
			Settings.Preferences = new Options(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));

			FileMruList.Add("Test a");
			FileMruList.Add("Test b");
		}
		catch (Exception) { }
	}

	private void AppCloseCommand_Executed(object sender, ExecutedRoutedEventArgs e) => Close();



	private void FileMenu_SubmenuOpened(object sender, RoutedEventArgs e)
	{
		//FileMenu_MruListSeparatur.GetBindingExpression(VisibilityProperty).UpdateTarget();
		//FileMenu_MruList.GetBindingExpression(VisibilityProperty).UpdateTarget();
		//BindingOperations.GetBindingExpression((CollectionViewSource)FileMenu_MruList.FindResource("MruFiles"), CollectionViewSource.SourceProperty).UpdateTarget();
	}

	private void FileMruItem_Executed(object sender, ExecutedRoutedEventArgs e)
	{
		MessageBox.Show(this, "Executed", "Test", MessageBoxButton.OK, MessageBoxImage.Information);
	}

	private void FileMruListClear_Executed(object sender, ExecutedRoutedEventArgs e)
	{
		FileMruList.Clear();

		e.Handled = true;
	}
}