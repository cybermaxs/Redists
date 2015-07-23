using System;
using System.Threading.Tasks;

namespace Redists.Core
{
    interface IRecordWriter
    {
        Task<long> AppendAsync(string redisKey, params Record[] record);
    }
}
