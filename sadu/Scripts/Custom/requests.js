$(function () {

    //submit on clicks
    //$('#loginForm').submit(function (e) {
    //    e.preventDefault();
    //    console.log('asd');
    //    $.ajax({
    //        type: "post",
    //        url: "Login/LoginRequest",
    //        data: $(this).serialize(),
    //        success: function (data) {
    //            if (!data) {
    //                $('#loginAlert').removeClass('hidden');
    //                $('#loginAlert').text("User not found!");
    //            }
    //        },
    //        error: function (xhr) {
    //            alert("Error" + xhr.responseText);
    //        }
    //    });
    //});

    $('#createSubmissionForm').submit(function (e) {
        e.preventDefault();
        console.log("create submission request");

        $.ajax({
            type: "post",
            url: "Submissions/Create",
            data: $(this).serialize(),
            success: function () {

            }
        });
    });

    //functions
    //function initializeSubmissions(model) {
    //    $.ajax({
    //        type: "post",
    //        url: "Functions/getPendingSubmissions",
    //        data: model,
    //        success: function (data) {
    //             return data;
    //        },
    //        error: function (xhs) {
    //            alert(xhs.responseText);
    //        }
    //    });
    //}
});