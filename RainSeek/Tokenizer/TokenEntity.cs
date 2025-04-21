using System;

namespace RainSeek.Tokenizer
{
    public class TokenModel : IEquatable<TokenModel>
    {
        public long Id = -1;

        public string Value;

        public int StartPosition;

        public int EndPosition;

        public bool Equals(TokenModel? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && Value == other.Value &&
                   StartPosition == other.StartPosition && EndPosition == other.EndPosition;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TokenModel)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Value, StartPosition, EndPosition);
        }
    }
}
