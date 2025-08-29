using System;

namespace WpfDataBindingMRE.Code.FileMru;

/// <summary>
///		Specifies an item in the Most Recently
///		Used Files collection.
/// </summary>
public class MruItem : _NotifyPropertyChanged, IEquatable<MruItem>
{
	private int _index;
	private string _filePath = null!;



	/// <summary>
	///		Gets or sets the index of this item
	///		in the <see cref="FileMruList"/>
	///		collection.
	/// </summary>
	public int Index
	{
		get => _index;

		set
		{
			_index = value;
			OnPropertyChanged();
		}
	}

	/// <summary>
	///		Gets the file path.
	/// </summary>
	public string FilePath
	{
		get => _filePath;

		private init
		{
			_filePath = value;
			OnPropertyChanged();
		}
	}



	/// <summary>
	///		Initializes a new <see cref="MruItem"/>
	///		object.
	/// </summary>
	/// <param name="filePath">
	///		A file path.
	/// </param>
	public MruItem(string filePath) => FilePath = filePath;



	/// <summary>
	///		Implicitly converts a string int
	///		an <see cref="MruItem"/> object.
	/// </summary>
	/// <param name="filePath">
	///		String representing a file path.
	/// </param>
	public static implicit operator MruItem(string filePath) => new MruItem(filePath);

	/// <inheritdoc/>
	public override string ToString() => _filePath;



	/// <inheritdoc/>
	public bool Equals(MruItem? other) => other is not null && string.Compare(FilePath, other.FilePath) == 0;

	/// <inheritdoc/>
	public override bool Equals(object? obj) => obj is MruItem o && Equals(o);

	/// <inheritdoc/>
	public override int GetHashCode() => FilePath.ToLowerInvariant().GetHashCode();


	/// <summary>
	///		Indicates whether the <paramref name="left"/>
	///		<see cref="MruItem"/> object points to the
	///		same file path as the <paramref name="right"/>
	///		<see cref="MruItem"/> object.
	/// </summary>
	/// <param name="left">
	///		<see cref="MruItem"/> object to compare
	///		with the <paramref name="right"/>
	///		<see cref="MruItem"/> object.
	/// </param>
	/// <param name="right">
	///		<see cref="MruItem"/> object to be compared
	///		with the <paramref name="left"/>
	///		<see cref="MruItem"/> object.
	/// </param>
	/// <returns>
	///		<see langword="true"/> if both <see cref="MruItem"/>
	///		objects are pointing to the same file;
	///		<see langword="false"/> otherwise.
	/// </returns>
	public static bool operator ==(MruItem left, MruItem right) => left.Equals(right);

	/// <summary>
	///		Indicates whether the <paramref name="left"/>
	///		<see cref="MruItem"/> object points to another
	///		file path than the <paramref name="right"/>
	///		<see cref="MruItem"/> object.
	/// </summary>
	/// <param name="left">
	///		<see cref="MruItem"/> object to compare
	///		with the <paramref name="right"/>
	///		<see cref="MruItem"/> object.
	/// </param>
	/// <param name="right">
	///		<see cref="MruItem"/> object to be compared
	///		with the <paramref name="left"/>
	///		<see cref="MruItem"/> object.
	/// </param>
	/// <returns>
	///		<see langword="false"/> if both <see cref="MruItem"/>
	///		objects are pointing to the same file;
	///		<see langword="true"/> otherwise.
	/// </returns>
	public static bool operator !=(MruItem left, MruItem right) => !left.Equals(right);

}