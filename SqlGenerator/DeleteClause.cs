using System.Text;

namespace SqlGenerator
{
    public class DeleteClause : IQueryPart
    {
        private readonly string _name;
        private readonly WhereClause _where;

        public DeleteClause(string name, WhereClause where)
        {
            _name = name;
            _where = where;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append("DELETE FROM TABLE ");
            sb.Append(_name);
            sb.Append(" ");
            sb.Append(_where.GetQuery());
        }
    }
}