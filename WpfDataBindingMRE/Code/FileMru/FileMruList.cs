using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

using WpfDataBindingMRE.Code.Preferences.Resources;

namespace WpfDataBindingMRE.Code.FileMru;

/// <summary>
///		Maintains a list of most recently used files.
/// </summary>
[DebuggerDisplay($"{nameof(Count)} = {{{nameof(Count)}}}, {nameof(Capacity)} = {{{nameof(Capacity)}}}")]
public class FileMruList : ObservableCollection<MruItem>
{
	private bool _confine = true;
	private byte _capacity;



	/// <summary>
	///		The maximum number of items in
	///		the file MRU list.
	/// </summary>
	public byte Capacity
	{
		get => _capacity;

		set
		{
			_capacity = value;

			ConfineItemsToCapacity();

			OnPropertyChanged(new PropertyChangedEventArgs(nameof(Capacity)));

			if (value > 19) throw new ArgumentException(string.Format(Messages.InvalidValue_Error_Message, nameof(Capacity), value));
		}
	}



	/// <summary>
	///		Initializes a new <see cref="FileMruList"/>
	///		object that doesn't store any file paths.
	/// </summary>
	public FileMruList() : this(0, []) { }

	/// <summary>
	///		Initializes a new <see cref="FileMruList"/>
	///		object.
	/// </summary>
	/// <param name="capacity">
	///		The maximum number of items in
	///		the file MRU list.
	/// </param>
	public FileMruList(byte capacity) : this(capacity, []) { }

	/// <summary>
	///		Initializes a new <see cref="FileMruList"/>
	///		object.
	/// </summary>
	/// <param name="capacity">
	///		The maximum number of items in
	///		the file MRU list.
	/// </param>
	/// <param name="filePaths">
	///		File paths to be added to the collection.
	/// </param>
	public FileMruList(byte capacity, string[] filePaths)
	{
		string filePath;

		Capacity = capacity;

		for (int i = 0;i < capacity && i < filePaths.Length;++i) if (File.Exists(filePath = filePaths[i])) Add(new MruItem(filePath));
	}



	/// <summary>
	///		Removes the first items from this
	///		collection until the number of items
	///		equals the <see cref="Capacity"/>.
	///		If the number of items is less than
	///		<see cref="Capacity"/>, this method
	///		does nothing.
	/// </summary>
	public void ConfineItemsToCapacity()
	{
		if (_confine)
		{
			while (Count > Capacity) RemoveAt(0);
			for (int i = 0;i < Count;) this[i].Index = ++i;
		}
	}

	/// <summary>
	///		Adds the specified file paths to
	///		the end of the collection.
	/// </summary>
	/// <param name="filePaths">
	///		Collection of file paths to add
	///		to this collection.
	/// </param>
	public void AddRange(IEnumerable<string> filePaths)
	{
		_confine = false;

		try { foreach (string file in filePaths) Add(new MruItem(file)); }
		finally { _confine = true; }

		ConfineItemsToCapacity();
	}

	/// <summary>
	///		Inserts the specified file path at
	///		the first position in the collection
	///		and removes all subsequent duplicate
	///		items.
	/// </summary>
	/// <param name="filePath">
	///		Path to a file.
	/// </param>
	public void InsertFirst(string filePath)
	{
		_confine = false;

		try { foreach (MruItem found in this.Where(mi => string.Compare(mi.FilePath, filePath, true) == 0).ToArray()) Remove(found); }
		finally { _confine = true; }

		Insert(0, filePath);
	}



	/// <inheritdoc/>
	protected override void InsertItem(int index, MruItem item)
	{
		base.InsertItem(index, item);

		ConfineItemsToCapacity();
	}

	/// <inheritdoc/>
	protected override void RemoveItem(int index)
	{
		base.RemoveItem(index);

		ConfineItemsToCapacity();
	}
}
