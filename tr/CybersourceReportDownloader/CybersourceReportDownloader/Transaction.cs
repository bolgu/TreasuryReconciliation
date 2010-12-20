using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;
using LINQtoCSV;

namespace LINQtoCSV
{
    // Because the fields in this type are used only indirectly, the compiler
    // will warn they are unused or unassigned. Disable those warnings.
#pragma warning disable 0169, 0414, 0649

    class Transaction
    {
        [CsvColumn(FieldIndex = 1)]
        public string TransactionRow;

        

#pragma warning restore 0169, 0414, 0649

        public override string ToString()
        {
            return
                "TransactionRow=" + (string.IsNullOrEmpty(TransactionRow) ? "" : TransactionRow);
                
        }
    }
}
