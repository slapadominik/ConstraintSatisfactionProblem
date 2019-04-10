using System;
using System.Collections.Generic;
using System.Text;

namespace CSP.Entities.Futoshiki
{
    public class FutoshikiData
    {
        public int Size { get; set; }
        public FutoshikiVariable[,] Board { get; set; }
        public IEnumerable<FutoshikiConstraint> Constraints { get; set; }

        public FutoshikiData(int size, FutoshikiVariable[,] board, IEnumerable<FutoshikiConstraint> constraints)
        {
            Size = size;
            Board = board;
            Constraints = constraints;
        }

        public bool IsBoardComplete()
        {
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    if (Board[i, j].Value == null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool IsBoardValid()
        {
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    if (Board[i, j].Value != null && !CheckRowColumnConstraints(Board[i,j]))
                    {
                        return false;
                    }
                }
            }
            foreach (var constraint in Constraints)
            {
                if (Board[constraint.HigherIndex.row, constraint.HigherIndex.column].Value <
                    Board[constraint.LowerIndex.row, constraint.LowerIndex.column].Value)
                {
                    return false;
                }
            }

            return true;
        }

        public FutoshikiVariable PickUnassignedVariable()
        {
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    if (Board[i, j].Value == null)
                    {
                        return Board[i, j];
                    }
                }
            }

            return null;
        }

        private bool CheckRowColumnConstraints(FutoshikiVariable variable)
        {
            int row = 0;
            int column = 0;
            while (!Board[row,column].Equals(variable))
            {
                column++;
                if (column == Board.GetLength(1))
                {
                    row++;
                    column = 0;
                }
            }

            for (int k=row+1; k < Board.GetLength(0); k++)
            {
                if (variable.Value == Board[k, column].Value)
                {
                    return false;
                }
            } 
            for (int k = row-1; k >= 0; k--)
            {
                if (variable.Value == Board[k, column].Value)
                {
                    return false;
                }
            }
            for (int k = column+1; k < Board.GetLength(1); k++)
            {
                if (variable.Value == Board[row, k].Value)
                {
                    return false;
                }
            }
            for (int k = column-1 ; k >= 0; k--)
            {
                if (variable.Value == Board[row, k].Value)
                {
                    return false;
                }
            }
            return true;
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