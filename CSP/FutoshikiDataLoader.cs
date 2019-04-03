using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using CSP.Consts;
using CSP.Entities;
using CSP.Helpers.Interfaces;

namespace CSP
{
    public class FutoshikiDataLoader : IDataLoader<FutoshikiData>
    {
        private readonly IFileHelper _fileHelper;

        public FutoshikiDataLoader(IFileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }

        public FutoshikiData LoadFromFile(string path)
        {
            var fileLines = _fileHelper.ReadFile(path);            
            return ParseFileData(fileLines);
        }


        private FutoshikiData ParseFileData(List<string> fileLines)
        {
            //add validation
            int size = int.Parse(fileLines[0]);
            int[,] board = new int[size,size];
            List<FutoshikiConstraint> futoshikiConstraints = new List<FutoshikiConstraint>();

            var startIndex = fileLines.FindIndex(x => x.StartsWith(FutoshikiFileLables.Start));
            var relationsIndex = fileLines.FindIndex(x => x.StartsWith(FutoshikiFileLables.Relations));
            int boardIndex = 0;
            for (int i = startIndex+1; i < relationsIndex; i++)
            {             
                var line = fileLines[i].Split(';');
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[boardIndex, j] = int.Parse(line[j]);
                }
                boardIndex++;
            }

            for (int i = relationsIndex+1; i < fileLines.Count; i++)
            {
                var constraint = fileLines[i].Split(';');
                futoshikiConstraints.Add(new FutoshikiConstraint(constraint[0], constraint[1]));
            }
            return new FutoshikiData {Size = size, Board = board, Constraints = futoshikiConstraints};
        }
    }
}