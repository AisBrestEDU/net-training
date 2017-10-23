using System;
using System.Linq.Expressions;
using System.Text;
using IQueryableTask;

public class QueryBilder : ExpressionVisitor
{
    private StringBuilder queryString = new StringBuilder("select * from person where ");
    
    protected override Expression VisitBinary(BinaryExpression binaryExpression)
    {
        if (binaryExpression.Left.ToString().Contains("LastName")
            || binaryExpression.Left.ToString().Contains("FullName"))
        {
            throw new NotSupportedException(); 
        }
           

      

        if ((binaryExpression.Left).NodeType == ExpressionType.Convert && ((binaryExpression.Left as UnaryExpression).Operand as MemberExpression).Member.Name.Equals("Type"))
        {
            VisitQuestionTypeBinaryExpression(binaryExpression);
        }
        else
        {
            Expression left = this.Visit(binaryExpression.Left);

            switch (binaryExpression.NodeType)
            {
                case ExpressionType.AndAlso: queryString.Append(" and " ); break;
                case ExpressionType.Equal: queryString.Append(" = "); break;
                case ExpressionType.NotEqual: queryString.Append(" != "); break;
                case ExpressionType.GreaterThan: queryString.Append(" > "); break;
                default: queryString.Append(binaryExpression.NodeType); break;
            }

            Expression right = this.Visit(binaryExpression.Right);
        }
        return binaryExpression;
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        queryString.Append(node.ToString());
        return node;
    }

    protected override Expression VisitMethodCall(MethodCallExpression m)
    {
        switch (m.Method.ToString())
        {
            case "Boolean Contains(System.String)": queryString.Append((m.Object as MemberExpression)
                .Member.Name + " like " + $@"'%{ m.Arguments[0].ToString().Replace("\"","")}%'"); break;

            default: return base.VisitMethodCall(m);
        }

        return m;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        queryString.Append(node.Member.Name);

        return node;
    }

    protected Expression VisitQuestionTypeBinaryExpression(BinaryExpression binaryExpression)
    {
        queryString.Append("Type="  + Enum.GetName(typeof(Person), (binaryExpression.Right as ConstantExpression).Value).ToLower());

        return binaryExpression;
    }

    public string GetQueryString(Expression expression)
    {
        if(!expression.ToString().Contains("\""))
        {
            throw new InvalidOperationException();
        }
        Visit(expression);

        var result = queryString.ToString();

        return result;
    }

}