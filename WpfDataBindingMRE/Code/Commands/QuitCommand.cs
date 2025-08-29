using System.Windows.Input;

namespace WpfDataBindingMRE.Code.Commands;

public class QuitCommand : _CommandBase
{
	public QuitCommand() => InputGestures.Add(LocalizeKeyGesture(new KeyGesture(Key.F4, ModifierKeys.Alt)));
}
