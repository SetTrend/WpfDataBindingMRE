using WpfDataBindingMRE.Code;
using WpfDataBindingMRE.Code.Preferences;

namespace WpfDataBindingMRE.WpfWindows.Main;

public class FullConfig : _NotifyPropertyChanged
{
	private Options _options;



	/// <summary>
	///		Gets or sets the runtime preferences
	///		properties of the application.
	/// </summary>
	/// <value>
	///		<see cref="Options"/> object
	///		specifying the current runtime
	///		preferences properties.
	/// </value>
	public Options Preferences
	{
		get => _options;

		set
		{
			_options = value;

			OnPropertyChanged();
		}
	}



	/// <summary>
	///		Initializes a new <see cref="FullConfig"/>
	///		object.
	/// </summary>
	public FullConfig()
	{
		_options = new Options();
	}
}
