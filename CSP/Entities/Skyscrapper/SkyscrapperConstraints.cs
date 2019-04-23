using System.Collections.Generic;

namespace CSP.Entities.Skyscrapper
{
    public class SkyscrapperConstraints
    {
        public IList<int> TopEdge { get; set; }
        public IList<int> BottomEdge { get; set; }
        public IList<int> LeftEdge { get; set; }
        public IList<int> RightEdge { get; set; }
    }
}