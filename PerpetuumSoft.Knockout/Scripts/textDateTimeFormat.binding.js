ko.bindingHandlers.textDateTimeFormat = {
	update: function (element, valueAccessor) {
		var params = valueAccessor();
		var value = ko.unwrap(params.value);
		var format = ko.unwrap(params.format);
		var formatedValue = moment(value).format(format);
		$(element).text(formatedValue);
	}
};