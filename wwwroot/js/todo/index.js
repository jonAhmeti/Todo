$(function () {
    console.log('hi');

    //create button
    const createSubmit = $("#createSubmit");

    $(createSubmit).on('click', function () {
        const createTitle = $('#createTitle').val();
        const createDue = new Date($('#createDueDate').val());

        var test = dateFormat(createDue);
        let newTask = { Title: createTitle, DueDate: dateFormat(createDue)}
        $.ajax({
            method: 'POST',
            url: 'AddItem',
            data: newTask,
            success: function (result) {
                let jsonResult = JSON.parse(result);
                alert(jsonResult.Message);
                $('#items').append(`
                    <div class=".table-item row row-cols-7">
                        <div class="col my-2" data-container-id="${jsonResult.Object.Id}" data-item-value="${jsonResult.Object.Title}">${jsonResult.Object.Title}</div>
                        <div class="col my-2" data-container-id="${jsonResult.Object.Id}" data-item-value="${jsonResult.Object.DueDate}">${jsonResult.Object.DueDate}</div>
                        <div class="col-1 my-2 crud text-warning" name="edit" data-item-id="${jsonResult.Object.Id}"><i class="fa-solid fa-pen-to-square"></i></div>
                        <div class="col-1 my-2 crud text-danger" name="delete" data-item-id="${jsonResult.Object.Id}"><i class="fa-solid fa-trash-can"></i></div>
                    </div>`);

                setEditBtns($('div[name="edit"]'));
                setDeleteBtns($('div[name="delete"]'));
            },
            error: function (error) {
                alert(error.responseText);
            }
        });
    });

    //edit and delete buttons for each item
    var editBtns = $('div[name="edit"]');
    var deleteBtns = $('div[name="delete"]');

    setEditBtns(editBtns);
    setDeleteBtns(deleteBtns);
    console.log(deleteBtns);
    
});
function setEditBtns(btns) {
    for (var i = 0; i < btns.length; i++) {
        $(btns[i]).on('click', function () {
            const itemId = $(this).attr('data-item-id');

            edit(itemId, this);
        });
    }
}

function setDeleteBtns(btns) {
    for (var i = 0; i < btns.length; i++) {
        $(btns[i]).on('click', function () {
            const itemId = $(this).attr('data-item-id');
            deleteItem(itemId);
        });
    }
}

function deleteItem(id) {
    $.ajax({
        method: 'DELETE',
        url: `/DeleteItem/${id}`,
        success: function (result) {
            $(`div.table-item[data-row-id="${id}"]`).remove();
        },
        error: function (error) {
            alert(error.responseText);
        }
    });
}
        

function edit(itemId, element) {
    const container = $(`div[data-container-id=${itemId}]`);
    //set title
    $(container[0]).html(`<input class="form-control" type="text" value="${$(container[0]).attr('data-item-value')}"/>`);

    //set dueDate
    let dueDate = new Date($(container[1]).attr('data-item-value'));
    $(container[1]).html(`<input class="form-control" type="datetime-local" value="${dateFormat(dueDate)}"/>`);
    $(element).removeClass('text-warning').addClass('text-primary');
    $(element).off();
    $(element).on('click', () => editSubmit(itemId, element));
}

function editSubmit(itemId, element) {
    const container = $(`div[data-container-id=${itemId}]`);
    const title = $(container[0]).find('input').val();

    const dueDate = new Date($(container[1]).find('input').val());

    let editedTask = { Id: itemId, Title: title, DueDate: dateFormat(dueDate)};

    $.ajax({
        method: 'PUT',
        url: '/EditItem',
        data: editedTask,
        success: function (result) {
            //set title
            $(container[0]).html(title);

            //set dueDate
            $(container[1]).html(dateFormat(dueDate));
            let containers = $(`div[data-container-id=${itemId}]`);
            $(containers[0]).attr('data-item-value', title);
            $(containers[1]).attr('data-item-value', dueDate);
            alert(result);
        },
        error: function (error) {
            //set title
            let prevTitle = $(container[0]).attr('data-item-value');
            $(container[0]).html(`<input class="form-control" type="text" value="${prevTitle}"/>`);

            //set dueDate
            let prevDueDate = new Date($(container[1]).attr('data-item-value'));
            $(container[1]).html(`<input class="form-control" type="datetime-local" value="${dateFormat(prevDueDate)}"/>`);

            alert(error.responseText);
        }
    });

    $(element).removeClass('text-primary').addClass('text-warning');
    $(element).off();
    $(element).on('click', () => edit(itemId, element));
}

function dateFormat(date) {
    let month = date.getMonth() < 10 ? `0${date.getMonth() + 1}` : date.getMonth() + 1;
    if (month == '010') month = '10';

    const day = date.getDate() < 10 ? `0${date.getDate()}` : date.getDate();
    const year = date.getFullYear();
    const hours = date.getHours() < 10 ? `0${date.getHours()}` : date.getHours();
    const minutes = date.getMinutes() < 10 ? `0${date.getMinutes()}` : date.getMinutes();

    return `${year}-${month}-${day}T${hours}:${minutes}`;
}