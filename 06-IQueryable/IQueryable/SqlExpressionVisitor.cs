using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IQueryableTask
{
    class SqlExpressionVisitor : ExpressionVisitor
    {
        private StringBuilder sb;

        internal string Translate(Expression expression)
        {
            this.sb = new StringBuilder();
            this.Visit(expression);

            return this.sb.ToString();
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression) e).Operand;
            }

            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {

            if (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Where")
            {
                LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);

                sb.Append("SELECT * FROM ");
                this.Visit(m.Arguments[0]);
                sb.Append(" WHERE ");


                this.Visit(lambda.Body);

                return m;
            }

            else if (m.Method.Name == "Contains")
            {
                this.Visit(m.Object);
                sb.Append(" LIKE ");
                this.Visit(m.Arguments[0]);

                return m;
            }

            throw new InvalidOperationException();
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            this.Visit(b.Left);

            switch (b.NodeType)
            {
                case ExpressionType.And:
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
                    throw new NotSupportedException($"The binary operator {b.NodeType} is not supported");
            }
            this.Visit(b.Right);
            return b;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            IQueryable q = c.Value as IQueryable;

            if (q != null)
            {
//                sb.Append("SELECT * FROM ");
                sb.Append(q.ElementType.Name);
            }
            else if (c.Value == null)
            {
                sb.Append("NULL");
            }

            else
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        sb.Append(((bool) c.Value) ? 1 : 0);
                        break;
                    case TypeCode.String:
                        sb.Append("'%");
                        sb.Append(c.Value);
                        sb.Append("%'");
                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException($"The constant for {c.Value} is not supported");
                    default:
                        sb.Append(c.Value);
                        break;
                }

            }
            return c;
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if(m.Member.Name == "LastName" || m.Member.Name == "FullName") throw new NotSupportedException();

            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                sb.Append(m.Member.Name);
                return m;
            }
            throw new NotSupportedException($"The member {m.Member.Name} is not supported");
        }
    }
}
