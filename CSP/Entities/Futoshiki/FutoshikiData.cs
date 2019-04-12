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

        public void RestoreDomainsOnCross(FutoshikiVariable variable)
        {
            if (!variable.Value.HasValue)
            {
                throw new ArgumentNullException();
            }
            var (row, column) = GetVariablePosition(variable);
            for (int k = row + 1; k < Board.GetLength(0); k++)
            {
                if (!Board[k, column].Value.HasValue)
                {
                    Board[k, column].ResetDomain();
                }
            }
            for (int k = row - 1; k >= 0; k--)
            {
                if (!Board[k, column].Value.HasValue)
                {
                    Board[k, column].ResetDomain();
                }
            }
            for (int k = column + 1; k < Board.GetLength(1); k++)
            {
                if (!Board[row, k].Value.HasValue)
                {
                    Board[row, k].ResetDomain();
                }
            }
            for (int k = column - 1; k >= 0; k--)
            {
                if (!Board[row, k].Value.HasValue)
                {
                    Board[row, k].ResetDomain();
                }
            }
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

        public bool TryRemoveFromDomainsOnCross(FutoshikiVariable variable)
        {
            if (!variable.Value.HasValue)
            {
                throw new ArgumentNullException();
            }
            var (row, column) = GetVariablePosition(variable);
            for (int k = row + 1; k < Board.GetLength(0); k++)
            {
                if (!Board[k,column].Value.HasValue && !Board[k, column].Domain.Remove(variable.Value.Value))
                {
                    return false;
                }
            }
            for (int k = row - 1; k >= 0; k--)
            {
                if (!Board[k, column].Value.HasValue && !Board[k, column].Domain.Remove(variable.Value.Value))
                {
                    return false;
                }
            }
            for (int k = column + 1; k < Board.GetLength(1); k++)
            {
                if (!Board[row, k].Value.HasValue && !Board[row, k].Domain.Remove(variable.Value.Value))
                {
                    return false;
                }
            }
            for (int k = column - 1; k >= 0; k--)
            {
                if (!Board[row, k].Value.HasValue && !Board[row, k].Domain.Remove(variable.Value.Value))
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckRowColumnConstraints(FutoshikiVariable variable)
        {
            var (row, column) = GetVariablePosition(variable);
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

        private (int row, int column) GetVariablePosition(FutoshikiVariable variable)
        {
            int row = 0;
            int column = 0;
            while (!Board[row, column].Equals(variable))
            {
                column++;
                if (column == Board.GetLength(1))
                {
                    row++;
                    column = 0;
                }
            }

            return (row, column);
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