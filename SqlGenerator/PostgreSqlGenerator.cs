using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlGenerator.DDL;
using SqlGenerator.DML;
using SqlGenerator.DML.FieldAndTable;
using SqlGenerator.DML.Truthy;

namespace SqlGenerator
{
    public class PostgreSqlGenerator : IGenerator
    {
        public const char ParameterPrefix = ':';
        
        public string GetQuery(IQueryPart queryPart)
        {
            var sb = new StringBuilder();

            Build(queryPart, sb);

            return sb.ToString();
        }

        public IEnumerable<ParameterExpression> GetParameters(IQueryPart queryPart)
        {
            var parameters = new List<ParameterExpression>();

            GetParameters(queryPart, parameters);

            return parameters;
        }

        public void Build(IQueryPart queryPart, StringBuilder sb)
        {
            switch (queryPart)
            {
                case CreateClause creat:
                    Build(creat, sb);
                    return;
                case ConnectQueryExpression connectQuery:
                    Build(connectQuery, sb);
                    return;
                case DeleteClause delete:
                    Build(delete, sb);
                    return;
                case DropClause drop:
                    Build(drop, sb);
                    return;
                case FromClause from:
                    Build(from, sb);
                    return;
                case GroupByClause groupBy:
                    Build(groupBy, sb);
                    return;
                case TableAndFieldValues tableAndFieldValues:
                    Build(tableAndFieldValues, sb);
                    return;
                case OrderByClause orderBy:
                    Build(orderBy, sb);
                    return;
                case SelectClause select:
                    Build(select, sb);
                    return;
                case WhereClause where:
                    Build(where, sb);
                    return;
                case ITableName tableName:
                    Build(tableName, sb);
                    return;
                case ICreate create:
                    Build(create, sb);
                    return;
                case IExpression expression:
                    Build(expression, sb);
                    return;
                case IQuery query:
                    Build(query, sb);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(queryPart));
            }
        }

        #region Build

        #region Interfaces
        
        private void Build(ITableName name, StringBuilder sb)
        {
            switch (name)
            {
                case TableName tableName:
                    Build(tableName, sb);
                    return;
                case JoinCondition joinCondition:
                    Build(joinCondition, sb);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(name));
            }
        }

        private void Build(IExpression expression, StringBuilder sb)
        {
            switch (expression)
            {
                case IValue value:
                    Build(value, sb);
                    return;
                case ITruthy truthy:
                    Build(truthy, sb);
                    return;
                case SortOrderClause orderClause:
                    Build(orderClause, sb);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(expression));
            }
        }

        private void Build(IValue value, StringBuilder sb)
        {
            switch (value)
            {
                case FieldReferenceExpression fieldReference:
                    Build(fieldReference, sb);
                    return;
                case FunctionCallExpression functionCall:
                    Build(functionCall, sb);
                    return;
                case FieldAliasExpression fieldAlias:
                    Build(fieldAlias, sb);
                    return;
                case LiteralExpression literal:
                    Build(literal, sb);
                    return;
                case ParameterExpression parameter:
                    Build(parameter, sb);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
        
        private void Build(ITruthy truthy, StringBuilder sb)
        {
            switch (truthy)
            {
                case ComparisonExpression comparison:
                    Build(comparison, sb);
                    return;
                case InExpression inExpression:
                    Build(inExpression, sb);
                    return;
                case IsNullExpression isNull:
                    Build(isNull, sb);
                    return;
                case Junction junction:
                    Build(junction, sb);
                    return;
                case Junctions junctions:
                    Build(junctions, sb);
                    return;
                case LikeExpression like:
                    Build(like, sb);
                    return;
                case NotExpression not:
                    Build(not, sb);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(truthy));
            }
        }

        private static void Build(ICreate create, StringBuilder sb)
        {
            switch (create)
            {
                case BaseTypeColumnDefinition baseTypeColumn:
                    Build(baseTypeColumn, sb);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(create));
            }
        }

        private void Build(IQuery query, StringBuilder sb)
        {
            switch (query)
            {
                case DeleteStatement delete:
                    Build(delete, sb);
                    return;
                case InsertStatement insert:
                    Build(insert, sb);
                    return;
                case SelectStatement select:
                    Build(select, sb);
                    return;
                case UpdateStatement update:
                    Build(update, sb);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(query));
            }
        }

        #endregion

        #region General

        private void Build(TableAndFieldValues tableAndFieldValues, StringBuilder sb)
        {
            switch (tableAndFieldValues)
            {
                case InsertClause insert:
                    Build(insert, sb);
                    return;
                case UpdateClause update:
                    Build(update, sb);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tableAndFieldValues));
            }
        }

        private void Build(ConnectQueryExpression expression, StringBuilder sb)
        {
            Build(expression.Lhs, sb);
            sb.Append(" ");
            Build(expression.Rhs, sb);
        }

        private static void Build(LiteralExpression expression, StringBuilder sb)
        {
            if (expression.Literal is string literal)
            {
                sb.Append("'");
                sb.Append(LiteralExpression.Sanitize(literal));
                sb.Append("'");
            }
            else
            {
                sb.Append(expression.Literal);
            }
        }

        private static void Build(ParameterExpression expression, StringBuilder sb)
        {
            sb.Append(ParameterPrefix);
            sb.Append(expression.Name);
        }

        private static void Build(FieldReferenceExpression expression, StringBuilder sb)
        {
            if (!string.IsNullOrWhiteSpace(expression.TableName))
            {
                sb.Append(expression.TableName);
                sb.Append(".");
            }

            sb.Append(expression.FieldName);
        }

        private void Build(FunctionCallExpression expression, StringBuilder sb)
        {
            sb.Append(expression.FunctionName);
            sb.Append("(");
            QueryHelper.BuildJoinedExpression(sb, ", ", expression.Parameters, this);
            sb.Append(")");
        }
        
        #endregion

        #region Create

        private static void Build(BaseTypeColumnDefinition definition, StringBuilder sb)
        {
            sb.Append(definition.Name);
            sb.Append(" ");
            sb.Append(definition.GetTypeValue(definition.Type));
        }

        private void Build(CreateClause clause, StringBuilder sb)
        {
            sb.Append("CREATE TABLE");
            sb.Append(" ");
            if (clause.IfNotExist)
            {
                sb.Append("IF NOT EXISTS ");
            }

            sb.Append(clause.Table);
            sb.Append(" (");
            QueryHelper.BuildJoinedExpression(sb, ", ", clause.Create, this);
            sb.Append(")");
        }

        #endregion

        #region Drop

        private static void Build(DropClause clause, StringBuilder sb)
        {
            sb.Append("DROP TABLE");
            sb.Append(" ");
            sb.Append(clause.Table);
        }

        #endregion

        #region Insert

        private void Build(InsertClause clause, StringBuilder sb)
        {
            const string seperator = ", ";
            sb.Append("INSERT INTO");
            sb.Append(" ");
            sb.Append(clause.Table);
            sb.Append(" (");
            QueryHelper.BuildSeperated(sb, seperator, clause.Values.Select(v => v.Field), (part, builder) => builder.Append(part));
            sb.Append(") ");
            sb.Append("VALUES ");
            sb.Append("(");
            QueryHelper.BuildJoinedExpression(sb, seperator, clause.Values.Select(v => v.Value), this);
            sb.Append(")");
        }

        private void Build(InsertStatement statement, StringBuilder sb)
        {
            Build(statement.InsertClause, sb);
        }

        #endregion

        #region Delete

        private static void Build(DeleteClause clause, StringBuilder sb)
        {
            sb.Append("DELETE FROM");
            sb.Append(" ");
            sb.Append(clause.Table);
        }

        private void Build(DeleteStatement statement, StringBuilder sb)
        {
            Build(statement.Delete, sb);
            QueryHelper.AppendQueryPart(this, sb, statement.Where);
        }

        #endregion

        #region Where

        private void Build(ComparisonExpression expression, StringBuilder sb)
        {
            Build(expression.Lhs, sb);
            sb.Append(" ");
            sb.Append(ComparisonExpression.GetOperatorValue(expression.Op));
            sb.Append(" ");
            Build(expression.Rhs, sb);
        }

        private void Build(Junction junction, StringBuilder sb)
        {
            sb.Append("(");
            Build(junction.Lhs, sb);
            sb.Append(") ");
            sb.Append(Junction.GetOperatorValue(junction.Op));
            sb.Append(" (");
            Build(junction.Rhs, sb);
            sb.Append(")");
        }

        private void Build(Junctions junctions, StringBuilder sb)
        {
            var op = $" {Junction.GetOperatorValue(junctions.Op)} ";
            sb.Append("(");
            QueryHelper.BuildJoinedExpression(sb, op, junctions.Truthies, (part, builder) =>
            {
                sb.Append("(");
                Build(part, builder);
                sb.Append(")");
            });
            sb.Append(")");
        }

        private void Build(IsNullExpression expression, StringBuilder sb)
        {
            sb.Append("(");
            Build(expression.Expr, sb);
            sb.Append(") IS Null");
        }

        private void Build(InExpression expression, StringBuilder sb)
        {
            Build(expression.Lhs, sb);
            sb.Append(" IN (");
            Build(expression.Rhs, sb);
            sb.Append(")");
        }

        private void Build(LikeExpression expression, StringBuilder sb)
        {
            Build(expression.Lhs, sb);
            sb.Append(" LIKE ");
            if (expression.Rhs is LiteralExpression rhs && rhs.Literal is string)
            {
                Build(expression.Rhs, sb);
            }
            else
            {
                sb.Append("'");
                Build(expression.Rhs, sb);
                sb.Append("'");
            }
        }

        private void Build(NotExpression expression, StringBuilder sb)
        {
            sb.Append("NOT (");
            Build(expression.Expr, sb);
            sb.Append(")");
        }

        private void Build(WhereClause clause, StringBuilder sb)
        {
            sb.Append("WHERE ");
            Build(clause.Expr, sb);
        }

        #endregion

        #region Select

        private void Build(FieldAliasExpression expression, StringBuilder sb)
        {
            sb.Append("(");
            Build(expression.Expr, sb);
            sb.Append(") AS ");
            sb.Append(expression.Alias);
        }

        private void Build(SelectClause clause, StringBuilder sb)
        {
            sb.Append("SELECT ");
            QueryHelper.BuildJoinedExpression(sb, ", ", clause.Sel, this);
        }

        private void Build(SelectStatement statement, StringBuilder sb)
        {
            Build(statement.Select, sb);
            sb.Append(" ");
            Build(statement.From, sb);
            QueryHelper.AppendQueryPart(this, sb, statement.Where);
            QueryHelper.AppendQueryPart(this, sb, statement.GroupBy);
            QueryHelper.AppendQueryPart(this, sb, statement.Orderby);
        }

        #endregion

        #region Update

        private void Build(UpdateClause clause, StringBuilder sb)
        {
            sb.Append("UPDATE ");
            sb.Append(clause.Table);
            sb.Append(" SET (");
            QueryHelper.BuildSeperated(sb, ", ", clause.Values, (part, builder) =>
            {
                sb.Append(part.Field);
                sb.Append(" = ");
                Build(part.Value, builder);
            });
            sb.Append(")");
        }

        private void Build(UpdateStatement statement, StringBuilder sb)
        {
            Build(statement.Update, sb);
            QueryHelper.AppendQueryPart(this, sb, statement.Where);
        }

        #endregion

        #region From

        private static void Build(TableName table, StringBuilder sb)
        {
            sb.Append(table.Table);
            if (string.IsNullOrWhiteSpace(table.Alias))
            {
                return;
            }

            sb.Append(" ");
            sb.Append(table.Alias);
        }

        private void Build(JoinCondition condition, StringBuilder sb)
        {
            Build(condition.Table, sb);
            sb.Append(" ");
            sb.Append(GetJoinValue(condition.Join));
            sb.Append(" ");
            Build(condition.TableToJoin, sb);
            sb.Append(" ON (");
            Build(condition.Condition, sb);
            sb.Append(")");
        }

        private static string GetJoinValue(JoinClause jn)
        {
            return jn switch
            {
                JoinClause.Inner => "INNER JOIN",
                JoinClause.Left => "LEFT JOIN",
                JoinClause.Right => "RIGHT JOIN",
                JoinClause.Full => "FULL JOIN",
                JoinClause.LeftOuter => "LEFT OUTER JOIN",
                JoinClause.RightOuter => "RIGHT OUTER JOIN",
                JoinClause.FullOuter => "FULL OUTER JOIN",
                _ => throw new Exception("JoinClause unknown!")
            };
        }

        private void Build(FromClause clause, StringBuilder sb)
        {
            sb.Append("FROM ");
            Build(clause.Table, sb);
        }

        #endregion

        #region Order by

        private void Build(SortOrderClause clause, StringBuilder sb)
        {
            Build(clause.Value, sb);
            sb.Append(" ");
            sb.Append(GetSortOrderValue(clause.Sort));
        }

        private void Build(OrderByClause clause, StringBuilder sb)
        {
            sb.Append("ORDER BY ");
            QueryHelper.BuildJoinedExpression(sb, ", ", clause.OrderBy, this);
        }

        private static string GetSortOrderValue(SortOrder field)
        {
            return field switch
            {
                SortOrder.Ascending => "ASC",
                SortOrder.Descending => "DESC",
                _ => throw new NotSupportedException("Unknown order!")
            };
        }

        #endregion

        #region Group by

        private void Build(GroupByClause clause, StringBuilder sb)
        {
            sb.Append("GROUP BY ");
            QueryHelper.BuildJoinedExpression(sb, ", ", clause.GroupBy, this);
        }

        #endregion

        #endregion

        #region GetParameters

        #region Interfaces

        private static void GetParameters(IQueryPart queryPart, ICollection<ParameterExpression> parameters)
        {
            switch (queryPart)
            {
                case ConnectQueryExpression connectQuery:
                    GetParameters(connectQuery, parameters);
                    return;
                case TableAndFieldValues tableAndFieldValues:
                    GetParameters(tableAndFieldValues, parameters);
                    return;
                case SelectClause select:
                    GetParameters(select, parameters);
                    return;
                case WhereClause where:
                    GetParameters(where, parameters);
                    return;
                case ITableName tableName:
                    GetParameters(tableName, parameters);
                    return;
                case IExpression expression:
                    GetParameters(expression, parameters);
                    return;
                case IQuery query:
                    GetParameters(query, parameters);
                    return;
                case GroupByClause groupBy:
                    GetParameters(groupBy, parameters);
                    return;
                case OrderByClause orderBy:
                    GetParameters(orderBy, parameters);
                    return;
                case FromClause fromClause:
                    GetParameters(fromClause, parameters);
                    return;
                case CreateClause _:
                case DeleteClause _:
                case DropClause _:
                case ICreate _:
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(queryPart));
            }
        }
        
        private static void GetParameters(ITableName tableName, ICollection<ParameterExpression> parameters)
        {
            switch (tableName)
            {
                case JoinCondition joinCondition:
                    GetParameters(joinCondition, parameters);
                    return;
                case TableName _:
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tableName));
            }
        }

        private static void GetParameters(IExpression expression, ICollection<ParameterExpression> parameters)
        {
            switch (expression)
            {
                case ITruthy truthy:
                    GetParameters(truthy, parameters);
                    return;
                case IValue value:
                    GetParameters(value, parameters);
                    return;
                case SortOrderClause sortOrder:
                    GetParameters(sortOrder, parameters);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(expression));
            }
        }

        private static void GetParameters(ITruthy truthy, ICollection<ParameterExpression> parameters)
        {
            switch (truthy)
            {
                case ComparisonExpression comparisonExpression:
                    GetParameters(comparisonExpression, parameters);
                    return;
                case InExpression inExpression:
                    GetParameters(inExpression, parameters);
                    return;
                case IsNullExpression isNullExpression:
                    GetParameters(isNullExpression, parameters);
                    return;
                case Junction junction:
                    GetParameters(junction, parameters);
                    return;
                case Junctions junctions:
                    GetParameters(junctions, parameters);
                    return;
                case LikeExpression likeExpression:
                    GetParameters(likeExpression, parameters);
                    return;
                case NotExpression notExpression:
                    GetParameters(notExpression, parameters);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(truthy));
            }
        }

        private static void GetParameters(IValue value, ICollection<ParameterExpression> parameters)
        {
            switch (value)
            {
                case LiteralExpression _:
                    return;
                case ParameterExpression parameterExpression:
                    GetParameters(parameterExpression, parameters);
                    return;
                case FieldAliasExpression fieldAliasExpression:
                    GetParameters(fieldAliasExpression, parameters);
                    return;
                case FieldReferenceExpression _:
                    return;
                case FunctionCallExpression functionCallExpression:
                    GetParameters(functionCallExpression, parameters);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        private static void GetParameters(IQuery query, ICollection<ParameterExpression> parameters)
        {
            switch (query)
            {
                case DeleteStatement deleteStatement:
                    GetParameters(deleteStatement, parameters);
                    return;
                case InsertStatement insertStatement:
                    GetParameters(insertStatement, parameters);
                    return;
                case SelectStatement selectStatement:
                    GetParameters(selectStatement, parameters);
                    return;
                case UpdateStatement updateStatement:
                    GetParameters(updateStatement, parameters);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(query));
            }
        }

        #endregion

        #region General

        private static void GetParameters(TableAndFieldValues tableAndFieldValues, ICollection<ParameterExpression> parameters)
        {
            foreach (FieldValue fieldValue in tableAndFieldValues.Values)
            {
                GetParameters(fieldValue.Value, parameters);
            }
        }

        private static void GetParameters(ConnectQueryExpression connectQuery, ICollection<ParameterExpression> parameters)
        {
            GetParameters(connectQuery.Lhs, parameters);
            GetParameters(connectQuery.Rhs, parameters);
        }

        private static void GetParameters(ParameterExpression parameterExpression, ICollection<ParameterExpression> parameters)
        {
            parameters.Add(parameterExpression);
        }

        private static void GetParameters(FunctionCallExpression parameterExpression, ICollection<ParameterExpression> parameters)
        {
            foreach (IExpression expression in parameterExpression.Parameters)
            {
                GetParameters(expression, parameters);
            }
        }
        
        private static void GetParameters(SortOrderClause sortOrderClause, ICollection<ParameterExpression> parameters)
        {
            GetParameters(sortOrderClause.Value, parameters);
        }

        #endregion

        #region Insert

        private static void GetParameters(InsertStatement insertStatement, ICollection<ParameterExpression> parameters)
        {
            GetParameters(insertStatement.InsertClause, parameters);
        }

        #endregion

        #region Delete

        private static void GetParameters(DeleteStatement deleteStatement, ICollection<ParameterExpression> parameters)
        {
            if (deleteStatement.Where == null)
            {
                return;
            }

            GetParameters(deleteStatement.Where, parameters);
        }

        #endregion

        #region Where

        private static void GetParameters(ComparisonExpression comparison, ICollection<ParameterExpression> parameters)
        {
            GetParameters(comparison.Lhs, parameters);
            GetParameters(comparison.Rhs, parameters);
        }

        private static void GetParameters(Junction junction, ICollection<ParameterExpression> parameters)
        {
            GetParameters(junction.Lhs, parameters);
            GetParameters(junction.Rhs, parameters);
        }

        private static void GetParameters(Junctions junctions, ICollection<ParameterExpression> parameters)
        {
            foreach (ITruthy junction in junctions.Truthies)
            {
                GetParameters(junction, parameters);
            }
        }

        private static void GetParameters(IsNullExpression isNull, ICollection<ParameterExpression> parameters)
        {
            GetParameters(isNull.Expr, parameters);
        }

        private static void GetParameters(InExpression inExpression, ICollection<ParameterExpression> parameters)
        {
            GetParameters(inExpression.Lhs, parameters);
            GetParameters(inExpression.Rhs, parameters);
        }

        private static void GetParameters(LikeExpression like, ICollection<ParameterExpression> parameters)
        {
            GetParameters(like.Lhs, parameters);
            GetParameters(like.Rhs, parameters);
        }

        private static void GetParameters(NotExpression not, ICollection<ParameterExpression> parameters)
        {
            GetParameters(not.Expr, parameters);
        }

        private static void GetParameters(WhereClause clause, ICollection<ParameterExpression> parameters)
        {
            GetParameters(clause.Expr, parameters);
        }

        #endregion

        #region Select

        private static void GetParameters(FieldAliasExpression fieldAlias, ICollection<ParameterExpression> parameters)
        {
            GetParameters(fieldAlias.Expr, parameters);
        }

        private static void GetParameters(SelectClause clause, ICollection<ParameterExpression> parameters)
        {
            foreach (ISelection selection in clause.Sel)
            {
                GetParameters(selection, parameters);
            }
        }

        private static void GetParameters(SelectStatement selectStatement, ICollection<ParameterExpression> parameters)
        {
            GetParameters(selectStatement.Select, parameters);

            if (selectStatement.Where != null)
            {
                GetParameters(selectStatement.Where, parameters);
            }

            if (selectStatement.GroupBy != null)
            {
                GetParameters(selectStatement.GroupBy, parameters);
            }

            if (selectStatement.Orderby != null)
            {
                GetParameters(selectStatement.Orderby, parameters);
            }
        }

        #endregion

        #region Update

        private static void GetParameters(UpdateStatement updateStatement, ICollection<ParameterExpression> parameters)
        {
            GetParameters(updateStatement.Update, parameters);

            if (updateStatement.Where == null)
            {
                return;
            }

            GetParameters(updateStatement.Where, parameters);
        }

        #endregion

        #region From

        private static void GetParameters(JoinCondition joinCondition, ICollection<ParameterExpression> parameters)
        {
            GetParameters(joinCondition.Condition, parameters);
        }

        private static void GetParameters(FromClause fromClause, ICollection<ParameterExpression> parameters)
        {
            GetParameters(fromClause.Table, parameters);
        }

        #endregion

        #region Order by

        private static void GetParameters(OrderByClause orderByClause, ICollection<ParameterExpression> parameters)
        {
            foreach (SortOrderClause expression in orderByClause.OrderBy)
            {
                GetParameters(expression, parameters);
            }
        }

        #endregion

        #region Group by

        private static void GetParameters(GroupByClause groupByClause, ICollection<ParameterExpression> parameters)
        {
            foreach (IValue expression in groupByClause.GroupBy)
            {
                GetParameters(expression, parameters);
            }
        }

        #endregion

        #endregion
    }
}