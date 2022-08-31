var dataTable;
var l;
$(function () {
    l = abp.localization.getResource('EmailMaketing');

    dataTable = $('#LetterTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: true,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(emailMaketing.contentEmails.contentEmail.getList),
            columnDefs: [
                {
                    title: l('No'),
                    data: "stt"
                },
                {
                    title: l('User'),
                    data: "customerName"
                },
                {
                    title: l('Email'),
                    data: "senderEmail",
                },
                {
                    title: l('Subject'),
                    data: "subject",

                },
                {
                    title: l('Body'),
                    data: "bodyShow"
                },
                {
                    title: l('Schedule'), data: "schedule",
                    render: function (data) {
                        if (data==null) {
                            return "";
                        }
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toLocaleString(luxon.DateTime.DATETIME_SHORT);
                    }
                },
                {
                    title: l('Status'),
                    data: "statusSend"
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
                                    text: l('Delete'),
                                    iconClass: "fa fa-trash-o",
                                    visible: abp.auth.isGranted('EmailMaketing.ContentEmails.Delete'),
                                    confirmMessage: function (data) {
                                        return l('Deleting Content Email', data.record.name);
                                    },
                                    action: function (data) {
                                        emailMaketing.contentEmails.contentEmail
                                            .delete(data.record.id)
                                            .then(function (data) {
                                                if (data) {
                                                    abp.notify.info(l('Successfully Deleted'));
                                                    dataTable.ajax.reload();
                                                } else{
                                                    abp.message.error(l("Delete Failed"));
                                                } 
                                            });
                                    }
                                },
                            ]
                    }
                }
            ]
        })
    );
});