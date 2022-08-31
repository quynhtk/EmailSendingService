
$(function () {
    l = abp.localization.getResource('EmailMaketing');
    /*var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'EmailManagement/SenderEmails/CreateModal',
        scriptUrl: '/Pages/SenderEmails/CreateModal.js'
    });*/
    var createModal = new abp.ModalManager(abp.appPath + 'SenderEmails/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'SenderEmails/EditModal');

    var dataTable = $('#SenderEmailTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: true,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(emailMaketing.senderEmails.senderEmail.getList),
            columnDefs: [
                {
                    title: l('No'),
                    data: "stt"
                },
                {
                    title: l('Email'),
                    data: "email"
                },
                {
                    title: l('Password'),
                    data: "password"
                },
                {
                    title: l('Customer Name'),
                    data: "customerName"
                    /*render: function (data) {
                        if (data != null) return data.fullName;
                        return "";
                    }*/
                },
                {
                    title: l('IsSend'),
                    data: "isSend"
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
                                    text: l('Edit'),
                                    iconClass: "fa fa-pencil-square-o",
                                    visible: abp.auth.isGranted('EmailMaketing.SenderEmails.Edit'),
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    iconClass: "fa fa-trash-o",
                                    visible: abp.auth.isGranted('EmailMaketing.SenderEmails.Delete'),
                                    confirmMessage: function (data) {
                                        return l(
                                            'Sender Email Deletion Confirmation Message',
                                            data.record.name
                                        );
                                    },
                                    action: function (data) {
                                        emailMaketing.senderEmails.senderEmail
                                            .delete(data.record.id)
                                            .then(function (data) {
                                                if (data) {
                                                    abp.notify.info(l('Successfully Deleted'));
                                                    dataTable.ajax.reload();
                                                } else {
                                                    abp.message.error(l("Delete Failed, Email has data in Content Email"));
                                                }
                                            });
                                    }
                                }
                            ]
                    }
                }
            ]

        })
    )
    
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
    });

    $('#NewSenderEmailButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});

$(function () {
    $(document).ready(function () {
        $('input[type="file"]').change(function (e) {
            var fileName = e.target.files[0].name;
            if (fileName != null) {
                $('#ImportExcelButton').reload(document.getElementById("ImportExcelButton").disabled = false);
            }
        });
    });
});