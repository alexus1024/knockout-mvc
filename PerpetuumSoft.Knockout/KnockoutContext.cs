﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace PerpetuumSoft.Knockout
{
	public static class KnockoutConstants
	{
		public const string ViewModelName = "viewModel";
	}

  public interface IKnockoutContext
  {
    string GetInstanceName();
    string GetIndex();
  }

	public class KnockoutContextWithBase<TModel, TBaseModel> : KnockoutContext<TModel>
		where TModel : TBaseModel
	{
		public KnockoutContextWithBase(ViewContext viewContext) : base(viewContext)
		{
		}

		public KnockoutContext<TBaseModel> GetBaseContext()
		{
			var baseContext = new KnockoutContext<TBaseModel>(this.viewContext);
			baseContext.model = (TBaseModel) this.model;
			baseContext.ContextStack = this.ContextStack;
			return baseContext;
		}

	}

	public class KnockoutContext<TModel> : IKnockoutContext
  {
		public const string ViewModelName = KnockoutConstants.ViewModelName;

	internal TModel model;

    public TModel Model
    {
      get
      {
        return model;
      }
    }

    internal List<IKnockoutContext> ContextStack { get; set; }

    public KnockoutContext(ViewContext viewContext)
    {
      this.viewContext = viewContext;
      ContextStack = new List<IKnockoutContext>();
    }

    protected readonly ViewContext viewContext;

    private bool isInitialized;

    private string GetInitializeData(TModel model, bool needBinding, string customMapping = null)
    {
      if (isInitialized)
        return "";
      isInitialized = true;
      KnockoutUtilities.ConvertData(model);
      this.model = model;

      var sb = new StringBuilder();

      var json = this.JsonSerializeObject(model);

      sb.AppendLine(@"<script type=""text/javascript""> ");
      sb.AppendLine(string.Format("var {0}Js = {1};", ViewModelName, json));
      var mappingData = KnockoutJsModelBuilder.CreateMappingData<TModel>();
      if (mappingData == "{}")
      {
	      if (string.IsNullOrWhiteSpace(customMapping))
	      {
			  sb.AppendLine(string.Format("var {0} = ko.mapping.fromJS({0}Js); ", ViewModelName));   
	      }
	      else
		  {
			  sb.AppendLine(string.Format("var {0} = ko.mapping.fromJS({0}Js, {1}); ", ViewModelName, customMapping));
	      }
      }
      else
      {
        sb.AppendLine(string.Format("var {0}MappingData = {1};", ViewModelName, mappingData));
        sb.AppendLine(string.Format("var {0} = ko.mapping.fromJS({0}Js, {0}MappingData); ", ViewModelName));
      }
      sb.Append(KnockoutJsModelBuilder.AddComputedToModel(model, ViewModelName));
      if (needBinding)
        sb.AppendLine(string.Format("ko.applyBindings({0});", ViewModelName));
      sb.AppendLine(@"</script>");
      return sb.ToString();
    }

    public HtmlString Initialize(TModel model, string custommapping = null)
    {
		return new HtmlString(GetInitializeData(model, false, custommapping));
    }

    public HtmlString Apply(TModel model, string customMapping = null)
    {
      if (isInitialized)
      {
        var sb = new StringBuilder();
        sb.AppendLine(@"<script type=""text/javascript"">");
        sb.AppendLine(string.Format("ko.applyBindings({0});", ViewModelName));
        sb.AppendLine(@"</script>");
        return new HtmlString(sb.ToString());
      }
	  return new HtmlString(GetInitializeData(model, true, customMapping));
    }
	public HtmlString AsyncLoadFullViewModel([AspMvcAction]string actionName, [AspMvcController]string controllerName, string afterLoadHandlerName = null)
	  {
		  var sb = new StringBuilder();
		  sb.AppendLine(@"<script type=""text/javascript""> ");
		  sb.AppendLine(@"try{viewModel['IsBusy'](true);}catch (e){console.log(e);}");
		  if (string.IsNullOrWhiteSpace(afterLoadHandlerName))
			  sb.AppendLine(string.Format(@"executeOnServer({0}, '{1}')", ViewModelName, Url().Action(actionName, controllerName)));
		  else
			sb.AppendLine(string.Format(@"executeOnServer({0}, '{1}', {2})", ViewModelName, Url().Action(actionName, controllerName), afterLoadHandlerName));
		  sb.AppendLine(@"</script>");
		  return new HtmlString(sb.ToString());
	  }

    public HtmlString LazyApply(TModel model, string actionName, string controllerName)
    {
      var sb = new StringBuilder();

      sb.AppendLine(@"<script type=""text/javascript""> ");
      sb.Append("$(document).ready(function() {");

      sb.AppendLine(string.Format("$.ajax({{ url: '{0}', type: 'POST', success: function (data) {{", Url().Action(actionName, controllerName)));

      string mappingData = KnockoutJsModelBuilder.CreateMappingData<TModel>();
      if (mappingData == "{}")
        sb.AppendLine(string.Format("var {0} = ko.mapping.fromJS(data); ", ViewModelName));
      else
      {
        sb.AppendLine(string.Format("var {0}MappingData = {1};", ViewModelName, mappingData));
        sb.AppendLine(string.Format("var {0} = ko.mapping.fromJS(data, {0}MappingData); ", ViewModelName));
      }
      sb.Append(KnockoutJsModelBuilder.AddComputedToModel(model, ViewModelName));
      sb.AppendLine(string.Format("ko.applyBindings({0});", ViewModelName));

      sb.AppendLine("}, error: function (error) { alert('There was an error posting the data to the server: ' + error.responseText); } });");

      sb.Append("});");
      sb.AppendLine(@"</script>");

      return new HtmlString(sb.ToString());
    }

    private int ActiveSubcontextCount
    {
      get
      {
        return ContextStack.Count - 1 - ContextStack.IndexOf(this);
      }
    }

    public KnockoutForeachContext<TItem> Foreach<TItem>(Expression<Func<TModel, IList<TItem>>> binding, string afterRender = null)
    {
      var expression = KnockoutExpressionConverter.Convert(binding, CreateData());
	    return Foreach<TItem>(expression, afterRender);
    }

	public KnockoutForeachContext<TItem> Foreach<TItem>(string expression, string afterRender = null)
	{
		var regionContext = new KnockoutForeachContext<TItem>(viewContext, expression, afterRender);
		regionContext.WriteStart(viewContext.Writer);
		regionContext.ContextStack = ContextStack;
		ContextStack.Add(regionContext);
		return regionContext;
	}

	public KnockoutWithContext<TItem> With<TItem>(Expression<Func<TModel, TItem>> binding)
    {
      var expression = KnockoutExpressionConverter.Convert(binding, CreateData());
      return With<TItem>(expression);
    }

	public KnockoutWithContext<TItem> With<TItem>(String expression)
	{
		var regionContext = new KnockoutWithContext<TItem>(viewContext, expression);
		regionContext.WriteStart(viewContext.Writer);
		regionContext.ContextStack = ContextStack;
		ContextStack.Add(regionContext);
		return regionContext;
	}

    public KnockoutIfContext<TModel> If(Expression<Func<TModel, bool>> binding)
    {
	    var expression = KnockoutExpressionConverter.Convert(binding);
	    return If(expression);
    }

	public KnockoutIfContext<TModel> If(String expression)
	{
		var regionContext = new KnockoutIfContext<TModel>(viewContext, expression);
		regionContext.InStack = false;
		regionContext.WriteStart(viewContext.Writer);
		return regionContext;
	}

    public string GetInstanceName()
    {
      switch (ActiveSubcontextCount)
      {
        case 0:
          return "";
        case 1:
          return "$parent";
        default:
          return "$parents[" + (ActiveSubcontextCount - 1) + "]";
      }
    }

    private string GetContextPrefix()
    {
      var sb = new StringBuilder();
      int count = ActiveSubcontextCount;
      for (int i = 0; i < count; i++)
        sb.Append("$parentContext.");
      return sb.ToString();
    }

    public string GetIndex()
    {
      return GetContextPrefix() + "$index()";
    }

	public string GetCounter(int indexOffset = 1)
	{
		return GetContextPrefix() + string.Format("$index() + {0}", indexOffset);
	}

    public virtual KnockoutExpressionData CreateData()
    {
      return new KnockoutExpressionData { InstanceNames = new[] { GetInstanceName() } };
    }

    public virtual KnockoutBinding<TModel> Bind
    {
      get
      {
        return new KnockoutBinding<TModel>(this, CreateData().InstanceNames, CreateData().Aliases);
      }
    }

    public virtual KnockoutHtml<TModel> Html
    {
      get
      {
        return new KnockoutHtml<TModel>(viewContext, this, CreateData().InstanceNames, CreateData().Aliases);
      }
    }

    public MvcHtmlString ServerAction(string actionName, string controllerName, object routeValues = null, String vmPath = null)
    {
      var url = Url().Action(actionName, controllerName, routeValues);
      url = url.Replace("%28", "(");
      url = url.Replace("%29", ")");
      url = url.Replace("%24", "$");

		var vmFullPath = String.IsNullOrWhiteSpace(vmPath)? ViewModelName: String.Join(".", ViewModelName, vmPath);

	  string exec = string.Format(@"executeOnServer({0}, '{1}')", vmFullPath, url);
      int startIndex = 0;
      const string parentPrefix = "$parentContext.";
      while (exec.Substring(startIndex).Contains("$index()"))
      {
        string pattern = "$index()";
        string nextPattern = parentPrefix + pattern;
        int index = startIndex + exec.Substring(startIndex).IndexOf(pattern);
        while (index - parentPrefix.Length >= startIndex &&
               exec.Substring(index - parentPrefix.Length, nextPattern.Length) == nextPattern)
        {
          index -= parentPrefix.Length;
          pattern = nextPattern;
          nextPattern = parentPrefix + pattern;
        }
        exec = exec.Substring(0, index) + "'+" + pattern + "+'" + exec.Substring(index + pattern.Length);
        startIndex = index + pattern.Length;
      }
      return new MvcHtmlString(exec);
    }

    protected UrlHelper Url()
    {
      return new UrlHelper(viewContext.RequestContext);
    }

	private string JsonSerializeObject(TModel model)
	{
		var settings = new JsonSerializerSettings();
		settings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
		var serializer = JsonSerializer.CreateDefault(settings);

		using (var sw = new System.IO.StringWriter())
		using (var writer = new JsonTextWriter(sw))
		{
			serializer.Serialize(writer, model);
			writer.Flush();
			return sw.ToString();
		}
	}
  }
}
