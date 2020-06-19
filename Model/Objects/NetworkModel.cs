using System;

namespace Model.Objects
{
    public class NetworkModel
    {
        public long Id { get; set; }

        public DateTime SchemaDate { get; set; }

        public DateTime DownloadedDate { get; set; }

        public DateTime UploadedDate { get; set; }

        public string Version { get; set; }

        public string AzureFilename { get; set; }
    }
}
