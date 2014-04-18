using System.Web.Mvc;

namespace PerpetuumSoft.Knockout
{
  public static class KnockoutExtensions
  {
    public static KnockoutContext<TModel> CreateKnockoutContext<TModel>(this HtmlHelper<TModel> helper)
    {
      return new KnockoutContext<TModel>(helper.ViewContext);
    }

	public static KnockoutContextWithBase<TModel, TModelBase> CreateKnockoutContextWithBase<TModel, TModelBase>(this HtmlHelper<TModel> helper) where TModel : TModelBase
	{
		return new KnockoutContextWithBase<TModel, TModelBase>(helper.ViewContext);
	}
  }
}
