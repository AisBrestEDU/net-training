using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace IQueryableTask
{
	public class SqlExpressionVisitor : ExpressionVisitor
	{
		private StringBuilder _sb;

		public string Translate(Expression expression)
		{
			_sb = new StringBuilder();
			Visit(expression);
			var output = _sb.ToString();

			if (output.Contains("LastName") || output.Contains("FullName"))
			{
				throw new NotSupportedException();
			}

			return output;
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Method.DeclaringType == typeof(Queryable) && node.Method.Name == "Where")
			{
				Visit(node.Arguments[0]);
				_sb.Append(" WHERE ");
				Visit(node.Arguments[1]);
				return node;
			}

			else if (node.Method.Name == "Contains")
			{
				Visit(node.Object);
				_sb.Append(" like ");
				Visit(node.Arguments[0]);
				return node;
			}

			throw new InvalidOperationException();
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			if (node.Expression != null && node.Expression.NodeType == ExpressionType.Parameter)
			{
				_sb.Append(node.Member.Name);
				return node;
			}

			throw new NotSupportedException($"The member '{node.Member.Name}' is not supported");
		}

		protected override Expression VisitConstant(ConstantExpression node)
		{
			var q = node.Value as IQueryable;
			if (q != null)
			{
				_sb.Append("select * from person");
			}
			else
			{
				var t = Type.GetTypeCode(node.Value.GetType());
				switch (t)
				{
					case TypeCode.String:
						_sb.Append("'%");
						_sb.Append(node.Value);
						_sb.Append("%'");
						break;
					case TypeCode.Int32:
						_sb.Append(node.Value);
						break;
					default:
						_sb.Append(node.Value);
						break;
				}
			}
			return node;
		}

		protected override Expression VisitBinary(BinaryExpression node)
		{
			Visit(node.Left);
			switch (node.NodeType)
			{
				case ExpressionType.GreaterThan:
					_sb.Append(" > ");
					break;
				case ExpressionType.AndAlso:
					_sb.Append(" AND ");
					break;
				case ExpressionType.Equal:
					_sb.Append(" = ");
					break;
				default:
					throw new NotSupportedException($"The binary operator '{node.NodeType}' is not supported");
			}

			Visit(node.Right);
			return node;
		}

		protected override Expression VisitUnary(UnaryExpression node)
		{
			//Expression operand = this.Visit(u.Operand);
			//if (operand != u.Operand)
			//{
			//	return Expression.MakeUnary(u.NodeType, operand, u.Type, u.Method);
			//}
			//return u;
			return Visit(node.Operand);
		}
	}
}