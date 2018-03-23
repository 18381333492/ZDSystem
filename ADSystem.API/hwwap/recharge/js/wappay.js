$(function () {
    localStorage.removeItem("payTip"); //清除缓存

    //获取链接参数
    function GetQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }
    var query_type = GetQueryString("rechargetype");

    //bar切换事件绑定
    $('.bar').on("click", function () {
        $(this).addClass("select_bar").siblings().removeClass("select_bar");
        //内容切换和显示
        $("." + $(this).siblings().eq(0).attr("data-value")).hide();
        $("." + $(this).attr("data-value")).show();

        if ($(this).attr("data-value") == "rechagre_phone")
            $('body').css("margin-bottom", "95px")
        if ($(this).attr("data-value") == "recharge_flow")
            $('body').css("margin-bottom", "40px")
    });

    //页面初始化的时候
    var recharge_type = localStorage.getItem("recharge_type"); //缓存充值类型 1-话费 2-流量
    if (query_type && !recharge_type) recharge_type = query_type;
    if (recharge_type == 1) $('.bar_left').click();
    if (recharge_type == 2) $('.bar_right').click();
    localStorage.removeItem("recharge_type");
    var recharge_ph = $('.rechagre_phone input').val().replace(/\D/g, '');
    var recharge_fl = $('.recharge_flow input').val().replace(/\D/g, '');
    if (recharge_ph.length == 11) {
        $('.rechagre_phone .weui-icon-clear').show();
        LoadMobile(recharge_ph, 1);
    }
    if (recharge_fl.length == 11) {
        $('.rechagre_flow .weui-icon-clear').show();
        LoadMobile(recharge_fl, 2);
    }

    //获取焦点事件
    $('input[name=mobile]').focus(function () {
        var value = $(this).val().replace(/\D/g, '');
        value = client.string.insert(value, 3, ' ');
        value = client.string.insert(value, 8, ' ');
        $(this).val(value);
    });

    //跳转客服帮助
    $('.kehu_help').on("click", function () {
        if ($(this).attr("data-value") == "phone")
            localStorage.setItem("recharge_type", 1);
        else
            localStorage.setItem("recharge_type", 2);
        location.href = "/hwwap/hwFAQ.html";
    });

    //跳转充值记录
    $('.recharge_record').on("click", function () {
        if ($(this).attr("data-value") == "phone")
            localStorage.setItem("recharge_type", 1);
        else
            localStorage.setItem("recharge_type", 2);
        location.href = "wapquery.html";
    });

    //清除输入框的值
    $('.weui-icon-clear').on("click", function () {
        $(this).hide();
        var $input = $(this).prev();
        var $ul = $(this).parents(".weui-cells").find("ul");
        $input.val('');
        $input.removeClass("mobile_focus");
        $input.focus();
        //设置默认样式
        if ($ul.hasClass("able_ul")) {
            $ul.removeClass("able_ul").addClass("unable_ul");
        }
        //清除选中项
        $ul.find('li').removeClass("selectItem")
        //流量的清除
        if ($(this).attr("data-value") == "flow") {
            $('.recharge_flow .detailinfo').html('');
            $ul.find('li').last().remove();
        }
        //话费的清除
        if ($(this).attr("data-value") == "phone") {
            $('#payrequest').css("background-color", "#FAE1DA");
        }
    });

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
            var $ul = $(this).parents(".weui-cells").find("ul");
            if ($ul.hasClass("able_ul")) {
                $ul.removeClass("able_ul").addClass("unable_ul");
                $ul.find('li').removeClass("selectItem"); //清除选中项
                if ($(this).next().attr("data-value") == "phone")
                    $('#payrequest').css("background-color", "#FAE1DA");
                if ($(this).next().attr("data-value") == "flow") {
                    $('.recharge_flow .detailinfo').html('');
                    $ul.find('li').last().remove();
                }
            }
        }
    });

    //样式的切换
    $('input[name=mobile]').on("input", function () {
        var value = $(this).val();
        if (value.length > 0)
            $(this).next().show();
        if (!$(this).hasClass("mobile_focus")) {
            $(this).addClass("mobile_focus");
        }
        if (value.length == 0) {
            $(this).next().hide();
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
            $(this).next().show();
            $(this).blur(); //失去焦点
            value = value.replace(/\D/g, '');
            //判断检查号码的区段
            if (client.regex.isPhone(value)) {
                //interfaceType 1-获取好码归属地 2-获取产品
                LoadMobile(value);
            }
            else {
                setTimeout(function () {
                    $.toast("号码格式错误", "cancel");
                }, 500);
            }
        }
    });

    //判断号码区段
    function LoadMobile(value, type) {
        client.ajax.ajaxRequest("/HWServer/MobileQuery.ashx", { sPhone: value, interfaceType: 1 }, function (r) {
            if (r.code == 100 && r.data.carrierNo) {
                var tip = "(" + r.data.provinceNo + " " + r.data.carrierNo + ")";
                //判断加载手机还是话费产品
                if (type) {
                    if (type == 1) {
                        loadPhoneProduct(value);
                        $('.rechagre_phone input').val(value + tip);
                    }
                    if (type == 2) {
                        loadFlowProduct(value);
                        $('.recharge_flow input').val(value + tip);
                    }
                }
                else {
                    if ($('.select_bar').attr("data-value") == "rechagre_phone") {
                        loadPhoneProduct(value);
                        $('.rechagre_phone input').val(value + tip);
                    }
                    if ($('.select_bar').attr("data-value") == "recharge_flow") {
                        loadFlowProduct(value);
                        $('.recharge_flow input').val(value + tip);
                    }
                }
            }
            else {
                $.toast("不支持的号码", "cancel");
            }
        });
    }


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
    //根据手机号码加载话费产品列表
    function loadPhoneProduct(phone) {
        client.ajax.ajaxRequest("/HWServer/MobileQuery.ashx", { sPhone: phone, interfaceType: 2, businessType: 1 }, function (r) {
            if (r.code == 100) {
                var array = r.data;
                productArray = array;
                var html = [];
                $(array).each(function () {
                    html.push('<li data-productId="' + this.productID + '" data-face="' + this.face + '">');
                    html.push('<p><b>' + this.face + '</b><a>元</a></p>');
                    html.push('<span data-payFee=' + this.payFee + '>售价:' + this.payFee + '</span>');
                    html.push('</li>');
                });
                if (array.length > 0) {
                    $('.rechagre_phone ul').html('').append(html.join(''));
                    $('.rechagre_phone ul').removeClass("unable_ul").addClass("able_ul");
                    //设置默认值面值100
                    var $li = $('.rechagre_phone ul li[data-face=100]');
                    $li.addClass("selectItem");
                    $(".paymoney p").eq(1).html("&yen;" + $li.find('span').attr("data-payFee"));
                    var selectList = $(".rechagre_phone .selectItem");
                    if (selectList.length == 1) {
                        $('#payrequest').css("background-color", "#F1682E");
                    }

                    //绑定点击事件
                    $('.rechagre_phone ul li').on("click", function () {
                        if ($('.rechagre_phone ul').hasClass("able_ul")) {
                            $(this).addClass("selectItem").siblings().removeClass("selectItem");
                            $(".paymoney p").eq(1).html("&yen;" + $(this).find('span').attr("data-payFee"));
                        }
                    });
                }
            } else {
                $.toast(r.msg, "cancel");
            }
        });
    }

    //话费点击购买事件
    $('#payrequest').on("click", function () {
        if ($(".rechagre_phone ul").hasClass("able_ul")) {
            var selectItem = $(".rechagre_phone .selectItem");
            if (selectItem.length == 1) {
                var phone = $('.rechagre_phone input').val().replace(/\D/g, ''); //获取充值手机号码
                var productId = selectItem.attr("data-productId");
                var item = getProduct(productId);
                var face = item.face;
                var fee = item.payFee;
                var flow_type = item.flowType == null ? "null" : item.flowType;
                var businessType = item.businessType;
                var product_name = "充话费-" + face + "元";
                localStorage.setItem("orderinfo", JSON.stringify({
                    businessType: businessType,
                    account: phone,
                    product_name: product_name,
                    mobile: phone, //充值的手机号
                    product_id: productId, //产品编号
                    face: face, //面值
                    price: fee, //售价,
                    card_type: 1, //充值类型 1-话费 2-流量
                    flow_type: flow_type//流量类型
                }));
                localStorage.setItem("recharge_type", 1); //缓存充值类型 1-话费 2-流量
                location.href = "wappayment.html";
            }
        }
    });




    //判断流量面值下面是否有活动
    function isActivity(array) {
        var ActivityValue = 1;
        for (var i = 0; i < array.length; i++) {
            if (array[i].activity != 1) {
                ActivityValue = array[i].activity;
                break;
            }
        }
        return ActivityValue;
    }

    var productObj = {}; //返回的产品对象
    var specificArray = []; //个性流量包数组
    var defaultSelected = []; //默认选中面值的数组
    var price = 0; //便宜的产品价格
    var priceFaceKey = '';
    //加载流量产品
    function loadFlowProduct(phone) {
        client.ajax.ajaxRequest("/HWServer/MobileQuery.ashx", { interfaceType: 2, sPhone: phone, businessType: 2 }, function (r) {
            if (r.code == 100) {
                productObj = {}; //清空对象
                specificArray = []; //清空数组
                defaultSelected = []; //清空数组
                price = 0;
                priceFaceKey = '';
                var array = r.data;
                var html = [];
                $(array).each(function () {
                    var faceKey = this.face < 1024 ? this.face + "M" : (this.face / 1024) + "G";
                    if (this.activity != 1) {
                        if (price == 0) price = this.payFee;
                        else {
                            if (this.payFee < price) {
                                price = this.payFee;
                                priceFaceKey = faceKey;
                            }
                        }
                    }
                    if (productObj[faceKey]) {
                        productObj[faceKey].push(this);
                    }
                    else {
                        //不存在
                        productObj[faceKey] = new Array(); //新建当前面值的数组
                        productObj[faceKey].push(this);
                    }
                    if (this.flowType == 2) //个性流量包
                        specificArray.push(this);
                    if (this.activity != 1 && $.inArray(faceKey, defaultSelected) == -1)
                        defaultSelected.push(faceKey);
                });
                if (specificArray.length > 0) productObj["0M"] = specificArray;
                //渲染html
                for (var key in productObj) {
                    var item = productObj[key][0]; //获取面值下面第一个对象
                    var keyArray = productObj[key];
                    var valueOrder = isActivity(keyArray);
                    html.push("<li>");
                    if (valueOrder != 1)//判断是否加活动图标
                        html.push("<img src='/HWServer/images/activity_" + valueOrder + ".png' />");
                    if (key != "0M") {
                        var s_face = "";
                        var s_dan = "";
                        if (key.split('M').length == 2) {
                            s_face = key.split('M')[0];
                            s_dan = "M";
                        }
                        if (key.split('G').length == 2) {
                            s_face = key.split('G')[0];
                            s_dan = "G";
                        }
                        html.push('<p data-key="' + key + '"><b>' + s_face + '</b><a>' + s_dan + '</a></p>');
                        html.push('<span>售价:' + item.fee + '元</span>');
                        html.push('</li>');
                    }
                    else {
                        html.push('<p style="margin-top: 22.5px; font-size:16px;" data-key="' + key + '">特惠流量包</p>');
                        html.push('</li>');
                    }
                }
                if (array.length > 0) {
                    $('.recharge_flow ul').html('').append(html.join(''));
                    $('.recharge_flow ul').removeClass("unable_ul").addClass("able_ul");
                    //设置选中的默认项
                    if (defaultSelected.length > 0) {
                        if (defaultSelected.length == 1)
                            $('.recharge_flow ul li p[data-key=' + defaultSelected[0] + ']').parent().addClass("selectItem");
                        else {
                            //多个推荐选中价格最便宜的面值
                            $('.recharge_flow ul li p[data-key=' + priceFaceKey + ']').parent().addClass("selectItem");
                        }
                    }
                    else {
                        if (productObj["1G"]) $('.recharge_flow ul li p[data-key=1G]').parent().addClass("selectItem");
                        else $('ul li').eq(0).addClass("selectItem");
                    }
                    //绑定面值点击事件
                    $('.recharge_flow ul li').on("click", function () {
                        if ($('.recharge_flow ul').hasClass("able_ul") && (!$(this).hasClass("selectItem") || click)) {
                            click = false;
                            $(this).addClass("selectItem").siblings().removeClass("selectItem");
                            var facekey = $(this).find('p').attr("data-key").trim(); //获取面值Key
                            var detailHtml = [];
                            if (facekey != "0M" && productObj[facekey].length > 1) {
                                //非个性包排序//按businessType升序
                                var compare = function (obj1, obj2) {
                                    var val1 = Number(obj1.businessType);
                                    var val2 = Number(obj2.businessType);
                                    if (val1 < val2) {
                                        return -1;
                                    } else if (val1 > val2) {
                                        return 1;
                                    } else {
                                        return 0;
                                    }
                                }
                                productObj[facekey].sort(compare);
                            }
                            for (var i = 0; i < productObj[facekey].length; i++) {
                                var item = productObj[facekey][i];
                                detailHtml.push('<div class="weui-cell line">');
                                detailHtml.push('<div class="weui-cell__bd">');
                                detailHtml.push('<div style="float:left; width:85% ">');
                                detailHtml.push('<span style="font-weight:500">' + item.fee + '元</span>');
                                detailHtml.push('&nbsp;<span style="font-size:12px;color:#a1a1a1">原价:<del>' + item.orignFee.toFixed(2) + '</del></span>');
                                if (item.promoteMsg)
                                    detailHtml.push(' &nbsp;<span class="icon_cash">' + item.promoteMsg + '</span>');
                                detailHtml.push("</br>");
                                if (facekey == "0M") {
                                    var useArea = item.face < 1024 ? item.face + "M" : (item.face / 1024) + "G";
                                    useArea = useArea + item.useArea
                                    detailHtml.push('<span style="font-size:12px;background-color:#F1682E;color:White;padding:2px 5px;border-radius:4px;">' + useArea + '</span>');
                                }
                                else
                                    detailHtml.push('<span style="font-size:12px;background-color:#F1682E;color:White;padding:2px 5px;border-radius:4px;">' + item.useArea + '</span>');
                                detailHtml.push('&nbsp;<span style=" font-size:12px; color:#a1a1a1">' + item.remark + '</span>');
                                detailHtml.push('</div>');
                                detailHtml.push('<div style="float:right;width:15%;text-align:center;right:0px;">');
                                detailHtml.push('<a class="weui-btn weui-btn_plain-primary" data-key=' + facekey + '>购 买</a>');
                                detailHtml.push('</div>');
                                detailHtml.push('</div>');
                                detailHtml.push('</div>');
                            }
                            $('.detailinfo').html(detailHtml.join(''));
                            bingEvent(phone); //绑定购买事件
                        }
                    });
                    //默认项的点击
                    var click = true;
                    $('.selectItem').click();
                }
            } else {
                $.toast(r.msg, "cancel");
            }
        });
    }

    //绑定流量购买事件
    function bingEvent(phone) {
        if ($('.detailinfo a').length > 0) {
            $('.detailinfo a').on("click", function () {
                var key = $(this).attr("data-key");
                var selectIndex = $(this).parents(".weui-cell").index(); //选中购买的项    
                var item = productObj[key][selectIndex];
                //订单信息
                if (key == "0M") key = item.face < 1024 ? item.face + "M" : (item.face / 1024) + "G";
                var product_name = item.useArea + key;
                localStorage.setItem("orderinfo", JSON.stringify({
                    businessType: item.businessType,
                    account: phone, //华为生活账号
                    product_name: product_name,
                    mobile: phone, //充值的手机号
                    product_id: item.productID, //产品编号
                    face: item.face, //面值
                    price: item.payFee, //售价,
                    card_type: 2, //充值类型 1-话费 2-流量
                    flow_type: item.flowType//流量类型
                }));
                localStorage.setItem("recharge_type", 2); //缓存充值类型
                location.href = "wappayment.html";
            });
        }
    }

});