using System;
using System.Text;
using System.Linq.Expressions;

namespace PerpetuumSoft.Knockout
{
  public abstract class KnockoutBindingItem
  {
    public string Name { get; set; }

    public abstract string GetKnockoutExpression(KnockoutExpressionData data);

    public virtual bool IsValid()
    {
      return true;
    }
  }

	public class KnockoutExpressionBindingItem: KnockoutBindingItem
	{
		public Expression ExpressionRaw { get; set; }

		public override string GetKnockoutExpression(KnockoutExpressionData data)
		{
			string value = KnockoutExpressionConverter.Convert(ExpressionRaw, data);
			if (string.IsNullOrWhiteSpace(value))
				value = "$data";

			var sb = new StringBuilder();

			sb.Append(Name);
			sb.Append(" : ");
			sb.Append(value);

			return sb.ToString();
		}

	}

	public class KnockoutBindingItem<TModel, TResult> : KnockoutExpressionBindingItem
	{
		public Expression<Func<TModel, TResult>> Expression
		{
			get { return (Expression<Func<TModel, TResult>>)ExpressionRaw; }
			set { ExpressionRaw = value; }
		}


	}
}
