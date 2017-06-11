using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchQur_an
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Downloader downloader = new Downloader();
            downloader.Load();
        }
    }
}
