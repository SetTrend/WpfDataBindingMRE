using System.Windows;

using WpfDataBindingMRE.Code;

namespace WpfDataBindingMRE
{
	public partial class App : Application
	{
		private void Application_Startup(object sender, StartupEventArgs e) => FileBackupService.DeleteAllBackupFiles();
	}
}
