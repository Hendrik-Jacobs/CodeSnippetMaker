using System.Windows.Forms;

namespace CodeSnippetMaker.General;
public static class Picker
{
    public static string FolderPicker()
    {
        FolderBrowserDialog openFileDlg = new FolderBrowserDialog();
        string result = openFileDlg.ShowDialog().ToString();
        if (result.ToLower() == "ok")
        {
            return openFileDlg.SelectedPath;
        }
        return "";
    }

    public static string OpenPicker(string path)
    {
        OpenFileDialog dlg = new();
        if (string.IsNullOrEmpty(path) == false)
        {
            dlg.InitialDirectory = path;
        }
        dlg.Filter = "Code Snippet (.snippet)|*.snippet";

        if (dlg.ShowDialog() == DialogResult.OK)
        {
            return dlg.FileName;
        }
        return "";
    }
}