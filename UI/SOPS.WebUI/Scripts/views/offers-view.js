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

function Entity() {
    this.Id = ko.observable();
    this.Title = ko.observable();
    this.Approved = ko.observable();
    this.Date = ko.observable();
    this.Type = ko.observable();
    this.Company = ko.observable();
    this.Trade = ko.observable();
    this.Description = ko.observable();
}

function ListItem(data) {
    this.Id = ko.observable(data.Id);
    this.Title = ko.observable(data.Title);
    this.Company = ko.observable(data.Company);
}

function OffersViewModel() {
    // properties
    var self = this;
    self.entity = ko.observable(new Entity());
    self.list = ko.observableArray([]);
    self.selectedItem = ko.observable();
    self.searchText = ko.observable("");
    
    // behaviours
    self.listIsNotEmpty = ko.computed(function () {
        return self.list().length > 0;
    });

    self.loadDetails = function (id) {
        var url = "/Administration/Offers/Details/" + id;
        $.getJSON(url, function (data) {
            self.entity().Id(data.Id);
            self.entity().Title(data.Title);
            self.entity().Approved(data.Approved);
            self.entity().Date(data.Date);
            self.entity().Type(data.Type);
            self.entity().Company(data.Company);
            self.entity().Trade(data.Trade);
            self.entity().Description(data.Description);
        });
    };
    
    self.selectEntity = function (data) {
        self.selectedItem(data.Id());
        self.loadDetails(self.selectedItem());
    };
    
    self.search = ko.computed(function () {
        var criteria = self.searchText();
        var url = "/Administration/Offers/DataTable?search=" + criteria;
        $.getJSON(url, function (data) {

            var mappedElements = $.map(data, function (item) {
                return new ListItem(item);
            });
            self.list(mappedElements);
            self.selectEntity(self.list()[0]);
        });
    });

    self.loadList = function () {
        var criteria = self.searchText();
        var url = "/Administration/Offers/DataTable?search=" + criteria;
        $.getJSON(url, function (data) {

            var mappedElements = $.map(data, function (item) {
                return new ListItem(item);
            });
            self.list(mappedElements);
            self.selectEntity(self.list()[0]);
        });
    };

    self.showSuccessMessage = function (message) {
        $.ambiance({
            message: message,
            title: "Sukces!",
            type: "success"
        });
    }

    // add dialog
    self.isAddDialogOpen = ko.observable(false);
    self.showAddDialog = function () {
        $('.add-message').empty();
        $('.field-validation-error').empty();
        self.isAddDialogOpen(true);
    };
    self.closeAddDialog = function () {
        self.isAddDialogOpen(false);
    };
    self.submitAddDialog = function (form) {
        $(form).validate();
        if ($(form).valid() == true) {
            $.ajax({
                type: "POST",
                url: "/Administration/Offers/Add",
                data: $(form).serialize()
            }).done(function (result) {
                if (result === true) {
                    self.loadList();
                    self.showSuccessMessage("Zmiany zostały zapisane");
                    self.isAddDialogOpen(false);
                } else {
                    $(form).find('.add-message').html(result);
                }
            });
        }
    };

    // edit dialog
    self.isEditDialogOpen = ko.observable(false);

    self.showEditDialog = function () {
        $('.edit-message').empty();
        $('.field-validation-error').empty();
        self.isEditDialogOpen(true);
    };
    self.closeEditDialog = function () {
        self.isEditDialogOpen(false);
    };
    self.submitEditDialog = function (form) {
        $(form).validate();
        if ($(form).valid() == true) {
            $.ajax({
                type: "POST",
                url: "/Administration/Offers/Edit",
                data: $(form).serialize()
            }).done(function (result) {
                if (result === true) {
                    self.loadList();
                    self.showSuccessMessage("Zmiany zostały zapisane");
                    self.isEditDialogOpen(false);
                } else {
                    $(form).find('.edit-message').html(result);
                }
            });
        }
    };

    // remove dialog
    self.isRemoveDialogOpen = ko.observable(false);
    self.showRemoveDialog = function () {
        $('.delete-message').empty();
        $('.field-validation-error').empty();
        self.isRemoveDialogOpen(true);
    };
    self.closeRemoveDialog = function () {
        self.isRemoveDialogOpen(false);
    };
    self.submitRemoveDialog = function (form) {
        $(form).validate();
        if ($(form).valid() == true) {
            $.ajax({
                type: "POST",
                url: "/Administration/Offers/Delete",
                data: $(form).serialize()
            }).done(function (result) {
                if (result === true) {
                    self.loadList();
                    self.showSuccessMessage("Zmiany zostały zapisane");
                    self.isRemoveDialogOpen(false);
                } else {
                    $(form).find('.delete-message').html(result);
                }
            });
        }
    };

    self.init = function() {
        self.loadList();
    };

    self.init();
}

ko.applyBindings(new OffersViewModel());