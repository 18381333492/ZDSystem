Lord = {}

Lord.LordOrderList = function (json) {
    for (var i = 0; i < json.length; i++) {
        var order = Lord.addDiv("order");
        order.appendTo("#centen");

        var tit = Lord.addDiv("container tit"); // head
        tit.appendTo(order);

        var row1 = Lord.addDiv("row");
        row1.appendTo(tit);

        var r1c1 = Lord.addDiv("col lft");
        r1c1.appendTo(row1);
        r1c1.text('商品单号: ' + json[i].ORDERNO);

        var row2 = Lord.addDiv("row time");
        row2.appendTo(tit);

        var r2c1 = Lord.addDiv("col lft");
        r2c1.appendTo(row2);
        r2c1.text(json[i].PRODUCTNAME);


        var r2c2 = Lord.addDiv("col rig");
        r2c2.appendTo(row2);
        var span = Lord.addSpan();
        span.text(json[i].CREATETIME);
        Lord.addI("fa fa-clock-o").prependTo(span);

        span.appendTo(r2c2);

        var tab = Lord.addDiv("container tab"); // table
        tab.appendTo(order);

        var tr1 = Lord.addDiv("row t");
        tr1.appendTo(tab);

        Lord.addDiv("col").text('支付方式').appendTo(tr1);
        Lord.addDiv("col").text('号码').appendTo(tr1);
        Lord.addDiv("col").text('金额').appendTo(tr1);
        Lord.addDiv("col").text('状态').appendTo(tr1);

        var tr2 = Lord.addDiv("row");
        tr2.appendTo(tab);

        var tr2c1 = Lord.addDiv("col align-items-start");
        if (json[i].PAYTYPE == 2) {
            Lord.addImg("/img/WX.png").appendTo(tr2c1);
        } else if (json[i].PAYTYPE == 1) {
            Lord.addImg("/img/ZFB.png").appendTo(tr2c1);
        }
        tr2c1.appendTo(tr2);
        Lord.addDiv("col").text(json[i].MOBILE).appendTo(tr2);
        Lord.addDiv("col").text(json[i].USERPAYED).appendTo(tr2);
        if (json[i].STATUS == "等待付款") {
            var tr2c3 = Lord.addDiv("col");
            Lord.addButton(json[i]).appendTo(tr2c3);
            tr2c3.appendTo(tr2);
        } else {
            Lord.addDiv("col").text(json[i].STATUS).appendTo(tr2);
        }

    }
}

Lord.addDiv = function (cla) {
    var div = $('<div />', {
        class: cla
    });
    return div
}

Lord.addI = function (cal) {
    var i = $('<i />', {
        class: cal
    });
    return i
}

Lord.addSpan = function () {
    return $('<span />');
}

Lord.addImg = function (src) {
    var img = $('<img />', {
        src: src
    });
    return img
}

Lord.addButton = function (res) {
    var button = $('<button />', {
        text: '继续支付',
        class: 'button waves-effect',
        click: function () {
            var info = $(this).attr("data-value");
            if (info) {
                info = JSON.parse(info);
                localStorage.setItem("orderinfo", JSON.stringify({
                    orderno: info.ORDERNO, //订单编号
                    product_name: info.PRODUCTNAME,
                    mobile: info.MOBILE, //充值的手机号
                    price: info.USERPAYED //售价,
                }));
                location.href = "/hwpayment.html";
            }
            else {
                $.toast("参数错误", "cancel");
            }
        }
    });
    button.attr("data-value", JSON.stringify(res));
    return button;
}

Lord.IsDataNull = function () {
    var div = Lord.addDiv("null");
    var cdiv = Lord.addDiv("flex-center flex-column");
    Lord.addImg('/img/dataNull.png').addClass("animated fadeIn mb-4").appendTo(cdiv);
    $('<p />', { class: 'animated fadeIn mb-4 font_color' }).text('没有查询到充值记录').appendTo(cdiv);
    cdiv.appendTo(div);
    div.appendTo("#centen");
}
