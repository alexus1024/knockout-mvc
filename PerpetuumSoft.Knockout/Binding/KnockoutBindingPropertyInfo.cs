using System;
using System.Linq.Expressions;

namespace PerpetuumSoft.Knockout
{
	/// <summary>
	///  From properties Expression and Value only one can be set
	/// </summary>
	public class KnockoutBindingPropertyInfo
	{
		public String Name { get; set; }
		public Expression Expression { get; set; }
		public String Value { get; set; }

		public Boolean IsExpression
		{
			get { return Expression != null; }
		}
	}
}