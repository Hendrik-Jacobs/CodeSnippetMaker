using CodeSnippetMaker.Models;
using EControls;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace CodeSnippetMaker.General
{
    public static class Export
    {
        public static void Run(ViewModel view)
        {
            if (SomeFieldIsEmpty(view)) { return; }
            if (CheckExportFolder(view)) { return; }
            if (CheckExportPath(view)) { return; }
            if (ShortCutExists(view)) { return; }
            WriteSnippet(view);
        }

        private static bool SomeFieldIsEmpty(ViewModel view)
        {
            foreach (PropertyInfo? prop in typeof(ViewModel).GetProperties())
            {
                if (string.IsNullOrEmpty(prop.GetValue(view).ToString()))
                {
                    if (prop.Name == "Description")
                    {
                        EMessageBox em = new($"Field {prop.Name} is empty.\nLeave it empty?",
                                              MessageBoxButton.OKCancel);
                        em.ShowDialog();
                        if (em.Result == MessageBoxResult.Cancel)
                        {
                            return true;
                        }
                        continue;
                    }

                    EMessageBox eb = new($"Error: Field {prop.Name} is empty.");
                    eb.ShowDialog();
                    return true;
                }
            }
            return false;
        }

        private static bool ShortCutExists(ViewModel view)
        {
            if (IsDefaultShortcut(view)) { return true; }
            if (IsCustomShortcut(view)) { return true; }
            return false;
        }

        private static bool IsDefaultShortcut(ViewModel view)
        {
            if (Helper.DefaultShortcuts.Contains(view.ShortCut))
            {
                EMessageBox eb = new($"Error: Shortcut '{view.ShortCut}' is a default shortcut.");
                eb.ShowDialog();
                return true;
            }
            return false;
        }

        private static bool IsCustomShortcut(ViewModel view)
        {
            string[] files = Directory.GetFiles(view.ExportFolder);
            foreach (string file in files)
            {
                string text = File.ReadAllText(file);
                int end = text.IndexOf("</Shortcut>");
                int start = text[..end].LastIndexOf(">") + 1;
                string shortcut = text[start..end];

                if (shortcut == view.ShortCut)
                {
                    EMessageBox eb = new($"Error: Shortcut '{view.ShortCut}' already exists as custom shortcut.");
                    eb.ShowDialog();
                    return true;
                }
            }

            return false;
        }

        private static bool CheckExportFolder(ViewModel view)
        {
            return !Directory.Exists(view.ExportFolder);
        }

        private static bool CheckExportPath(ViewModel view)
        {
            return Directory.Exists($@"{view.ExportFolder}\{view.FileName}.snippet");
        }

        private static void WriteSnippet(ViewModel view)
        {
            string literals = GetLiterals(view);

            string text = "<?xml version=\"1.0\" encoding=\"utf - 8\"?>\n" +
                           "<CodeSnippets xmlns=\"http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet\">\n" +
                           "    <CodeSnippet Format=\"1.0.0\">\n" +
                           "        <Header>\n" +
                          $"            <Title>{view.Title}</Title>\n" +
                          $"            <Author>{view.Author}</Author>\n" +
                          $"            <Description>{view.Description}</Description>\n" +
                          $"            <Shortcut>{view.ShortCut}</Shortcut>\n" +
                           "        </Header>\n" +
                           "        <Snippet>\n" +
                           "            <Code Language=\"CSharp\">\n" +
                          $"                <![CDATA[{view.Code}]]>\n" +
                           "            </Code>\n" +
                          $"{literals}" +
                           "        </Snippet>\n" +
                           "    </CodeSnippet>\n" +
                           "</CodeSnippets>";

            File.WriteAllText($@"{view.ExportFolder}\{view.FileName}.snippet", text);
        }

        private static string GetLiterals(ViewModel view)
        {
            if (view.Literals == null) { return ""; }
            if (view.Literals.Count == 0) { return ""; }

            string literals = "            <Declarations>\n";
            foreach (LiteralModel literal in view.Literals)
            {
                if (string.IsNullOrEmpty(literal.ID) ||
                    string.IsNullOrEmpty(literal.Default))
                {
                    continue;
                }

                literals += "                <Literal>\n" +
                           $"                    <ID>{literal.ID}</ID>\n" +
                           $"                    <Default>{literal.Default}</Default>\n" +
                            "                </Literal>\n";
            }

            if (literals.IndexOf('\n') == literals.Length - 1) { return ""; }

            literals += "            </Declarations>\n";
            return literals;
        }
    }
}