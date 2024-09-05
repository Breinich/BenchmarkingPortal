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
                '<tr id="' + newItem.configId + '" class="generated">' +
                '<td>' +
                '<input readOnly value="' + newItem.configKey + '" class="form-control-plaintext"/>' +
                '</td>' +
                '<td>' +
                '<input readOnly value="' + newItem.configValue + '" class="form-control-plaintext"/>' +
                '</td>' +
                '<td>' +
                '<button type="button" class="btn btn-danger m-1" onclick="deleteConfigItem(\'' + newItem.configId + '\')">' +
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
                '<tr id="' + newItem.constraintId + '" class="generated">' +
                '<td>' +
                '<input readOnly value="' + newItem.expression + '" class="form-control-plaintext"/>' +
                '</td>' +
                '<td>' +
                '<button type="button" class="btn btn-danger m-1" onclick="deleteConstraint(\'' + newItem.constraintId + '\')">' +
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

function addConfig(){
    $.ajax({
        type: "POST",
        data: $("#newBenchmarkForm").serialize(),
        url: "/Home?handler=Config",
        contentType: "application/json; charset=utf-8",

        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },

        success: function (newItem) {
            let startButton = $("#startButton")
            startButton.hidden = false;
            startButton.attr("onclick", "startBenchmark('" + newItem.configId + "')");
            
            let deleteButton = $("#deleteConfigButton")
            deleteButton.hidden = false;
            deleteButton.attr("onclick", "deleteConfig('" + newItem.configId + "')");
            
            $("#cancelButton").hidden = true;
            $("#saveConfigButton").hidden = true;
            $("#escapeButton").hidden = true;
        },
        failure: function (response) {
            alert("failure: " + response.responseText);
        },
        error: function (response) {
            alert("error: " + response.responseText);
        }
    });
}

$(function () {
    $("#CreateInput_SourceSetId").on("change", function() {
        const sourceSetId = $(this).val();
        const propertyFilePathList = $("#CreateInput_PropertyFilePath");
        const setFilePathList = $("#CreateInput_SetFilePath");
        
        $.ajax({
            type: "GET",
            url: `?handler=SetFiles&sourceSetId=${sourceSetId}`,
            contentType: "application/json; charset=utf-8",
            
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            
            success: function (data) {
                setFilePathList.empty();
                setFilePathList.append("<option value='' selected disabled>Select set file</option>");
                console.log(data);
                $.each(data, function (i, item) {
                    setFilePathList.append(`<option value="${item.value}">${item.text}</option>`);
                });
                setFilePathList.prop("disabled", false);
            }
        });
        
        
        $.ajax({
            type: "GET",
            url: `?handler=PropertyFiles&sourceSetId=${sourceSetId}`,
            contentType: "application/json; charset=utf-8",
            
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            
            success: function (data) {
                propertyFilePathList.empty();
                propertyFilePathList.append("<option value='' selected disabled>Select property file</option>");
                console.log(data);
                $.each(data, function (i, item) {
                    propertyFilePathList.append(`<option value="${item.value}">${item.text}</option>`);
                });
                propertyFilePathList.prop("disabled", false);
            }
        });
        
    });
});

function startBenchmark(id) {
    // enable loading animation
    
    $.ajax({
        type: "POST",
        url: "/Home?configId=" + id + "&handler=Start",
        contentType: "application/json; charset=utf-8",

        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },

        success: function () {
            // disable loading animation
            // redirect to home page
        },
        failure: function (response) {
            alert("failure: " + response.responseText);
        },
        error: function (response) {
            alert("error: " + response.responseText);
        }
    });
}

function deleteConfig(id){
    $.ajax({
        type: "POST",
        url: "/Home?id=" + id + "&handler=DeleteConfig",
        contentType: "application/json; charset=utf-8",

        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },

        success: function () {
            $("#startButton").hidden = true;
            $("#deleteConfigButton").hidden = true;
            $("#cancelButton").hidden = false;
            $("#saveConfigButton").hidden = false;
            $("#escapeButton").hidden = false;
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
