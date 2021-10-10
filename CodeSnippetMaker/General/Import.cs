using CodeSnippetMaker.Models;
using System.IO;

namespace CodeSnippetMaker.General
{
    public static class Import
    {
        public static ViewModel Run(ViewModel model)
        {
            ViewModel view = new ViewModel(model);
            string path = Picker.OpenPicker(model.ExportFolder);
            if (string.IsNullOrEmpty(path)) { return null; }
            string text = File.ReadAllText(path);

            view.Title = GetValue(text, "<Title>", "</Title>");
            view.Author = GetValue(text, "<Author>", "</Author>");
            view.Description = GetValue(text, "<Description>", "</Description>");
            view.ShortCut = GetValue(text, "<Shortcut>", "</Shortcut>");
            view.Code = GetCode(text);
            view.Language = GetLanguage(text);
            view.FileName = GetFileName(path);

            int index = text.IndexOf("<Literal>");
            if (index == -1) { return view; }

            text = text[index..];
            while (true)
            {
                string id = GetValue(text, "<ID>", "</ID>");
                string def = GetValue(text, "<Default>", "</Default>");

                if (string.IsNullOrEmpty(id) == false)
                {
                    view.Literals.Add(new LiteralModel(id, def));
                }

                text = text[1..];
                text = text[text.IndexOf("</Default>")..];
                if (text.IndexOf("<Default>") == -1)
                {
                    break;
                }
            }

            return view;
        }

        private static string GetValue(string text, string startTag, string endTag)
        {
            int index = text.IndexOf(startTag);
            if (index == -1) { return ""; }

            text = text[index..].Replace(startTag, "");
            index = text.IndexOf(endTag);
            text = text[..index];

            return text;
        }

        private static string GetLanguage(string text)
        {
            int index = text.IndexOf("<Code Language=");
            if (index == -1) { return ""; }

            text = text[index..];

            index = text.IndexOf('"');
            if (index == -1) { return ""; }
            index++;

            text = text[index..];

            index = text.IndexOf('"');
            if (index == -1) { return ""; }

            text = text[..index];

            return text;
        }

        private static string GetCode(string text)
        {
            int start = text.IndexOf("<![CDATA[");
            if (start == -1) { return ""; }
            start += 9;

            int end = text.IndexOf("</Code>");
            if (end == -1) { return ""; }


            text = text[start..end].Trim();
            text = text[..^3];

            return text;
        }

        private static string GetFileName(string text)
        {
            string[] parts = text.Split('\\');
            return parts[^1].Replace(".snippet", "");
        }
    }
}