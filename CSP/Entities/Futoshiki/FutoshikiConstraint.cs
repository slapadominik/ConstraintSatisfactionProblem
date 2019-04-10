using System;
using CSP.Consts;

namespace CSP.Entities.Futoshiki
{
    public class FutoshikiConstraint
    {
        public FutoshikiConstraint(string lowerField, string higherField)
        {
            var lowerChars = lowerField.ToCharArray();
            var lowerRow = Fields.Rows[lowerChars[0]];
            LowerIndex = (lowerRow, (int) Char.GetNumericValue(lowerChars[1])-1);

            var higherChars = higherField.ToCharArray();
            var higherRow = Fields.Rows[higherChars[0]];
            HigherIndex = (higherRow, (int)Char.GetNumericValue(higherChars[1])-1);
        }

        public (int row, int column) LowerIndex { get; set; }
        public (int row, int column) HigherIndex { get; set; }
    }
}