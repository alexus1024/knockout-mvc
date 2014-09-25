function refresh(list) {
	var data = list().slice(0);
	list([]);
	list(data);
};

ko.bindingHandlers.addItem = {
	init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
		$(element).on('click', function () {
			var params = valueAccessor();

			var list = ko.unwrap(params.list);


			var jsTemplate = ko.mapping.toJS(params.templateInstance); // если не делать такого копирования, то некоторые observable берутся из оригнигинала без копирования, что приводит к тому, что новый элемент биндится не туда.
			var newItem = ko.mapping.fromJS(jsTemplate);

			list.push(newItem);
			refresh(params.list);

			var callback = window[params.afterAddCallback];
			if (callback) {
				callback(newItem, list);
			}

		});
	}
};

ko.bindingHandlers.removeItem = {
	init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
		$(element).on('click', function () {
			var params = valueAccessor();

			var list = ko.unwrap(params.list);
			var index = params.index();

			list.splice(index, 1);
			refresh(params.list);
		});
	}
}

ko.bindingHandlers.listItemManipulation = {
	init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
		$(element).on('click', function () {
			var params = valueAccessor();

			var list = params.list;
			var index = params.index();

			var funcName = ko.unwrap(params.funcName);

			funcName(list, index);
		});
	}
};


ko.bindingHandlers.listManipulation = {
	init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
		$(element).on('click', function () {
			var params = valueAccessor();

			var list = params.list;

			var funcName = ko.unwrap(params.funcName);

			funcName(list);
		});
	}
};


ko.bindingHandlers.moveItemUp = {
	init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
		var params = valueAccessor();

		var list = ko.unwrap(params.list);
		var index = params.index();
		var buttonAutoDisable = params.buttonAutoDisable;

		if (buttonAutoDisable && index == 0)
			$(element).attr("disabled", "disabled");

		$(element).on('click', function () {
			if (index >= 1) {
				var array = list;
				list.splice(index - 1, 2, array[index], array[index - 1]);

				refresh(params.list);
			}
		});
	}
};

ko.bindingHandlers.moveItemDown = {
	init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
		var params = valueAccessor();

		var list = ko.unwrap(params.list);
		var index = params.index();
		var buttonAutoDisable = params.buttonAutoDisable;

		if (buttonAutoDisable && index == list.length - 1)
			$(element).attr("disabled", "disabled");

		$(element).on('click', function () {

			if (index < list.length - 1) {
				var array = list;
				list.splice(index, 2, array[index + 1], array[index]);

				refresh(params.list);
			}
		});
	}
};