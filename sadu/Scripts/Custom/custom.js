$(function () {

    $('#submissionDeadline').datetimepicker({ format: 'm/d/Y H:m' });

    /*
        ACTION LISTENERS
    */

    //resets form fields if modal is closed
    $('[id$=Modal]').on('hidden.bs.modal', function () {
        $(this).find('form').trigger('reset');
    });

});
/*
    for loading gif to slowly disappear
*/
$(window).load(function () {
    $('#loading-gif').fadeOut(1000);
});