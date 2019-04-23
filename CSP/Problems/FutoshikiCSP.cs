using System;
using System.Diagnostics;
using CSP.Entities;
using CSP.Entities.Futoshiki;
using CSP.Exceptions;
using CSP.Problems.Interfaces;

namespace CSP.Problems
{
    public class FutoshikiCSP : IFutoshiki
    {
        private int _nodesVisitedCount = 0;

        public FutoshikiResult SolveGame(FutoshikiData data)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var hasFoundSolution = ForwardChecking(data);
            stopwatch.Stop();
            return new FutoshikiResult(data.Title, hasFoundSolution ? data.Board : null, _nodesVisitedCount,stopwatch.Elapsed);
        }

        private bool ForwardChecking(FutoshikiData data)
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

        private bool Backtracking(FutoshikiData data)
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