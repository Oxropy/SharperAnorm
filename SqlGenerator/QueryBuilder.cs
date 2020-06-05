#nullable enable
using System.Collections.Generic;
using System.Text;

namespace SqlGenerator
{
    #region Interfaces

    public interface IQuery : IQueryPart
    {
    }

    public interface IQueryPart
    {
        void Build(StringBuilder sb);
    }

    public interface ISelection : IQueryPart
    {
    }

    public interface ITableName : ISelection
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

    public interface IGroupBy : IQueryPart
    {
    }

    public interface ICreate : IQueryPart
    {
    }

    #endregion

    #region Genaral Expressions

    public class ConnectQueryExpression : IQueryPart
    {
        private readonly IQueryPart _lhs;
        private readonly IQueryPart _rhs;

        public ConnectQueryExpression(IQueryPart lhs, IQueryPart rhs)
        {
            _lhs = lhs;
            _rhs = rhs;
        }

        public void Build(StringBuilder sb)
        {
            _lhs.Build(sb);
            sb.Append(" ");
            _rhs.Build(sb);
        }
    }

    public class LiteralExpression : ILiteralExpression
    {
        public readonly object Literal;

        public LiteralExpression(object literal)
        {
            Literal = literal;
        }

        public static string Sanitize(string s)
        {
            return s; // TODO: ???
        }

        public void Build(StringBuilder sb)
        {
            if (Literal is string literal)
            {
                sb.Append("'");
                sb.Append(Sanitize(literal));
                sb.Append("'");
            }
            else
            {
                sb.Append(Literal);
            }
        }
    }

    public class FieldReferenceExpression : IExpression, ISelection, IOrderBy, IGroupBy
    {
        private readonly string _tableName;
        private readonly string _fieldName;

        public FieldReferenceExpression(string fieldName, string tableName = "")
        {
            _tableName = tableName;
            _fieldName = fieldName;
        }

        public void Build(StringBuilder sb)
        {
            if (!string.IsNullOrWhiteSpace(_tableName))
            {
                sb.Append(_tableName);
                sb.Append(".");
            }

            sb.Append(_fieldName);
        }
    }

    public class FunctionCallExpression : IExpression, ISelection
    {
        private readonly string _functionName;
        private readonly IEnumerable<IExpression> _parameters;

        public FunctionCallExpression(string functionName, IEnumerable<IExpression> parameters)
        {
            _functionName = functionName;
            _parameters = parameters;
        }

        public void Build(StringBuilder sb)
        {
            sb.Append(_functionName);
            sb.Append("(");
            QueryHelper.BuildJoinedExpression(sb, ", ", _parameters);
            sb.Append(")");
        }
    }

    public class ListExpression : IExpression
    {
        private readonly IEnumerable<IExpression> _expressions;

        public ListExpression(IEnumerable<IExpression> expressions)
        {
            _expressions = expressions;
        }

        public void Build(StringBuilder sb)
        {
            QueryHelper.BuildJoinedExpression(sb, ", ", _expressions);
        }
    }

    public abstract class FieldValue
    {
        public string Field { get; }
        public object Value { get; }
        
        protected FieldValue(string field, object value)
        {
            Field = field;
            Value = value;
        }
    }
    
    #endregion

    public static class QueryBuilderExtensions
    {
        public static string GetQuery(this IQueryPart part)
        {
            var sb = new StringBuilder();
            part.Build(sb);
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

        #region Build

        public static IQueryPart AddFrom(this IQueryPart query, FromClause from)
        {
            return new ConnectQueryExpression(query, from);
        }

        public static IQueryPart AddWhere(this IQueryPart query, WhereClause where)
        {
            return new ConnectQueryExpression(query, where);
        }

        public static IQueryPart AddOrderBy(this IQueryPart query, OrderByClause orderBy)
        {
            return new ConnectQueryExpression(query, orderBy);
        }

        public static IQueryPart AddGroupBy(this IQueryPart query, GroupByClause groupBy)
        {
            return new ConnectQueryExpression(query, groupBy);
        }

        #endregion

        #region Create

        public static CreateClause Create(string name, bool ifNotExist, params ICreate[] create)
        {
            return new CreateClause(name, ifNotExist, create);
        }

        public static BaseTypeColumnDefinition ColDefinition(this string name, BaseType type, int typeLength = 0)
        {
            return new BaseTypeColumnDefinition(name, type, typeLength);
        }

        #endregion

        #region Drop

        public static DropClause Drop(string name)
        {
            return new DropClause(name);
        }

        #endregion

        #region Delete

        public static DeleteClause Delete(string name)
        {
            return new DeleteClause(name);
        }

        #endregion

        #region Insert

        public static InsertClause Insert(string name, params InsertValue[] values)
        {
            return new InsertClause(name, values);
        }

        public static InsertValue InsValue(this string name, object value)
        {
            return new InsertValue(name, value);
        }

        #endregion

        #region Update

        public static UpdateClause Insert(string name, params UpdateValue[] values)
        {
            return new UpdateClause(name, values);
        }

        public static UpdateValue UpdValue(this string name, object value)
        {
            return new UpdateValue(name, value);
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
            return Join(lhs, JoinClause.Inner, rhs, comp);
        }

        public static ITableName LeftJoin(this ITableName lhs, ITableName rhs, ITruthy comp)
        {
            return Join(lhs, JoinClause.Left, rhs, comp);
        }

        public static ITableName RightJoin(this ITableName lhs, ITableName rhs, ITruthy comp)
        {
            return Join(lhs, JoinClause.Right, rhs, comp);
        }

        public static ITableName FullJoin(this ITableName lhs, ITableName rhs, ITruthy comp)
        {
            return Join(lhs, JoinClause.Full, rhs, comp);
        }

        public static ITableName LeftOuterJoin(this ITableName lhs, ITableName rhs, ITruthy comp)
        {
            return Join(lhs, JoinClause.LeftOuter, rhs, comp);
        }

        public static ITableName RightOuterJoin(this ITableName lhs, ITableName rhs, ITruthy comp)
        {
            return Join(lhs, JoinClause.RightOuter, rhs, comp);
        }

        public static ITableName FullOuterJoin(this ITableName lhs, ITableName rhs, ITruthy comp)
        {
            return Join(lhs, JoinClause.FullOuter, rhs, comp);
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

        public static ITruthy Not(this ITruthy e)
        {
            return new NotExpression(e);
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

        #region Group By

        public static GroupByClause GroupBy(params IGroupBy[] groupBy)
        {
            return new GroupByClause(groupBy);
        }

        #endregion
    }
}