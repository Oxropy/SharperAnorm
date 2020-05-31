#nullable enable
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
    
    public class DeleteStatement : IQuery
    {
        private readonly DeleteClause _delete;
        private readonly WhereClause? _where;

        public DeleteStatement(DeleteClause delete)
        {
            _delete = delete;
        }
        
        private DeleteStatement(DeleteClause delete, WhereClause where)
        {
            _delete = delete;
            _where = where;
        }

        public DeleteStatement AddWhere(WhereClause where)
        {
            return new DeleteStatement(_delete, where);
        }
        
        public void Build(StringBuilder sb)
        {
            _delete.Build(sb);
            QueryHelper.AppendQueryPart(sb, _where);
        }
    }
}