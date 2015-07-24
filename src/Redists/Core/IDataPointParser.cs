using System;

namespace Redists.Core
{
    interface IDataPointParser
    {
        DataPoint Deserialize(string rawDataPoint);
        DataPoint[] ParseRawString(string raw);
        string Serialize(params DataPoint[] dataPoints);
    }
}
