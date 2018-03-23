
//获取链接参数
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

//产品页面
function initProduct(phone) {
    client.ajax.ajaxRequest("/JSYHServer/server/ProductQuery.ashx", { interfaceType: 2, sPhone: phone, businessType: 2 }, function (r) {
        loadProduct(r.data);
    });
}

//加载产品
function loadProduct(array) {
    var dire_obj = {};
    var gra_obj = {};
    var dire_min_fee = {};
    var gra_min_fee = {}
    dire_obj.count = 0;
    $(array).each(function () {
        if (this.flowType == 1) {
            if (dire_obj["face_" + this.face] == undefined) {
                dire_obj["face_" + this.face] = [];
                dire_obj["face_" + this.face].push(this);
            }
            else {
                dire_obj["face_" + this.face].push(this);
            }
            var fee = this.fee;
            var face = this.face;
            if (dire_min_fee["face_" + face] == undefined) {
                dire_min_fee["face_" + face] = fee;
            } else {
                var old_fee = dire_min_fee["face_" + face];
                var new_fee = parseFloat(old_fee) > parseFloat(fee) ? parseFloat(fee) : parseFloat(old_fee);
                dire_min_fee["face_" + face] = new_fee;
            }

            dire_obj.count = parseInt(dire_obj.count) + 1;
        } else if (this.flowType == 2) {
            if (gra_obj["face_" + this.face] == undefined) {
                gra_obj["face_" + this.face] = [];
                gra_obj["face_" + this.face].push(this);
            }
            else {
                gra_obj["face_" + this.face].push(this);
            }

            var fee = this.fee;
            var face = this.face;
            if (gra_min_fee["face_" + face] == undefined) {
                gra_min_fee["face_" + face] = fee;
            } else {
                var old_fee = gra_min_fee["face_" + face];
                var new_fee = parseFloat(old_fee) > parseFloat(fee) ? parseFloat(fee) : parseFloat(old_fee);
                gra_min_fee["face_" + face] = new_fee;
            }
            gra_obj.count = parseInt(gra_obj.count) + 1;
        }
    });
    var count = dire_obj.count;
    var htmlStr = intGraProduct(dire_obj, dire_min_fee, "dire_tab_");
    $("#dire_tab").html(htmlStr);
    count = parseInt(count) + 1;
    initDireDetailsTab(count);
    count = gra_obj.count;
    htmlStr = intGraProduct(gra_obj, gra_min_fee, "gra_tab_");
    $("#gra_tab").html(htmlStr);
    count = parseInt(count) + 1;
    initDireDetailsTab(count);
    buyClick();
}

//默认页面
function initDefalutProduct() {
    var htmlDefaultStr = intDefaultGraProduct("dire_tab_");
    $("#dire_tab").html(htmlDefaultStr);
    htmlDefaultStr = intDefaultGraProduct("gra_tab_");
    $("#gra_tab").html(htmlDefaultStr);
}


//加载流量包产品
function intGraProduct(gra_obj, gra_min_fee, tab_id_prefix) {
    var graProductStr = '';
    var i = 0;
    var count = gra_obj.count;
    for (var key in gra_obj) {
        i = parseInt(i) + 1;
        if (key == "count") {
            continue;
        }
        var face = key.substring(5);
        if (parseInt(face) % 1024 == 0) {
            face = parseInt(face) / 1024 + "G";
        } else {
            face = face + "M";
        }
       var tab_id = tab_id_prefix + i;
        graProductStr += '<div class="product_group">';
        graProductStr += '<div class="weui-cells" style="background-size: 50px; margin-top: 0px;">';
        graProductStr += '<a class="weui-cell main_list" href="javascript:void(0);" >'; //onclick="showGraDetails(' + tab_id + ')
        graProductStr += '<div class="weui-cell__bd"><p class="face_name">' + face + '&nbsp;<span style="font-size:15px;color:#cacaca">(原价' + gra_obj[key][0].orignFee + '元)</span></p></div>';
        graProductStr += '<div class="weui-cell__ft">'; //<div style="float: left; margin: 0 auto;"><span style="color: #09b6f2">' + gra_min_fee[key] + "元" + '</span></div>
        //graProductStr += '<div class="show_detail_btn"><img src="jt.png" /></div>';
        graProductStr += '</div></a></div>';
        //var tab_id = tab_id_prefix + i;
        graProductStr += '<div id="' + tab_id + '" class="tabItem">';
        for (var k = 0; k < gra_obj[key].length; k++) {
            var data_json = JSON.stringify(gra_obj[key][k]);
            graProductStr += '<div class="weui-cells" style=" margin-top:0px;">';
            graProductStr += '<div class="weui-cell" style="background-color: #eee">';
            graProductStr += '<div class="weui-cell__bd details_info">';
            graProductStr += '<div><span>' + gra_obj[key][k].fee + "元" + '</span></div><div class="details_info_des"><span style="background:#1296db;color:white;display:block; heigth:20px; padding-right:6px; padding-left:6px; padding-top:2px;padding-bottom:3px;float:left;border-radius:4px; magin-right:2px;">' + gra_obj[key][k].useArea + '</span><span style="magin-left:2px;height: 21px;display: inline-block;line-height: 21px;margin-left: 5px;">' + gra_obj[key][k].remark + '</span></div>';
            graProductStr += '</div>';
            graProductStr += '<div class="weui-cell__ft"><a href="javascript:void(0);" style="border:1px solid #09b6f2" class="weui-btn details_info_btn" data-orderinfo = ' + data_json + '>购买</a></div></div></div>';
        }
        graProductStr += '</div>';
        graProductStr += '</div>';
    }


    graProductStr += '<div class="hw_bottom" style="font-size: 12px;padding-top: 3px;color: #0B346E; text-align: center; width: 100%;"><a  href="query.html">充值记录</a><div style="margin-bottom: 0px;"><a style="display: inline-block; color: #91989F;">©此服务由千行你我科技提供<img id="help_tip" align="absmiddle"; src="help_tip.png" style="width:18px;height: 18px;margin-bottom: 3.5px" /></a></div></div>';


    return graProductStr;
}


function buyClick() {
    $(".details_info_btn").on("click", function (e) {
        var mobile_type = GetQueryString("mobileType"); //获取手机类型
        var user_id = GetQueryString("user_id") || '';
        if (mobile_type) {
            localStorage.setItem("mobileType", mobile_type);
        }
        else {
            mobile_type = localStorage.getItem("mobileType");
        }
        if (!mobile_type || mobile_type == 0) {
            return dialog.alert("请用建设银行APP发起支付", null, 3000);
        }
        var phoneType = mobile_type == 1 ? "IOS" : "Android";
        var info = $(this).attr("data-orderinfo");
        if (info) {
            info = JSON.parse(info);
            var face = "";
            if (parseInt(info.face) % 1024 == 0) {
                face = parseInt(info.face) / 1024 + "G";
            } else {
                face = info.face + "M";
            }
            localStorage.setItem("recharge_mobile", $("input[name=mobile]").val());
            var phone = $("input[name=mobile]").val().replace(/\D/g, '');
            var product_name = info.useArea + face;
            //支付请求
            $.showLoading("支付请求中");
            client.ajax.ajaxRequest("/JSYHServer/server/BookOrder.ashx", {
                orderInfo: JSON.stringify({
                    businessType: info.businessType,
                    account: user_id + "-" + phoneType, //账号加手机类型
                    product_name: product_name,
                    mobile: phone, //充值的手机号
                    product_id: info.productID, //产品编号
                    face: info.face, //面值
                    price: info.payFee, //售价,
                    card_type: 2, //充值类型 1-话费 2-流量
                    flow_type: info.flowType, //流量类型
                    pay_type: 4//龙支付
                })
            }, function (r) {
                $.hideLoading();
                if (r.success) {
                    if (mobile_type == 1) {
                        //ios跳转APP龙支付
                        window.location = "mbspay://direct?" + r.data;
                    }
                    if (mobile_type == 2) {
                        //安卓跳转APP龙支付
                        mbspay.directpay(r.data);
                    }
                }
                else {
                    dialog.alert(r.info, null, 3000);
                }
            });
        }
        else {
            $.toast("参数错误", "text");
        }
    });
 }

//特惠流量产品默认加载页面
 function intDefaultGraProduct(tab_id_prefix) {
    var graProductStr = '';
    var flowSize = ["30M", "50M", "1G", "5G"]
    for (var i = 0; i < 4; i++) {
        var tab_id = tab_id_prefix + i;
        graProductStr += '<div class="product_group">';
        if (i == 1) {
            graProductStr += '<div class="weui-cells" style="background-size: 50px;margin-top: 0px;">';
        } else {
            graProductStr += '<div class="weui-cells" style="background-size: 50px; margin-top: 0px;">';
        }
        graProductStr += '<a class="weui-cell main_list" href="javascript:void(0);">';
        graProductStr += '<div class="weui-cell__bd"><p class="face_name">' + flowSize[i] + '</p></div>';
        graProductStr += '<div class="weui-cell__ft"><div style="float: left; margin: 0 auto;"></div>';
        graProductStr += '<div class="show_detail_btn"></div>';
        graProductStr += '</div></a></div>';
    }
    return graProductStr;
}


function LoadMobile(value) {
    client.ajax.ajaxRequest("/HWServer/MobileQuery.ashx", { sPhone: value, interfaceType: 1 }, function (r) {
        if (r.code == 100 && r.data.carrierNo) {
            $('#tip').val(r.data.provinceNo + r.data.cityNo + r.data.carrierNo);
            $('#tip').css("color", "#09b6f2");
            initProduct(value);
            if ($('.weui-bar__item--on').index() == 0) {
                dre_click_style();
            } else {
                gra_click_style();
            }
        }
        else {
            $.toast("无效的号码", "text");
        }
    });
}


//隐藏直充流量包产品详情
function initDireDetailsTab(count) {
//    for (var i = 1; i <= count; i++) {
//        var id = "#dire_tab_" + i;
//        $(id).hide();
//    }
}

//隐藏特惠流量包产品详情
function initGraDetailsTab(count) {
    for (var i = 1; i <= count; i++) {
        var id = "#gra_tab_" + i;
        $(id).hide();
    }
}

//展示直充流量包产品详情
function showDireDetails(id) {
    var target_id = "#dire_tab_" + id;
    var value = $(target_id).attr("style");
    if (value.indexOf("none") != -1) {
        $(target_id).show();
    } else {
        $(target_id).hide();
    }
}

//展示特惠流量包产品详情
function showGraDetails(target_id) {
    var value = $(target_id).attr("style");
    if (value.indexOf("none") != -1) {
        $('.product_group').find('div:last')
        $(target_id).show();
    } else {
        $(target_id).hide();
    }

}


function defalult_style() {
    $("#nav_direcharge").addClass("nav_defalut_show_line");
    $("#nav_gratia").addClass("nav_defalut_hide_line")
    $("#nav_direcharge").attr("style", "color:black;background:white;");
    $("#nav_gratia").attr("style", "color:black;background:white;");
}


function defalut_dre_click_style() {
    $("#nav_direcharge").removeClass("nav_defalut_hide_line");
    $("#nav_gratia").addClass("nav_defalut_hide_line");
    $("#nav_direcharge").addClass("nav_defalut_show_line");
    $("#nav_direcharge").attr("style", "color:black;background:white;");
    $("#nav_gratia").attr("style", "color:black;background:white;");
}

function dre_click_style() {
    $("#nav_direcharge,#nav_gratia").removeClass("nav_defalut_hide_line");
    $("#nav_direcharge,#nav_gratia").removeClass("nav_defalut_show_line");
    $("#nav_direcharge").removeClass("nav_hide_line");
    $("#nav_gratia").addClass("nav_hide_line");
    $("#nav_direcharge").addClass("nav_show_line");
    $("#nav_direcharge").attr("style", "color:#09b6f2;background:white;");
    $("#nav_gratia").attr("style", "color:black;background:white;");
}

function defalut_gra_click_style() {
    $("#nav_direcharge").addClass("nav_defalut_hide_line");
    $("#nav_gratia").addClass("nav_defalut_show_line");
    $("#nav_gratia").removeClass("nav_defalut_hide_line");
    $("#nav_gratia").attr("style", "color:black;background:white;");
    $("#nav_direcharge").attr("style", "color:black;background:white;");
}

function gra_click_style() {
    $("#nav_direcharge,#nav_gratia").removeClass("nav_defalut_hide_line");
    $("#nav_direcharge,#nav_gratia").removeClass("nav_defalut_show_line");
    $("#nav_direcharge").addClass("nav_hide_line");
    $("#nav_gratia").addClass("nav_show_line");
    $("#nav_gratia").removeClass("nav_hide_line");
    $("#nav_gratia").attr("style", "color:#09b6f2;background:white;");
    $("#nav_direcharge").attr("style", "color:black;background:white;");
}   
                        
                            
                            
                                
                            
                    

            
                    
                    
                        
                            
                                
                                    
                                        
                                        
                                    
                                    
                                