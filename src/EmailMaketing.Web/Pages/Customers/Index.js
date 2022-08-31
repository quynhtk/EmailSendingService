var dataTable;
var l;
$(function () {
    l = abp.localization.getResource('EmailMaketing');
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Customers/CreateModal',
        scriptUrl: '/Pages/Customers/CreateCusotmer.js'
    });

    /*viewUrl: abp.appPath + 'Categories/CreateModal',
        scriptUrl : '/Pages/Categories/Create.js'*/
    var editModal = new abp.ModalManager(abp.appPath + 'Customers/EditModal');
    var resetPasswordModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Customers/ResetPassword',
        scriptUrl: '/Pages/Customers/CreateCusotmer.js'
    });
    var editRole = new abp.ModalManager(abp.appPath + 'Customers/EditRoleModal')

    dataTable = $('#CustomerTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: true,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(emailMaketing.customers.customer.getList),
            columnDefs: [
                {
                    title: l('No'),
                    data: "stt"
                },
                {
                    title: l('User Name'),
                    data: "userName"
                },
                {
                    title: l('Full Name'),
                    data: "fullName",
                },
                {
                    title: l('Phone Number'),
                    data: "phoneNumber",

                },
                {
                    title: l('Email'),
                    data: "email"
                },
                {
                    title: l('Type'),
                    data: "type"
                },
                {
                    "orderable": false,
                    title: l('Status'),
                    data: { status: "status", id: "id" },
                    render: function (data) {

                        var check = '';
                        if (data.status == 1)
                            check = "checked";
                        var str = '<label class="switch">' +
                            `<input type = "checkbox" id="${data.id}" ${check} onclick="ChangeStatus(this.id,${data.status})">` +
                            '<span class="slider round"></span>' +
                            '</label >';
                        return str;
                    }
                },
                {
                    title: l('Creation Time'), data: "creationTime",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toLocaleString(luxon.DateTime.DATETIME_SHORT);
                    }
                },
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Edit Roles'),
                                    iconClass: "fa fa-user-circle-o",
                                    /*visible: abp.auth.isGranted('EmailMaketing.Customers.Edit'),*/
                                    action: function (data) {
                                        editRole.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Edit'),
                                    iconClass: "fa fa-pencil-square-o",
                                    visible: abp.auth.isGranted('EmailMaketing.Customers.Edit'),
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    iconClass: "fa fa-trash-o",
                                    visible: abp.auth.isGranted('EmailMaketing.Customers.Delete'),
                                    confirmMessage: function (data) {
                                        return l('Deleting Customer', data.record.name);
                                    },
                                    action: function (data) {
                                        emailMaketing.customers.customer
                                            .delete(data.record.id)
                                            .then(function (data) {
                                                if (data== "Ok") {
                                                    abp.notify.info(l('Successfully Deleted'));
                                                    dataTable.ajax.reload();
                                                } else if (data == "Customer have data with Content Email") {
                                                    abp.message.error(l("Customer have data with Content Email"));
                                                }else {
                                                    abp.message.error(l("Customer have data with Sender Email"));
                                                }

                                            });
                                    }
                                },
                                {
                                    text: l('Reset Password'),
                                    iconClass: "fa fa-key",
                                    /*visible: abp.auth.isGranted('EmailMaketing.Customers.Edit'),*/
                                    action: function (data) {
                                        resetPasswordModal.open({ id: data.record.id });
                                    }
                                }
                            ]
                    }
                }
            ]
        })
    );
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });
    createModal.onOpen(function () {
        $('#Customer_FullName').keypress(function (e) {
            var keyCode = e.which;
            if ((keyCode <= 48 && keyCode != 32) || (keyCode > 48 && keyCode < 65) || (keyCode > 90 && keyCode < 97) || keyCode > 122) {
                return false;
            }
        });

        $('#showpass').on('click', function () {
            var passInput = $("#Customer_Password");
            if (passInput.attr('type') === 'text') {
                passInput.attr('type', 'password');
            } else {
                passInput.attr('type', 'text');
            }
        });


    });
    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    resetPasswordModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editRole.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewCustomerButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
    $('#Customer_FullName').keypress(function (e) {
        console.log(e);
    });

});
function ChangeStatus(id, status) {
    if ($(id).is(':checked')) {
        $(id).prop("checked", false);
    }
    else {
        $(id).prop("checked", true);
    }
    dataTable.ajax.reload();
    var mess = l('Block The User');
    if (status == 0) {
        mess = l('Unlock The User');
    }
    abp.message.confirm(mess, l('Notify'))
        .then(function (confirmed) {

            if (confirmed) {
                emailMaketing.customers.customer.changeStatus(id)
                /*abp.message.success(l('Successfully'), l('Congratulations'));*/
                abp.notify.info(l('Successfully'));
                dataTable.ajax.reload();
                if ($(id).is(':checked')) {
                    $(id).prop("checked", false);
                    dataTable.ajax.reload();
                }
                else {
                    $(id).prop("checked", true);
                    dataTable.ajax.reload();
                }
                dataTable.ajax.reload();
            }
            else {
                if ($(id).is(':checked')) {
                    $(id).prop("checked", false);
                    dataTable.ajax.reload();
                }
                else {
                    $(id).prop("checked", true);
                }
                dataTable.ajax.reload();
            }
        });

};


