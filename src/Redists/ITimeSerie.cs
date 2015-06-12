using System;
using System.Threading.Tasks;

namespace Redists
{
    public interface ITimeSerie
    {
        Task AddAsync(long value, DateTime at);
        Task<Record[]> AllAsync(DateTime at);
    }
}
