$(function () {
    $('#loginForm').submit(function (e) {
        e.preventDefault();
        console.log('asd');
        $.ajax({
            type: "post",
            url: "Login/LoginRequest",
            data: $(this).serialize(),
            success: function (data) {
                if (!data) {
                    $('#loginAlert').removeClass('hidden');
                    $('#loginAlert').text("User not found!");
                }
            },
            error: function (xhr) {
                alert(xhr.responseText + "lmaooo");
            }
        });
    });
});