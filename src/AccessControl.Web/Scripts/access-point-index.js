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

    function onSiteChanged() {
        var site = $("#siteSelector option:selected").text();
        var departments = $("input[site=" + site + "]").val();

        clearDepartments();
        fillDepartments(departments);
    }

    $("#siteSelector").change(onSiteChanged);
})