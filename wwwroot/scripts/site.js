/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="../typings/gijgo/gijgo.d.ts" />
var SiteViewModel = (function () {
    function SiteViewModel(model) {
        var _this = this;
        this.model = model;
        this.grid = $('#grid').grid({
            primaryKey: 'ID',
            columns: [
                { field: 'Name' },
                { field: 'IsCompleted', title: 'Done?', width: 70, align: 'center' },
                { title: '', field: 'Edit', width: 32, align: 'center', type: 'icon', icon: 'glyphicon glyphicon-pencil', tooltip: 'Edit', events: { 'click': function (e) { return _this.edit(e); } } },
                { title: '', field: 'Delete', width: 32, align: 'center', type: 'icon', icon: 'glyphicon glyphicon-remove', tooltip: 'Delete', events: { 'click': function (e) { return _this.remove(e); } } }
            ],
            pager: { limit: 10 },
            uiLibrary: 'bootstrap'
        });
        this.grid.on('cellDataBound', function (e, $wrapper, id, column, record) {
            var $checkbox;
            if ("IsCompleted" === column.field) {
                $wrapper.off('click').empty();
                $checkbox = $("<input type=\"checkbox\" />");
                $checkbox.prop('checked', record.IsCompleted);
                $checkbox.on('click', function () { return _this.changeStatus(record.ID, !record.IsCompleted); });
                $wrapper.append($checkbox);
            }
        });
        this.dialog = $('#todoItemModel').dialog({
            autoOpen: false,
            title: 'TODO List',
            width: 400,
            height: 220,
            uiLibrary: 'bootstrap'
        });
        $('#btnAddPlayer').click(function () { return _this.add(); });
        $('#btnSave').click(function () { return _this.save(); });
        $('#btnSearch').click(function () { return _this.search(); });
    }
    SiteViewModel.prototype.add = function () {
        $('#itemId').val('');
        $('#name').val('');
        this.dialog.open();
    };
    SiteViewModel.prototype.edit = function (e) {
        $('#itemId').val(e.data.id);
        $('#name').val(e.data.record.Name);
        this.dialog.open();
    };
    SiteViewModel.prototype.remove = function (e) {
        var _this = this;
        this.model.remove(e.data.id, function (response) { _this.grid.reload(); }, function () { alert('Unable to remove.'); });
    };
    SiteViewModel.prototype.save = function () {
        var _this = this;
        var item = {
            ID: $('#itemId').val(),
            Name: $('#name').val()
        }, onDone = function (response) {
            _this.grid.reload();
            _this.dialog.close();
        }, onFail = function (response) {
            alert('Unable to save.');
            _this.dialog.close();
        };
        this.model.save(item, onDone, onFail);
    };
    SiteViewModel.prototype.search = function () {
        this.grid.reload({ searchString: $('#search').val() });
    };
    SiteViewModel.prototype.changeStatus = function (id, status) {
        var _this = this;
        var item = {
            ID: id,
            IsCompleted: status
        }, onDone = function (response) {
            _this.grid.reload();
        }, onFail = function (response) {
            alert('Unable to save.');
        };
        this.model.save(item, onDone, onFail);
    };
    return SiteViewModel;
}());
var SiteModel = (function () {
    function SiteModel() {
    }
    SiteModel.prototype.save = function (item, onDone, onFail) {
        $.ajax({ url: 'Home/Save', type: 'POST', data: { todoItem: item } }).done(onDone).fail(onFail);
    };
    SiteModel.prototype.remove = function (id, onDone, onFail) {
        $.ajax({ url: 'Home/Remove', type: 'POST', data: { id: id } }).done(onDone).fail(onFail);
    };
    return SiteModel;
}());
$(document).ready(function () {
    new SiteViewModel(new SiteModel());
});
