var stmp = "";
function nst(t) {
    if (t.value == stmp) return;
    if (isNaN(t.value)) {
        alert("请输入数字");
        return $("#blance").val("0");
    }
    else {
        if ($("#blance").val()< 0) {
            alert("金额不能小于0");
            return $("#blance").val("0");
        }
        var ms = t.value.replace(/[^\\d\\.]/g, "").replace(/(\\.\\d{2}).+$/, "$1").replace(/^0+([1-9])/, "$1").replace(/^0+$/, "0");
        var ms = t.value;
        var txt = ms.split(".");
        while (/\\d{4}(,|$)/.test(txt[0]))
            txt[0] = txt[0].replace(/(\\d)(\\d{3}(,|$))/, "$1,$2");
        t.value = stmp = txt[0] + (txt.length > 1 ? "." + txt[1] : "");
        $("#bigMoney").html(number2num1(ms - 0, t));
    }
}

function number2num1(strg, obj) {
    var number = Math.round(strg * 100) / 100;
    number = number.toString(10).split(".");
    var a = number[0];
    if (a.length > 12) {
        obj.value = obj.value.substring(0, 12);
        return "数值超出范围！支持的最大数值为 999999999999.99";
    }
    else {
        var e = "零壹贰叁肆伍陆柒捌玖";
        var num1 = "";
        var len = a.length - 1;
        for (var i = 0; i <= len; i++)
            num1 += e.charAt(parseInt(a.charAt(i))) + [["圆", "万", "亿"][Math.floor((len - i) / 4)], "拾", "佰", "仟"][(len - i) % 4];
        if (number.length == 2 && number[1] != "") {
            var a = number[1];
            for (var i = 0; i < a.length; i++)
                num1 += e.charAt(parseInt(a.charAt(i))) + ["角", "分"][i];
        }
        num1 = num1.replace(/零佰|零拾|零仟|零角/g, "零");
        num1 = num1.replace(/零{2,}/g, "零");
        num1 = num1.replace(/零(?=圆|万|亿)/g, "");
        num1 = num1.replace(/亿万/, "亿");
        num1 = num1.replace(/^圆零?/, "");

        if (num1 != "" && !/分$/.test(num1))
            num1 += "整";
        return num1;
    }
}