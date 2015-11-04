$(function() {
    $("#Editor_UserName").change(function (){
        $("#Editor_UserGroupName option").first().prop("selected", true);
    });

    $("#Editor_UserGroupName").change(function () {
        $("#Editor_UserName option").first().prop("selected", true);
    });
})