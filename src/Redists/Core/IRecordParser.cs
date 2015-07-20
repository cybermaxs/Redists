using System;

namespace Redists.Core
{
    interface IRecordParser
    {
        Record Deserialize(string rawRecord);
        Record[] ParseRawString(string raw);
        string Serialize(Record record);
    }
}
