using System.Collections.Generic;
using System.Text;

namespace CSP.Entities
{
    public class FutoshikiData
    {
        public int Size { get; set; }
        public int[,] Board { get; set; }
        public Dictionary<(int, int), List<int>>Domains { get; set; } 
        public IEnumerable<FutoshikiConstraint> Constraints { get; set; }

        public FutoshikiData(int size, int[,] board, Dictionary<(int, int), List<int>> domains, IEnumerable<FutoshikiConstraint> constraints)
        {
            Size = size;
            Board = board;
            Domains = domains;
            Constraints = constraints;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Board \n");
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    sb.Append(Board[i,j]);
                    sb.Append(" ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}