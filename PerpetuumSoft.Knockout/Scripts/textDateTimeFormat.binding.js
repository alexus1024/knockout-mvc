ko.bindingHandlers.textDateTimeFormat = {
	update: function (element, valueAccessor) {
		var params = valueAccessor();
		var value = ko.unwrap(params.value);
		var format = ko.unwrap(params.format);
		var isFromNow = ko.unwrap(params.fromNow);

		var momentValue = moment(value);

		var formatedValue;

		if (isFromNow == undefined | isFromNow == null) {
			formatedValue = momentValue.format(format);
		} else {
			var hasPrefix = (isFromNow === 'true');
			formatedValue = momentValue.fromNow(hasPrefix);
		}

		$(element).text(formatedValue);

	}
};