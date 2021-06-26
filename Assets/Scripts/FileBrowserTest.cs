using UnityEngine;
using System.Collections;
using System.IO;
using SimpleFileBrowser;

public class FileBrowserTest : MonoBehaviour
{
	// Warning: paths returned by FileBrowser dialogs do not contain a trailing '\' character
	// Warning: FileBrowser can only show 1 dialog at a time

	private string _destinationPath = string.Empty;
	private const string DEFAULT_LOC_FOLDER_NAME = "Localization Files";

	private readonly string[] DEFAULT_FILES = new string[] { "loc_en", "loc_es", "loc_jp" };
	private void SetDestinationPath(string path)
	{
		_destinationPath = path;
		try { Messenger<string>.Broadcast(Messages.DirectoryChosen, _destinationPath); } catch { }
	}

	/// <summary>
	/// For some reason the persistent data path does not have the correct path separator.
	/// </summary>
	/// <returns></returns>
	private string ReplaceBackslashWithSeparator(string path)
    {
		path = path.Replace('/', Path.DirectorySeparatorChar);
		return path;
	}

    private void Start()
    {
		// Do this in start
		var path = Path.Combine(ReplaceBackslashWithSeparator(Application.persistentDataPath), DEFAULT_LOC_FOLDER_NAME);

		if (!Directory.Exists(path))
			Directory.CreateDirectory(path);

		_CopyDefaultLocFiles(path);

		SetDestinationPath(path);
	}

	/// <summary>
	/// Copy the default loc files to the specified path.
	/// </summary>
	private void _CopyDefaultLocFiles(string path)
	{
		TextAsset ta;

        foreach (var fileName in DEFAULT_FILES)
        {
			if (Csv.CsvHandler.TryLoadTextFileFromResources(fileName, out ta))
			{
				StreamWriter writer = new StreamWriter(Path.Combine(path, fileName + ".csv"), false);
				writer.WriteLine(ta.text);
				writer.Close();
			}
        }
	}

    public void OpenFileBrowser()
	{
		// Set filters (optional)
		// It is sufficient to set the filters just once (instead of each time before showing the file browser dialog), 
		// if all the dialogs will be using the same filters
		FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));

		// Set default filter that is selected when the dialog is shown (optional)
		// Returns true if the default filter is set successfully
		// In this case, set Images filter as the default filter
		FileBrowser.SetDefaultFilter(".jpg");

		// Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
		// Note that when you use this function, .lnk and .tmp extensions will no longer be
		// excluded unless you explicitly add them as parameters to the function
		FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

		// Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
		// It is sufficient to add a quick link just once
		// Name: Users
		// Path: C:\Users
		// Icon: default (folder icon)
		FileBrowser.AddQuickLink("Users", "C:\\Users", null);

		// Show a save file dialog 
		// onSuccess event: not registered (which means this dialog is pretty useless)
		// onCancel event: not registered
		// Save file/folder: file, Allow multiple selection: false
		// Initial path: "C:\", Initial filename: "Screenshot.png"
		// Title: "Save As", Submit button text: "Save"
		// FileBrowser.ShowSaveDialog( null, null, FileBrowser.PickMode.Files, false, "C:\\", "Screenshot.png", "Save As", "Save" );

		// Show a select folder dialog 
		// onSuccess event: print the selected folder's path
		// onCancel event: print "Canceled"
		// Load file/folder: folder, Allow multiple selection: false
		// Initial path: default (Documents), Initial filename: empty
		// Title: "Select Folder", Submit button text: "Select"
		// FileBrowser.ShowLoadDialog( ( paths ) => { Debug.Log( "Selected: " + paths[0] ); },
		//						   () => { Debug.Log( "Canceled" ); },
		//						   FileBrowser.PickMode.Folders, false, null, null, "Select Folder", "Select" );

		// Coroutine example
		StartCoroutine(ShowLoadDialogCoroutine());
	}

	IEnumerator ShowLoadDialogCoroutine()
	{
		// Show a load file dialog and wait for a response from user
		// Load file/folder: both, Allow multiple selection: true
		// Initial path: default (Documents), Initial filename: empty
		// Title: "Load File", Submit button text: "Load"
		yield return FileBrowser.WaitForLoadDialog(
			FileBrowser.PickMode.Folders,
			true,
			_destinationPath,
			null,
			"Load Files and Folders", "Load");

		// Dialog is closed
		// Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
		Debug.Log(FileBrowser.Success);

		if (FileBrowser.Success)
        {
			SetDestinationPath(ReplaceBackslashWithSeparator(FileBrowser.Result[0]));
		}

		//if (FileBrowser.Success)
		//{
		//	// Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
		//	for (int i = 0; i < FileBrowser.Result.Length; i++)
		//		Debug.Log(FileBrowser.Result[i]);

		//	// Read the bytes of the first file via FileBrowserHelpers
		//	// Contrary to File.ReadAllBytes, this function works on Android 10+, as well
		//	byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

		//	// Or, copy the first file to persistentDataPath
		//	string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
		//	FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
		//}
	}
}
