var MyShips = [];
var EnShips = [];

var CountCells_X = 10;
var CountCells_Y = 10;

var GameStarted = false;
var WIDTH;
var HEIGHT;
var cellSize;
var ctxMy;
var ctxEn;
var cell;

var nameRoom;
var posTarget = {
    x: 0,
    y: 0
}
var Canvases = {};

window.onload = function () {
    AddEvent();
    //InitGame();
    InitConsts();
    PrintMap(ctxMy);
    PrintMap(ctxEn);
    PrintTarget();
}

function InitConsts() {
    Canvases.My = $("#can1");
    Canvases.En = $("#can2");

    WIDTH = Canvases.My.width();
    HEIGHT = Canvases.My.height();
    if (CountCells_X > CountCells_Y) {
        cellSize = HEIGHT / CountCells_X;
    } else {
        cellSize = HEIGHT / CountCells_Y;
    }

    ctxMy = Canvases.My[0].getContext('2d');
    ctxEn = Canvases.En[0].getContext('2d');
}

function PrintMap(canvasCtx) {
    for (var y = 0; y < CountCells_Y; y++) {
        for (var x = 0; x < CountCells_X; x++) {
            canvasCtx.strokeRect(cellSize * x, cellSize * y, cellSize, cellSize)
        }
    }
}

function SetShip() {
    if (GameStarted) {
        return false;
    }
    for (var i = 0; i < MyShips.length; i++) {
        var item = MyShips[i];
        if (item.pX == posTarget.x && item.pY == posTarget.y) {
            //MyShips.forEach(function (item, i, arr) {
            //    if (item.pX == posTarget.x && item.pY == posTarget.y) {
            //        MyShips.splice(i, 1);
            //    }
            //})
            MyShips.splice(i, 1);
            ClearAllCells(ctxMy);
            PrintTarget();
            PrintShips(ctxMy, MyShips);
            console.log(MyShips);
            return false;
        }
    }
    //Добавить коллекцию с положением союзных кораблей
    var pX = posTarget.x;
    var pY = posTarget.y;
    MyShips.push({ pX, pY, TypeC: 1 });
    ClearAllCells(ctxMy);
    PrintTarget();
    PrintShips(ctxMy, MyShips);
    console.log(MyShips);
}

function MoveTarget(dir) {
    switch (dir) {
        case "left":
            if (posTarget.x > 0) {
                posTarget.x--;
            }
            break;
        case "up":
            if (posTarget.y > 0) {
                posTarget.y--;
            }
            break;
        case "rigth":
            if (posTarget.x < CountCells_X - 1) {
                posTarget.x++;
            }
            break;
        case "down":
            if (posTarget.y < CountCells_Y - 1) {
                posTarget.y++;
            }
            break;
    }
    var currCtx = GetCurrentCtx();

    if (currCtx != null) {
        PrintTarget();
    }
}

function ShotInCell(where, x, y, type) {
    var currCtxLoc;
    var currShipsLoc;
    if (where == 1) {
        currCtxLoc = ctxMy;
        ships = MyShips;

        //проверка на попадание по нашему кораблю
        for (var i = 0; i < ships.length; i++) {
            var item = ships[i];
            if (item.pX == x && item.pY == y) {
                item.TypeC = 2;
                ClearAllCells(ctxMy);
                //PrintTarget();
                PrintShips(ctxMy, ships);
                console.log(MyShips);
                return false;
            }
        }
        //в остальных случаях добавляем ячейка с промахом
    } else {
        currCtxLoc = ctxEn;
        currCtxLoc = EnShips;
    }

    ships.push({ pX, pY, TypeC: type});

    PrintShips(currCtxLoc,);
}

function PrintTarget() {
    var currCtx = GetCurrentCtx();

    ClearAllCells(currCtx);
    var tempColor = currCtx.fillStyle;
    var tempWidth = currCtx.lineWidth;

    currCtx.strokeStyle = "rgb(50,50,50)";
    currCtx.lineWidth = 6;

    currCtx.strokeRect(posTarget.x * cellSize + 5, posTarget.y * cellSize + 5, cellSize - 10, cellSize - 10);

    currCtx.strokeStyle = tempColor;
    currCtx.lineWidth = tempWidth;
}

function AddEvent() {
    document.addEventListener('keydown', function (event) {
        var direction = null;
        switch (event.keyCode) {
            case 65: // A
                direction = "left";
                break;
            case 87: // W
                direction = "up";
                break;
            case 68: // D
                direction = "rigth";
                break;
            case 83: // S
                direction = "down";
                break;
            case 32: // Space
                if (GameStarted && MyTurn) {
                    //Shoot();
                } else {
                    SetShip();
                }
                break;
        }
        if (direction != null) {
            MoveTarget(direction);
            PrintShips(GetCurrentCtx(), GetCurrentShips());
        }
    });
}

function InitGame() {
    var b = $('#Start')[0];
    posTarget.x = 0;
    posTarget.y = 0;
    GameStarted = true;
    b.disabled = true;
    b.hidden = true;
    PrintTarget();    
}

function ClearAllCells(ctx) {
    for (var y = 0; y < CountCells_Y; y++) {
        for (var x = 0; x < CountCells_X; x++) {
            ClearCell(x, y, ctx)
        }
    }
}

function ClearCell(x, y, ctx) {
    ctx.clearRect(x * cellSize + 2, y * cellSize + 2, cellSize - 4, cellSize - 4);
}

function PrintShips(ctx, Ships) {
    //Рисуем    
    for (var i = 0; i < Ships.length; i++) {
        var item = Ships[i];
        PrintOneShip(item, ctx);
    }
}

function GetCurrentCtx() {
    if (GameStarted) {
        return ctxEn;
    } else {
        return ctxMy;
    }
}

function GetCurrentShips() {
    if (GameStarted) {
        return EnShips;
    } else {
        return MyShips;
    }
}

function GetColorShip(item) {
    switch (item.TypeC) {
        case 1:
            return "rgb(40,200,40)";
            break;
        case 2:
            return "rgb(200,40,40)";
            break;
        case 3:
            return "rgb(40,40,200)"
            break;
    }
}

function PrintOneShip(item, currCtx) {
    switch (item.TypeC) {
        case 1:
            var tempColor = currCtx.fillStyle;
            var border = cellSize / 15;

            currCtx.fillStyle = GetColorShip(item);
            currCtx.fillRect(item.pX * cellSize + border, item.pY * cellSize + border, cellSize - (border * 2), cellSize - (border * 2))
            currCtx.fillStyle = tempColor;
            return true;
            break;
        case 2:
            var tempColor = currCtx.fillStyle;
            var tempWidth = currCtx.lineWidth;
            var tempCap = currCtx.lineCap;

            var border = cellSize / 4;

            currCtx.strokeStyle = GetColorShip(item);
            currCtx.lineWidth = cellSize / 2.5;
            currCtx.lineCap = "round";

            currCtx.beginPath();
            currCtx.moveTo(item.pX * cellSize + border, item.pY * cellSize + border);
            currCtx.lineTo(item.pX * cellSize + cellSize - border, item.pY * cellSize + cellSize - border);
            currCtx.moveTo(item.pX * cellSize + cellSize - border, item.pY * cellSize + border);
            currCtx.lineTo(item.pX * cellSize + border, item.pY * cellSize + cellSize - border);
            currCtx.stroke();

            currCtx.strokeStyle = tempColor;
            currCtx.lineWidth = tempWidth;
            currCtx.lineCap   = tempCap;
            return true;
            break;
        case 3:
            var tempColor = currCtx.fillStyle;
            currCtx.fillStyle = GetColorShip(item);
            currCtx.beginPath();
            currCtx.arc(item.pX * cellSize + cellSize / 2, item.pY * cellSize + + cellSize / 2, cellSize / 5, cellSize - 10, 0, 2 * Math.PI)
            currCtx.fill();
            currCtx.fillStyle = tempColor;
            return true;
            break;
    }
}

function CheckError(json) {
    if (json.error != "" && json.error != undefined) {
        alert(json.error);
    }
}