$(document).ready(function () {
    $('#getChecked').on('click', function () {
        debugger;
        var selected = [];
        $('input:checked').each(function () {
            selected.push($(this).attr("value"));
        });
        $.ajax({
            url: "/Announcements/GetSelected",
            type: "POST",
            data:  JSON.stringify(selected) ,
            contentType: "application/json",
            dataType: "html",
            traditional: true
        }).done(function (response) {

            $('#display').html(response);

        }).fail(function (error) {
            alert("error:" + error);
        }).done(function() {
            $(document).ready(function () {
                $('#announcementForm').load('/Announcements/Announcement');
            });
        });
    });
});