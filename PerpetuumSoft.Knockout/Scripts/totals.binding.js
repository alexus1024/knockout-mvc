ko.bindingHandlers.totals = {
	update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {

		var sourceList = valueAccessor().sourceList();
		var propertyName = valueAccessor().totalProperyName;
		var totalsItem = valueAccessor().totalsItem;



		var sumValue = 0;
		for (var itemIndex in sourceList) {
			var item = sourceList[itemIndex];
			var itemValue = item[propertyName]();

			var intItemValue = parseInt(itemValue);

			if (isNaN(intItemValue)) {
				intItemValue = 0;
			}

			sumValue += intItemValue;
		}

		totalsItem[propertyName](sumValue);
	}
};