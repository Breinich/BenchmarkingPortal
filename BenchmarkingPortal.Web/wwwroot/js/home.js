function addConfigItem(scope, keyId, valueId, inputId) {
    const key = $(keyId).val();
    const value = $(valueId).val();

    $.ajax({
        type: "POST",
        url: "/Home?scope=" + scope + "&key=" + key + "&value=" + value + "&handler=AddConfigItem",
        contentType: "application/json; charset=utf-8",

        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },

        success: function (newItem) {
            $(inputId).before(
                '<tr id="' + newItem.Id + '" class="generated">' +
                '<td>' +
                '<input readOnly value="' + newItem.Key + '" class="form-control-plaintext"/>' +
                '</td>' +
                '<td>' +
                '<input readOnly value="' + newItem.Value + '" class="form-control-plaintext"/>' +
                '</td>' +
                '<td>' +
                '<button type="button" class="btn btn-danger m-1" onclick="deleteConfigItem(\'' + newItem.Id + '\')">' +
                '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash-fill" viewBox="0 0 16 16">\n' +
                '<path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z"/>\n' +
                '</svg>' +
                '</button>' +
                '</td>' +
                '</tr>');


            $(keyId).val("");
            $(valueId).val("");
        },
        failure: function (response) {
            alert("failure: " + response.responseText);
        },
        error: function (response) {
            alert("error: " + response.responseText);
        }
    });
}

function deleteConfigItem(id) {
    $.ajax({
        type: "POST",
        url: "/Home?id=" + id + "&handler=DeleteConfigItem",
        contentType: "application/json; charset=utf-8",

        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },

        success: function () {
            $("#" + id).remove();
        },
        failure: function (response) {
            alert("failure: " + response.responseText);
        },
        error: function (response) {
            alert("error: " + response.responseText);
        }
    });
}

function addConstraint(expressionId, inputId) {
    const expression = $(expressionId).val();

    $.ajax({
        type: "POST",
        url: "/Home?expression=" + expression + "&handler=AddConstraint",
        contentType: "application/json; charset=utf-8",

        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },

        success: function (newItem) {
            $(inputId).before(
                '<tr id="' + newItem.Id + '" class="generated">' +
                '<td>' +
                '<input readOnly value="' + newItem.Expression + '" class="form-control-plaintext"/>' +
                '</td>' +
                '<td>' +
                '<button type="button" class="btn btn-danger m-1" onclick="deleteConstraint(\'' + newItem.Id + '\')">' +
                '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash-fill" viewBox="0 0 16 16">\n' +
                '<path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z"/>\n' +
                '</svg>' +
                '</button>' +
                '</td>' +
                '</tr>');


            $(expressionId).val("");
        },
        failure: function (response) {
            alert("failure: " + response.responseText);
        },
        error: function (response) {
            alert("error: " + response.responseText);
        }
    });
}

function deleteConstraint(id) {

    $.ajax({
        type: "POST",
        url: "/Home?id=" + id + "&handler=DeleteConstraint",
        contentType: "application/json; charset=utf-8",

        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },

        success: function () {
            $("#" + id).remove();
        },
        failure: function (response) {
            alert("failure: " + response.responseText);
        },
        error: function (response) {
            alert("error: " + response.responseText);
        }
    });
}

function deleteSession() {
    $.ajax({
        type: "POST",
        url: "/Home?handler=DeleteSession",
        contentType: "application/json; charset=utf-8",

        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },

        success: function () {
            $(".generated").remove();
        },
        failure: function (response) {
            alert("failure: " + response.responseText);
        },
        error: function (response) {
            alert("error: " + response.responseText);
        }
    });
}
