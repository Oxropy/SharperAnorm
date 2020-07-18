using System;
using System.Linq;
using System.Text;

namespace SqlGenerator
{
    public class PostgreSqlGenerator : IGenerator
    {
        #region Interface

        public void Build(IQueryPart queryPart, StringBuilder sb)
        {
            switch (queryPart)
            {
                case ConnectQueryExpression connectQuery:
                    Build(connectQuery, sb);
                    break;
                case ComparisonExpression comparison:
                    Build(comparison, sb);
                    break;
                case BaseTypeColumnDefinition baseTypeColumn:
                    Build(baseTypeColumn, sb);
                    break;
                case CreateClause create:
                    Build(create, sb);
                    break;
                case DeleteClause delete:
                    Build(delete, sb);
                    break;
                case DeleteStatement delete:
                    Build(delete, sb);
                    break;
                case DropClause drop:
                    Build(drop, sb);
                    break;
                case FieldAliasExpression fieldAlias:
                    Build(fieldAlias, sb);
                    break;
                case FieldReferenceExpression fieldReference:
                    Build(fieldReference, sb);
                    break;
                case FromClause from:
                    Build(from, sb);
                    break;
                case FunctionCallExpression functionCall:
                    Build(functionCall, sb);
                    break;
                case GroupByClause groupBy:
                    Build(groupBy, sb);
                    break;
                case ICreate create:
                    Build(create, sb);
                    break;
                case LiteralExpression literal:
                    Build(literal, sb);
                    break;
                case ILiteralExpression literal:
                    Build(literal, sb);
                    break;
                case InExpression inExpression:
                    Build(inExpression, sb);
                    break;
                case IsNullExpression isNull:
                    Build(isNull, sb);
                    break;
                case Junction junction:
                    Build(junction, sb);
                    break;
                case Junctions junctions:
                    Build(junctions, sb);
                    break;
                case LikeExpression like:
                    Build(like, sb);
                    break;
                case NotExpression not:
                    Build(not, sb);
                    break;
                case ITruthy truthy:
                    Build(truthy, sb);
                    break;
                case ListExpression list:
                    Build(list, sb);
                    break;
                case PlaceholderExpression placeholder:
                    Build(placeholder, sb);
                    break;
                case IExpression expression:
                    Build(expression, sb);
                    break;
                case IGroupBy groupBy:
                    Build(groupBy, sb);
                    break;
                case InsertClause insert:
                    Build(insert, sb);
                    break;
                case InsertStatement insert:
                    Build(insert, sb);
                    break;
                case SortOrderClause sortOrder:
                    Build(sortOrder, sb);
                    break;
                case IOrderBy orderBy:
                    Build(orderBy, sb);
                    break;
                case SelectStatement select:
                    Build(select, sb);
                    break;
                case UpdateStatement update:
                    Build(update, sb);
                    break;
                case IQuery query:
                    Build(query, sb);
                    break;
                case JoinCondition join:
                    Build(join, sb);
                    break;
                case TableName tableName:
                    Build(tableName, sb);
                    break;
                case ITableName tableName:
                    Build(tableName, sb);
                    break;
                case ISelection selection:
                    Build(selection, sb);
                    break;
                case OrderByClause orderBy:
                    Build(orderBy, sb);
                    break;
                case SelectClause select:
                    Build(select, sb);
                    break;
                case UpdateClause update:
                    Build(update, sb);
                    break;
                case WhereClause where:
                    Build(where, sb);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(queryPart));
            }
        }

        public void Build(ISelection selection, StringBuilder sb)
        {
            switch (selection)
            {
                case FieldAliasExpression fieldAlias:
                    Build(fieldAlias, sb);
                    break;
                case FieldReferenceExpression fieldReference:
                    Build(fieldReference, sb);
                    break;
                case FunctionCallExpression functionCall:
                    Build(functionCall, sb);
                    break;
                case JoinCondition join:
                    Build(join, sb);
                    break;
                case TableName tableName:
                    Build(tableName, sb);
                    break;
                case ITableName tableName:
                    Build(tableName, sb);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selection));
            }
        }

        public void Build(ITableName name, StringBuilder sb)
        {
            switch (name)
            {
                case TableName tableName:
                    Build(tableName, sb);
                    break;
                case JoinCondition joinCondition:
                    Build(joinCondition, sb);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(name));
            }
        }

        public void Build(IExpression expression, StringBuilder sb)
        {
            switch (expression)
            {
                case FieldReferenceExpression fieldReference:
                    Build(fieldReference, sb);
                    break;
                case ComparisonExpression comparison:
                    Build(comparison, sb);
                    break;
                case FunctionCallExpression functionCall:
                    Build(functionCall, sb);
                    break;
                case LiteralExpression literal:
                    Build(literal, sb);
                    break;
                case ILiteralExpression literal:
                    Build(literal, sb);
                    break;
                case InExpression inExpression:
                    Build(inExpression, sb);
                    break;
                case IsNullExpression isNull:
                    Build(isNull, sb);
                    break;
                case Junction junction:
                    Build(junction, sb);
                    break;
                case Junctions junctions:
                    Build(junctions, sb);
                    break;
                case LikeExpression like:
                    Build(like, sb);
                    break;
                case NotExpression not:
                    Build(not, sb);
                    break;
                case ITruthy truthy:
                    Build(truthy, sb);
                    break;
                case ListExpression list:
                    Build(list, sb);
                    break;
                case PlaceholderExpression placeholder:
                    Build(placeholder, sb);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(expression));
            }
        }

        public void Build(ITruthy truthy, StringBuilder sb)
        {
            switch (truthy)
            {
                case ComparisonExpression comparison:
                    Build(comparison, sb);
                    break;
                case InExpression inExpression:
                    Build(inExpression, sb);
                    break;
                case IsNullExpression isNull:
                    Build(isNull, sb);
                    break;
                case Junction junction:
                    Build(junction, sb);
                    break;
                case Junctions junctions:
                    Build(junctions, sb);
                    break;
                case LikeExpression like:
                    Build(like, sb);
                    break;
                case NotExpression not:
                    Build(not, sb);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(truthy));
            }
        }

        public void Build(ILiteralExpression literal, StringBuilder sb)
        {
            switch (literal)
            {
                case LiteralExpression literalExpression:
                    Build(literalExpression, sb);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(literal));
            }
        }

        public void Build(IOrderBy orderBy, StringBuilder sb)
        {
            switch (orderBy)
            {
                case FieldReferenceExpression fieldReference:
                    Build(fieldReference, sb);
                    break;
                case SortOrderClause orderClause:
                    Build(orderClause, sb);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(orderBy));
            }
        }

        public void Build(IGroupBy groupBy, StringBuilder sb)
        {
            switch (groupBy)
            {
                case FieldReferenceExpression fieldReference:
                    Build(fieldReference, sb);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(groupBy));
            }
        }

        public void Build(ICreate create, StringBuilder sb)
        {
            switch (create)
            {
                case BaseTypeColumnDefinition baseTypeColumn:
                    Build(baseTypeColumn, sb);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(create));
            }
        }

        #endregion

        #region General

        public void Build(ConnectQueryExpression expression, StringBuilder sb)
        {
            Build(expression.Lhs, sb);
            sb.Append(" ");
            Build(expression.Rhs, sb);
        }

        public void Build(LiteralExpression expression, StringBuilder sb)
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

        public void Build(FieldReferenceExpression expression, StringBuilder sb)
        {
            if (!string.IsNullOrWhiteSpace(expression.Name))
            {
                sb.Append(expression.Name);
                sb.Append(".");
            }

            sb.Append(expression.FieldName);
        }

        public void Build(FunctionCallExpression expression, StringBuilder sb)
        {
            sb.Append(expression.FunctionName);
            sb.Append("(");
            QueryHelper.BuildJoinedExpression(sb, ", ", expression.Parameters, this);
            sb.Append(")");
        }

        public void Build(ListExpression expression, StringBuilder sb)
        {
            QueryHelper.BuildJoinedExpression(sb, ", ", expression.Expressions, this);
        }

        #endregion

        #region Create

        public void Build(BaseTypeColumnDefinition definition, StringBuilder sb)
        {
            sb.Append(definition.Name);
            sb.Append(" ");
            sb.Append(definition.GetTypeValue(definition.Type1));
        }

        public void Build(CreateClause clause, StringBuilder sb)
        {
            sb.Append("CREATE TABLE ");
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

        public void Build(DropClause clause, StringBuilder sb)
        {
            sb.Append("DROP TABLE ");
            sb.Append(clause.Table);
        }

        #endregion

        #region Insert

        public void Build(InsertClause clause, StringBuilder sb)
        {
            const string seperator = ", ";
            sb.Append("INSERT INTO ");
            sb.Append(clause.Table);
            sb.Append(" (");
            QueryHelper.BuildSeperated(sb, seperator, clause.Values.Select(v => v.field), (part, builder) => builder.Append(part));
            sb.Append(") ");
            sb.Append("VALUES ");
            sb.Append("(");
            QueryHelper.BuildSeperated(sb, seperator, clause.Values.Select(v => v.value), (part, builder) => builder.Append(part));
            sb.Append(")");
        }

        public void Build(InsertStatement statement, StringBuilder sb)
        {
            Build(statement.InsertClause, sb);
        }

        #endregion

        #region Delete

        public void Build(DeleteClause clause, StringBuilder sb)
        {
            sb.Append("DELETE FROM ");
            sb.Append(clause.Table);
        }

        public void Build(DeleteStatement statement, StringBuilder sb)
        {
            Build(statement.Delete, sb);
            QueryHelper.AppendQueryPart(this, sb, statement.Where);
        }

        #endregion

        #region Where

        public void Build(ComparisonExpression expression, StringBuilder sb)
        {
            Build(expression.Lhs, sb);
            sb.Append(" ");
            sb.Append(ComparisonExpression.GetOperatorValue(expression.Op));
            sb.Append(" ");
            Build(expression.Rhs, sb);
        }

        public void Build(Junction junction, StringBuilder sb)
        {
            sb.Append("(");
            Build(junction.Lhs, sb);
            sb.Append(") ");
            sb.Append(Junction.GetOperatorValue(junction.Op));
            sb.Append(" (");
            Build(junction.Rhs, sb);
            sb.Append(")");
        }

        public void Build(Junctions junctions, StringBuilder sb)
        {
            string op = $" {Junction.GetOperatorValue(junctions.Op)} ";
            sb.Append("(");
            QueryHelper.BuildJoinedExpression(sb, op, junctions.Truthies, (part, builder) =>
            {
                sb.Append("(");
                Build(part, builder);
                sb.Append(")");
            });
            sb.Append(")");
        }

        public void Build(IsNullExpression expression, StringBuilder sb)
        {
            sb.Append("(");
            Build(expression.Expr, sb);
            sb.Append(") IS Null");
        }

        public void Build(InExpression expression, StringBuilder sb)
        {
            Build(expression.Lhr, sb);
            sb.Append(" IN (");
            Build(expression.Rhr, sb);
            sb.Append(")");
        }

        public void Build(LikeExpression expression, StringBuilder sb)
        {
            Build(expression.Lhr, sb);
            sb.Append(" LIKE ");
            if (expression.Rhs is LiteralExpression rhs)
            {
                if (rhs.Literal is string)
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
            else
            {
                Build(expression.Rhs, sb);
            }
        }

        public void Build(NotExpression expression, StringBuilder sb)
        {
            sb.Append("NOT (");
            Build(expression.Expr, sb);
            sb.Append(")");
        }

        public void Build(PlaceholderExpression expression, StringBuilder sb)
        {
            sb.Append("?");
        }

        public void Build(WhereClause clause, StringBuilder sb)
        {
            sb.Append("WHERE ");
            Build(clause.Expr, sb);
        }

        #endregion

        #region Select

        public void Build(FieldAliasExpression expression, StringBuilder sb)
        {
            sb.Append("(");
            Build(expression.Expr, sb);
            sb.Append(") AS ");
            sb.Append(expression.Alias);
        }

        public void Build(SelectClause clause, StringBuilder sb)
        {
            sb.Append("SELECT ");
            QueryHelper.BuildJoinedExpression(sb, ", ", clause.Sel, this);
        }

        public void Build(SelectStatement statement, StringBuilder sb)
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

        public void Build(UpdateClause clause, StringBuilder sb)
        {
            sb.Append("UPDATE ");
            sb.Append(clause.Table);
            sb.Append(" SET (");
            QueryHelper.BuildSeperated(sb, ", ", clause.Values, (part, builder) =>
            {
                (string field, object value) = part;
                sb.Append(field);
                sb.Append(" = ");
                sb.Append(value);
            });
            sb.Append(")");
        }

        public void Build(UpdateStatement statement, StringBuilder sb)
        {
            Build(statement.Update, sb);
            QueryHelper.AppendQueryPart(this, sb, statement.Where);
        }

        #endregion

        #region From

        public void Build(TableName table, StringBuilder sb)
        {
            sb.Append(table.Table);
            if (string.IsNullOrWhiteSpace(table.Alias))
            {
                return;
            }

            sb.Append(" ");
            sb.Append(table.Alias);
        }

        public void Build(JoinCondition condition, StringBuilder sb)
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

        public void Build(FromClause clause, StringBuilder sb)
        {
            sb.Append("FROM ");
            Build(clause.Table, sb);
        }

        #endregion

        #region Order by

        public void Build(SortOrderClause clause, StringBuilder sb)
        {
            Build(clause.Field, sb);
            sb.Append(" ");
            sb.Append(GetSortOrderValue(clause.Sort));
        }

        public void Build(OrderByClause clause, StringBuilder sb)
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

        public void Build(GroupByClause clause, StringBuilder sb)
        {
            sb.Append("GROUP BY ");
            QueryHelper.BuildJoinedExpression(sb, ", ", clause.GroupBy, this);
        }

        #endregion
    }
}