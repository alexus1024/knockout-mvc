ko.bindingHandlers.textDateTimeFormat = {
	update: function (element, valueAccessor) {
		var params = valueAccessor();
		var value = ko.unwrap(params.value);
		var format = ko.unwrap(params.format);
		var timeZone = ko.unwrap(params.timeZone);

		if (!timeZone) {
			if (!viewModel.ServerTimeZone) {
				//https://echo.centre-it.com/jira/browse/IBSSC-2285
				var e = 'viewModel does not implements the IServerTimeZoneViewModel interface';
				console.error(e);
				throw new Error(e);
			}

			timeZone = 'Etc/GMT-' + viewModel.ServerTimeZone();
		}

		var formatedValue = moment(value).tz(timeZone).format(format);
		$(element).text(formatedValue);
	}
};