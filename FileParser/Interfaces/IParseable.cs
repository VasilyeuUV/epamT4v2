using FileParser.Enums;
using System;
using System.Collections.Generic;

namespace FileParser.Interfaces
{
    interface IParseable
    {

        event EventHandler<IDictionary<string, string>> FieldParsed;
        event EventHandler<bool> ParsingCompleted;
        event EventHandler<EnumErrors> ErrorParsing;

        IEnumerable<IDictionary<string, string>> GetParsingResult(string filePath, string[] delimiters = null);
        void Stop();
    }
}
