using System.Diagnostics;
using CSP.Entities.Futoshiki;
using CSP.Entities.Skyscrapper;
using CSP.Problems.Interfaces;

namespace CSP.Problems
{
    public class SkyscrapperCSP : ISkyscrapper
    {
        private int _nodesVisitedCount = 0;

        public SkyscrapperResult SolveGame(SkyscrapperData data)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var solutionExists = Backtracking(data);
            stopwatch.Stop();
            return new SkyscrapperResult(data.Title, solutionExists ? data.Board : null, _nodesVisitedCount, stopwatch.Elapsed, data.Constraints);
        }

        private bool ForwardChecking(SkyscrapperData data)
        {
            _nodesVisitedCount++;
            if (!data.IsBoardValid())
            {
                return false;
            }
            if (data.IsBoardComplete())
            {
                return true;
            }
            var variable = data.PickUnassignedVariable();
            foreach (var value in variable.Domain)
            {
                variable.Value = value;
                if (data.CheckRowColumnConstraints(variable) && data.TryRemoveFromDomainsOnCross(variable))
                {
                    if (ForwardChecking(data))
                    {
                        return true;
                    }
                }
                else
                {
                    data.RestoreDomainsOnCross(variable);
                }
            }
            variable.Value = null;
            variable.ResetDomain();
            return false;
        }

        private bool Backtracking(SkyscrapperData data)
        {
            _nodesVisitedCount++;
            if (!data.IsBoardValid())
            {
                return false;
            }
            if (data.IsBoardComplete())
            {
                return true;
            }
            var variable = data.PickUnassignedVariable();
            foreach (var value in variable.Domain)
            {
                variable.Value = value;
                if (Backtracking(data))
                {
                    return true;
                }
            }
            variable.Value = null;
            return false;
        }
    }
}