using System.Collections.Generic;
using System.Linq;
using CSP.Entities.Futoshiki;

namespace CSP.Entities.Skyscrapper
{
    public class SkyscrapperVariable
    {
        public int Id { get; }
        public int? Value { get; set; }
        public IList<int> Domain { get; private set; }
        private readonly IList<int> _domainCopy;
        private static int _instancesCount;

        public SkyscrapperVariable(int? value, IList<int> domain)
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

        protected bool Equals(SkyscrapperVariable other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SkyscrapperVariable)obj);
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