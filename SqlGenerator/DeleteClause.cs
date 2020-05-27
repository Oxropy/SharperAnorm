using System.Text;

namespace SqlGenerator
{
    public class DeleteClause : IQueryPart
    {
        private readonly string _table;

        public DeleteClause(string table)
        {
            _table = table;
        }

        public void Build(StringBuilder sb)
        {
            sb.Append("DELETE FROM ");
            sb.Append(_table);
        }
    }
}