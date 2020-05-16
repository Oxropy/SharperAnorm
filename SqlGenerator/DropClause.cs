using System.Text;

namespace SqlGenerator
{
    public class DropClause : IQueryPart
    {
        private readonly string _name;

        public DropClause(string name)
        {
            _name = name;
        }

        public void Build(StringBuilder sb)
        {
            sb.Append("DROP TABLE ");
            sb.Append(_name);
        }
    }
}