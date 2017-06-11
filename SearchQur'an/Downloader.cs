using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace SearchQur_an
{
    public class Downloader
    {
        public string Url { get; private set; }
        public Dictionary<string, Dictionary<int, List<string>>> Surahs { get; set; }


        public Dictionary<int, string[]> rawHtmls = new Dictionary<int, string[]>();
        public Downloader()
        {
            Surahs = new Dictionary<string, Dictionary<int, List<string>>>();
            Url = "http://www.ewige-religion.info/koran/Koran/$.htm";
            // $ will be replaced by surah number.
        }

        public void Load()
        {
            Console.WriteLine("Bismi 'llahi 'r-rahmani 'r-rahimi");
            Console.WriteLine("LOADING SURAHS!!! CAN TAKE A WHILE");
            _internalLoading();
        }

        public Dictionary<string, Dictionary<int, List<string>>> Search(string word)
        {
            Dictionary<string, Dictionary<int, List<string>>> result = new Dictionary<string, Dictionary<int, List<string>>>();

            foreach (var data in Surahs)
            {
                var author = data.Key;

                foreach (var surahs in data.Value)
                {
                    int number = surahs.Key;
    
                    foreach (var ayah in surahs.Value)
                    {
                        if(ayah.ToUpper().Contains(word.ToUpper()))
                        {
                            if (!result.Keys.Contains(author))
                                result.Add(author, new Dictionary<int, List<string>>());
                            if(!result[author].Keys.Contains(number))
                                result[author].Add(number, new List<string>()); ;

                            result[author][number].Add(ayah);
                            
                        }

                    }

                }


            }

            return result;
        }

        private void _internalLoading()
        {
            Console.WriteLine("Downloading surahs!");
            for (int surah = 1; surah < 57 * 2 + 1; surah++)
            {
                string text = Utilities.GetWebBrowserDocument(Url, Utilities.IntToEnumeration(surah, 3));

                text = text.Replace("<B>", "").Replace("</B>", "").Replace("</TR>", "").Replace("<TR>", "");
                string[] lines = text.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);
                rawHtmls.Add(surah, Utilities.CutArray<string>(lines, 13, lines.Length - 1));

            }

            // READ ALL TRANSLATORS

            var author_lines = rawHtmls[1];
            for (int i = 0; i < 5; i++)
            {
                Surahs.Add(Utilities.ReadUntilEOF(author_lines[i], "<TD>", "</TD>"), new Dictionary<int, List<string>>());
                Console.WriteLine("Reading author...");
            }
            // EXTRACT AYAHS


            foreach (var text in rawHtmls)
            {
                int number = text.Key;
                string[] lines = text.Value;

                int increment = 0;
                bool beginOperation = false;


                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];

                    if (line.Contains("<TD align=center>1</TD>"))
                    {
                        beginOperation = true;
                        i++;
                    }
                    if (beginOperation)
                    {
                        if (increment > Surahs.Count)
                            increment = 0;

                        line = Utilities.ReadUntilEOF(line, "<TD>", "</TD>");
                        if (increment < Surahs.Count && !Surahs[Surahs.Keys.ElementAt(increment)].Keys.Contains(number))
                            Surahs[Surahs.Keys.ElementAt(increment)].Add(number, new List<string>());
                        else if (increment < Surahs.Count) Surahs[Surahs.Keys.ElementAt(increment)][number].Add(line);

                        increment++;
                    }
                }

                Console.WriteLine("Parsing HTML...");

            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Define folder to save!");


            FolderBrowserDialog openFile = new  FolderBrowserDialog();
            openFile.ShowDialog();
            if (openFile.SelectedPath == null)
                return;

            SaveTo(openFile.SelectedPath);
        }

        private void SaveTo(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Folder not found!");
                return;
            }

            foreach (var surah in Surahs)
            {
                Directory.CreateDirectory(folder + @"\" + surah.Key);
                foreach (var item in surah.Value)
                {
                    int number = item.Key;
                    File.WriteAllLines(folder + @"\" + surah.Key + @"\" + number + ".txt", item.Value.ToArray());
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Done.");
            Console.Read();

        }


    }
}
