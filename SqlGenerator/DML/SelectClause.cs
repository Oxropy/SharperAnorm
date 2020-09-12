using System.Collections.Generic;

namespace SqlGenerator.DML
{
    public class SelectClause : IQueryPart
    {
        public IEnumerable<ISelection> Sel { get; }

        public SelectClause(IEnumerable<ISelection> sel)
        {
            Sel = sel;
        }
    }
}