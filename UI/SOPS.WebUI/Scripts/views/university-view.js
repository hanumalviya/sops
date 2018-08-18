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

function UniversityViewModel() {
    // properties
    var self = this;
    self.courseId = ko.observable(0);
    self.departmentId = ko.observable(0);
    self.deparmentName = ko.observable('');

    // add course dialog
    self.isAddCourseDialogOpen = ko.observable(false);
    self.showAddCourseDialog = function () {
        $('.delete-message').empty();
        $('.field-validation-error').empty();
        self.isAddCourseDialogOpen(true);
    };
    self.closeAddCourseDialog = function () {
        self.isAddCourseDialogOpen(false);
    };

    self.addCourse = function (element, e) {
        var id = $(e.target).attr('data-itemid');
        self.departmentId(id);
        self.showAddCourseDialog();
    };
    
    // add department dialog
    self.isAddDepartmentDialogOpen = ko.observable(false);
    self.showAddDepartmentDialog = function () {
        $('.delete-message').empty();
        $('.field-validation-error').empty();
        self.isAddDepartmentDialogOpen(true);
    };
    self.closeAddDepartmentDialog = function () {
        self.isAddDepartmentDialogOpen(false);
    };

    self.addDepartment = function (element, e) {
        self.showAddDepartmentDialog();
    };
    
    // edit department dialog
    self.isEditDepartmentDialogOpen = ko.observable(false);
    self.showEditDepartmentDialog = function () {
        $('.delete-message').empty();
        $('.field-validation-error').empty();
        self.isEditDepartmentDialogOpen(true);
    };
    self.closeEditDepartmentDialog = function () {
        self.isEditDepartmentDialogOpen(false);
    };

    self.editDepartment = function (element, e) {
        var id = $(e.target).attr('data-itemid');
        var name = $(e.target).attr('data-name');
        self.deparmentName(name);
        self.departmentId(id);
        self.showEditDepartmentDialog();
    };

    // remove course dialog
    self.isRemoveCourseDialogOpen = ko.observable(false);
    self.showRemoveCourseDialog = function () {
        $('.delete-message').empty();
        $('.field-validation-error').empty();
        self.isRemoveCourseDialogOpen(true);
    };
    self.closeRemoveCourseDialog = function () {
        self.isRemoveCourseDialogOpen(false);
    };
    
    self.removeCourse = function (element, e) {
        var id = $(e.target).attr('data-itemid');
        self.courseId(id);
        self.showRemoveCourseDialog();
    };
    

    // remove department dialog
    self.isRemoveDepartmentDialogOpen = ko.observable(false);
    self.showRemoveDepartmentDialog = function () {
        $('.delete-message').empty();
        $('.field-validation-error').empty();
        self.isRemoveDepartmentDialogOpen(true);
    };
    self.closeRemoveDepartmentDialog = function () {
        self.isRemoveDepartmentDialogOpen(false);
    };

    self.removeDepartment = function (element, e) {
        var id = $(e.target).attr('data-itemid');
        self.departmentId(id);
        self.showRemoveDepartmentDialog();
    };
}

ko.applyBindings(new UniversityViewModel());