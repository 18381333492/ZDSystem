$(function () {
    localStorage.removeItem("payTip"); //清除缓存
    $('.kehu_help').on("click", function () {
        localStorage.setItem("recharge_mobile", $('input[name=mobile]').val());
        location.href = "/hwwap/hwFAQ.html";
    });

    //清除输入框的值
    $('.weui-icon-clear').on("click", function () {
        $('input[name=mobile]').val('');
        $('input[name=mobile]').removeClass("mobile_focus");
        $(this).hide();
        //关闭效果    
        $('#tip').val("移动 联通 电信");
        $('#tip').css("color", "#ccc");
        if ($('ul').hasClass("able_ul")) {
            $('ul').removeClass("able_ul").addClass("unable_ul");
            $('ul li').removeClass('select'); //清楚选择值
        }
        $('input[name=mobile]').focus();
    });

    //页面初始化的时候进行
    $('input[name=mobile]').val(localStorage.getItem("recharge_mobile"));
    var ph = $('input[name=mobile]').val();
    if (ph.length == 13) {
        $('.weui-icon-clear').show();
        ph = ph.replace(/\D/g, '');
        LoadMobile(ph);
    }

    //失去焦点的时候
    $('input[name=mobile]').on("blur", function () {
        $('.hw_bottom').css("z-index", "10");
    });

    //获取焦点的时候
    $('input[name=mobile]').on("focus", function () {
        $('.hw_bottom').css("z-index", "-100");
    })

    //输入手机号码效果的切换
    $('input[name=mobile]').on("keyup", function (e) {
        var value = $(this).val();
        if (e.keyCode != 8) {
            if (/^[0-9]*$/.test(value.substr(value.length - 1, 1))) {
                if (value.length == 4 || value.length == 9) {
                    var i = value.length;
                    value = client.string.insert(value, i - 1, " ");
                    $(this).val(value);
                }
            }
            else {
                $(this).val(value.substr(0, value.length - 1));
                if ($(this).val().length == 0)
                    $(this).removeClass("mobile_focus");
            }
        }
        else {
            if (value.length == 9 || value.length == 4)
                value = $(this).val(value.trim());
            if (value.length < 13) {
                //关闭效果    
                $('#tip').val("移动 联通 电信");
                $('#tip').css("color", "#ccc");
            }
            if ($('ul').hasClass("able_ul")) {
                $('ul').removeClass("able_ul").addClass("unable_ul");
                $('ul li').removeClass('select');
            }
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
        if (value.trim().length == 13) {
            $('.weui-icon-clear').show();
            $(this).blur(); //失去焦点
            value = value.replace(/\D/g, '');
            //判断检查号码的区段
            if (client.regex.isPhone(value)) {
                //interfaceType 1-获取好码归属地 2-获取产品
                LoadMobile(value);
            }
            else {
                setTimeout(function () {
                    $.toast("手机号码格式错误", "text");
                }, 500);
            }
        }
    });

    var PhoneTip = '';
    //判断号码区段
    function LoadMobile(value) {
        client.ajax.ajaxRequest("/HWServer/MobileQuery.ashx", { sPhone: value, interfaceType: 1 }, function (r) {
            if (r.code == 100 && r.data.carrierNo) {
                $('#tip').val('');
                PhoneTip = r.data.provinceNo + r.data.carrierNo;
                $('#tip').css("color", "#FF4500");
                loadProduct(value);
            }
            else {
                $.toast("不支持的号码", "cancel");
            }
        });
    }

    var recharge_phone = '';
    //发起支付请求
    $('#payrequest').on("click", function () {
        if ($('li[class=select]').length == 1) {
            var phone = recharge_phone;
            var $this = $('li[class=select]').eq(0);
            var productId = $this.attr("data-productId");
            var item = getProduct(productId);
            var face = item.face;
            var fee = item.payFee; ;
            var flow_type = item.flowType == null ? "null" : item.flowType;
            var businessType = item.businessType;
            var product_name = "充慢充话费-" + face + "元";
            var rechargeMode = item.rechargeMode;
            //获取账户信息
            var show_mobile = client.string.insert(phone, 3, ' ');
            show_mobile = client.string.insert(show_mobile, 8, ' ');
            localStorage.setItem("recharge_mobile", show_mobile);
            localStorage.setItem("orderinfo", JSON.stringify({
                businessType: businessType,
                account: phone, //华为生活账号
                product_name: product_name,
                mobile: phone, //充值的手机号
                product_id: productId, //产品编号
                face: face, //面值
                price: fee, //售价,
                card_type: 1, //充值类型 1-话费 2-流量
                flow_type: flow_type, //流量类型
                recharge_mode: rechargeMode //慢充类型
            }));
            location.href = "/hwwap/hwpayment.html";
        }
        else {
            $.toast("请选择充值面值", "text");
        }
    });

    //根据产品ID获取产品
    function getProduct(productId) {
        var item = {};
        for (var i = 0; i < productArray.length; i++) {
            if (productArray[i].productID == productId) {
                item = productArray[i];
                break;
            }
        }
        return item;
    }


    var productArray = [];
    //根据手机号码加载产品列表
    function loadProduct(phone) {
        recharge_phone = phone;
        client.ajax.ajaxRequest("/HWServer/MobileQuery.ashx", { sPhone: phone, interfaceType: 2, businessType: 1, rm: 3 }, function (r) {
            if (r.code == 100) {
                var array = r.data;
                productArray = array; //重新赋值
                var html = [];
                $(array).each(function () {
                    html.push('<li data-productId="' + this.productID + '">');
                    html.push('<p>' + this.face + '元</p>');
                    html.push('<span>仅售:' + this.payFee + '</span>');
                    html.push('</li>');
                });
                if (array.length > 0) {
                    $('#tip').val(PhoneTip);
                    $('ul').html('').append(html.join(''));
                    $('ul').removeClass("unable_ul").addClass("able_ul");
                    $('ul li').removeClass('select');

                    //绑定点击事件
                    $('ul li').on("click", function () {
                        if ($('ul').hasClass("able_ul")) {
                            $(this).addClass('select').siblings().removeClass("select");
                        }
                    });
                }
                else {
                    $('#tip').val("暂不支持" + PhoneTip + "用户充特价话费");
                    $('#tip').css("color", "#FF4500");
                }
            } else {
                $.toast(r.msg, "cancel");
            }
        });
    }
});