using System;

namespace Redists.Core
{
    internal interface IStringParser<T>
    {
        T[] Parse(string rawString);
        string Serialize(T[] items);
    }
}
