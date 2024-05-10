using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using SftpIntegrationLibrary.Configuration;

namespace SftpIntegrationLibrary
{
    public class SftpClient
    {
        private readonly SftpSettings _settings;

        public SftpClient(IConfiguration configuration)
        {
            _settings = configuration.GetSection("SftpSettings").Get<SftpSettings>();
        }

        public void DownloadFile()
        {
            ExecuteSftpCommand("get", _settings.RemotePath, _settings.LocalPath);
        }

        public void UploadFile()
        {
            ExecuteSftpCommand("put", _settings.LocalPath, _settings.RemotePath);
        }

        public void CreateRemoteDirectory(string remoteDirectory)
        {
            ExecuteSftpCommand("mkdir", "", remoteDirectory);
        }

        private void ExecuteSftpCommand(string command, string sourcePath, string targetPath)
        {
            string sftpCommand;

            switch (command.ToLower())
            {
                case "get":
                    sftpCommand = $"sftp -i \"{_settings.PrivateKeyPath}\" {_settings.RemoteUser}@{_settings.RemoteHost}:{sourcePath} {targetPath}";
                    break;
                case "put":
                    sftpCommand = $"sftp -i \"{_settings.PrivateKeyPath}\" {_settings.RemoteUser}@{_settings.RemoteHost}:{targetPath} {sourcePath}";
                    break;
                case "mkdir":
                    sftpCommand = $"sftp -i \"{_settings.PrivateKeyPath}\" {_settings.RemoteUser}@{_settings.RemoteHost} -b - <<EOF\nmkdir {targetPath}\nEOF";
                    break;
                default:
                    throw new ArgumentException("Unsupported command");
            }

            ExecuteCommand(sftpCommand);
        }

        private void ExecuteCommand(string command)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = $"/C {command}";
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                LogEvent(output, EventLogEntryType.Information);
                Console.WriteLine(output);
            }
            catch (Exception ex)
            {
                LogEvent($"Error executing command: {ex.Message}", EventLogEntryType.Error);
                throw;
            }
        }

        private void LogEvent(string message, EventLogEntryType type)
        {
            if (!EventLog.SourceExists("SftpClient"))
            {
                EventLog.CreateEventSource("SftpClient", "Application");
            }

            EventLog.WriteEntry("SftpClient", message, type);
        }
    }
}
