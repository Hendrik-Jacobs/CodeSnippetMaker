using CodeSnippetMaker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeSnippetMaker.General
{
    public static class Texts
    {
        public static Dictionary<string, string> SplitColors(ViewModel view)
        {
            StringBuilder white = new();
            StringBuilder violet = new();

            int i = -1;
            int end = -1;

            foreach (char c in view.Code)
            {
                i++;
                if (end == -1)
                {
                    if (c == '$')
                    {
                        int next = view.Code.IndexOf('$', i + 1);
                        if (next == -1)
                        {
                            white.Append(c);
                            violet.Append(' ');
                            continue;
                        }

                        int start = i + 1;
                        string name = view.Code[start..next];

                        int result = view.Literals.Where(x => x.ID == name).ToList().Count;
                        if (result == 1)
                        {
                            end = next;
                        }
                    }
                }

                if (end == -1)
                {
                    if (c == '\n' || c == '\t' || c == '\r')
                    {
                        white.Append(c);
                        violet.Append(c);
                        continue;
                    }
                    white.Append(c);
                    violet.Append(' ');
                }
                else
                {
                    white.Append(' ');
                    violet.Append(c);

                    if (end == i)
                    {
                        end = -1;
                    }
                }
            }

            Dictionary<string, string> texts = new();
            texts.Add("White", white.ToString());
            texts.Add("Violet", violet.ToString());

            return texts;
        }
    }
}