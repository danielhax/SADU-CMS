function uploadFile(input) {

    var submissionId = input.id.substr(-1);

    console.log("Entered function. Id: " + submissionId);

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