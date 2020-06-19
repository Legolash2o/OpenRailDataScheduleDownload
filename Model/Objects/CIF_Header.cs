using Model.Enums;
using System;
using System.Diagnostics;
using System.IO;

namespace Model.Objects
{
    public class CIF_Header
    {
        #region CONSTRUCTOR
        public CIF_Header(string file)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (StreamReader sr = new StreamReader(file))
            {
                string line = string.Empty;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("HD"))
                    {
                        FileIdentity = line.Substring(2, 20);
                        string dateString = line.Substring(22, 10);

                        FilePath = file;
                        TPSExportDate = DateTime.ParseExact(dateString, "ddMMyyHHmm", null);

                        string _type = line.Substring(46, 1);
                        if (_type == "F")
                            UpdateType = UpdateType.F;
                        else if (_type == "U")
                            UpdateType = UpdateType.U;
                        break;
                    }                  
                }

                sw.Stop();
                Debug.WriteLine($"Loaded {file} in {sw.ElapsedMilliseconds}ms.");

                if (string.IsNullOrWhiteSpace(FilePath))
                    throw new Exception("Not a valid CIF file");
            }
        }

        #endregion

        #region PUBLIC PROPERTIES
        /// <summary>
        ///     The identity of the file.
        /// </summary>
        public string FileIdentity { get; set; }

        /// <summary>
        ///     The date the CIF file was created.
        /// </summary>
        public DateTime TPSExportDate { get; set; }

        /// <summary>
        /// The date is became available on the feeds.
        /// </summary>
        public DateTime UploadedDate { get; set; }

        public string FilePath { get; set; }

        /// <summary>
        ///     The type of update.
        /// </summary>
        public UpdateType UpdateType { get; set; }

        #endregion


    }
}
