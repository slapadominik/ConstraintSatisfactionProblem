using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using CSP.Consts;
using CSP.Entities;
using CSP.Entities.Futoshiki;
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
            int size = int.Parse(fileLines[0]);
            FutoshikiVariable[,] board = new FutoshikiVariable[size,size];
            List<FutoshikiConstraint> futoshikiConstraints = new List<FutoshikiConstraint>();

            var startIndex = fileLines.FindIndex(x => x.StartsWith(FutoshikiFileLables.Start));
            var relationsIndex = fileLines.FindIndex(x => x.StartsWith(FutoshikiFileLables.Relations));
            int boardIndex = 0;
            for (int i = startIndex+1; i < relationsIndex; i++)
            {             
                var line = fileLines[i].Split(';');
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    var value = int.Parse(line[j]);
                    board[boardIndex, j] = new FutoshikiVariable(value == 0 ? (int?) null : value, Enumerable.Range(1, size).ToList());
                }
                boardIndex++;
            }

            for (int i = relationsIndex+1; i < fileLines.Count; i++)
            {
                var constraint = fileLines[i].Split(';');
                futoshikiConstraints.Add(new FutoshikiConstraint(constraint[0], constraint[1]));
            }
            return new FutoshikiData(size,board, futoshikiConstraints);
        }
    }
}