using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq.Expressions;
using System.Web;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace PerpetuumSoft.Knockout
{
	public class KnockoutBinding<TModel> : KnockoutSubContext<TModel>, IHtmlString
	{
		public KnockoutBinding(KnockoutContext<TModel> context, string[] instanceNames = null, Dictionary<string, string> aliases = null)
			: base(context, instanceNames, aliases)
		{
		}

		// *** Controlling text and appearance ***

		// Visible
		public KnockoutBinding<TModel> Visible(Expression<Func<TModel, object>> binding)
		{
			Items.Add(new KnockoutBindingItem<TModel, object> { Name = "visible", Expression = binding });
			return this;
		}

		// Text
		public KnockoutBinding<TModel> Text(Expression<Func<TModel, object>> binding)
		{
			Items.Add(new KnockoutBindingItem<TModel, object> { Name = "text", Expression = binding });
			return this;
		}

		public KnockoutBinding<TModel> Text(string binding)
		{
			Items.Add(new KnockoutBindingStringItem("text", binding, false));
			return this;
		}

		// Html
		public KnockoutBinding<TModel> Html(Expression<Func<TModel, string>> binding)
		{
			Items.Add(new KnockoutBindingItem<TModel, string> { Name = "html", Expression = binding });
			return this;
		}

		public KnockoutBinding<TModel> Html(Expression<Func<TModel, Expression<Func<string>>>> binding)
		{
			Items.Add(new KnockoutBindingItem<TModel, Expression<Func<string>>> { Name = "html", Expression = binding });
			return this;
		}

		// *** Working with form fields ***
		public KnockoutBinding<TModel> Value(Expression<Func<TModel, object>> binding)
		{
			Items.Add(new KnockoutBindingItem<TModel, object> { Name = "value", Expression = binding });
			return this;
		}

		public KnockoutBinding<TModel> Readonly(Expression<Func<TModel, object>> binding)
		{
			this.Attr("readonly", binding);
			return this;
		}

		public KnockoutBinding<TModel> Disable(Expression<Func<TModel, bool>> binding)
		{
			Items.Add(new KnockoutBindingItem<TModel, bool> { Name = "disable", Expression = binding });
			return this;
		}

		public KnockoutBinding<TModel> Enable(Expression<Func<TModel, bool>> binding)
		{
			Items.Add(new KnockoutBindingItem<TModel, bool> { Name = "enable", Expression = binding });
			return this;
		}

		public KnockoutBinding<TModel> Checked(Expression<Func<TModel, object>> binding)
		{
			Items.Add(new KnockoutBindingItem<TModel, object> { Name = "checked", Expression = binding });
			return this;
		}

		public KnockoutBinding<TModel> Options(Expression<Func<TModel, IEnumerable>> binding)
		{
			Items.Add(new KnockoutBindingItem<TModel, IEnumerable> { Name = "options", Expression = binding });
			return this;
		}

		public KnockoutBinding<TModel> SelectedOptions(Expression<Func<TModel, IEnumerable>> binding)
		{
			Items.Add(new KnockoutBindingItem<TModel, IEnumerable> { Name = "selectedOptions", Expression = binding });
			return this;
		}

		public KnockoutBinding<TModel> OptionsCaption(Expression<Func<TModel, object>> binding)
		{
			Items.Add(new KnockoutBindingItem<TModel, object> { Name = "optionsCaption", Expression = binding });
			return this;
		}

		public KnockoutBinding<TModel> OptionsCaption(string text)
		{
			Items.Add(new KnockoutBindingStringItem("optionsCaption", text));
			return this;
		}

		public KnockoutBinding<TModel> OptionsText(string text, bool isWord = false)
		{
			Items.Add(new KnockoutBindingStringItem("optionsText", text, isWord));
			return this;
		}

		public KnockoutBinding<TModel> OptionsValue(string text, bool isWord = false)
		{
			Items.Add(new KnockoutBindingStringItem("optionsValue", text, isWord));
			return this;
		}


		public KnockoutBinding<TModel> UniqueName()
		{
			Items.Add(new KnockoutBindingStringItem("uniqueName", "true", false));
			return this;
		}

		public KnockoutBinding<TModel> ValueUpdate(KnockoutValueUpdateKind kind)
		{
			Items.Add(new KnockoutBindingStringItem("valueUpdate", Enum.GetName(typeof(KnockoutValueUpdateKind), kind).ToLower()));
			return this;
		}

		public KnockoutBinding<TModel> HasFocus(Expression<Func<TModel, object>> binding)
		{
			Items.Add(new KnockoutBindingItem<TModel, object> { Name = "hasfocus", Expression = binding });
			return this;
		}

		// *** Complex ***
		public KnockoutBinding<TModel> Css(string name, Expression<Func<TModel, object>> binding)
		{
			ComplexItem("css").Add(new KnockoutBindingItem<TModel, object> { Name = name, Expression = binding });
			return this;
		}

		public KnockoutBinding<TModel> Style(string name, Expression<Func<TModel, object>> binding)
		{
			ComplexItem("style").Add(new KnockoutBindingItem<TModel, object> { Name = name, Expression = binding });
			return this;
		}

		public KnockoutBinding<TModel> Attr(string name, Expression<Func<TModel, object>> binding)
		{
			ComplexItem("attr").Add(new KnockoutBindingItem<TModel, object> { Name = name, Expression = binding });
			return this;
		}

		// *** Events ***
		protected virtual KnockoutBinding<TModel> Event([AspMvcAction]string eventName, [AspMvcController]string actionName, string controllerName, object routeValues)
		{
			var sb = new StringBuilder();
			sb.Append("function() {");
			sb.Append(Context.ServerAction(actionName, controllerName, routeValues));
			sb.Append(";}");
			Items.Add(new KnockoutBindingStringItem(eventName, sb.ToString(), false));
			return this;
		}

		public KnockoutBinding<TModel> Click([AspMvcAction]string actionName, [AspMvcController]string controllerName, object routeValues = null)
		{
			return Event("click", actionName, controllerName, routeValues);
		}

		public KnockoutBinding<TModel> Submit([AspMvcAction]string actionName, [AspMvcController]string controllerName, object routeValues = null)
		{
			return Event("submit", actionName, controllerName, routeValues);
		}

		// *** Custom ***    
		public KnockoutBinding<TModel> Custom(string name, string value, Boolean needQuotes = false)
		{
			Items.Add(new KnockoutBindingStringItem(name, value, needQuotes));
			return this;
		}

		public KnockoutBinding<TModel> Custom(string name, Expression<Func<TModel, object>> binding)
		{
			Items.Add(new KnockoutBindingItem<TModel, object> { Name = name, Expression = binding });
			return this;
		}

		/// <summary>
		/// binding like "tooltip : {text : Amount, delay : Delay}"
		/// </summary>
		public KnockoutBinding<TModel> CustomObject(string name, params KnockoutBindingPropertyInfo[] properties)
		{
			var item = new KnockoutBingindComplexItem() { Name = name };

			foreach (var property in properties)
			{
				if (property.IsExpression)
				{
					item.Add(new KnockoutExpressionBindingItem() { Name = property.Name, ExpressionRaw = property.Expression });
				}
				else
				{
					item.Add(new KnockoutBindingStringItem() { Name = property.Name, Value = property.Value });
				}
			}

			Items.Add(item);
			return this;
		}

		public KnockoutBinding<TModel> FixSeletedDropDownItem<TItem>(Expression<Func<TModel, IList<TItem>>> itemsPath, Expression<Func<TModel, object>> objectValuePath,
	String uniquePropertyPath)
		{
			var item = new KnockoutBingindComplexItem() { Name = "fixSeletedDropDownItem" };
			item.Add(new KnockoutExpressionBindingItem() { Name = "items", ExpressionRaw = itemsPath });
			item.Add(new KnockoutExpressionBindingItem() { Name = "valuePath", ExpressionRaw = objectValuePath });
			item.Add(new KnockoutBindingStringItem() {Name = "uniquePropertyPath", Value = uniquePropertyPath, NeedQuotes = true});

			Items.Insert(0, item);
			return this;
		}


		/// <summary>
		/// Биндинг отображеиня TimeSpan'a с использованием http://momentjs.com/
		/// </summary>
		public KnockoutBinding<TModel> TextTimeSpan(Expression<Func<TModel, TimeSpan>> timeSpanBinding)
		{
			var item = new KnockoutBingindComplexItem() { Name = "textTimeSpan" };
			item.Add(new KnockoutExpressionBindingItem() { Name = "value", ExpressionRaw = timeSpanBinding });

			Items.Add(item);
			return this;
		}

		
		/// <summary>
		/// Биндинг с форматированием даты с использованием http://momentjs.com/
		/// </summary>
		/// <param name="titleBinding"></param>
		/// <param name="momentJsTimeFormat"></param>
		/// <param name="isFromNow">использовать ли .fromNow()</param>
		/// <returns></returns>
		KnockoutBinding<TModel> TextDateTimeFormatHelper(Expression<Func<TModel, object>> titleBinding, string momentJsTimeFormat, string momentJsTimeZoneName = null)
		{
			var item = new KnockoutBingindComplexItem() { Name = "textDateTimeFormat" };
			item.Add(new KnockoutExpressionBindingItem() { Name = "value", ExpressionRaw = titleBinding });
			item.Add(new KnockoutBindingStringItem("format", momentJsTimeFormat));
			item.Add(new KnockoutBindingStringItem("timeZone", momentJsTimeZoneName));

			Items.Add(item);
			return this;
		}

		public KnockoutBinding<TModel> TextDateTimeFormat(Expression<Func<TModel, object>> titleBinding, string momentJsTimeFormat, string momentJsTimeZoneName = null)
		{
			return TextDateTimeFormatHelper(titleBinding, momentJsTimeFormat, momentJsTimeZoneName);
		}

		public KnockoutBinding<TModel> TextMoscowDateTimeFormat(Expression<Func<TModel, object>> titleBinding, string momentJsTimeFormat)
		{
			return TextDateTimeFormatHelper(titleBinding, momentJsTimeFormat, "Europe/Moscow");
		}

		// *** list manipulation ***  
		public KnockoutBinding<TModel> AddItem<T>(Expression<Func<TModel, object>> listPath, T prefilInstance)
		{
			var item = new KnockoutBingindComplexItem() { Name = "addItem" };
			item.Add(new KnockoutExpressionBindingItem() { Name = "list", ExpressionRaw = listPath });
			var templateInstance = JsonConvert.SerializeObject(prefilInstance).ToString(CultureInfo.InvariantCulture).Replace('"', '\'');
			item.Add(new KnockoutBindingStringItem("templateInstance", templateInstance, false));

			Items.Add(item);
			return this;
		}

		public KnockoutBinding<TModel> AddItem(Expression<Func<TModel, object>> listPath, Expression<Func<TModel, object>> instanceTemplatePath)
		{
			var item = new KnockoutBingindComplexItem() { Name = "addItem" };
			item.Add(new KnockoutExpressionBindingItem() { Name = "list", ExpressionRaw = listPath });
			item.Add(new KnockoutExpressionBindingItem() { Name = "templateInstance", ExpressionRaw = instanceTemplatePath });

			Items.Add(item);
			return this;
		}

		public KnockoutBinding<TModel> ListItemManipulation(Expression<Func<TModel, object>> listPath, string manipulationFunctionName)
		{
			var item = new KnockoutBingindComplexItem() { Name = "listItemManipulation" };
			item.Add(new KnockoutExpressionBindingItem() { Name = "list", ExpressionRaw = listPath });
			item.Add(new KnockoutBindingStringItem("index", "$index", false));
			item.Add(new KnockoutBindingStringItem("funcName", manipulationFunctionName, false));

			Items.Add(item);
			return this;
		}

		public KnockoutBinding<TModel> RemoveItem(Expression<Func<TModel, object>> listPath)
		{
			var item = new KnockoutBingindComplexItem() { Name = "removeItem" };
			item.Add(new KnockoutExpressionBindingItem() { Name = "list", ExpressionRaw = listPath });
			item.Add(new KnockoutBindingStringItem("index", "$index", false));

			Items.Add(item);
			return this;
		}

		// *** list manipulation ***  

		// *** Custom ***    
		public KnockoutBinding<TModel> Template(Expression<Func<TModel, object>> name, Expression<Func<TModel, object>> dataPath)
		{
			var item = new KnockoutBingindComplexItem() { Name = "template" };
			item.Add(new KnockoutExpressionBindingItem() { Name = "data", ExpressionRaw = dataPath });
			item.Add(new KnockoutExpressionBindingItem() { Name = "name", ExpressionRaw = name });

			Items.Add(item);
			return this;
		}

		// *** Tooltip ***    

		public KnockoutBinding<TModel> Tooltip(Expression<Func<TModel, object>> titleBinding, Int32 delayShowMs = 0, Int32 delayHideMs = 0, Placement placement = Placement.Top)
		{

			var delayItem = new KnockoutBingindComplexItem() { Name = "delay" };
			delayItem.Add(new KnockoutBindingStringItem("show", delayShowMs.ToString(CultureInfo.InvariantCulture)));
			delayItem.Add(new KnockoutBindingStringItem("hide", delayHideMs.ToString(CultureInfo.InvariantCulture)));

			var item = new KnockoutBingindComplexItem() { Name = "tooltip" };
			item.Add(new KnockoutExpressionBindingItem() { Name = "title", ExpressionRaw = titleBinding });
			item.Add(new KnockoutBindingStringItem("placement", placement.ToString().ToLower()));
			item.Add(delayItem);

			Items.Add(item);
			return this;

			//CustomObject("tooltip", new KnockoutBindingPropertyInfo {Name = "title", Expression = listPath},
			//	new KnockoutBindingPropertyInfo());

		}

		public KnockoutBinding<TModel> Tooltip(Expression<Func<TModel, int>> titleBinding, Int32 delayShowMs = 0, Int32 delayHideMs = 0, Placement placement = Placement.Top)
		{

			var delayItem = new KnockoutBingindComplexItem() { Name = "delay" };
			delayItem.Add(new KnockoutBindingStringItem("show", delayShowMs.ToString(CultureInfo.InvariantCulture)));
			delayItem.Add(new KnockoutBindingStringItem("hide", delayHideMs.ToString(CultureInfo.InvariantCulture)));

			var item = new KnockoutBingindComplexItem() { Name = "tooltip" };
			item.Add(new KnockoutExpressionBindingItem() { Name = "title", ExpressionRaw = titleBinding });
			item.Add(new KnockoutBindingStringItem("placement", placement.ToString().ToLower()));
			item.Add(delayItem);

			Items.Add(item);
			return this;

			//CustomObject("tooltip", new KnockoutBindingPropertyInfo {Name = "title", Expression = listPath},
			//	new KnockoutBindingPropertyInfo());

		}

		public KnockoutBinding<TModel> Tooltip(Expression<Func<TModel, object>> titleBinding, Placement placement)
		{
			return Tooltip(titleBinding, 0, 0, placement);
		}

		public KnockoutBinding<TModel> TooltipForDropDownSelectedText(Int32 delayShowMs = 0, Int32 delayHideMs = 0, int maxWidth = 150)
		{
			var delayItem = new KnockoutBingindComplexItem() { Name = "delay" };
			delayItem.Add(new KnockoutBindingStringItem("show", delayShowMs.ToString(CultureInfo.InvariantCulture)));
			delayItem.Add(new KnockoutBindingStringItem("hide", delayHideMs.ToString(CultureInfo.InvariantCulture)));

			var item = new KnockoutBingindComplexItem() { Name = "tooltipForDropDownSelectedText" };
			item.Add(new KnockoutBindingStringItem("maxWidth", maxWidth.ToString(CultureInfo.InvariantCulture)));

			item.Add(delayItem);

			Items.Add(item);
			return this;

			//CustomObject("tooltip", new KnockoutBindingPropertyInfo {Name = "title", Expression = listPath},
			//	new KnockoutBindingPropertyInfo());

		}

		public KnockoutBinding<TModel> OnlyNumericInput()
		{
			Items.Add(new KnockoutBindingStringItem { Name = "numeric", Value = ""});
			return this;
		}

		public KnockoutBinding<TModel> Totals<TItem>(Expression<Func<TModel, IList<TItem>>> sourceList, Expression<Func<TItem, object>> totalPropertyPath, Expression<Func<TModel, TItem>> totalsItem)
		{
			var totalProperyName = KnockoutExpressionConverter.Convert(totalPropertyPath);
			var totalsBinding = new KnockoutBingindComplexItem() { Name = "totals" };
			totalsBinding.Add(new KnockoutExpressionBindingItem() { Name = "sourceList", ExpressionRaw = sourceList });
			totalsBinding.Add(new KnockoutBindingStringItem() { Name = "totalProperyName", Value = totalProperyName, NeedQuotes = true});
			totalsBinding.Add(new KnockoutExpressionBindingItem() { Name = "totalsItem", ExpressionRaw = totalsItem });

			Items.Add(totalsBinding);
			return this;
		}

		// *** Common ***

		private readonly List<KnockoutBindingItem> items = new List<KnockoutBindingItem>();

		private readonly Dictionary<string, KnockoutBingindComplexItem> complexItems = new Dictionary<string, KnockoutBingindComplexItem>();

		public List<KnockoutBindingItem> Items
		{
			get
			{
				return items;
			}
		}

		private KnockoutBingindComplexItem ComplexItem(string name)
		{
			if (!complexItems.ContainsKey(name))
			{
				complexItems[name] = new KnockoutBingindComplexItem { Name = name };
				items.Add(complexItems[name]);
			}
			return complexItems[name];
		}

		public virtual string ToHtmlString()
		{
			var sb = new StringBuilder();
			sb.Append(@"data-bind=""");
			sb.Append(BindingAttributeContent());
			sb.Append(@"""");
			return sb.ToString();
		}

		public string BindingAttributeContent()
		{
			var sb = new StringBuilder();
			bool first = true;
			foreach (var item in Items)
			{
				if (!item.IsValid())
					continue;
				if (first)
					first = false;
				else
					sb.Append(',');
				sb.Append(item.GetKnockoutExpression(CreateData()));
			}
			return sb.ToString();
		}
	}
}
