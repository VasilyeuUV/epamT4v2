using FileParser.Enums;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileParser.Parsers
{
    public sealed class CsvParser : ParserBase
    {

        protected override IEnumerable<IDictionary<string, string>> ParseFileContent(string filePath, string[] delimiters)
        {
            FileInfo csvFile = GetFileInfo(filePath);
            if (csvFile == null)
            {
                OnParsingCompleted();                
                return null;
            }

            if (delimiters == null || delimiters.Length < 1) { delimiters = new[] { "," }; }

            IEnumerable<IDictionary<string, string>> lstFields = ParseCsvFile(csvFile.FullName, delimiters);

            OnParsingCompleted();
            return (lstFields == null || lstFields.Count() < 1) ? null : lstFields;
        }



        private IEnumerable<IDictionary<string, string>> ParseCsvFile(string fullName, string[] delimiters)
        {
            List<IDictionary<string, string>> lstFields = new List<IDictionary<string, string>>();
            int count = 0;
            string[] fieldNames = new string[0];

            using (TextFieldParser tfp = new TextFieldParser(fullName))
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
                            OnFieldParsed(dicField);                            
                        }
                    }
                }                
            }
            if (this._abort) { lstFields = null; }
            return lstFields;
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
    }
}
