
function StartSpiner(Parent) {
    StopSpiner(Parent);
    var spiner = $("#" + Parent).append('<div class="loader"></div>')
    return spiner;
}

function StopSpiner(Parent) {
    var DIVS = $('#' + Parent).find("DIV");
    for (var i = 0; i < DIVS.length; i++) {
        if (DIVS[i].className == "loader") {
            DIVS[i].remove();
        }
    }
}

function CheckError(json) {
    if (json.error != "" && json.error != undefined) {
        alert(json.error);
    }
}

function FillGrid(nameGrd, data, fn, params) {
    if (data.length == 0) {
        return false;
    }
    var options = {
        data: data,
        autoColumns: true,
        selectable: 1,
        keybindings: false,
        rowSelected: function (row) {
            fn(nameGrd, row._row.data);
            SwichCurrentGrid(row);
        }
    }
    if (params != undefined) {
        options = Object.assign(options, params);
    }
    var table = new Tabulator(nameGrd, options);
    return table;
}

function isEmpty(str) {
    if (str.trim() == '')
        return true;

    return false;
}