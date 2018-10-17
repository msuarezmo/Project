
$("#btnBuscar").click(function () {
    filterUsers();
});
function filterUsers() {
    var sortOrder = $('#IdTeacher').val();
    var currentFilter = $('#IdCourse').val();
    var searchString = $('#IdDay').val();
    var page = $('#IdSubject').val();
    var rol = $('#IdSubject').val();
    UsersRequest.filterUsers(sortOrder, currentFilter, searchString, page, rol).done(function (data) {


    }).fail(function (jqXHR, textStatus, errorThrown) {
        $.UifNotify('show', { 'type': 'info', 'message': "test", 'autoclose': true });
    });

}
