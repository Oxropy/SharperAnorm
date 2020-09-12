#nullable enable
using System.Collections.Generic;
using SqlGenerator.DDL;
using SqlGenerator.DML;
using SqlGenerator.DML.FieldAndTable;
using SqlGenerator.DML.Truthy;

namespace SqlGenerator
{
    #region Interfaces

    public interface IQuery : IQueryPart { }

    public interface IQueryPart { }

    public interface ISelection : IValue { }

    public interface ITableName : IQueryPart { }

    public interface IExpression : IQueryPart { }

    public interface ITruthy : IExpression { }

    public interface ICreate : IQueryPart { }

    public interface IValue : IExpression { }

    #endregion

    #region Genaral Expressions

    public class ConnectQueryExpression : IQueryPart
    {
        public IQueryPart Lhs { get; }
        public IQueryPart Rhs { get; }

        public ConnectQueryExpression(IQueryPart lhs, IQueryPart rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }
    }

    public class LiteralExpression : IValue
    {
        public readonly object Literal;

        public LiteralExpression(object literal)
        {
            Literal = literal;
        }

        public static string Sanitize(string s)
        {
            return s.Replace("'", "''");
        }
    }

    public class ParameterExpression : IValue
    {
        public string Name { get; }
        public object Parameter { get; }

        public ParameterExpression(string name, object parameter)
        {
            Name = name;
            Parameter = parameter;
        }
    }

    public class FieldReferenceExpression : ISelection
    {
        public string TableName { get; }
        public string FieldName { get; }

        public FieldReferenceExpression(string fieldName, string tableName = "")
        {
            TableName = tableName;
            FieldName = fieldName;
        }
    }

    public class FunctionCallExpression : ISelection
    {
        public string FunctionName { get; }
        public IEnumerable<IExpression> Parameters { get; }

        public FunctionCallExpression(string functionName, IEnumerable<IExpression> parameters)
        {
            FunctionName = functionName;
            Parameters = parameters;
        }
    }

    #endregion

    public static class QueryBuilderExtensions
    {
        public static string GetQuery(this IQueryPart part, IGenerator generator)
        {
            return generator.GetQuery(part);
        }

        public static LiteralExpression Val(object s)
        {
            return new LiteralExpression(s);
        }

        public static ParameterExpression Para(this string name, object value)
        {
            return new ParameterExpression(name, value);
        }

        public static FieldReferenceExpression Col(this string field, string table = "")
        {
            return new FieldReferenceExpression(field, table);
        }

        public static FunctionCallExpression Call(this string name, params IExpression[] parameters)
        {
            return new FunctionCallExpression(name, parameters);
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

        public static BaseTypeColumnDefinition ColDefinition(this string name, BaseType type, int typeLength = 0)
        {
            return new BaseTypeColumnDefinition(name, type, typeLength);
        }

        public static CreateClause Create(string name, bool ifNotExist, params ICreate[] create)
        {
            return new CreateClause(name, ifNotExist, create);
        }

        #endregion

        #region Drop

        public static DropClause Drop(string name)
        {
            return new DropClause(name);
        }

        #endregion

        #region Insert

        public static InsertClause Insert(string name, params FieldValue[] values)
        {
            return new InsertClause(name, values);
        }

        #endregion

        #region Update

        public static UpdateClause Update(string name, params FieldValue[] values)
        {
            return new UpdateClause(name, values);
        }

        #endregion

        #region Delete

        public static DeleteClause Delete(TableName name)
        {
            return new DeleteClause(name);
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

        public static TableName Table(string name, string alias = "")
        {
            return new TableName(name, alias);
        }

        public static FromClause From(ITableName t)
        {
            return new FromClause(t);
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

        private static ITableName Join(this ITableName lhs, JoinClause jn, ITableName rhs, ITruthy comp)
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

        private static ITruthy Junc(this ITruthy lhs, JunctionOp op, ITruthy rhs)
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

        private static ITruthy Comp(this IExpression lhs, ComparisonOperator co, IExpression rhs)
        {
            return new ComparisonExpression(lhs, co, rhs);
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

        public static ITruthy Like(this IValue lhs, IValue rhs)
        {
            return new LikeExpression(lhs, rhs);
        }

        #endregion

        #region Order By

        public static SortOrderClause SortOrder(this FieldReferenceExpression field, SortOrder sortOrder)
        {
            return new SortOrderClause(field, sortOrder);
        }

        public static OrderByClause OrderBy(params SortOrderClause[] orderBy)
        {
            return new OrderByClause(orderBy);
        }

        #endregion

        #region Group By

        public static GroupByClause GroupBy(params IValue[] groupBy)
        {
            return new GroupByClause(groupBy);
        }

        #endregion
    }
}