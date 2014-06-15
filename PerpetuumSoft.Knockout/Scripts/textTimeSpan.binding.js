ko.bindingHandlers.textTimeSpan = {
	update: function (element, valueAccessor) {
		var params = valueAccessor();
		var value = ko.unwrap(params.value);
	//	var values = value.split(":");
		//		var duration = moment.duration({ 'days': 0, hours: values[0], minuts: values[0], hours: values[0], });
		var duration = moment.duration(value);
		var text = duration.days() + "д " + duration.hours() + "ч " + duration.minutes() + "м";
		$(element).text(text);
	}
};