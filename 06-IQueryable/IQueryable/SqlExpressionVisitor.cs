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
        private string str;

        public string Translate(Expression expression)
        {
            str = "";
            Visit(expression);
            if (str.Contains("LastName") || str.Contains("FullName"))
            {
                throw new NotSupportedException();
            }
            return str;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {

            if (node.Method.Name == "Where")
            {
                Visit(node.Arguments[0]);
                str += " where ";
                Visit(node.Arguments[1]);
                return node;
            }

            if (node.Method.Name == "Contains")
            {
                Visit(node.Object);
                str += " like ";
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
                    str += " AND ";
                    break;
                case ExpressionType.Equal:
                    str += " = ";
                    break;
                case ExpressionType.GreaterThan:
                    str += " > ";
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
                str += "select * from person";
            }
            else
            {
                switch (Type.GetTypeCode(node.Value.GetType()))
                {
                    case TypeCode.String:
                        str += "'%";
                        str += (node.Value);
                        str += "%'";
                        break;
                    default:
                        str += (node.Value);
                        break;
                }
            }
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression != null)
            {
                str += node.Member.Name;
                return node;
            }

            throw new NotSupportedException();
        }
    }
}
