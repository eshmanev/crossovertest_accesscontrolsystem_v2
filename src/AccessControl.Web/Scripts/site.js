$(function () {
    $('.date input').datetimepicker();

    $('.time').datetimepicker({
        pickDate: false,
        format: 'HH:mm:ss',
        pickDate: false,
        pickSeconds: false,
        pick12HourFormat: false
    });
});