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

function Student() {
    this.Name = ko.observable();
    this.LastName = ko.observable();
    this.City = ko.observable();
    this.Street = ko.observable();
    this.PostalCode = ko.observable();
    this.Album = ko.observable();
    this.Course = ko.observable();
    this.Department = ko.observable();
    this.Email = ko.observable();
    this.Id = ko.observable();
    this.Mode = ko.observable();
    this.Phone = ko.observable();
}

function ListItem(data) {
    this.Id = ko.observable(data.Id);
    this.Name = ko.observable(data.Name);
    this.Album = ko.observable(data.Album);
}

function StudentsViewModel() {
    // properties
    var self = this;
    self.student = ko.observable(new Student());
    self.studentsList = ko.observableArray([]);
    self.selectedItem = ko.observable();
    self.searchText = ko.observable("");
    
    // behaviours
    self.anyStudent = ko.computed(function() {
        return self.studentsList().length > 0;
    });

    self.loadStudentDetails = function (id) {
        var url = "/Administration/Students/Details/" + id;
        $.getJSON(url, function (data) {
            self.student().Name(data.Name);
            self.student().LastName(data.LastName);
            self.student().City(data.City);
            self.student().Street(data.Street);
            self.student().PostalCode(data.PostalCode);
            self.student().Album(data.Album);
            self.student().Course(data.Course);
            self.student().Department(data.Department);
            self.student().Email(data.Email);
            self.student().Id(data.Id);
            self.student().Mode(data.Mode);
            self.student().Phone(data.Phone);
        });
    };
    
    self.selectStudent = function (data) {
        self.selectedItem(data.Id());
        self.loadStudentDetails(self.selectedItem());
    };
    
    self.search = ko.computed(function () {
        var criteria = self.searchText();
        var url = "/Administration/Students/DataTable?search=" + criteria;
        $.getJSON(url, function (data) {

            var mappedStudents = $.map(data, function (item) {
                return new ListItem(item);
            });
            self.studentsList(mappedStudents);
            self.selectStudent(self.studentsList()[0]);
        });
    });

    self.loadList = function () {
        var criteria = self.searchText();
        var url = "/Administration/Students/DataTable?search=" + criteria;
        $.getJSON(url, function (data) {

            var mappedStudents = $.map(data, function (item) {
                return new ListItem(item);
            });
            self.studentsList(mappedStudents);
            self.selectStudent(self.studentsList()[0]);
        });
    };

    self.showSuccessMessage = function (message) {
        $.ambiance({
            message: message,
            title: "Sukces!",
            type: "success"
        });
    }

    // add student dialog
    self.isAddStudentDialogOpen = ko.observable(false);
    self.showAddStudentDialog = function () {
        $('.add-message').empty();
        $('.field-validation-error').empty();
        self.isAddStudentDialogOpen(true);
    };
    self.closeAddStudentDialog = function () {
        self.isAddStudentDialogOpen(false);
    };
    self.submitAddStudentDialog = function (form) {
        $(form).validate();
        if ($(form).valid() == true) {
            $.ajax({
                type: "POST",
                url: "/Administration/Students/Add",
                data: $(form).serialize()
            }).done(function (result) {
                if (result === true) {
                    self.loadList();
                    self.showSuccessMessage("Zmiany zostały zapisane");
                    self.isAddStudentDialogOpen(false);
                } else {
                    $(form).find('.add-message').html(result);
                }
            });
        }
    };

    // edit student dialog
    self.isEditStudentDialogOpen = ko.observable(false);

    self.showEditStudentDialog = function () {
        $('.edit-message').empty();
        $('.field-validation-error').empty();
        self.isEditStudentDialogOpen(true);
    };
    self.closeEditStudentDialog = function () {
        self.isEditStudentDialogOpen(false);
    };
    self.submitEditStudentDialog = function (form) {
        $(form).validate();
        if ($(form).valid() == true) {
            $.ajax({
                type: "POST",
                url: "/Administration/Students/Edit",
                data: $(form).serialize()
            }).done(function (result) {
                if (result === true) {
                    self.loadList();
                    self.showSuccessMessage("Zmiany zostały zapisane");
                    self.isEditStudentDialogOpen(false);
                } else {
                    $(form).find('.edit-message').html(result);
                }
            });
        }
    };

    // remove student dialog
    self.isRemoveStudentDialogOpen = ko.observable(false);
    self.showRemoveStudentDialog = function () {
        $('.delete-message').empty();
        $('.field-validation-error').empty();
        self.isRemoveStudentDialogOpen(true);
    };
    self.closeRemoveStudentDialog = function () {
        self.isRemoveStudentDialogOpen(false);
    };
    self.submitRemoveStudentDialog = function (form) {
        $(form).validate();
        if ($(form).valid() == true) {
            $.ajax({
                type: "POST",
                url: "/Administration/Students/Delete",
                data: $(form).serialize()
            }).done(function (result) {
                if (result === true) {
                    self.loadList();
                    self.showSuccessMessage("Zmiany zostały zapisane");
                    self.isRemoveStudentDialogOpen(false);
                } else {
                    $(form).find('.delete-message').html(result);
                }
            });
        }
    };
    
    // remove student dialog
    self.isImportStudentsDialogOpen = ko.observable(false);
    self.showImportStudentsDialog = function () {
        $('.import-message').empty();
        $('.field-validation-error').empty();
        self.isImportStudentsDialogOpen(true);
    };
    self.closeImportStudentsDialog = function () {
        self.isImportStudentsDialogOpen(false);
    };

    self.init = function() {
        self.loadList();
    };

    self.init();
}

ko.applyBindings(new StudentsViewModel());