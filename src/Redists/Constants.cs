using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redists
{
    /// <summary>
    /// Various Constants.
    /// </summary>
    class Constants
    {
        public const char IntraRecordDelimiter = ':';
        public const char InterRecordDelimiter = '#';

        public const char RecordPadChar = '0';

        public const int BufferSize = 10000;

        public const string TsPrefix = "ts";
        public const string FixedPrefix = "fx";
    }
}
