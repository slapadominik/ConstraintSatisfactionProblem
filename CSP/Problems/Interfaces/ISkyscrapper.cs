using CSP.Entities.Skyscrapper;

namespace CSP.Problems.Interfaces
{
    public interface ISkyscrapper
    {
        SkyscrapperResult SolveGame(SkyscrapperData data);
    }
}