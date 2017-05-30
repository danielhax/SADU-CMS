$(function () {
    $('#deadlinePicker').datetimepicker({ format: 'm/d/Y H:m' });

    /*
        ACTION LISTENERS
    */

    //resets form fields if modal is closed
    $('[id$=Modal]').on('hidden.bs.modal', function () {
        $(this).find('form').trigger('reset');
    });
});