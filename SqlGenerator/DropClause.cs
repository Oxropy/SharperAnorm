using System.Text;

namespace SqlGenerator
{
    public class DropClause : IQueryPart
    {
        private readonly string _table;

        public DropClause(string table)
        {
            _table = table;
        }

        public void Build(StringBuilder sb)
        {
            sb.Append("DROP TABLE ");
            sb.Append(_table);
        }
    }
}