$(function () {
    var activeEditor = null;
    $("#userHashTable #hash").click(function (e) {
        if (activeEditor != null) {
            submitEditor();
            activeEditor.closest("#hash").html(activeEditor.val());
            activeEditor = null;
        }

        var target = $(e.target);
        var hashValue = target.html();
        target.empty();
        activeEditor = $("<input class='form-control' style='max-width: 100%' placeholder='Enter biometric hash' type='text' value='" + hashValue + "' />")
            .appendTo(target)
            .focus();
    });

    function submitEditor() {
        var username = $("input[type=hidden]", activeEditor.closest("tr")).val();
        var hash = activeEditor.val();
        $.post("Biometric/UserHash?userName=" + username + "&hash=" + hash);
    }
});