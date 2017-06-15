function uploadFile(input) {

    var submissionId = input.id.substr(-1);

    var files = new FormData();
    //also submit id for reference
    files.append('Id', submissionId);

    $.each($(input).get(0).files, function (i, file) {
        files.append(file.name, file);
        console.log(file.name);
    });

    $.ajax({
        type: "post",
        url: "Submissions/Submit",
        data: files,
        contentType: false,
        processData: false,
        success: function (data) {
            console.log(data.Message);
            $('#' + submissionId + ' .progress').addClass("hidden");
            $('#' + submissionId + ' .btn-browse').toggle();
            updateUploadsPartialView(submissionId);
        },
        error: function (xhr) {
            console.log("Upload request error.");
            $('#' + submissionId + ' .progress').addClass("hidden");
            $('#' + submissionId + ' .btn-browse').toggle();
        },
        beforeSend: function () {
            $('#' + submissionId + ' .btn-browse').toggle();
            $('#' + submissionId + ' .progress').removeClass("hidden");
        },
        complete: function () {
            $(input).replaceWith($(input).val('').clone(true));
        }
    });
}

function storeTempImage(input) {

    var files = new FormData();

    $.each($(input).get(0).files, function (i, file) {
        files.append(file.name, file);
        console.log(file.name);
    });

    $.ajax({
        type: "post",
        url: "Organizations/storeTempImage",
        data: files,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data["Message"]) {
                console.log(date["Message"]);
            } else {
                $(".org-img-preview").prop("src", data);
            }
        },
        error: function (xhr) {
            console.log("Upload to temp error: " + xhr.responseText);
        }
    });
}