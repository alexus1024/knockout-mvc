ko.bindingHandlers.numeric = {
	init: function (element, valueAccessor, allBindingsAccessors) {
		$(element).on("keydown", function (event) {
			// Allow: backspace, delete, tab, escape, and enter
			if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
				// Allow: Ctrl+A
				(event.keyCode == 65 && event.ctrlKey === true) ||
				// Allow: . ,
				(event.keyCode == 188 || event.keyCode == 190 || event.keyCode == 110) ||
				// Allow: home, end, left, right
				(event.keyCode >= 35 && event.keyCode <= 39) ||
				//Allow F5
				(event.keyCode >= 116)) {
				// let it happen, don't do anything
				return;
			}
			else {
				// Ensure that it is a number and stop the keypress
				if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
					event.preventDefault();
				}
			}
		});


		var allBindings = ko.unwrap(allBindingsAccessors());
		if (allBindings.value) {
			var valueObservable = allBindings.value;
			if (ko.isObservable(valueObservable)) {
				valueObservable.subscribe(function (v) {

					if (typeof (v) == 'number')
						return;

					if (v == "" || v == null) {
						valueObservable(null);
					} else if (v == "0") {
						valueObservable(0);
					} else {
						var newValue = parseInt(v);
						var currentValue = parseInt(valueObservable());

						if (currentValue == newValue)
							return;

						if (newValue != NaN)
							valueObservable(newValue);
					}
				});
			}
		}
	}
};