using System.Collections.Generic;

namespace SqlGenerator.DDL
{
    public class CreateClause : IQueryPart
    {
        public string Table { get; }
        public bool IfNotExist { get; }
        public IEnumerable<ICreate> Create { get; }

        public CreateClause(string table, bool ifNotExist, IEnumerable<ICreate> create)
        {
            Table = table;
            IfNotExist = ifNotExist;
            Create = create;
        }
    }
}