using System.Text;

namespace SqlGenerator
{
    public interface IGenerator
    {
        void Build(IQueryPart queryPart, StringBuilder sb);
    }
}