/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="../typings/gijgo/gijgo.d.ts" />

interface ITODOItem {
    ID: number;
    Name?: string;
    IsCompleted?: boolean;
}

class SiteViewModel {

    private model: SiteModel;
    private grid: Gijgo.Grid<ITODOItem, any>;
    private dialog: Gijgo.Dialog;

    constructor(model: SiteModel) {
        this.model = model;

        this.grid = $('#grid').grid({
            primaryKey: 'ID',
            columns: [
                { field: 'Name' },
                { field: 'IsCompleted', title: 'Done?', width: 70, align: 'center' },
                { title: '', field: 'Edit', width: 32, align: 'center', type: 'icon', icon: 'glyphicon glyphicon-pencil', tooltip: 'Edit', events: { 'click': (e) => this.edit(e) } },
                { title: '', field: 'Delete', width: 32, align: 'center', type: 'icon', icon: 'glyphicon glyphicon-remove', tooltip: 'Delete', events: { 'click': (e) => this.remove(e) } }
            ],
            pager: { limit: 10 },
            uiLibrary: 'bootstrap'
        });
        this.grid.on('cellDataBound', (e, $wrapper, id, column, record) => {
            var $checkbox;

            if ("IsCompleted" === column.field) {
                $wrapper.off('click').empty();
                $checkbox = $("<input type=\"checkbox\" />");
                $checkbox.prop('checked', record.IsCompleted);
                $checkbox.on('click', () => this.changeStatus(record.ID, !record.IsCompleted))
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

        $('#btnAddPlayer').click(() => this.add());
        $('#btnSave').click(() => this.save());
        $('#btnSearch').click(() => this.search());
    }

    add() {
        $('#itemId').val('');
        $('#name').val('');
        this.dialog.open();
    }

    edit(e: any) {
        $('#itemId').val(e.data.id);
        $('#name').val(e.data.record.Name);
        this.dialog.open();
    }

    remove(e: any) {
        this.model.remove(e.data.id, (response) => { this.grid.reload(); }, function () { alert('Unable to remove.'); });
    }

    save() {
        var item: ITODOItem = {
                ID: $('#itemId').val(),
                Name: $('#name').val()
            },
            onDone = (response) => {
                this.grid.reload();
                this.dialog.close();
            },
            onFail = (response) => {
                alert('Unable to save.');
                this.dialog.close();
            };
        this.model.save(item, onDone, onFail);
    }

    search() {
        this.grid.reload({ searchString: $('#search').val() });
    }

    changeStatus(id: number, status: boolean) {
        var item: ITODOItem = {
                ID: id,
                IsCompleted: status
            },
            onDone = (response) => {
                this.grid.reload();
            },
            onFail = (response) => {
                alert('Unable to save.');
            };
        this.model.save(item, onDone, onFail);
    }
}

class SiteModel {
    save(item: ITODOItem, onDone, onFail) {
        $.ajax({ url: 'Home/Save', type: 'POST', data: { todoItem: item } }).done(onDone).fail(onFail);
    }

    remove(id: number, onDone, onFail) {
        $.ajax({ url: 'Home/Remove', type: 'POST', data: { id: id } }).done(onDone).fail(onFail);
    }
}

$(document).ready(function () {
    new SiteViewModel(new SiteModel());
});