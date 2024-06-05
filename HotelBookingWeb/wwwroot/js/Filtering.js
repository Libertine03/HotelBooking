
$(document).ready(function () {

})

function Search() {
    $('#searchButton').click(function () {
    })
    $.ajax({
        type: "GET",
        url: 'Customer/Home/GetFiltered',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response) {
            var hotels = $('hotelsList');
            hotels.empty();

            $.each(response.d, function (index, item) {
                hotels.append("<p>" + item + "</p>");
            });
        },

        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}