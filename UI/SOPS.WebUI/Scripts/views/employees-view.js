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
    this.FirstName = ko.observable();
    this.LastName = ko.observable();
    this.Email = ko.observable();
    this.Course = ko.observable();
    this.Type = ko.observable();
    this.Department = ko.observable();
    this.SuperAdministrator = ko.observable();
    this.Administrator = ko.observable();
    this.Moderator = ko.observable();
    this.ManageUrl = ko.observable();
}

function ListItem(data) {
    this.Id = ko.observable(data.Id);
    this.Name = ko.observable(data.Name);
    this.Course = ko.observable(data.Course);
}

function Course(text, value) {
    this.Text = ko.observable(text);
    this.Value = ko.observable(value);
}

function EmployeesViewModel() {
    // properties
    var self = this;
    self.entity = ko.observable(new Entity());
    self.list = ko.observableArray();
    self.selectedItem = ko.observable();
    self.searchText = ko.observable("");

    self.availableCourses = ko.observableArray(),
    self.editSelectedCourse = ko.observable();
    self.selectedDepartment = ko.observable(0);
    
    self.updateEntityCourse = ko.computed(function () {
        var courseId = self.editSelectedCourse();
        self.entity().Course(courseId);
    });

    self.updateAvalaibleCourses = function (departmentId) {
        var url = "/Administration/Employees/Courses?department=" + departmentId;
        $.getJSON(url, function (data) {
            var mappedElements = $.map(data, function (item) {
                return new Course(item.Text, item.Value);
            });
            self.availableCourses(mappedElements);
        });
    };
    
    self.coursesForDepartment = ko.computed(function () {
        var id = self.entity().Department();
        self.updateAvalaibleCourses(id);
    });
    
    self.updateCoursesForDepartment = ko.computed(function () {
        var id = self.selectedDepartment();
        self.updateAvalaibleCourses(id);
    });
    
    // behaviours
    self.listIsNotEmpty = ko.computed(function () {
        return self.list().length > 0;
    });

    self.loadDetails = function (id) {
        var url = "/Administration/Employees/Details/" + id;
        $.getJSON(url, function (data) {
            self.entity().Id(data.Id);
            self.entity().FirstName(data.FirstName);
            self.entity().LastName(data.LastName);
            self.entity().Email(data.Email);
            self.entity().Course(data.Course);
            self.entity().Department(data.Department);
            self.entity().SuperAdministrator(data.SuperAdministrator);
            self.entity().Administrator(data.Administrator);
            self.entity().Moderator(data.Moderator);
            var manageUrl = "/Administration/Account/Manage?UserId=" + data.Id;
            self.entity().ManageUrl(manageUrl);
            
            setTimeout(function () {
                self.editSelectedCourse(data.Course);
            }, 20);
        });
    };
    
    self.selectEntity = function (data) {
        self.selectedItem(data.Id());
        self.loadDetails(self.selectedItem());
    };
    
    self.search = ko.computed(function () {
        var criteria = self.searchText();
        var url = "/Administration/Employees/DataTable?search=" + criteria;
        $.getJSON(url, function (data) {
            var mappedElements = $.map(data, function (item) {
                return new ListItem(item);
            });
            self.list(mappedElements);
        }).done(function () {
            self.selectEntity(self.list()[0]);
        });
    });

    self.loadList = function () {
        var criteria = self.searchText();
        var url = "/Administration/Employees/DataTable?search=" + criteria;
        $.getJSON(url, function(data) {
            var mappedElements = $.map(data, function(item) {
                return new ListItem(item);
            });
            self.list(mappedElements);
        }).done(function() {
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
        self.updateCoursesForDepartment();
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
                url: "/Administration/Employees/Add",
                data: $(form).serialize()
            }).done(function (result) {
                if (result === true) {
                    self.loadList();
                    self.showSuccessMessage("Zmiany zostały zapisane");
                    self.isAddDialogOpen(false);
                }
                else {
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
                url: "/Administration/Employees/Edit",
                data: $(form).serialize()
            }).done(function (result) {
                if (result === true) {
                    self.loadList();
                    self.showSuccessMessage("Zmiany zostały zapisane");
                    self.isEditDialogOpen(false);
                }
                else {
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
                url: "/Administration/Employees/Delete",
                data: $(form).serialize()
            }).done(function (result) {
                if (result === true) {
                    self.loadList();
                    self.showSuccessMessage("Zmiany zostały zapisane");
                    self.isRemoveDialogOpen(false);
                }
                else {
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

ko.applyBindings(new EmployeesViewModel());