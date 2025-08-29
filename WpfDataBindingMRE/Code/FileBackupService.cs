using System;
using System.IO;

namespace WpfDataBindingMRE.Code;

internal static class FileBackupService
{
	private static readonly string _tempFilePath = Path.Combine(Path.GetTempPath(), "GAQL REPL Tool Query Backups");



	/// <summary>
	///		Deletes all backup files that exist in the
	///		"GAQL REPL Tool Query Backups" directory.
	/// </summary>
	/// <remarks>
	///		This method intentionally will never fail.
	///		Any error that may occur will be silently
	///		ignored.
	/// </remarks>
	internal static void DeleteAllBackupFiles()
	{
		try
		{
			DirectoryInfo tempDir = new DirectoryInfo(_tempFilePath);

			if (tempDir.Exists && (tempDir.Attributes & FileAttributes.Directory) == FileAttributes.Directory) Directory.Delete(_tempFilePath, true);
		}
		catch { }
	}

	/// <summary>
	///		Copies the specified file to the user's
	///		temporary folder.
	/// </summary>
	/// <param name="srcFilePath">
	///		Path to the original file to be copied.
	///	</param>
	/// <remarks>
	///		This method intentionally will never fail.
	///		Any error that may occur will be silently
	///		ignored.
	/// </remarks>
	internal static void SaveBackupFile(string srcFilePath)
	{
		try
		{
			FileInfo file = new FileInfo(srcFilePath);

			if (file.Exists && (file.Attributes & FileAttributes.Directory) == 0)
			{
				Directory.CreateDirectory(_tempFilePath);

				File.Copy(srcFilePath
								, Path.Combine(_tempFilePath
															, $"{Path.GetFileNameWithoutExtension(srcFilePath)} ({DateTime.Now:yyyy-MM-dd HH-mm-ss}){Path.GetExtension(srcFilePath)}"
															)
								, true);
			}
		}
		catch { }
	}
}
