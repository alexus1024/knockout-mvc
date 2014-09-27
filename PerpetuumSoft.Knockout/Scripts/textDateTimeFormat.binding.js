ko.bindingHandlers.textDateTimeFormat = {
	update: function (element, valueAccessor) {
		var params = valueAccessor();
		var value = ko.unwrap(params.value);
		var format = ko.unwrap(params.format);
		var timeZone = ko.unwrap(params.timeZone);
		var fromNow = ko.unwrap(params.fromNow) === "True";
		var fromNowSuffix = ko.unwrap(params.fromNowSuffix) === "True";

		if (!timeZone) {
			// если зона не указана явно

			if (viewModel && viewModel.ServerTimeZone) {
				// ищем дефолтное значение
				timeZone = 'Etc/GMT-' + viewModel.ServerTimeZone();

			} else {
				// и во вью-модели не найдено дефолтного значения

				////https://echo.centre-it.com/jira/browse/IBSSC-2285
				//var e = 'viewModel does not implements the IServerTimeZoneViewModel interface';
				//console.error(e);
				//throw new Error(e); - убрал это, так как это чтавит крест на повторном использовании библиотеки в других проектах

				//  используем локальную зону
				timeZone = null;
			}


		}

		var formatedValue;
		if (value) {
			var m = moment(value);

			// учитываем зоны только если подключен moment.tz
			if (m.tz) {
				m = m.tz(timeZone);
			}

			if (fromNow) {
				formatedValue = m.fromNow(fromNowSuffix);
			} else {
				formatedValue = m.format(format);
			}

		} else {
			formatedValue = "";
		}

		$(element).text(formatedValue);
	}
};