//using System.Collections.Generic;
//using System.Linq.Expressions;
//using System.Text;

//namespace PerpetuumSoft.Knockout
//{
//	public class KnockoutObjectBindingItem : KnockoutBindingItem
//	{
//		Dictionary<string, Expression> _props = new Dictionary<string, Expression>();

//		public Dictionary<string, Expression> Properties
//		{
//			get { return _props; }
//		}

//		public override string GetKnockoutExpression(KnockoutExpressionData data)
//		{
//			var sb = new StringBuilder();
//			sb.Append(Name);
//			sb.Append(" : {");
//			foreach (var prop in Properties	)
//			{
//				sb.Append(prop.Key);
//				sb.Append(" : ");
//				string value = KnockoutExpressionConverter.Convert(prop.Value, data);
//				if (string.IsNullOrWhiteSpace(value))
//					value = "$data";
//				sb.Append(value);
//			}
//			sb.Append("}");
//			return sb.ToString();
//		}

//	}
//}