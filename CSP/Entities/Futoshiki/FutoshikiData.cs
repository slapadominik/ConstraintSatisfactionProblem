﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace CSP.Entities.Futoshiki
{
    public class FutoshikiData
    {
        public string Title { get; set; }
        public int Size { get; set; }
        public FutoshikiVariable[,] Board { get; set; }
        public IEnumerable<FutoshikiConstraint> Constraints { get; set; }

        public FutoshikiData(string title, int size, FutoshikiVariable[,] board, IEnumerable<FutoshikiConstraint> constraints)
        {
            Title = title;
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

        public void RestoreValueFromDomainOnCross(FutoshikiVariable variable, int value)
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
                    Board[k, column].Domain.Add(value);
                }
            }
            for (int k = row - 1; k >= 0; k--)
            {
                if (!Board[k, column].Value.HasValue)
                {
                    Board[k, column].Domain.Add(value);
                }
            }
            for (int k = column + 1; k < Board.GetLength(1); k++)
            {
                if (!Board[row, k].Value.HasValue)
                {
                    Board[row, k].Domain.Add(value);
                }
            }
            for (int k = column - 1; k >= 0; k--)
            {
                if (!Board[row, k].Value.HasValue)
                {
                    Board[row, k].Domain.Add(value);
                }
            }
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

        public void RemoveRepeatingDomainValuesFromBoard()
        {
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    if (Board[i, j].Value.HasValue)
                    {
                        RemoveFromDomainsOnCross(Board[i,j]);
                    }
                }
            }
        }

        public FutoshikiVariable PickMostRestrictiveVariableFw()
        {
            int minDomainLength = Size;
            int row = 0;
            int column = 0;
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    if (!Board[i, j].Value.HasValue)
                    {
                        if (Board[i, j].Domain.Count < minDomainLength && Board[i, j].Domain.Any())
                        {
                            minDomainLength = Board[i, j].Domain.Count;
                            row = i;
                            column = j;
                        }
                    }
                }
            }
            return Board[row, column];
        }

        public FutoshikiVariable PickMostRestrictiveVariableBt()
        {
            foreach (var constraint in Constraints)
            {
                if (!Board[constraint.LowerIndex.row, constraint.LowerIndex.column].Value.HasValue)
                {
                    return Board[constraint.LowerIndex.row, constraint.LowerIndex.column];
                }
            }
            int maxVariablesAssignedOnCross = 0;
            int variablesAssigned = 0;
            int row = 0;
            int column = 0;
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    if (!Board[i, j].Value.HasValue)
                    {
                        variablesAssigned = CountAssignedVariablesInColumn(j) + CountAssignedVariablesInRow(i);
                        if (variablesAssigned > maxVariablesAssignedOnCross)
                        {
                            maxVariablesAssignedOnCross = variablesAssigned;
                            row = i;
                            column = j;
                        }
                    }
                }
            }
            return Board[row, column];
        }

        private int CountAssignedVariablesInColumn(int column)
        {
            int count = 0;
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                if (Board[i, column].Value.HasValue)
                {
                    count++;
                }
            }

            return count;
        }

        private int CountAssignedVariablesInRow(int row)
        {
            int count = 0;
            for (int j = 0; j < Board.GetLength(0); j++)
            {
                if (Board[row, j].Value.HasValue)
                {
                    count++;
                }
            }

            return count;
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

        public void RemoveFromDomainsOnCross(FutoshikiVariable variable)
        {
            if (!variable.Value.HasValue)
            {
                throw new ArgumentNullException();
            }
            var (row, column) = GetVariablePosition(variable);
            for (int k = row + 1; k < Board.GetLength(0); k++)
            {
                if (!Board[k,column].Value.HasValue)
                {
                    Board[k, column].Domain.Remove(variable.Value.Value);
                }
            }
            for (int k = row - 1; k >= 0; k--)
            {
                if (!Board[k, column].Value.HasValue)
                {
                    Board[k, column].Domain.Remove(variable.Value.Value);
                }
            }
            for (int k = column + 1; k < Board.GetLength(1); k++)
            {
                if (!Board[row, k].Value.HasValue)
                {
                    Board[row, k].Domain.Remove(variable.Value.Value);
                }
            }
            for (int k = column - 1; k >= 0; k--)
            {
                if (!Board[row, k].Value.HasValue)
                {
                    Board[row, k].Domain.Remove(variable.Value.Value);
                }
            }
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