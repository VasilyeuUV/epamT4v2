using BLL.Enums;
using BLL.Tools;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BLL.Parsers
{
    internal class CSVParser
    {
        private bool _abort = false;

        public event EventHandler<IDictionary<string, string>> FieldParsed;
        public event EventHandler<bool> ParsingCompleted;
        public event EventHandler<EnumErrors> ErrorParsing;


        /// <summary>
        /// Stop Parser Job
        /// </summary>
        public void Stop()
        {
            this._abort = true;
        }


        /// <summary>
        /// Parse file name by delimiters
        /// </summary>
        /// <param name="filePath">file full path</param>
        /// <param name="pattern">file name parsing template</param>
        /// <param name="delimiters"></param>
        /// <returns></returns>
        public string[] ParseFileName(string filePath, string pattern, char[] delimiters = null)
        {
            if (FileManager.CheckFileNameMatch(filePath, pattern)) { OnErrorParsing(EnumErrors.fileNameError); }

            string fileName = FileManager.GetFileName(filePath);
            if (delimiters == null || delimiters.Length < 1) { delimiters = new[] { '_' }; }

            return fileName.Split(delimiters);
        }


        /// <summary>
        /// Parsing Process
        /// </summary>
        /// <param name="filePath">file full path</param>
        /// <param name="delimiters"></param>
        /// <returns></returns>
        public IEnumerable<IDictionary<string, string>> ParseFileContent(string filePath, string[] delimiters = null)
        {
            FileInfo fileInfo = FileManager.GetFileInfo(filePath);
            if (fileInfo == null) 
            {
                ParsingCompleted?.Invoke(this, this._abort);
                return null;
            }
                       
            if (delimiters == null || delimiters.Length < 1) { delimiters = new[] { "," }; }            

            int count = 0;
            string[] fieldNames = new string[0];
            List<IDictionary<string, string>> lstFields = new List<IDictionary<string, string>>();
            using (TextFieldParser tfp = new TextFieldParser(fileInfo.FullName))
            {
                tfp.TextFieldType = FieldType.Delimited;
                tfp.SetDelimiters(delimiters);

                while (!tfp.EndOfData && !this._abort)
                {
                    string[] fields = tfp.ReadFields();

                    if (++count == 1) { fieldNames = fields; }
                    else
                    {
                        IDictionary<string, string> dicField = ParseFields(fieldNames, fields);
                        if (dicField == null)
                        {
                            OnErrorParsing(EnumErrors.fileContentError);
                        }
                        else
                        {
                            lstFields.Add(dicField);
                            FieldParsed?.Invoke(this, dicField);
                        }
                    }
                }
                if (this._abort) { lstFields = null; }
            }

            ParsingCompleted?.Invoke(this, this._abort);
            return (lstFields == null || lstFields.Count() < 1) ? null : lstFields;
        }

        /// <summary>
        /// Parse fields
        /// </summary>
        /// <param name="fieldNames">Columns names</param>
        /// <param name="fields">Columns values</param>
        /// <returns></returns>
        private IDictionary<string, string> ParseFields(string[] fieldNames, string[] fields)
        {
            if (fieldNames == null || fieldNames.Length < 1
                || fields == null || fields.Length < 1
                || fieldNames.Length != fields.Length)
            {
                return null;
            }

            IDictionary<string, string> dicField = new Dictionary<string, string>();
            for (int i = 0; i < fields.Length; i++)
            {
                dicField.Add(fieldNames[i], fields[i]);
            }
            return dicField;
        }


        private void OnErrorParsing(EnumErrors error)
        {
            ErrorParsing?.Invoke(this, error);
            Stop();
        }

    }
}
