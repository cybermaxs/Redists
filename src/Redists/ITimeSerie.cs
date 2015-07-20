using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redists
{
    public interface ITimeSerie
    {
        Task AddAsync(long value, DateTime at);
        Task AddAsync(KeyValuePair<long, DateTime>[] datas);
        Task<Record[]> AllAsync(DateTime at);
    }
}
