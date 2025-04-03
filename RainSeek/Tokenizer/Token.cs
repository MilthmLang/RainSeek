using System;

namespace RainSeek.Tokenizer
{
    public class Token : IComparable, IEquatable<Token>
    {
        public long Id = -1;

        public string DocumentId;

        public string Value;

        public int StartPosition;

        public int EndPosition;

        public int CompareTo(object obj)
        {
            if (!(obj is Token that))
            {
                throw new ArgumentException("Object is not a Token");
            }

            if (this.Id.CompareTo(that.Id) == 0)
            {
                return 0;
            }

            if (string.Compare(this.DocumentId, that.DocumentId, StringComparison.InvariantCulture) == 0)
            {
                return 0;
            }

            if (string.Compare(this.Value, that.Value, StringComparison.CurrentCulture) == 0)
            {
                return 0;
            }


            if (this.StartPosition.CompareTo(that.StartPosition) == 0)
            {
                return 0;
            }

            return this.EndPosition.CompareTo(that.EndPosition);
        }

        public bool Equals(Token? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && DocumentId == other.DocumentId && Value == other.Value &&
                   StartPosition == other.StartPosition && EndPosition == other.EndPosition;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Token)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, DocumentId, Value, StartPosition, EndPosition);
        }
    }
}
