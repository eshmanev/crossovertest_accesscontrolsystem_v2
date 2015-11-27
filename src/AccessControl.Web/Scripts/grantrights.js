$(function () {
    function submit(href, callback) {
        $.post($(event.target).attr("href"), function(response) {
            if (response.Fault != null)
                alert(response.Fault.Summary);
            else
                callback();
        });
    }

    function onGrantButtonClick(event) {
        event.preventDefault();
        submit($(event.target).attr("href"), function () {
            var row = $(event.target).closest("tr");
            $("td:first-child span", row).removeClass("sr-only");
        });
    }

    function onRevokeButtonClick(event) {
        event.preventDefault();
        submit($(event.target).attr("href"), function () {
            var row = $(event.target).closest("tr");
            $("td:first-child span", row).addClass("sr-only");
        });
    }

    $(".grant-button").click(onGrantButtonClick);
    $(".revoke-button").click(onRevokeButtonClick);
});