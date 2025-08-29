using System.Windows.Input;

namespace WpfDataBindingMRE.Code.Commands;

public class ClearCommand : _CommandBase
{
	public ClearCommand() => InputGestures.Add(LocalizeKeyGesture(new KeyGesture(Key.Delete, ModifierKeys.Control)));
}
