using System.Collections.Generic;
using System.Linq;
using CSP.Consts;
using CSP.Entities.Futoshiki;
using CSP.Entities.Skyscrapper;
using CSP.Helpers.Interfaces;

namespace CSP
{
    public class SkyscrapperDataLoader : IDataLoader<SkyscrapperData>
    {
        private readonly IFileHelper _fileHelper;

        public SkyscrapperDataLoader(IFileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }

        public SkyscrapperData LoadFromFile(string path)
        {
            var fileLines = _fileHelper.ReadFile(path);
            return ParseFileData(path.Split(@"\").Last(), fileLines);
        }

        private SkyscrapperData ParseFileData(string title, List<string> fileLines)
        {
            int size = int.Parse(fileLines[0]);
            SkyscrapperVariable[,] board = new SkyscrapperVariable[size, size];
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = new SkyscrapperVariable((int?)null, Enumerable.Range(1, size).ToList());
                }
            }

            var topIndex = fileLines.FindIndex(x => x.StartsWith(SkyscrapperEdges.Top));
            var bottomIndex = fileLines.FindIndex(x => x.StartsWith(SkyscrapperEdges.Bottom));
            var leftIndex = fileLines.FindIndex(x => x.StartsWith(SkyscrapperEdges.Left));
            var rightIndex = fileLines.FindIndex(x => x.StartsWith(SkyscrapperEdges.Right));

            List<int> topEdgeConstraints = new List<int>();
            var topVisibleBuildings = fileLines[topIndex].Split(';');
            for (int i = 1; i < topVisibleBuildings.Length; i++)
            {
                topEdgeConstraints.Add(int.Parse(topVisibleBuildings[i]));
            }

            List<int> bottomEdgeConstraints = new List<int>();
            var bottomVisibleBuildings = fileLines[bottomIndex].Split(';');
            for (int i = 1; i < bottomVisibleBuildings.Length; i++)
            {
                bottomEdgeConstraints.Add(int.Parse(bottomVisibleBuildings[i]));
            }

            List<int> leftEdgeConstraints = new List<int>();
            var leftVisibleBuildings = fileLines[leftIndex].Split(';');
            for (int i = 1; i < leftVisibleBuildings.Length; i++)
            {
                leftEdgeConstraints.Add(int.Parse(leftVisibleBuildings[i]));
            }

            List<int> rightEdgeConstraints = new List<int>();
            var rightVisibleBuildings = fileLines[rightIndex].Split(';');
            for (int i = 1; i < rightVisibleBuildings.Length; i++)
            {
                rightEdgeConstraints.Add(int.Parse(rightVisibleBuildings[i]));
            }


            return new SkyscrapperData(
                title,
                size, 
                board, 
                new SkyscrapperConstraints
                {
                    TopEdge = topEdgeConstraints,
                    BottomEdge = bottomEdgeConstraints,
                    RightEdge = rightEdgeConstraints,
                    LeftEdge = leftEdgeConstraints
                });
        }
    }
}