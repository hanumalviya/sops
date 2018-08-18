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

function AccountViewModel() {
    // properties
    var self = this;
    
    // Reset password dialog
    self.isResetDialogOpen = ko.observable(false);
    self.showResetDialog = function () {
        $('.delete-message').empty();
        $('.field-validation-error').empty();
        self.isResetDialogOpen(true);
    };
    self.closeResetDialog = function () {
        self.isResetDialogOpen(false);
    };
    
    self.resetPassword = function (element, e) {
        self.showResetDialog();
    };
}

ko.applyBindings(new AccountViewModel());