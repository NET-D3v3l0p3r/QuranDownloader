using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchQur_an
{
    public static class Utilities
    {
        public static string IntToEnumeration(int number, int amountZeroes)
        {
            string result = "";

            for (int i = 0; i < amountZeroes; i++)
            {
                result += "0";
            }

            return result.Substring(0, result.Length - (number + "").Length) + (number + "");
        }

        public static string GetWebBrowserDocument(string url, string number)
        {
            return StartSTATask(new Func<string>(() =>
            {

                WebBrowser webBrowser = new WebBrowser();
                webBrowser.Navigate(url.Replace("$", number));


                while (webBrowser.ReadyState != WebBrowserReadyState.Complete) { Application.DoEvents(); }

                return webBrowser.Document.Body.InnerHtml;


            })).Result;
        }
        public static Task<string> StartSTATask(Func<string> func)
        {
            var tcs = new TaskCompletionSource<string>();
            var thread = new Thread(() =>
            {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }

        public static T[] CutArray<T>(T[] array, int from, int to)
        {
            int length = to - from;
            T[] tmp = new T[length];
            int increment = 0;
            for (int i = from; i < to; i++)
            {
                tmp[increment++] = array[i];
            }
            return tmp;
        }

        public static string ReadUntilEOF(string line, string begin, string end)
        {
            string result = "";
            line = line.Replace(begin, "*").Replace(end, "*");
            char[] chars = line.ToCharArray();
            bool beginWrite = false;
            for (int i = 0; i < chars.Length - 1; i++)
            {
                char c = chars[i];
                if ((c == '*'))
                {
                    beginWrite = true;
                    i++;
                }

                result += beginWrite ? chars[i] + "" : "";

            }

            return result;
        }
    }
}
