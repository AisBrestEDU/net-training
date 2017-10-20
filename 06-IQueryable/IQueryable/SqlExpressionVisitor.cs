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
			return _sb.ToString();
		}

		private static Expression StripQuotes(Expression expression)
		{

			while (expression.NodeType == ExpressionType.Quote)
			{

				expression = ((UnaryExpression)expression).Operand;

			}

			return expression;

		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			switch (node.Method.Name)
			{
				case "Where":
					Visit(node.Arguments[0]);
					_sb.Append(" where ");
					Visit(node.Arguments[1]);
					break;
				case "Contains":
					Visit(node.Object);
					_sb.Append("=");
					Visit(node.Arguments[0]);
					break;
				default:
					throw new InvalidOperationException();
			}

			return node;


			//if (node.Method.DeclaringType == typeof(Queryable) && node.Method.Name == "Where")
			//{
			//	_sb.Append("SELECT * FROM ");
			//	Visit(node.Arguments[0]);
			//	_sb.Append(" WHERE ");
			//	LambdaExpression lambda = (LambdaExpression)StripQuotes(node.Arguments[1]);
			//	Visit(lambda.Body);
			//	return node;
			//}

			//else if (node.Method.DeclaringType == typeof(Queryable) && node.Method.Name == "Contains")
			//{
			//	Visit(node.Object);
			//	_sb.Append("=");
			//	Visit(node.Arguments[0]);
			//	return node;
			//}

			//throw new NotSupportedException($"The method '{node.Method.Name}' is not supported");
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			return base.VisitMember(node);
		}

		protected override Expression VisitConstant(ConstantExpression node)
		{
			var q = node.Value as IQueryable;
			if (q != null)
			{
				_sb.Append("select * from person ");
			}
			else
			{
				var t = Type.GetTypeCode(node.Value.GetType());
				switch (t)
				{
					case TypeCode.String:
						_sb.Append("\"");
						_sb.Append(node.Value);
						_sb.Append("\"");
						break;
					case TypeCode.Int32:
						_sb.Append("\"");
						_sb.Append(node.Value);
						_sb.Append("\"");
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
			return base.VisitBinary(node);
		}

		protected override Expression VisitUnary(UnaryExpression node)
		{
			switch (node.NodeType)
			{
				case ExpressionType.Not:
					_sb.Append(" NOT ");
					this.Visit(node.Operand);
					break;
				default:
					throw new NotSupportedException($"The unary operator '{node.NodeType}' is not supported");
			}

			return node;
		}
	}
}