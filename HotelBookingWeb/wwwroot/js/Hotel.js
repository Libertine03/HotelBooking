var dataTable;

$(document).ready(function () {
    loadHotelsDataTable();
    loadRoomsDataTable();
});

function loadHotelsDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/hotels/getall' },
        "language": {
            "search": "Поиск:",
            "zeroRecords": "По вашему запросу не найдены записи.",
            "info": "Информация об отелях",
            "infoEmpty": "Информация об отелях",
            "infoFiltered": "(поиск из _MAX_ записей)",
            "emptyTable": "Нет записей",
            "lengthMenu": "Показывать _MENU_ записей на странице",
            "loadingRecords": "Загрузка записей...",
            "processing": "В процессе...",
            "paginate": {
                "next": "Вперёд",
                "previous": "Назад",
                "first": "Первая",
                "last": "Последняя"
            },
        },

        "columns": [
            { data: 'name', "width": "25%" },
            { data: 'country', "width": "15%" },
            { data: 'city', "width": "10%" },
            { data: 'street', "width": "25%" },
            {
                data: 'id', 
                "render": function (data) {
                    return `<a href="/admin/hotels/upsert?id=${data}" class="btn btn-info mx-2"> <i class="bi bi-pencil-square"></i></a>
                                <a onClick=Delete('/admin/hotels/delete/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i></a>
                                <a href="/admin/rooms/index?hotelId=${data}" class="btn btn-success mx-2"> <i class="bi bi-hospital"></i></a>`
                },
                "width": "25%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Вы уверены?",
        text: "Вы не сможете обратить это действие!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        cancelButtonText: "Назад",
        confirmButtonText: "Да, удалить!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();

                    if (data.message === 'Комната успешно удалена') {
                        location.reload(true);
                    }

                    toastr.success(data.message);
                }
            })
        }
    });
}
