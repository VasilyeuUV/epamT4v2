using FileParser.Enums;
using FileParser.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileParser.Parsers
{
    public abstract class ParserBase : IParseable
    {
        protected static readonly object locker = new object();
        protected bool _abort = false;

        public event EventHandler<IDictionary<string, string>> FieldParsed;
        public event EventHandler<bool> ParsingCompleted;
        public event EventHandler<EnumErrors> ErrorParsing;


        public void Stop()
        {
            this._abort = true;
        }


        public IEnumerable<IDictionary<string, string>> GetParsingResult(string filePath, string[] delimiters = null)
        {
            lock (locker)
            {
                return ParseFileContent(filePath, delimiters);
            }
        }

        protected abstract IEnumerable<IDictionary<string, string>> ParseFileContent(string filePath, string[] delimiters);






        #region ON_EVENTS
        //######################################################################################################################

        protected void OnParsingCompleted()
        {
            ParsingCompleted?.Invoke(this, this._abort);
        }


        protected void OnFieldParsed(IDictionary<string, string> dicField)
        {
            FieldParsed?.Invoke(this, dicField);
        }


        protected void OnErrorParsing(EnumErrors error)
        {
            ErrorParsing?.Invoke(this, error);
            Stop();
        }


        #endregion // ON_EVENTS






        #region CHECK_FILE_EXIST
        //######################################################################################################################

        protected FileInfo GetFileInfo(string path)
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




        #endregion // CHECK_FILE_EXIST




    }
}
