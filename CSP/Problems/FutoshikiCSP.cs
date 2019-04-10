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
            var hasFoundSolution = Backtracking(data);
            stopwatch.Stop();
            return new FutoshikiResult(hasFoundSolution ? data.Board : null, _nodesVisitedCount,stopwatch.Elapsed);
        }

        private FutoshikiVariable[,] ForwardChecking(FutoshikiData data)
        {
            throw new NotImplementedException();
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