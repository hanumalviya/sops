function CompanyViewModel() {
    // properties
    var self = this;
    
    self.setup = function () {
        var a = $("a[data-search]");
        a.removeAttr("href");
    };

    self.selectPage = function (data, e) {
        var link = $(e.target);
        var search = link.attr('data-search');
        var page = link.attr('data-page');
        var url = "/Company/Grid/?Search=" + search + "&Page=" + page;
        
        $.ajax({
            type: "GET",
            url: url,
            dataType: "html"
        }).done(function(html) {
            $(".gridContainer").html(html);
            self.setup();
            ko.applyBindings(self);
        });
    };

    self.setup();
}

ko.applyBindings(new CompanyViewModel());