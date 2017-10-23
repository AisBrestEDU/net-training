using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace IQueryableTask
{
    class VstrPeople:ExpressionVisitor
    {
        StringBuilder sb = new StringBuilder("select * from person");

        protected  override Expression VisitMethodCall(MethodCallExpression node)
        {

            switch (node.Method.Name)
            {
                case "Contains":
                    sb.Append($" where ");
                    sb.Append((node.Object as MemberExpression).Member.Name.ToLower() + " like " + EscapeName(node.Arguments[0].ToString(),1,1));
                    break;

                default: return base.VisitMethodCall(node);
            }

            return node;
        }

        protected  override Expression VisitMember(MemberExpression node)
        {

            if (!CheckName(node.Member.Name)) throw new NotSupportedException();

            sb.Append(node.Member.Name);

            return node;
        }

        protected  override Expression VisitConstant(ConstantExpression node)
        {
            sb.Append(node.ToString());
            return node;
        }

        protected  override Expression VisitBinary(BinaryExpression node)
        {
            {
                Expression left = this.Visit(node.Left);

                switch (node.NodeType)
                {
                    case ExpressionType.AndAlso: sb.Append(" and "); break;
                    case ExpressionType.Equal: sb.Append(" = "); break;
                    case ExpressionType.NotEqual: sb.Append(" != "); break;
                    case ExpressionType.GreaterThan: sb.Append(" > "); break;
                    default: sb.Append(node.NodeType); break;
                }

                Expression right = this.Visit(node.Right);
            }
            return node;
        }

        public string GetQueryString(Expression expression)
        {
            Visit(expression);

            return sb.ToString();
        }

        private string EscapeName(string name, int ld=0, int rd=0)
        {
            var buff = name.Substring(ld, name.Length - rd-1);
            return $@"'%{buff}%'";
        }

        private  bool CheckName(string name)
        {
            switch (name)
            {
                case "FirstName":
                case "Sex":
                case "Age":
                    return true;
                default:
                    return false;
            }
        }

    }
}
