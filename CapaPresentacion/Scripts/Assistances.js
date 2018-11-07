//Scripts para el index
var solicitudes = [];
var solString;
var IdCourse;
var IdSubject;
$("#callStudents").click(function (e) {
    //$("#testForm").valid();
    IdCourse = $('#IdCourse').val();
    IdSubject = $('#IdSubject').val();
    if (IdCourse === undefined || IdCourse === "") {
        toastr.warning("Seleccione un Curso");
    }
    if (IdSubject === undefined || IdSubject === "") {
        toastr.warning("Seleccione una materia");
    }
    if (IdCourse !== "" && IdSubject !== "") {
        e.preventDefault;
        console.log($("#divConsulta").val());
        $.ajax({
            type: 'Get',
            url: "Students",
            data: { idCourse: IdCourse },
            cache: false
        }).then(function (html) {
            $("#divConsulta").html(html);
        });
    }
});
function SaveAssistances(elem) {
    IdCourse = $('#IdCourse').val();
    IdSubject = $('#IdSubject').val();
    if (IdCourse === undefined || IdCourse === "") {
        toastr.warning("Seleccione un Curso");
    }
    if (IdSubject === undefined || IdSubject === "") {
        toastr.warning("Seleccione una materia");
    }
    if (IdCourse !== "" && IdSubject !== "") {
        $.ajax({
            type: "POST",
            url: "SaveAssistances",
            //url: "/ControllerHelper/SaveClasses",
            data: { ids: solicitudes, idCourse: IdCourse, idSubject: IdSubject },
            success: function (result) {
                submit();
            },
        });
    }
}
function checkboxcheck(id) {
    var name = "chkSol" + id;
    var check = document.getElementById(name);

    if (check.checked) {
        solicitudes.push(id);
    }
    else {
        var pos1 = solicitudes.indexOf(id);
        solicitudes.splice(pos1, 1);
    }
    solString = solicitudes.toString();
    console.log(id);
}
//$("#saveAssistance").click(function (e) {
//    //$("#testForm").valid();
//    var table = $('#tableStudents').val();
//    var assistance = $('#tableStudents').val();

//    if (table !== "" && IdSubject !== "") {
//        e.preventDefault;
//        console.log($("#divConsulta").val());
//        $.ajax({
//            type: 'Get',
//            url: "SaveAssistances",
//            data: { idCourse: IdCourse },
//            cache: false
//        }).then(function (html) {
//            $("#divConsulta").html(html);
//        });
//    }
//});

