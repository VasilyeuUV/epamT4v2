using BLL.BLLModels;
using BLL.DTO;
using BLL.Enums;
using BLL.Services;
using EFCF.Repositories;
using FileParser.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BLL.Threads
{
    public class CsvWorkThread
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

        public string Name { get; private set; }


        public event EventHandler<bool> WorkCompleted;
        public event EventHandler<string> ErrorEvent;




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
        public bool Start(string filePath)
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
            this.Name = filePath;

            // PARSE FILE NAME
            if (this.Abort) { return; }
            this._sfnm = GetFileNameData(filePath);
            if (this._sfnm == null || this._sfnm.ExistInDB) 
            {
                OnErrorEvent(EnumErrors.fileNameError, "Can't get the file name data or this file exist in DB");
                return;
            }

            this.Name = this._sfnm.FileName;
            this._thread.Name = this._sfnm.FileName;

            // PARSE FILE CONTENT
            if (this.Abort) { return; }
            IEnumerable<IDictionary<string, string>> fileData = GetFileContentData(filePath);
            if (fileData == null || fileData.Count() < 1)
            {
                OnErrorEvent(EnumErrors.fileContentError, "Incorrect file data");
                return;
            }

            // CHECK CORRECT PARSING DATA
            if (this.Abort) { return; }
            if (this._parsingFieldsList == null
                || fileData == null
                || this._parsingFieldsList.Count() != fileData.Count())
            {
                OnErrorEvent(EnumErrors.saveToDbError, "Error file content field count");
                return;
            }

            // SAVE SALES TO DB
            if (this.Abort) { return; }
            if (SaveDataToDB(this._sfnm, this._parsingFieldsList)) { OnWorkCompleting(); }
            else { OnErrorEvent(EnumErrors.saveToDbError, "Error saving to database"); }
        }



        /// <summary>
        /// Save to DB
        /// </summary>
        /// <param name="sfnm"></param>
        /// <param name="fileData"></param>
        private bool SaveDataToDB(SalesFileNameModel sfnm, List<SaleParsingModel> parsingData)
        {
            lock(locker)
            {
                SaleService service = new SaleService(new EFUnitOfWork());
                List<SaleDTO> salesDTO = GetSalesDTO(sfnm, parsingData);
                return service.SaveEntities(salesDTO);
            }
        }

        private List<SaleDTO> GetSalesDTO(SalesFileNameModel sfnm, List<SaleParsingModel> parsingData)
        {
            List<SaleDTO> salesDTO = new List<SaleDTO>();
            foreach (var item in parsingData)
            {
                SaleDTO saleDTO = new SaleDTO()
                {
                    DTG = item.DTG,
                    Sum = item.Sum,
                    Manager = sfnm.Manager,
                    Product = item.Product,
                    Client = item.Client,
                    FileName = new FileNameDTO() { Id = 0, Name = sfnm.FileName, DTG = sfnm.DTG}
                };
                salesDTO.Add(saleDTO);
            }
            return salesDTO;
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



        #region ON_THIS_EVENTS
        //############################################################################################################

        /// <summary>
        /// Thread work completed event
        /// </summary>
        /// <param name="abort">true - if thread was being aborted</param>
        /// <param name="deleteTmpData">true - if data was saved to database</param>
        private void OnWorkCompleting(bool abort = false)
        {
            WorkCompleted?.Invoke(this, abort);
        }

        /// <summary>
        /// File Content Error Event
        /// </summary>
        private void OnErrorEvent(EnumErrors error, string erMsg, int n = 0)
        {
            bool deleteTmpData = true;
            switch (error)
            {
                case EnumErrors.fileError:
                    ErrorEvent?.Invoke(this, $"Error File: {erMsg}");
                    break;
                case EnumErrors.fileNameError:
                    ErrorEvent?.Invoke(this, $"Error file name: {erMsg}");
                    break;
                case EnumErrors.fileContentError:
                    ErrorEvent?.Invoke(this, $"Error file content: {erMsg}");
                    break;
                case EnumErrors.managerError:
                    ErrorEvent?.Invoke(this, $"Error getting manager information: {erMsg}");
                    break;
                case EnumErrors.productError:
                    ErrorEvent?.Invoke(this, $"Error getting product information: {erMsg}");
                    break;
                case EnumErrors.dateError:
                    ErrorEvent?.Invoke(this, $"Error DateTime information: {erMsg}");
                    break;
                case EnumErrors.costError:
                    ErrorEvent?.Invoke(this, $"Error cost converting: {erMsg}");
                    break;
                case EnumErrors.fileWasSaved:
                    ErrorEvent?.Invoke(this, $"File was saved earlier: {erMsg}");
                    deleteTmpData = false;
                    break;
                case EnumErrors.saveToDbError:
                    string[] msg =
                    {
                        $"{n}.The amount of data processed is not equal to the amount of recorded data: {erMsg}",
                        $"{n}.Error saving parsed field to DB: {erMsg}",
                        $"{n}.Error saving data from temporary table: {erMsg}",
                        $"{n}.Error deleting temporary data after saving: {erMsg}",
                        $"{n}.Error accessing database while saving temporary data: {erMsg}",
                        $"{n}.Error deleting temporary data from the database: {erMsg}",
                        $"{n}.Error accessing database while deleting temporary data: {erMsg}"
                    };
                    ErrorEvent?.Invoke(this, msg[n]);
                    break;
                default: return;
            }
            OnWorkCompleting(true);
        }

        #endregion // ON_THIS_EVENT




        #region CSV_PARSER_EVENTS
        //############################################################################################################


        private void CsvParser_ErrorParsing(object sender, FileParser.Enums.EnumErrors e)
        {



        }

        private void CsvParser_FieldParsed(object sender, IDictionary<string, string> e)
        {
            SaleParsingModel sale = SaleParsingModel.CreateInstance(e);
            if (sale == null || sale.DTG.Date != this._sfnm.DTG.Date)
            {
                (sender as CsvParser)?.Stop();
                return;
            }
            this._parsingFieldsList.Add(sale);
        }

        private void CsvParser_ParsingCompleted(object sender, bool e)
        {



        }

        #endregion // CSV_PARSER_EVENTS



    }
}
