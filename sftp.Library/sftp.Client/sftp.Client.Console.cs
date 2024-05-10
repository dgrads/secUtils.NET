using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SftpIntegrationLibrary;

namespace SftpConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var sftpClient = host.Services.GetRequiredService<SftpClient>();

            // Example: Download a file
            sftpClient.DownloadFile();

            // Example: Upload a file
            // sftpClient.UploadFile();

            // Example: Create a remote directory
            // sftpClient.CreateRemoteDirectory("/remote/newdirectory");
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<SftpClient>();
                });
    }
}
