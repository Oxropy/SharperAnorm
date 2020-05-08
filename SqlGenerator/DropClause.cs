using System.Text;

namespace SqlGenerator
{
    public class DropClause : IQueryPart
    {
        public string Name { get; }

        public DropClause(string name)
        {
            Name = name;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append("DROP TABLE ");
            sb.Append(Name);
        }
    }
}