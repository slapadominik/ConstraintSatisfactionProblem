using System;
using System.Diagnostics;
using CSP.Consts;
using CSP.Entities;
using CSP.Entities.Futoshiki;
using CSP.Exceptions;
using CSP.Problems.Interfaces;

namespace CSP.Problems
{
    public class FutoshikiCSP : IFutoshiki
    {
        private int _nodesVisitedCount = 0;

        public FutoshikiResult SolveGame(FutoshikiData data, Algorithm algorithm)
        {
            Stopwatch stopwatch = new Stopwatch();
            bool foundSolution;
            stopwatch.Start();
            if (algorithm == Algorithm.Backtracking)
            {
                data.RemoveRepeatingDomainValuesFromBoard();
                foundSolution = Backtracking(data);
            }
            else
            {
                data.RemoveRepeatingDomainValuesFromBoard();
                foundSolution = ForwardChecking(data);
            }
            stopwatch.Stop();

            return new FutoshikiResult(data.Title, foundSolution ? data.Board : null, _nodesVisitedCount,stopwatch.Elapsed);
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

            var variable = data.PickMostRestrictiveVariableBt();
            foreach (var value in variable.Domain)
            {
                variable.Value = value;
                if (data.CheckRowColumnConstraints(variable))
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
            var variable = data.PickMostRestrictiveVariableBt();
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