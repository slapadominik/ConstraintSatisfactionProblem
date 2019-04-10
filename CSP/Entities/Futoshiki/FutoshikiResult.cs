using System;
using System.Text;

namespace CSP.Entities.Futoshiki
{
    public class FutoshikiResult
    {
        public FutoshikiVariable[,] Board { get; }
        public int NodesVisitedCount { get; }
        public TimeSpan ElapsedTime { get; }

        public FutoshikiResult(FutoshikiVariable[,] board, int nodesVisitedCount, TimeSpan elapsedTime)
        {
            Board = board;
            NodesVisitedCount = nodesVisitedCount;
            ElapsedTime = elapsedTime;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Board \n");
            if (Board != null)
            {
                for (int i = 0; i < Board.GetLength(0); i++)
                {
                    for (int j = 0; j < Board.GetLength(1); j++)
                    {
                        sb.Append(Board[i, j]);
                        sb.Append(" ");
                    }
                    sb.AppendLine();
                }
            }
            sb.AppendLine($"Nodes visited: {NodesVisitedCount}");
            sb.AppendLine($"Elapsed time [milliseconds]:{ElapsedTime.Milliseconds}");
            return sb.ToString();
        }
    }
}