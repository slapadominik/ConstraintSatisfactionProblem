using System.Diagnostics;
using CSP.Consts;
using CSP.Entities.Futoshiki;
using CSP.Entities.Skyscrapper;
using CSP.Problems.Interfaces;

namespace CSP.Problems
{
    public class SkyscrapperCSP : ISkyscrapper
    {
        private int _nodesVisitedCount = 0;

        public SkyscrapperResult SolveGame(SkyscrapperData data, Algorithm algorithm)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            bool foundSolution;
            if (algorithm == Algorithm.Backtracking)
            {
                foundSolution = Backtracking(data);
            }
            else
            {
                foundSolution = ForwardChecking(data);
            }
            stopwatch.Stop();
            return new SkyscrapperResult(data.Title, foundSolution ? data.Board : null, _nodesVisitedCount, stopwatch.Elapsed, data.Constraints);
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

            var variable = data.PickMostRestrictiveVariable();
            data.SortDomainValues(variable);
            foreach (var value in variable.Domain)
            {
                variable.Value = value;
                if (data.CheckRowColumnConstraints(variable) && data.CheckBuildingsConstraints())
                {
                    data.RemoveFromDomainsOnCross(variable);
                    if (ForwardChecking(data))
                    {
                        return true;
                    }
                    data.RestoreValueFromDomainOnCross(variable, value);
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

            var variable = data.PickMostRestrictiveVariable();
            data.SortDomainValues(variable);
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