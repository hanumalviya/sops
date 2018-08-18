ko.bindingHandlers.jqDialog = {
    init: function(element, valueAccessor, allBindingsAccessor) {
        var options = ko.utils.unwrapObservable(valueAccessor()) || {};

        setTimeout(function() {
            options.close = function() {
                allBindingsAccessor().dialogVisible(false);
            };

            $(element).dialog(options);
        }, 5);

        ko.utils.domNodeDisposal.addDisposeCallback(element, function() {
            $(element).dialog("destroy");
        });
    },
    update: function(element, valueAccessor, allBindingsAccessor) {
        var shouldBeOpen = ko.utils.unwrapObservable(allBindingsAccessor().dialogVisible);
        setTimeout(function() {
            $(element).dialog(shouldBeOpen ? "open" : "close");
        }, 0);
    }
};


function ContractViewModel() {
    // properties
    var self = this;

    // add dialog
    self.isAddCompanyDialogOpen = ko.observable(false);
    self.showAddCompanyDialog = function () {
        $('.add-message').empty();
        $('.field-validation-error').empty();
        self.isAddCompanyDialogOpen(true);
    };
    self.closeAddCompanyDialog = function () {
        self.isAddCompanyDialogOpen(false);
    };
}

ko.applyBindings(new ContractViewModel());