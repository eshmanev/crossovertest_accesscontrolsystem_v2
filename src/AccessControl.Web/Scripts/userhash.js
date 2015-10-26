$(function () {
    var activeEditor = null;

    function submitEditor() {
        var username = $("#userName", activeEditor.closest("tr")).html();
        var hash = activeEditor.val();
        $.post("Biometric/UserHash?userName=" + username + "&hash=" + hash, function (response) {
            if (response.Fault != null)
                alert(response.Fault.Summary);
        });
    }

    function releaseEditor() {
        if (activeEditor != null) {
            submitEditor();
            activeEditor.closest("#hash").html(activeEditor.val());
            activeEditor = null;
        }
    }

    $("#userHashTable #hash:not(.active)").click(function (e) {
        if (activeEditor != null)
            return;
        
        var target = $(e.target);
        var hashValue = target.html();
        target.empty();
        activeEditor = $("<input class='form-control' style='max-width: 100%' placeholder='Enter biometric hash' type='text' value='" + hashValue + "' />")
            .appendTo(target)
            .focus();
    });

    $(document).keypress(function (e) {
        if (e.which === 13) {
            releaseEditor();
        }
    });
});