using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HotPotato.Http.Default
{
    public class SpecValidationTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            //current dummy data
            yield return new object[] { 1, 2, 3 };
            yield return new object[] { -4, -6, -10 };
            yield return new object[] { -2, 2, 0 };
            yield return new object[] { int.MinValue, -1, int.MaxValue};
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
