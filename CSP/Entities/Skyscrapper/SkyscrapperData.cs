using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using CSP.Entities.Futoshiki;

namespace CSP.Entities.Skyscrapper
{
    public class SkyscrapperData
    {
        public string Title { get; set; }
        public int Size { get; set; }
        public SkyscrapperVariable[,] Board { get; set; }
        public SkyscrapperConstraints Constraints { get; set; }

        public SkyscrapperData(string title, int size, SkyscrapperVariable[,] board, SkyscrapperConstraints constraints)
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

        public void RestoreValueFromDomainOnCross(SkyscrapperVariable variable, int value)
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

        public void RestoreDomainsOnCross(SkyscrapperVariable variable)
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
                    if (Board[i, j].Value != null && !CheckRowColumnConstraints(Board[i, j]))
                    {
                        return false;
                    }
                }
            }

            return CheckBuildingsConstraints();
        }

        public bool CheckBuildingsConstraints()
        {
            for (int i = 0; i < Constraints.TopEdge.Count; i++)
            {
                if (Constraints.TopEdge[i].HasValue && !CheckTopEdgeConstraints(i))
                {
                    return false;
                }
            }

            for (int i = 0; i < Constraints.BottomEdge.Count; i++)
            {
                if (Constraints.BottomEdge[i].HasValue && !CheckBottomEdgeConstraints(i))
                {
                    return false;
                }
            }

            for (int i = 0; i < Constraints.LeftEdge.Count; i++)
            {
                if (Constraints.LeftEdge[i].HasValue && !CheckLeftEdgeConstraints(i))
                {
                    return false;
                }
            }

            for (int i = 0; i < Constraints.RightEdge.Count; i++)
            {
                if (Constraints.RightEdge[i].HasValue && !CheckRightEdgeConstraints(i))
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckTopEdgeConstraints(int column)
        {
            if (CountAssignedVariableForColumn(column) == Board.GetLength(0) &&
                CountVisibleBuildingsForTop(column) != Constraints.TopEdge[column])
            {
                return false;
            }

            return true;
        }

        private bool CheckBottomEdgeConstraints(int column)
        {
            if (CountAssignedVariableForColumn(column) == Board.GetLength(0) 
                && CountVisibleBuildingsForBottom(column) != Constraints.BottomEdge[column])
            {
                return false;
            }

            return true;
        }

        private bool CheckLeftEdgeConstraints(int row)
        {
            if (CountAssignedVariableForRow(row) == Board.GetLength(1)
                && CountVisibleBuildingsForLeft(row) != Constraints.LeftEdge[row])
            {
                return false;
            }

            return true;
        }

        private bool CheckRightEdgeConstraints(int row)
        {
            if (CountAssignedVariableForRow(row) == Board.GetLength(1)
                && CountVisibleBuildingsForRight(row) != Constraints.RightEdge[row])
            {
                return false;
            }

            return true;
        }

        private int CountVisibleBuildingsForLeft(int row)
        {
            int max = 0;
            int visibleBuildings = 0;
            for (int j = 0; j < Board.GetLength(1); j++)
            {
                if (Board[row, j].Value.HasValue)
                {
                    if (Board[row, j].Value > max)
                    {
                        max = Board[row, j].Value.Value;
                        visibleBuildings++;
                    }
                }
            }
            return visibleBuildings;
        }

        private int CountVisibleBuildingsForRight(int row)
        {
            int max = 0;
            int visibleBuildings = 0;
            for (int j = Board.GetLength(1)-1; j >= 0; j--)
            {
                if (Board[row, j].Value.HasValue)
                {
                    if (Board[row, j].Value > max)
                    {
                        max = Board[row, j].Value.Value;
                        visibleBuildings++;
                    }
                }
            }
            return visibleBuildings;
        }

        private int CountVisibleBuildingsForTop(int column)
        {
            int max = 0;
            int visibleBuildings = 0;
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                if (Board[i, column].Value.HasValue)
                {
                    if (Board[i, column].Value > max)
                    {
                        max = Board[i, column].Value.Value;
                        visibleBuildings++;
                    }
                }
            }
            return visibleBuildings;
        }

        private int CountVisibleBuildingsForBottom(int column)
        {
            int max = 0;
            int visibleBuildings = 0;
            for (int i = Board.GetLength(0)-1; i >= 0; i--)
            {
                if (Board[i, column].Value.HasValue)
                {
                    if (Board[i, column].Value > max)
                    {
                        max = Board[i, column].Value.Value;
                        visibleBuildings++;
                    }
                }
            }
            return visibleBuildings;
        }

        private int CountAssignedVariableForRow(int row)
        {
            int assignedVariable = 0;
            for (int j=0; j < Board.GetLength(0); j++)
            {
                if (Board[row,j].Value.HasValue)
                {
                    assignedVariable++;
                }
            }

            return assignedVariable;
        }

        private int CountAssignedVariableForColumn(int column)
        {
            int assignedVariable = 0;
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                if (Board[i, column].Value.HasValue)
                {
                    assignedVariable++;
                }
            }

            return assignedVariable;
        }

        public SkyscrapperVariable PickUnassignedVariable()
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

        public SkyscrapperVariable PickMostRestrictiveVariable()
        {
            for (int i = 0; i < Constraints.TopEdge.Count; i++)
            {
                if (Constraints.TopEdge[i] == 1 && !Board[0, i].Value.HasValue)
                {
                    return Board[0, i];
                }
            }

            for (int i = 0; i < Constraints.LeftEdge.Count; i++)
            {
                if (Constraints.LeftEdge[i] == 1 && !Board[i, 0].Value.HasValue)
                {
                    return Board[i, 0];
                }
            }

            for (int i = 0; i < Constraints.RightEdge.Count; i++)
            {
                if (Constraints.RightEdge[i] == 1 && !Board[i, Size-1].Value.HasValue)
                {
                    return Board[i, Size - 1];
                }
            }

            for (int i = 0; i < Constraints.BottomEdge.Count; i++)
            {
                if (Constraints.BottomEdge[i] == 1 && !Board[Size-1, i].Value.HasValue)
                {
                    return Board[Size - 1, i];
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

        public void RemoveFromDomainsOnCross(SkyscrapperVariable variable)
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

        public bool CheckRowColumnConstraints(SkyscrapperVariable variable)
        {
            var (row, column) = GetVariablePosition(variable);
            for (int k = row + 1; k < Board.GetLength(0); k++)
            {
                if (variable.Value == Board[k, column].Value)
                {
                    return false;
                }
            }
            for (int k = row - 1; k >= 0; k--)
            {
                if (variable.Value == Board[k, column].Value)
                {
                    return false;
                }
            }
            for (int k = column + 1; k < Board.GetLength(1); k++)
            {
                if (variable.Value == Board[row, k].Value)
                {
                    return false;
                }
            }
            for (int k = column - 1; k >= 0; k--)
            {
                if (variable.Value == Board[row, k].Value)
                {
                    return false;
                }
            }
            return true;
        }

        public void SortDomainValues(SkyscrapperVariable variable)
        {
            var position = GetVariablePosition(variable);
            if (position.row == 0 && Constraints.TopEdge[position.column] == 1)
            {
                variable.Domain.Clear();
                variable.Domain.Add(Size);
            }

            if (position.row == Size-1 && Constraints.BottomEdge[position.column] == 1)
            {
                variable.Domain.Clear();
                variable.Domain.Add(Size);
            }

            if (position.column == 0 && Constraints.LeftEdge[position.row] == 1)
            {
                variable.Domain.Clear();
                variable.Domain.Add(Size);
            }

            if (position.column == Size-1 && Constraints.RightEdge[position.row] == 1)
            {
                variable.Domain.Clear();
                variable.Domain.Add(Size);
            }
        }

        private (int row, int column) GetVariablePosition(SkyscrapperVariable variable)
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
                    sb.Append(Board[i, j]);
                    sb.Append(" ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}