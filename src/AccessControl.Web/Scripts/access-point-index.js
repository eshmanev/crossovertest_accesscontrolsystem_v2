$(function () {
    function appendDepartment(departmentName) {
        $('#departmentSelector').append($("<option></option>").attr("value", departmentName).text(departmentName));
    }

    function clearDepartments() {
        var select = $("#departmentSelector")[0];
        while (select.options.length > 0)
            select.remove(0);

        appendDepartment("--");
    }

    function fillDepartments(departments) {
        if (departments == null)
            return;

        var departmentArray = departments.split(";");
        $.each(departmentArray, function (key, value) {
            appendDepartment(value);
        });
    }

    function guid() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
              .toString(16)
              .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    }

    function onSiteChanged() {
        var site = $("#siteSelector option:selected").text();
        var departments = $("input[site=" + site + "]").val();

        clearDepartments();
        fillDepartments(departments);
    }

    function onNewGuidClick() {
        $("#Editor_AccessPointId").val(guid());
    }

    $("#siteSelector").change(onSiteChanged);
    $("#newGuid").click(onNewGuidClick);
})