window.onload = function () {
    sc1 = 50;
    sc2 = 1;
    SetStartedParameters();
    canvas = document.getElementById('can');
    ctx = canvas.getContext('2d');

    drawMy();
    ChangeScroll();
};
var ctx;
var canvas;

var WIDTH = 600;
var HEIGHT = 600;

var r1;
var r2;
var A = 0, B = 0;

var x_offset = WIDTH / 2;
var y_offset = HEIGHT / 2;

var cosA;
var cosB;
var sinA;
var sinB;

var sinT;
var cosT;
var sinP;
var cosP;

var x;
var y;
var x2;
var y2;

var sc1
var sc2

var red;
var green;
var blue;

var c1;
var c2;
var c3;

var size;
var lumen;
var liquid;

function diaChange() {
    r1 = document.getElementById("r1").value * 1;
    setCookie("r1", r1, { secure: true, 'max-age': 3600 });
    r2 = document.getElementById("r2").value * 1;
    setCookie("r2", r2, { secure: true, 'max-age': 3600 });
}

function SetStartedParameters() {
    sc1 = getCookie("sc1") * 1;
    sc2 = getCookie("sc2") * 1;
    red = getCookie("red") * 1;
    green = getCookie("green") * 1;
    blue = getCookie("blue") * 1;
    //c1 = getCookie("c1");
    //c2 = getCookie("c2");
    //c3 = getCookie("c3");
    size = getCookie("size") * 1;
    lumen = getCookie("lumen") * 1;
    liquid = getCookie("liquid") * 1;
    r1 = getCookie("r1") * 1;
    r2 = getCookie("r2") * 1;

    document.getElementById("sc1").value = sc1;
    document.getElementById("sc2").value = sc2;

    document.getElementById("color1").value = red;
    document.getElementById("color2").value = green;
    document.getElementById("color3").value = blue;

    document.getElementById("size").value = size;

    document.getElementById("lumen").value = lumen;

    document.getElementById("liquid").value = liquid;

    document.getElementById("r1").value = r1;
    document.getElementById("r2").value = r2;
}

function getCookie(name) {
    let matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

function setCookie(name, value, options = {}) {

    options = {
        path: '/',
        ...options
    };

    if (options.expires instanceof Date) {
        options.expires = options.expires.toUTCString();
    }

    let updatedCookie = encodeURIComponent(name) + "=" + encodeURIComponent(value);

    for (let optionKey in options) {
        updatedCookie += "; " + optionKey;
        let optionValue = options[optionKey];
        if (optionValue !== true) {
            updatedCookie += "=" + optionValue;
        }
    }

    document.cookie = updatedCookie;
}

function ChangeScroll(){
    sc1 = document.getElementById("sc1").value * 1;
    sc2 = document.getElementById("sc2").value * 1;

    setCookie("sc1", sc1, { secure: true, 'max-age': 3600 });
    setCookie("sc2", sc2, { secure: true, 'max-age': 3600 });

    document.getElementById("qwe1").innerHTML = "Поперечные: " + Math.round(628 / sc1);
    document.getElementById("qwe2").innerHTML = "Продольные: " + Math.round(628 / sc2);

    size = document.getElementById("size").value * 1;
    setCookie("size", size, { secure: true, 'max-age': 3600 });

    lumen = document.getElementById("lumen").value * 1;
    setCookie("lumen", lumen, { secure: true, 'max-age': 3600 });

    liquid = document.getElementById("liquid").value * 1;
    setCookie("liquid", liquid, { secure: true, 'max-age': 3600 });
}

function ChangeColor() {
    red = document.getElementById("color1").value * 1;
    green = document.getElementById("color2").value * 1;
    blue = document.getElementById("color3").value * 1;
    setCookie("red", red, { secure: true, 'max-age': 3600 });
    setCookie("green", green, { secure: true, 'max-age': 3600 });
    setCookie("blue", blue, { secure: true, 'max-age': 3600 });

    c1 = document.getElementById("checkbox1").checked;
    c2 = document.getElementById("checkbox2").checked;
    c3 = document.getElementById("checkbox3").checked;
}

function drawMy() {
    setInterval(function () {
        var K2 = 5000;
        var K1 = WIDTH * K2 * 3 / (8 * (r1 + r2));
        ctx.fillStyle = "rgb(0,0,0)";
        ctx.clearRect(0, 0, WIDTH, HEIGHT);
        cosA = Math.cos(A);
        sinA = Math.sin(A);
        cosB = Math.cos(B);
        sinB = Math.sin(B);
        for (var T = 0; T < 628; T += sc1) {
            sinT = Math.sin(T / 100);
            cosT = Math.cos(T / 100);
            x2 = r2 + r1 * cosT;
            y2 = r1 * sinT;
            for (var P = 0; P < 628; P += sc2) {
                sinP = Math.sin(P / 100);
                cosP = Math.cos(P / 100);

                x = x2 * (cosB * cosP + sinA * sinB * sinP) - y2 * cosA * sinB;
                y = x2 * (cosP * sinB - cosB * sinA * sinP) + y2 * cosA * cosB;

                var z = K2 + r1 * sinA * sinT + cosA * sinP * x2;
                var ooz = 1 / z;

                var xp = x * K1 * ooz;
                var yp = -y * K1 * ooz;

                var l = cosP * cosT * sinB - cosA * cosT * sinP - sinA * sinT + cosB * (cosA * sinT - cosT * sinA * sinP);
                if (l > lumen) {
                    l = RoundToDec(l, 2);
                    var L = l * 180;
                    PrintCan(xp + x_offset, yp + y_offset, L, l);
                }
            }
        }
        A += 0.01;
        B += 0.001; 
    }, 1);
}

function PrintCan(x, y, l, f) {
    var r = red;
    var g = green;
    var b = blue;
    if (c1) {
        r = l;
    }
    if (c2) {
        g = l;
    }
    if (c3) {
        b = l;
    }
    ctx.beginPath();
    ctx.fillStyle = "rgba(" + r + "," + g + "," + b + ", " + (f - liquid) +")";
    ctx.fillRect(x, y, size, size);
}

function RoundToDec(num, dec) {
    return Math.round(num * Math.pow(10, dec)) / Math.pow(10, dec);
}
