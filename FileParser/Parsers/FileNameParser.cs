using System;
using System.Collections.Generic;
using System.IO;

namespace FileParser.Parsers
{
    public sealed class FileNameParser
    {
        private static readonly object locker = new object();


        /// <summary>
        /// File name parser
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="pattern"></param>
        /// <param name="delimiters"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetFileNameParsingResult(string filePath, string[] pattern, char[] delimiters = null)
        {
            lock (locker)
            {
                return ParseFileName(filePath, pattern, delimiters);
            }
        }



        private IDictionary<string, string> ParseFileName(string filePath, string[] fileNameStruct, char[] delimiters)
        {
            FileInfo csvFile = GetFileInfo(filePath);
            if (!csvFile.Exists) { return null; }
            if (fileNameStruct == null || fileNameStruct.Length < 1) { return null; }

            if (delimiters == null || delimiters.Length < 1) { delimiters = new[] { '_' }; }
            string fileName = csvFile.Name;

            string[] fields = fileName.Split(delimiters);
            if (fields.Length < fileNameStruct.Length) { return null; }

            IDictionary<string, string> fileNameData = new Dictionary<string, string>();
            for (int i = 0; i < fileNameStruct.Length; i++)
            {
                fileNameData.Add(fileNameStruct[i], fields[i]);
            }
            return fileNameData.Count < 1 ? null : fileNameData;
        }




        private FileInfo GetFileInfo(string path)
        {
            FileInfo file = null;
            try
            {
                file = new FileInfo(path);
                if (!file.Exists) { file = null; }
            }
            catch (Exception)
            {
            }
            return file;
        }

    }
}
