using System.Collections.Generic;
using System.Text;

namespace SqlGenerator
{
    public interface IQueryPart
    {
        void BuildQuery(StringBuilder sb);
    }

    public interface ISelection : IQueryPart
    {
    }

    public interface IExpression : IQueryPart
    {
    }

    public interface ITruthy : IExpression
    {
    }

    public interface ILiteralExpression : IExpression
    {
    }

    public interface IOrderBy : IQueryPart
    {
    }

    public interface ICreate : IExpression
    {
    }

    public interface IInsert : IExpression
    {
    }

    public class FieldReferenceExpression : IExpression, ISelection, IOrderBy
    {
        public string TableName { get; }
        public string FieldName { get; }

        public FieldReferenceExpression(string fieldName, string tableName = "")
        {
            TableName = tableName;
            FieldName = fieldName;
        }

        public void BuildQuery(StringBuilder sb)
        {
            if (!string.IsNullOrWhiteSpace(TableName))
            {
                sb.Append(TableName);
                sb.Append(".");
            }

            sb.Append(FieldName);
        }
    }

    public class FunctionCallExpression : IExpression, ISelection
    {
        public string FunctionName { get; }
        public IEnumerable<IExpression> Parameters { get; }

        public FunctionCallExpression(string functionName, IEnumerable<IExpression> parameters)
        {
            FunctionName = functionName;
            Parameters = parameters;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append(FunctionName);
            sb.Append("(");
            QueryHelper.BuildJoinedExpression(sb, ", ", Parameters);
            sb.Append(")");
        }
    }

    public static class QueryBuilderExtensions
    {
        public static string GetQuery(this IQueryPart part)
        {
            var sb = new StringBuilder();
            part.BuildQuery(sb);
            return sb.ToString();
        }

        public static FunctionCallExpression Call(this string name, params IExpression[] parameters)
        {
            return new FunctionCallExpression(name, parameters);
        }

        public static FieldReferenceExpression Col(this string field, string table = "")
        {
            return new FieldReferenceExpression(field, table);
        }

        #region Create

        public static CreateClause Create(string name, bool ifNotExist, params ICreate[] create)
        {
            return new CreateClause(name, ifNotExist, create);
        }

        public static CreateClause Create(string name, bool ifNotExist, IEnumerable<ICreate> create)
        {
            return new CreateClause(name, ifNotExist, create);
        }

        public static ColumnDefinition ColDefinition(this string name, BaseType type, int typeLenth = 0)
        {
            return new ColumnDefinition(name, type, typeLenth);
        }

        #endregion

        #region Drop

        public static DropClause Drop(string name)
        {
            return new DropClause(name);
        }

        #endregion

        #region Delete

        public static DeleteClause Delete(string name, WhereClause where)
        {
            return new DeleteClause(name, where);
        }

        #endregion

        #region Insert

        public static InsertClause Insert(string name, params InsertValue[] values)
        {
            return new InsertClause(name, values);
        }

        public static InsertValue Value(this string name, object value)
        {
            return new InsertValue(name, value);
        }

        #endregion

        #region Select

        public static SelectClause Select(params ISelection[] s)
        {
            return new SelectClause(s);
        }

        public static ISelection As(this IExpression expr, string alias)
        {
            return new FieldAliasExpression(expr, alias);
        }

        #endregion

        #region From

        public static FromClause From(ITableName t)
        {
            return new FromClause(t);
        }

        public static ITableName Table(string name, string alias = "")
        {
            return new TableName(name, alias);
        }

        public static ITableName InnerJoin(this ITableName lhs, ITableName rhs, ITruthy comp)
        {
            return new JoinCondition(lhs, JoinClause.Inner, rhs, comp);
        }

        public static ITableName LeftJoin(this ITableName lhs, ITableName rhs, ITruthy comp)
        {
            return new JoinCondition(lhs, JoinClause.Left, rhs, comp);
        }

        public static ITableName RightJoin(this ITableName lhs, ITableName rhs, ITruthy comp)
        {
            return new JoinCondition(lhs, JoinClause.Right, rhs, comp);
        }

        public static ITableName FullJoin(this ITableName lhs, ITableName rhs, ITruthy comp)
        {
            return new JoinCondition(lhs, JoinClause.Full, rhs, comp);
        }

        public static ITableName LeftOuterJoin(this ITableName lhs, ITableName rhs, ITruthy comp)
        {
            return new JoinCondition(lhs, JoinClause.LeftOuter, rhs, comp);
        }

        public static ITableName RightOuterJoin(this ITableName lhs, ITableName rhs, ITruthy comp)
        {
            return new JoinCondition(lhs, JoinClause.RightOuter, rhs, comp);
        }

        public static ITableName FullOuterJoin(this ITableName lhs, ITableName rhs, ITruthy comp)
        {
            return new JoinCondition(lhs, JoinClause.FullOuter, rhs, comp);
        }

        public static ITableName Join(this ITableName lhs, JoinClause jn, ITableName rhs, ITruthy comp)
        {
            return new JoinCondition(lhs, jn, rhs, comp);
        }

        #endregion

        #region Where

        public static WhereClause Where(ITruthy t)
        {
            return new WhereClause(t);
        }

        public static ITruthy And(this ITruthy lhs, ITruthy rhs)
        {
            return Junc(lhs, JunctionOp.And, rhs);
        }

        public static ITruthy Or(this ITruthy lhs, ITruthy rhs)
        {
            return Junc(lhs, JunctionOp.Or, rhs);
        }

        public static ITruthy Junc(this ITruthy lhs, JunctionOp op, ITruthy rhs)
        {
            return new Junction(lhs, op, rhs);
        }

        public static ITruthy Eq(this IExpression lhs, IExpression rhs)
        {
            return Comp(lhs, ComparisonOperator.Equal, rhs);
        }

        public static ITruthy Neq(this IExpression lhs, IExpression rhs)
        {
            return Comp(lhs, ComparisonOperator.NotEqual, rhs);
        }

        public static ITruthy Gt(this IExpression lhs, IExpression rhs)
        {
            return Comp(lhs, ComparisonOperator.GreaterThan, rhs);
        }

        public static ITruthy GtEq(this IExpression lhs, IExpression rhs)
        {
            return Comp(lhs, ComparisonOperator.GreaterThanOrEqual, rhs);
        }

        public static ITruthy Lt(this IExpression lhs, IExpression rhs)
        {
            return Comp(lhs, ComparisonOperator.LowerThan, rhs);
        }

        public static ITruthy LtEq(this IExpression lhs, IExpression rhs)
        {
            return Comp(lhs, ComparisonOperator.LowerThanOrEqual, rhs);
        }

        public static ITruthy Comp(this IExpression lhs, ComparisonOperator co, IExpression rhs)
        {
            return new ComparisonExpression(lhs, co, rhs);
        }

        public static LiteralExpression Val(object s)
        {
            return new LiteralExpression(s);
        }

        public static ListExpression List(params IExpression[] parameters)
        {
            return new ListExpression(parameters);
        }

        public static ITruthy IsNull(this IExpression e)
        {
            return new IsNullExpression(e);
        }

        public static ITruthy In(this IExpression lhs, IExpression rhs)
        {
            return new InExpression(lhs, rhs);
        }

        public static ITruthy Like(this IExpression lhs, IExpression rhs)
        {
            return new LikeExpression(lhs, rhs);
        }

        public static PlaceholderExpression Plc()
        {
            return new PlaceholderExpression();
        }

        #endregion

        #region Order By

        public static OrderByClause OrderBy(params IOrderBy[] orderBy)
        {
            return new OrderByClause(orderBy);
        }

        #endregion
    }

    public static class QueryHelper
    {
        public static void BuildJoinedExpression(StringBuilder sb, string seperator, IEnumerable<IQueryPart> parts)
        {
            using var part = parts.GetEnumerator();
            if (!part.MoveNext())
            {
                return;
            }

            var current = part.Current;
            current.BuildQuery(sb);

            while (part.MoveNext())
            {
                current = part.Current;
                sb.Append(seperator);
                current.BuildQuery(sb);
            }
        }
    }
}