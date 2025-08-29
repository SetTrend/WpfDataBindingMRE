using System;
using System.Configuration;
using System.Linq;

using WpfDataBindingMRE.Code.FileMru;
using WpfDataBindingMRE.Code.Preferences.Resources;

namespace WpfDataBindingMRE.Code.Preferences;

public class Options : _NotifyPropertyChanged, IEquatable<Options>
{
	private const string _mruCapacitySetting = "MruFilesCapacity";
	private const char _mruConfigListSplitter = '|';



	private byte _tabSize = 2;
	private FileMruList _fileMruList = null!;



	/// <summary>
	///		Gets whether this object's property
	///		values are valid.
	/// </summary>
	/// <value>
	///		<see langword="true"/>, if this
	///		object's property values are all
	///		valid; <see langword="false"/>
	///		otherwise.
	/// </value>
	public bool IsValid => _tabSize < 10;

	/// <summary>
	///		Gets or sets the number of spaces that
	///		will be inserted into the input text
	///		field instead of a TAB character.
	/// </summary>
	/// <value>
	///		<see langword="int"/> number of spaces
	///		that will be inserted into the input
	///		text field instead of a TAB character.
	/// </value>
	public byte TabSize
	{
		get => _tabSize;

		set
		{
			_tabSize = value;

			OnPropertyChanged();

			if (value > 9) throw new ArgumentException(string.Format(Messages.InvalidValue_Error_Message, nameof(TabSize), value));
		}
	}

	/// <summary>
	///		Gets or sets the Most Recently Used
	///		Files collection.
	/// </summary>
	public FileMruList FileMruList
	{
		get => _fileMruList;

		private init
		{
			_fileMruList = value;
			OnPropertyChanged();
		}
	}



	/// <summary>
	///		Creates a new, uninitialized, invalid
	///		<see cref="Options"/> object.
	/// </summary>
	public Options() => FileMruList = [];

	/// <summary>
	///		Initializes a new <see cref="Options"/>
	///		object from the specified configuration
	///		file.
	/// </summary>
	/// <param name="configuration">
	///		Configuration file to read application
	///		settings from.
	/// </param>
	public Options(Configuration configuration)
	{
		TabSize = byte.TryParse(configuration.AppSettings.Settings[nameof(TabSize)]?.Value, out byte tabSize) ? tabSize : (byte)2;

		byte capacity = byte.TryParse(configuration.AppSettings.Settings[_mruCapacitySetting]?.Value, out byte c) ? c : (byte)4;
		string? fileMruList = configuration.AppSettings.Settings[nameof(FileMruList)]?.Value;

		FileMruList = new FileMruList(capacity, string.IsNullOrWhiteSpace(fileMruList) ? [] : fileMruList.Split(_mruConfigListSplitter));
	}

	/// <summary>
	///		Initializes a new <see cref="Options"/>
	///		object from the specified source
	///		<see cref="Options"/> object's properties.
	/// </summary>
	/// <param name="other">
	///		<see cref="Options"/> object to copy
	///		property values from.
	/// </param>
	public Options(Options other)
		=> (TabSize, FileMruList)
		= (other._tabSize, other.FileMruList);



	/// <summary>
	///		Saves this object's current property
	///		values to the specified configuration
	///		file.
	/// </summary>
	/// <param name="configuration">
	///		Configuration file to write application
	///		settings to.
	/// </param>
	public void Save(Configuration configuration)
	{
		if (IsValid)
		{
			configuration.AppSettings.Settings.Remove(nameof(TabSize));
			configuration.AppSettings.Settings.Add(nameof(TabSize), TabSize.ToString());

			configuration.AppSettings.Settings.Remove(_mruCapacitySetting);
			configuration.AppSettings.Settings.Add(_mruCapacitySetting, FileMruList.Capacity.ToString());

			configuration.AppSettings.Settings.Remove(nameof(FileMruList));
			configuration.AppSettings.Settings.Add(nameof(FileMruList), FileMruList.Count == 0 ? null : string.Join(_mruConfigListSplitter, FileMruList));

			configuration.Save(ConfigurationSaveMode.Modified);
		}
	}



	public bool Equals(Options? other)
	{
		if (other is not null
				&& _tabSize == other._tabSize
				&& FileMruList.Capacity == other.FileMruList.Capacity
				&& FileMruList.SequenceEqual(other.FileMruList)
				)
			return true;

		return false;
	}

	public override bool Equals(object? obj) => Equals(obj as Options);

	public override int GetHashCode()
		=> _tabSize.GetHashCode()
			+ FileMruList.Capacity.GetHashCode()
			+ FileMruList.GetHashCode()
			;



	public static bool operator ==(Options a, Options b) => a.Equals(b);

	public static bool operator !=(Options a, Options b) => !a.Equals(b);
}
