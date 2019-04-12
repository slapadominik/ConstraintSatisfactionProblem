using System.Collections.Generic;
using System.Linq;

namespace CSP.Entities.Futoshiki
{
    public class FutoshikiVariable
    {
        public int Id { get; }
        public int? Value { get; set; }
        public IList<int> Domain { get; private set; }
        private readonly IList<int> _domainCopy;
        private static int _instancesCount;

        public FutoshikiVariable(int? value, IList<int> domain)
        {
            Id = _instancesCount++;
            Value = value;
            Domain = domain;
            _domainCopy = new List<int>(domain);
        }

        public void ResetDomain()
        {
            Domain = _domainCopy.Union(Domain).ToList();
        }

        protected bool Equals(FutoshikiVariable other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FutoshikiVariable) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return Value == null ? "0" : Value.ToString();
        }
    }
}