using System.Collections.Generic;

namespace ExtensionMethods
{
    public class MultiComparer<TSource> : IComparer<TSource>
    {
        private readonly IEnumerable<IComparer<TSource>> comparers;

        public MultiComparer(IComparer<TSource> first, IComparer<TSource> second)
            : this(new[] { first, second })
        {
        }

        public MultiComparer(IEnumerable<IComparer<TSource>> comparers)
        {
            this.comparers = comparers;
        }

        public int Compare(TSource x, TSource y)
        {
            foreach (var comparer in comparers)
            {
                var comparison = comparer.Compare(x, y);
                if (comparison != 0)
                {
                    return comparison;
                }
            }

            return 0;
        }
    }
}