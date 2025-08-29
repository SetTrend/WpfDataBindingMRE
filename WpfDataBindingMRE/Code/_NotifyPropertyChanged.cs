using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfDataBindingMRE.Code;

/// <inheritdoc/>
public abstract class _NotifyPropertyChanged : INotifyPropertyChanged
{
	/// <inheritdoc/>
	public event PropertyChangedEventHandler? PropertyChanged;



	/// <summary>
	///		Causes the <see cref="PropertyChanged"/> event
	///		to be fired for the specified property.
	/// </summary>
	/// <param name="name">
	///		Name of the property whose valus has changed;
	///		or <see langword="null"/> if all properties of
	///		the object are to be considered changed.
	/// </param>
	protected void OnPropertyChanged([CallerMemberName] string? name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
