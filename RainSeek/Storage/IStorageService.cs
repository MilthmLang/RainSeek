using System;

namespace RainSeek.Storage
{
    public interface IIndexRepository
    {
        IndexEntry? FindOrNull(string name, string token);

        public void Add(string name, IndexEntry indexEntry);

        public void Update(string name, IndexEntry existingEntry);
    }
}
