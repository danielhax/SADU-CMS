$(function () {

    //submit on clicks
    $('#loginForm').submit(function (e) {
        e.preventDefault();
        console.log('asd');
        $.ajax({
            type: "post",
            url: "Session/Login",
            data: $(this).serialize(),
            success: function (data) {
                if (!data) {
                    $('#loginAlert').removeClass('hidden');
                    $('#loginAlert').addClass('alert-danger');
                    $('#loginAlert').text("User not found!");
                }
                else {
                    window.location.href = data;
                }
            },
            error: function (xhr) {
                alert("Error" + xhr.responseText);
            }
        });
    });

    $('#createSubmissionForm').submit(function (e) {
        e.preventDefault();
        console.log("create submission request");

        

        $.ajax({
            type: "post",
            url: "Submissions/Create",
            data: submissionObject($(this).serializeArray()),
            success: function (data) {
                $("#createSubmissionModal").modal("toggle");
                //updateSubmissionsPartialView();
                console.log(data);
            },
            error: function (xhs) {
                console.log("error" + xhs.responseText);
            }
        });
    });

    //functions
    function updateSubmissionsPartialView() {
        $.ajax({
            type: "get",
            url: "Submissions/GetSubmissions",
            success: function (partialView) {
                $("#submissionPartial").html(partialView);
            },
            error: function (xhs) {
                console.log(xhs.responseText);
            }
        });
    }

    //an object based on the model is needed to be able to be processed by the controller
    function submissionObject(formData) {
        var obj = new Object();
        $.each(formData, function (index, item) {
            switch (item.name) {
                case "__RequestVerificationToken":
                    obj.__RequestVerificationToken = item.value;
                    break;
                case "submissionOrganization":
                    obj.Organization = item.value;
                    break;
                case "submissionTitle":
                    obj.title = item.value;
                    break;
                case "submissionDetails":
                    obj.details = item.value;
                    break;
                case "deadlinePicker":
                    obj.date_created = item.value;
                    break;
                default:
                    return null;
            }
        });

        return obj;
    }
});