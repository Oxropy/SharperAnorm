using System.Text;

namespace SqlGenerator
{
    public class DeleteClause : IQueryPart
    {
        public string Name { get; }
        public WhereClause Where { get; }

        public DeleteClause(string name, WhereClause where)
        {
            Name = name;
            Where = where;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append("DELETE FROM TABLE ");
            sb.Append(Name);
            sb.Append(" ");
            sb.Append(Where.GetQuery());
        }
    }
}