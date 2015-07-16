using System.Threading.Tasks;

namespace Redists.Core
{
    interface IRecordReader
    {
        Task<Record[]> ReadAllAsync(string redisKey);
    }
}
