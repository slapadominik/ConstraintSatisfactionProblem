using System.Collections.Generic;
using System.Text;

namespace CSP.Entities
{
    public class FutoshikiData
    {
        public int Size { get; set; }
        public int[,] Board { get; set; }
        public List<int>[,] Domains { get; set; } 
        public IEnumerable<FutoshikiConstraint> Constraints { get; set; }

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