using System.Collections.Concurrent;

namespace NguyenDucManh_SE1884_A01_AIAPI.Services
{
    public interface ITagLearningCache
    {
        void RecordTagSelection(int tagId);
        List<int> GetFrequentTags(int count = 5);
    }

    public class TagLearningCache : ITagLearningCache
    {
        private readonly ConcurrentDictionary<int, int> _tagFrequency = new();

        public void RecordTagSelection(int tagId)
        {
            _tagFrequency.AddOrUpdate(tagId, 1, (_, count) => count + 1);
        }

        public List<int> GetFrequentTags(int count = 5)
        {
            return _tagFrequency
                .OrderByDescending(x => x.Value)
                .Take(count)
                .Select(x => x.Key)
                .ToList();
        }
    }
}
