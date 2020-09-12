using System.Collections.Generic;
using System.Text;

namespace SqlGenerator
{
    public interface IGenerator
    {
        string GetQuery(IQueryPart queryPart);
        IEnumerable<ParameterExpression> GetParameters(IQueryPart queryPart);
        void Build(IQueryPart queryPart, StringBuilder sb);
    }
}