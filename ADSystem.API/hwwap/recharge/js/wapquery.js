
$(function () {
    var orderParams = {
        page: 1,
        rows: 6,
        mobile: '',
        state: true
    };
    var time = 60;
    var codeStatus = true;
    var ableQuery = false;
    var t;

    //获取手机验证码
    $('#getCode').on("click", function () {
        if (codeStatus) {
            var phone = $('input[name=mobile]').val();
            if (phone.length == 0) {
                return dialog.alert("请输入手机号码!");
            }
            phone = phone.replace(/\D/g, '');
            if (phone.length > 0 && client.regex.isPhone(phone)) {
                client.ajax.ajaxRequest("/HWServer/CodeHelper.ashx", { phone: phone, FuncType: 1 }, function (r) {
                    if (r.success) {
                        ableQuery = true;
                        codeStatus = false;
                        dialog.alert("验证码获取成功!");
                        $('#getCode').css({ "border": "1px solid #A1A1A1", "color": "#A1A1A1" });
                        clearInterval(t);
                        time = 60;
                        t = setInterval(function () {
                            if (time >= 0) {
                                $('#getCode').text(time + "s");
                                time--;
                            }
                            else {
                                $('#getCode').css({ "border": "1px solid #FF4500", "color": "#FF4500" });
                                $('#getCode').text("获取验证码");
                            }
                        }, 1000);
                    }
                    else {
                        dialog.alert("验证码获取失败!");
                    }
                });

            }
            else {
                dialog.alert("手机号码格式错误!");
            }
        }
    });

    //查询数据
    $('#query').on("click", function () {
        if (ableQuery) {
            var phone = $('input[name=mobile]').val().replace(/\D/g, '');
            var code = $('#code').val().trim();
            if (code.length == 0) {
                return dialog.alert("请输入验证码!");
            }
            client.ajax.ajaxRequest("/HWServer/CodeHelper.ashx", { phone: phone, code: code, FuncType: 2 }, function (r) {
                if (r.success) {
                    orderParams.page = 1;
                    orderParams.state = true;
                    loadOrderList(phone);
                }
                else {
                    dialog.alert("验证码错误!");
                }
            });
        }
        else {
            dialog.alert("请先获取验证码!");
        }
    });


    //清除输入框的值
    $('.weui-icon-clear').on("click", function () {
        $('input[name=mobile]').val('');
        $('input[name=mobile]').removeClass("mobile_focus");
        $(this).hide();
        $('input[name=mobile]').focus();
    });

    //输入手机号码效果的切换
    $('input[name=mobile]').on("keyup", function (e) {
        var value = $(this).val();
        if (e.keyCode != 8) {
            if (value.length == 4 || value.length == 9) {
                var i = value.length;
                value = client.string.insert(value, i - 1, " ");
                $(this).val(value);
            }
        }
        else {
            if (value.length == 9 || value.length == 4)
                value = $(this).val(value.trim());
        }
    });

    //样式的切换
    $('input[name=mobile]').on("input", function () {
        var value = $(this).val();
        if (value.length > 0) $('.weui-icon-clear').show();
        if (!$(this).hasClass("mobile_focus")) {
            $(this).addClass("mobile_focus");
        }
        if (value.length == 0) {
            $('.weui-icon-clear').hide();
            $(this).removeClass("mobile_focus");
        }
        if (value.replace(/\D/g, '').length > 0 && client.regex.isPhone(value.replace(/\D/g, ''))) {
            //如果直接赋值号码
            value = value.replace(/\D/g, '');
            value = client.string.insert(value, 3, ' ');
            value = client.string.insert(value, 8, ' ');
            $(this).val(value);
        }
        if (value.length == 13) {
            $(this).blur(); //失去焦点
            value = value.replace(/\D/g, '');
            //判断检查号码的区段
            if (value.length > 0 && client.regex.isPhone(value)) {
                $('#noData').hide();
            }
            else {
                dialog.alert("手机号码格式错误!");
            }
        }
    });

    $(window).scroll(function () {//绑定滚动事件
        if ($(document).scrollTop() == $("#centen").height() - $(window).height()) {
            if (orderParams.state) {
                orderParams.page = orderParams.page + 1;
                loadOrderList(orderParams.mobile);
            }
        }
    });

    //根据手机号码加载产品列表
    function loadOrderList(phone) {
        $('.hw_bottom').remove();
        $('.weui-cells_form').remove();
        $.showLoading();
        orderParams.mobile = phone;
        client.ajax.ajaxRequest("/HWServer/QueryByPhone.ashx", orderParams, function (r) {
            $.hideLoading();
            if (r.rows.length > 0) {
                var array = r.rows;
                var html = [];
                $(array).each(function () {
                    html.push('<div class="order">');
                    html.push('<div class="container tit">');
                    html.push('<div class="row">');
                    html.push('<div class="col lft">订单号:' + this.ORDERNO + '</div>');
                    html.push('</div>');
                    html.push('<div class="row time">');
                    html.push('<div style=" padding-left:3px;">' + this.PRODUCTNAME + '</div>');
                    html.push('<div class="col rig"><span style=" color:#CACACA"><i class="fa fa-clock-o"></i>' + this.CREATETIME + '</span></div>');
                    html.push('</div>');
                    html.push('</div>');
                    html.push('<div class="container tab" style="background-color: #eee;">');
                    html.push('<div class="row" style="padding-bottom:0px;">');
                    html.push('<div class="col" style="color:#CACACA">手机号码</div>');
                    html.push('<div class="col" style="color:#CACACA">支付金额</div>');
                    html.push('<div class="col" style="color:#CACACA">充值状态</div>');
                    html.push('</div>');
                    html.push('<div class="row">');
                    html.push('<div class="col align-items-start">' + this.MOBILE + '</div>');
                    html.push('<div class="col">' + this.USERPAY + '</div>');
                    html.push('<div class="col" style="color:#FF4500">' + this.STATUS + '</div>');
                    html.push('</div>');
                    html.push('</div>');
                    html.push('</div>');
                });
                $('#centen').append(html.join(''));
                if (r.rows.length < orderParams.rows) {
                    orderParams.state = false;
                }
            }
            else {
                if ($('#centen .order').length == 0) {
                    $('#noData').show();
                }
                else {
                    if (!r.rows)
                        $.toast(r, "cancel");
                    else
                        orderParams.state = false;
                }
            }
        });
    }
});
