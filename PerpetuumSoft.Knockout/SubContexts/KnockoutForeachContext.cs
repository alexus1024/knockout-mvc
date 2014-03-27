using System.IO;
using System.Web.Mvc;

namespace PerpetuumSoft.Knockout
{
	public class KnockoutForeachContext<TModel> : KnockoutCommonRegionContext<TModel>
	{

		public KnockoutForeachContext(ViewContext viewContext, string expression, string afterRender = null)
			: base(viewContext, expression)
		{
			this.AfterRender = afterRender;
		}

		public string AfterRender { get; private set; }

		public override void WriteStart(TextWriter writer)
		{
			if (!string.IsNullOrEmpty(AfterRender))
			{
				writer.WriteLine(string.Format(@"<!-- ko {0}:  {{ data:{1}, afterRender:{2} }} -->", Keyword, Expression,
											   AfterRender));
			}
			else
			{
				base.WriteStart(writer);
			}
		}

		protected override string Keyword
		{
			get
			{
				return "foreach";
			}
		}
	}
}
