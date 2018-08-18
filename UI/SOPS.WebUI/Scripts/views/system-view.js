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

function SystemViewModel() {
    // properties
    var self = this;

    self.templateId = ko.observable();
    self.documentId = ko.observable();
    
    // remove template dialog
    self.isRemoveTemplateDialogOpen = ko.observable(false);
    self.showRemoveTemplateDialog = function () {
        $('.delete-message').empty();
        $('.field-validation-error').empty();
        self.isRemoveTemplateDialogOpen(true);
    };
    self.closeRemoveTemplateDialog = function () {
        self.isRemoveTemplateDialogOpen(false);
    };
    
    self.removeTemplate = function (element, e) {
        var id = $(e.target).attr('data-itemid');
        self.templateId(id);
        self.showRemoveTemplateDialog();
    };
    

    // remove Document dialog
    self.isRemoveDocumentDialogOpen = ko.observable(false);
    self.showRemoveDocumentDialog = function () {
        $('.delete-message').empty();
        $('.field-validation-error').empty();
        self.isRemoveDocumentDialogOpen(true);
    };
    self.closeRemoveDocumentDialog = function () {
        self.isRemoveDocumentDialogOpen(false);
    };

    self.removeDocument = function (element, e) {
        var id = $(e.target).attr('data-itemid');
        self.documentId(id);
        self.showRemoveDocumentDialog();
    };
    
    // upload Document dialog
    self.isUploadDocumentDialogOpen = ko.observable(false);
    self.showUploadDocumentDialog = function () {
        $('.delete-message').empty();
        $('.field-validation-error').empty();
        self.isUploadDocumentDialogOpen(true);
    };
    self.closeUploadDocumentDialog = function () {
        self.isUploadDocumentDialogOpen(false);
    };

    self.uploadDocument = function (element, e) {
        self.showUploadDocumentDialog();
    };
    
    // upload template dialog
    self.isUploadTemplateDialogOpen = ko.observable(false);
    self.showUploadTemplateDialog = function () {
        $('.delete-message').empty();
        $('.field-validation-error').empty();
        self.isUploadTemplateDialogOpen(true);
    };
    self.closeUploadTemplateDialog = function () {
        self.isUploadTemplateDialogOpen(false);
    };

    self.uploadTemplate = function (element, e) {
        self.showUploadTemplateDialog();
    };

    // remove students dialog
    self.isRemoveStudentsDialogOpen = ko.observable(false);
    self.showRemoveStudentsDialog = function () {
        $('.delete-message').empty();
        $('.field-validation-error').empty();
        self.isRemoveStudentsDialogOpen(true);
    };
    self.closeRemoveStudentsDialog = function () {
        self.isRemoveStudentsDialogOpen(false);
    };

    self.removeStudents = function (element, e) {
        self.showRemoveStudentsDialog();
    };
}

ko.applyBindings(new SystemViewModel());