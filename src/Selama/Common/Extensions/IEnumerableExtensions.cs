using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selama.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static List<TOut> ToListOfDifferentType<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, TOut> converter)
        {
            List<TOut> result = new List<TOut>();
            foreach (var item in source)
            {
                result.Add(converter(item));
            }
            return result;
        }
    }
}
