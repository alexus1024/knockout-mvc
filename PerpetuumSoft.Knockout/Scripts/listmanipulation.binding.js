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
			var newItem = ko.mapping.fromJS(params.templateInstance);

			list.push(newItem);
			refresh(params.list);
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