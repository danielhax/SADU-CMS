$(function () {

    $('#submissionDeadline').datetimepicker({ format: 'm/d/Y H:m' });

    /*
        ACTION LISTENERS
    */

    //resets form fields if modal is closed
    $('[id$=Modal]').on('hidden.bs.modal', function () {
        $(this).find('form').trigger('reset');
    });

    //admin toggle archived/non-archived submission view
    $("#toggleArchiveBtn").click(function () {

        if ($("#nonArchivedDiv").hasClass("hidden")) {
            /*
                archived to non-archived
            */
            $("#archivedDiv").addClass("hidden");

            $("#toggleArchiveBtn").text("View Archive");
            $("#toggleArchiveBtn").css("color", "black");
            $("#toggleArchiveBtn").css("background-color", "transparent");

            $("#nonArchivedDiv").removeClass("hidden");
        } else {
            $("#archivedDiv").removeClass("hidden");

            $("#toggleArchiveBtn").text("View Pending");
            $("#toggleArchiveBtn").css("color", "white");
            $("#toggleArchiveBtn").css("background-color", "#0035a7");
            

            $("#nonArchivedDiv").addClass("hidden");
        }
    });

});
/*
    for loading gif to slowly disappear
*/
$(window).load(function () {
    $('#loading-gif').fadeOut(1000);
});

//functions

function updateImagePreview(image) {
    $(".org-img-preview").attr('src', image);
}