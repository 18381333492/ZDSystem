$(function () {
    //页面初始化的时候进行
    $('input[name=mobile]').val(localStorage.getItem("recharge_mobile"));
    var ph = $('input[name=mobile]').val();
    if (ph.length == 13) {
        $('.weui-icon-clear').show();
        ph = ph.replace(/\D/g, '');
        LoadMobile(ph);
    }

    //清除输入框的值
    $('.weui-icon-clear').on("click", function () {
        $('input[name=mobile]').val('');
        $('input[name=mobile]').removeClass("mobile_focus");
        $(this).hide();
        //设置默认样式
        defaultStyle();
        //关闭效果    
        $('#tip').val("移动 联通 电信");
        $('#tip').css("color", "#ccc");
        $('input[name=mobile]').focus();
    });

    //默认样式
    function defaultStyle() {
        $('ul').removeClass("able_ul").addClass("unable_ul");
        $('ul li').removeClass("selectItem");
        $('.detailinfo').html('');
        $('ul li').last().remove();
    }

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
                $('#tip').css("color", "#CACACA");
            }
            if ($('ul').hasClass("able_ul")) {
                defaultStyle();
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
            $('.weui-icon-clear').hide()
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
            if (client.regex.isPhone(value)) {
                LoadMobile(value);
            }
            else {
                setTimeout(function () {
                    $.toast("手机号码格式错误", "text");
                }, 500);
            }
        }
    });

    //判断号码区段
    function LoadMobile(value) {
        client.ajax.ajaxRequest("/HWServer/MobileQuery.ashx", { sPhone: value, interfaceType: 1 }, function (r) {
            if (r.code == 100 && r.data.carrierNo) {
                $('#tip').val(r.data.provinceNo + r.data.carrierNo);
                $('#tip').css("color", "#FF4500");
                loadProduct(value);
            }
            else {
                $.toast("不支持的号码", "cancel");
            }
        });
    }

    //判断面值下面是否有活动
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
    //根据手机号码加载流量产品列表
    function loadProduct(phone) {
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
                        html.push('<p data-key="' + key + '">' + key + '</p>');
                        html.push('<span>售价:' + item.fee + '元</span>');
                        html.push('</li>');
                    }
                    else {
                        html.push('<p style="margin-top: 22.5px; font-size:16px;" data-key="' + key + '">特惠流量包</p>');
                        html.push('</li>');
                    }
                }
                if (array.length > 0) {
                    $('ul').html('').append(html.join(''));
                    $('ul').removeClass("unable_ul").addClass("able_ul");
                    //设置选中的默认项
                    if (defaultSelected.length > 0) {
                        if (defaultSelected.length == 1)
                            $('ul li p[data-key=' + defaultSelected[0] + ']').parent().addClass("selectItem");
                        else {
                            //多个推荐选中价格最便宜的面值
                            $('ul li p[data-key=' + priceFaceKey + ']').parent().addClass("selectItem");
                        }
                    }
                    else {
                        if (productObj["1G"]) $('ul li p[data-key=1G]').parent().addClass("selectItem");
                        else $('ul li').eq(0).addClass("selectItem");
                    }
                    //绑定面值点击事件
                    $('ul li').on("click", function () {
                        if ($('ul').hasClass("able_ul") && (!$(this).hasClass("selectItem") || click)) {
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
                                detailHtml.push('<div class="weui-cell">');
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
                                    detailHtml.push('<span style="font-size:12px;background-color:#FF4500;color:White;padding:2px 5px;border-radius:4px;">' + useArea + '</span>');
                                }
                                else
                                    detailHtml.push('<span style="font-size:12px;background-color:#FF4500;color:White;padding:2px 5px;border-radius:4px;">' + item.useArea + '</span>');
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

    //绑定购买事件
    function bingEvent(phone) {
        if ($('.detailinfo a').length > 0) {
            $('.detailinfo a').on("click", function () {
                var key = $(this).attr("data-key");
                var selectIndex = $(this).parents(".weui-cell").index(); //选中购买的项    
                var item = productObj[key][selectIndex];
                var show_mobile = client.string.insert(phone, 3, ' ');
                show_mobile = client.string.insert(show_mobile, 8, ' ');
                localStorage.setItem("recharge_mobile", show_mobile);
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
                location.href = "wappayment.html";
            });
        }
    }
});
