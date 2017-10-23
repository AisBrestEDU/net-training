using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IQueryableTask
{
	class SqlTranslator : ExpressionVisitor
	{
		StringBuilder sb;
		public SqlTranslator()
		{

		}
		internal string Translate(Expression expression)
		{
			this.sb = new StringBuilder();
			this.Visit(expression);
			return this.sb.ToString();
		}
		private static Expression StripQuotes(Expression e)
		{
			while (e.NodeType == ExpressionType.Quote)
				e = ((UnaryExpression)e).Operand;
			return e;
		}
		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.DeclaringType == typeof(Queryable) && node.Method.Name == "Where")
			{
				this.Visit(node.Arguments[0]);
				sb.Append(" WHERE ");
				LambdaExpression lambda = (LambdaExpression)StripQuotes(node.Arguments[1]);
				this.Visit(lambda.Body);
				return node;
			}
			else if (node.Method.Name == "Contains")
			{
				this.Visit(node.Object);
				sb.Append(" like ");
				this.Visit(node.Arguments[0]);
				return node;
			}
			throw new InvalidOperationException();
		}

		protected override Expression VisitMember(MemberExpression m)
		{
			if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
			{
				string prop = m.Member.Name;
				if (prop.Equals("FullName", StringComparison.OrdinalIgnoreCase) ||
					prop.Equals("LastName", StringComparison.OrdinalIgnoreCase))
				{
					throw new NotSupportedException();
				}
				else
				{
					sb.Append(m.Member.Name);
					return m;
				}
			}
			throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
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
					throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
			}

			this.Visit(b.Right);
			return b;
		}
		protected override Expression VisitConstant(ConstantExpression c)
		{
			IQueryable q = c.Value as IQueryable;

			if (q != null)
			{
				sb.Append("SELECT * FROM ");
				sb.Append(q.ElementType.Name);
			}

			else if (c?.Value == null)
				sb.Append("NULL");
			
			else
			{
				switch (Type.GetTypeCode(c.Value.GetType()))
				{
					case TypeCode.Boolean:
						sb.Append(((bool)c.Value) ? 1 : 0);
						break;
					case TypeCode.String:
						sb.Append("'%");
						sb.Append(c.Value);
						sb.Append("%'");
						break;
					case TypeCode.Object:
						throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));
					default:
						sb.Append(c.Value);
						break;
				}
			}

			return c;
		}

	}

}
