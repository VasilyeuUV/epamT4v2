using BLL.Enums;
using BLL.BLLModels;
using FileParser.Parsers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;

namespace BLL.Threads
{
    internal class CsvWorkThread
    {
        private static readonly object locker = new object();

        private Thread _thread = null;

        private SalesFileNameModel _sfnm = null;
        private List<SaleParsingModel> _parsingFieldsList = null;


        private string[] _fileNameStruct = null;
        private string[] _fileContextStruct = null;


        private bool _abort = false;
        private bool Abort
        {
            get => _abort;
            set
            {
                _abort = value;
                OnWorkCompleting(_abort);
            }
        }

        internal string Name { get; private set; }


        internal event EventHandler<bool> WorkCompleted;
        internal event EventHandler<string> ErrorEvent;




        /// <summary>
        /// CTOR
        /// </summary>
        public CsvWorkThread(string[] fns, string[] fds)
        {
            this._fileContextStruct = fds;
            this._fileNameStruct = fns;

            this._parsingFieldsList = new List<SaleParsingModel>();

            this._thread = new Thread(this.RunProcess);
        }


        /// <summary>
        /// Start this Thread
        /// </summary>
        /// <param name="products"></param>
        internal bool Start(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                this._thread.IsBackground = true;
                this._thread?.Start(filePath);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Main thread method
        /// </summary>
        private void RunProcess(object obj)
        {
            string filePath = (string)obj;

            // PARSE FILE NAME
            if (this.Abort) { return; }
            this._sfnm = GetFileNameData(filePath);
            if (this._sfnm == null) { this.Abort = true; }

            this.Name = this._sfnm.FileName;
            this._thread.Name = this._sfnm.FileName;

            // PARSE FILE CONTENT
            if (this.Abort) { return; }
            IEnumerable<IDictionary<string, string>> fileData = GetFileContentData(filePath);
            if (fileData == null || fileData.Count() < 1)
            {
                //OnErrorEvent(EnumErrors.fileContentError, "filedata == null");
                return;
            }

            // CHECK CORRECT FILE CONTENT PARSING




        }



        private IEnumerable<IDictionary<string, string>> GetFileContentData(string filePath)
        {
            CsvParser csvParser = new CsvParser();

            csvParser.ParsingCompleted += CsvParser_ParsingCompleted;
            csvParser.FieldParsed += CsvParser_FieldParsed;
            csvParser.ErrorParsing += CsvParser_ErrorParsing;

            var parsingResult = csvParser.GetParsingResult(filePath);

            csvParser.ParsingCompleted -= CsvParser_ParsingCompleted;
            csvParser.FieldParsed -= CsvParser_FieldParsed;
            csvParser.ErrorParsing -= CsvParser_ErrorParsing;

            return parsingResult;
        }



        private SalesFileNameModel GetFileNameData(string filePath)
        {
            lock (locker)
            {
                FileNameParser parser = new FileNameParser();
                IDictionary<string, string> fileNameData = parser.GetFileNameParsingResult(filePath, this._fileNameStruct);
                if (fileNameData == null
                    || fileNameData.Count != this._fileNameStruct.Length)
                {
                    //OnErrorEvent(EnumErrors.fileNameError, $"fileNameData == null");
                    return null;
                }
                return SalesFileNameModel.CreateInstance(filePath, fileNameData);
            }
        }





        /// <summary>
        /// Thread work completed event
        /// </summary>
        /// <param name="abort">true - if thread was being aborted</param>
        /// <param name="deleteTmpData">true - if data was saved to database</param>
        private void OnWorkCompleting(bool abort)
        {
            WorkCompleted?.Invoke(this, abort);
        }




        #region CSV_PARSER_EVENTS
        //############################################################################################################


        private void CsvParser_ErrorParsing(object sender, FileParser.Enums.EnumErrors e)
        {



        }

        private void CsvParser_FieldParsed(object sender, IDictionary<string, string> e)
        {
            SaleParsingModel sale = SaleParsingModel.CreateInstance(e);

            if (sale.DTG != this._sfnm.DTG)
            {
                // ERROR!
            }

            this._parsingFieldsList.Add(sale);
        }

        private void CsvParser_ParsingCompleted(object sender, bool e)
        {



        }

        #endregion // CSV_PARSER_EVENTS



    }
}
