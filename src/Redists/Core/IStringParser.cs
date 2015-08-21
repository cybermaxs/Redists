using System;

namespace Redists.Core
{
    interface IStringParser<T>
    {
        T[] Parse(string rawString);
        string Serialize(T[] items);
    }
}
