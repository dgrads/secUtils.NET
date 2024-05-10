// Configuration/sftp.classSettings.cs
namespace SftpIntegrationLibrary.Configuration
{
    public class SftpSettings
    {
        public string RemoteHost { get; set; }
        public string RemoteUser { get; set; }
        public string PrivateKeyPath { get; set; }
        public string LocalPath { get; set; }
        public string RemotePath { get; set; }
    }
}
