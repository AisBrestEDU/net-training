using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace IQueryableTask
{
    class SqlExpressionVisitor: ExpressionVisitor
    {
        private StringBuilder sb;

        public string Translate(Expression expression)
        {
            sb = new StringBuilder();
            Visit(expression);
            var query = sb.ToString();
            if (query.Contains("LastName") || query.Contains("FullName"))
            {
                throw new NotSupportedException();
            }
            return query;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {

            if (node.Method.DeclaringType == typeof(Queryable) && node.Method.Name == "Where")
            {
                Visit(node.Arguments[0]);
                sb.Append(" where ");
                Visit(node.Arguments[1]);
                return node;
            }

            if (node.Method.Name == "Contains")
            {
                Visit(node.Object);
                sb.Append(" like ");
                Visit(node.Arguments[0]);
                return node;
            }

            throw new InvalidOperationException();
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            return Visit(node.Operand);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Left);
            switch (node.NodeType)
            {
                case ExpressionType.AndAlso:
                    sb.Append(" AND ");
                    break;
                case ExpressionType.Or:
                    sb.Append(" OR");
                    break;
                case ExpressionType.Equal:
                    sb.Append(" = ");
                    break;
                case ExpressionType.NotEqual:
                    sb.Append(" <> ");
                    break;
                case ExpressionType.LessThan:
                    sb.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    sb.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    sb.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(" >= ");
                    break;
                default:
                    throw new NotSupportedException();
            }
            Visit(node.Right);
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Value as IQueryable != null)
            {
                sb.Append("select * from person");
            }
            else
            {
                switch (Type.GetTypeCode(node.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        sb.Append(((bool)node.Value) ? 1 : 0);
                        break;
                    case TypeCode.String:
                        sb.Append("'%");
                        sb.Append(node.Value);
                        sb.Append("%'");
                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException();
                    default:
                        sb.Append(node.Value);
                        break;
                }
            }
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression != null && node.Expression.NodeType == ExpressionType.Parameter)
            {
                sb.Append(node.Member.Name);
                return node;
            }

            throw new NotSupportedException();
        }

    }
}
