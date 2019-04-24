using CSP.Consts;
using CSP.Entities;
using CSP.Entities.Futoshiki;

namespace CSP.Problems.Interfaces
{
    public interface IFutoshiki
    {
        FutoshikiResult SolveGame(FutoshikiData data, Algorithm algorithm);
    }
}