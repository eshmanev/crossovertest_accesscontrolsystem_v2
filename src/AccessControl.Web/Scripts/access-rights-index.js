$(function() {
    $("#Editor_UserName").change(function (){
        $("#Editor_UserGroupName option").first().prop("selected", true);
    });

    $("#Editor_UserGroupName").change(function () {
        $("#Editor_UserName option").first().prop("selected", true);
    });

    $("#accessRightsEditor #toggleSchedule").click(function(event) {
        var hidden = $("#scheduleApplied");
        if (hidden.val().toLowerCase() === "true") {
            hidden.val("false");
            $("#schedule").addClass("hidden");
            $("#toggleSchedule").html("Show schedule");
        } else {
            hidden.val("true");
            $("#schedule").removeClass("hidden");
            $("#toggleSchedule").html("Hide schedule");
        }
        
        event.preventDefault();
    });
})