using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace PhantomUI
{
    public class PhantomJs
    {
        public enum FileType
        {
            Gif,
            Jpeg,
            Png,
            Pdf
        }

        public Process Process { get; set; }

        public bool IsDone { get; set; }
        public async Task<string> GetPdfTask(string url, FileType returnType)
        {
            return await Task.Run(() => UrlToPdf(url, returnType));
        }

        private string UrlToPdf(string url, FileType returnType)
        {
            var isDone  = false;
            var isError = false;

            Process = new Process
            {
                EnableRaisingEvents = true
            };

            Process.Exited += (sender, e) =>
            {
                isDone = true;
            };

            Process.ErrorDataReceived += (sender, e) =>
            {
                isDone  = true;
                isError = true;
            };

            //To get the location the assembly normally resides on disk or the install directory
            string basePath = Assembly.GetExecutingAssembly().Location;
            string baseDir  = Path.GetDirectoryName(basePath);

            var path = String.Format(@"{0}\lib\phantomjs.exe", baseDir);

            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                throw new FileNotFoundException("Could not find PhantomJS executable.");
            }

            var dir = Path.GetDirectoryName(path);

            if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir))
            {
                throw new FileNotFoundException("Could not determin PhantomJS base directory.");
            }

            var fileName = Path.GetFileName(path);
            var savePath = String.Format(@"{0}{1}.{2}", Path.GetTempPath(), Guid.NewGuid(), returnType.ToString().ToLower());


            // If is a local file, we need to adapt the path a bit
            if (File.Exists(url))
            {
                url = string.Format("file:///{0}", Uri.EscapeDataString(url).Replace("%3A", ":").Replace("%5C", "/"));
            }


            Process.StartInfo.WindowStyle      = ProcessWindowStyle.Hidden;
            Process.StartInfo.CreateNoWindow   = true;

            Process.StartInfo.ErrorDialog = true;


            Process.StartInfo.FileName         = fileName;
            Process.StartInfo.Arguments        = string.Format(@"--config=config.json scripts/run.js {0} {1}", url, savePath);

            Process.StartInfo.WorkingDirectory = dir;
            Process.Start();
            
            while (!isDone) { }


            if (isError)
            {
                throw new Exception(Process.StandardError.ReadToEnd());
            }

            return savePath;
        }
    }
}
