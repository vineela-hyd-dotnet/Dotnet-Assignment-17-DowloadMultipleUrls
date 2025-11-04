using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Dotnet_Assignment_17_DownloadMultipleUrls
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // 1. List of URLs
            var urls = new List<string>
{
  "https://wallpaperaccess.com/full/7061854.jpg",

"https://monovisionsawards.com/upload/images/1607862572jxn7zportrait_of_liza.jpg",

"https://www.cartoonize.net/wp-content/uploads/2022/01/black-and-white-portraits-12-768x1024.jpg",
"https://jsonplaceholder.typicode.com/todos/1",
"https://freetestdata.com/wp-content/uploads/2021/09/Free_Test_Data_100KB_PDF.pdf"


};

           
            string downloadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");
            Directory.CreateDirectory(downloadFolder);

            var httpClient = new HttpClient();
            var stopwatch = Stopwatch.StartNew();

            Console.WriteLine("Starting downloads...\n");

          
            var downloadTasks = new List<Task>();
            foreach (var url in urls)
            {
                downloadTasks.Add(Task.Run(async () =>
                {
                    try
                    {
                       
                        var fileName = Path.GetFileName(new Uri(url).LocalPath);
                        if (string.IsNullOrEmpty(fileName))
                            fileName = Guid.NewGuid().ToString();

                        string filePath = Path.Combine(downloadFolder, fileName);

                       
                        byte[] data = await httpClient.GetByteArrayAsync(url);

                       
                        using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            await fs.WriteAsync(data, 0, data.Length);
                        }

                        Console.WriteLine($"Downloaded: {fileName}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to download {url}: {ex.Message}");
                    }
                }));
            }

          
            await Task.WhenAll(downloadTasks);

            stopwatch.Stop();
            Console.WriteLine($"\nAll downloads finished in {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
