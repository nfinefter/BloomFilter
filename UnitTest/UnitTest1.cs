using BloomFilter;

namespace UnitTest
{
    public class UnitTest1
    {
        BloomFilter<int> bloomfilter = new BloomFilter<int>(9, 4);

        [Theory]
        [InlineData(5)]
        public void ProbablyContains(int item)
        {
            bloomfilter.Insert(item);

            bool test = bloomfilter.ProbablyContains(item);

            Assert.True(test);
        }

        [Theory]
        [InlineData(5)]
        public void Insert(int item)
        {
            int count = bloomfilter.Count;

            bloomfilter.Insert(item);

            Assert.True(count < bloomfilter.Count);
        }

        

        [Theory]
        [InlineData(5, '*')]
        public void LoadHashFunc(int val, char op)
        {
            Func<int, int> hasher;

            switch(op)
            {
                case '*':
                    hasher = m => m * val;
                    break;
                default:
                    hasher = m => m + val;
                    break;
            }

            int count = bloomfilter.HashFunctions.Count;

            bloomfilter.LoadHashFunc(hasher);

            Assert.True(count < bloomfilter.HashFunctions.Count);
        }
    }
}