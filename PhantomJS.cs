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

        public async Task<string> GetPdfTask(string url, FileType returnType)
        {
            return await Task.Run(() => UrlToPdf(url, returnType));
        }

        private string UrlToPdf(string url, FileType returnType)
        {
            //To get the location the assembly normally resides on disk or the install directory
            string basePath = Assembly.GetExecutingAssembly().Location;
            string baseDir  = Path.GetDirectoryName(basePath);

            var path = string.Format(@"{0}\lib\phantomjs-custom.exe", baseDir);

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
            var savePath = string.Format(@"{0}{1}.{2}", Path.GetTempPath(), Guid.NewGuid(), returnType.ToString().ToLower());

            // If is a local file, we need to adapt the path a bit
            if (File.Exists(url))
            {
                url = string.Format("file:///{0}", Uri.EscapeDataString(url).Replace("%3A", ":").Replace("%5C", "/"));
            }

            var arguments = string.Format(@"--config=config.json scripts/run.js {0} {1}", url, savePath);

            using (var process = new Process())
            {
                process.EnableRaisingEvents = true;

                process.ErrorDataReceived += (sender, e) =>
                {
                    throw new Exception("process.ErrorDataReceived");
                };

                process.StartInfo.WindowStyle      = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow   = true;
                process.StartInfo.ErrorDialog      = true;

                process.StartInfo.FileName         = fileName;
                process.StartInfo.Arguments        = arguments;
                process.StartInfo.WorkingDirectory = dir;
                process.Start();

                process.WaitForExit(10000);
            }

            return savePath;
        }
    }
}