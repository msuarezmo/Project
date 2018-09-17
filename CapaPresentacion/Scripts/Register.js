
$("#Name").prop('disabled', true);
$('#Roles').on('change', changeRol);

function changeRol() {
    var rol = $("#Roles").val();
    if (rol != "") {
        $("#Name").prop('disabled', false);
    }
    else {
        $("#Name").prop('disabled', true);
    }
}

