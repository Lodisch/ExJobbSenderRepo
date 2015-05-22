$(function () {
    $("#postMsgBtn").click(function () {
        
        $.ajax({
            type: "POST",
            url: '/Announcements/Announcement',
            data: $('#messageForm').serialize(),
            datatype: "html",
            success: function (data) {
                $('#display').html(data);
            }
        });
    });
});