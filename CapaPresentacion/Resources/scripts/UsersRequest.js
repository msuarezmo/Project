class UsersRequest {

    static filterUsers(sortOrder, currentFilter, searchString, page, rol) {
        return $.ajax({
            type: "POST",
            url: '@Url.Action("../Schedules/Index")',
            data: JSON.stringify({ sortOrder: sortOrder, currentFilter: currentFilter, searchString: searchString, page: page, rol: rol }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}