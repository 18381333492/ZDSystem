
//产品页面
function initProduct(phone) {
    client.ajax.ajaxRequest("/HWServer/MobileQuery.ashx", { interfaceType: 2, sPhone: phone, businessType: 2 }, function (r) {
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
    gra_obj.count = 0;
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
    initGraDetailsTab(count);
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
        graProductStr += '<a class="weui-cell main_list" href="javascript:void(0);" onclick="showGraDetails(' + tab_id + ');">';

        var isteihui = false;
        for (var k = 0; k < gra_obj[key].length; k++) {
            if (gra_obj[key][k].activity == 0)
                isteihui = true;
        }
        if (!isteihui)
            graProductStr += '<div class="weui-cell__bd"><p>' + face + '</p></div>';
        else
            graProductStr += '<div class="weui-cell__bd"><p>' + face + '&nbsp;<img src="/hwserver/images/tehui.png" style="width: 42px;height: 20px;text-overflow: middle;vertical-align: text-top;"> </p></div>';
        
        graProductStr += '<div class="weui-cell__ft"><div style="float: left; margin: 0 auto;"><span style="color: green">' + gra_min_fee[key] + "元" + '</span></div>';
        graProductStr += '<div class="show_detail_btn"><img src="img/jt.png" /></div>';
        graProductStr += '</div></a></div>';
        //var tab_id = tab_id_prefix + i;
        graProductStr += '<div id="' + tab_id + '">';
        for (var k = 0; k < gra_obj[key].length; k++) {
            gra_obj[key][k].remark = gra_obj[key][k].remark.replace(/\s+/g, ",");
            var data_json = JSON.stringify(gra_obj[key][k]);
         //   data_json = '{"productID":"104720","businessType":10,"face":500,"fee":0.01,"flowType":2,"useArea":"全国日包","remark":"立即生效 24小时内有效","orignFee":10,"payFee":0.01,"rechargeMode":0}';
            graProductStr += '<div class="weui-cells" style=" margin-top:0px;">';
            graProductStr += '<div class="weui-cell" style="background-color: #eee">';
            graProductStr += '<div class="weui-cell__bd details_info">';
            graProductStr += '<div><span>' + gra_obj[key][k].fee + "元" + '</span></div><div class="details_info_des"><span style="background:#1296db;color:white;display:block; heigth:20px; padding-right:6px; padding-left:6px; padding-top:2px;padding-bottom:3px;float:left;border-radius:4px; magin-right:2px;">' + gra_obj[key][k].useArea + '</span><span style="magin-left:2px;height: 21px;display: inline-block;line-height: 21px;margin-left: 5px;">' + gra_obj[key][k].remark + '</span></div>';
            graProductStr += '</div>';
            graProductStr += '<div class="weui-cell__ft"><a href="javascript:void(0);"class="weui-btn weui-btn_plain-primary details_info_btn" data-orderinfo = ' + data_json + '>购买</a></div></div></div>';
        }
        graProductStr += '</div>';
        graProductStr += '</div>';
    }
    return graProductStr;
}


function buyClick() {
    $(".details_info_btn").on("click", function () {
        var info = $(this).attr("data-orderinfo");
        if (info) {
            info = JSON.parse(info);
            var orderinfo = localStorage.getItem("orderinfo");
            if (orderinfo) {
                var face = "";
                if (parseInt(info.face) % 1024 == 0) {
                    face = parseInt(info.face) / 1024 + "G";
                } else {
                    face = info.face + "M";
                }
                localStorage.setItem("show_mobile", $("input[name=mobile]").val());
                orderinfo = JSON.parse(orderinfo);
                orderinfo.product_name = info.useArea + face;
                orderinfo.mobile = $("input[name=mobile]").val().replace(/\D/g, '');
                orderinfo.product_id = info.productID;
                orderinfo.flow_type = info.flowType;
                orderinfo.card_type = 2;
                orderinfo.face = info.face;
                orderinfo.price = info.fee;
                orderinfo.businessType = info.businessType;
                localStorage.setItem("orderinfo", JSON.stringify(orderinfo));
                location.href = "/hwpayment.html";
            }
            else {
                $.toast("参数错误", "text");
            }
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
        graProductStr += '<div class="weui-cell__bd"><p>' + flowSize[i] + '</p></div>';
        graProductStr += '<div class="weui-cell__ft"><div style="float: left; margin: 0 auto;"></div>';
        graProductStr += '<div class="show_detail_btn"></div>';
        graProductStr += '</div></a></div>';
    }
    return graProductStr;
}


function LoadMobile(value) {
    client.ajax.ajaxRequest("/HWServer/MobileQuery.ashx", { sPhone: value, interfaceType: 1 }, function (r) {
        if (r.code == 100 && r.data.carrierNo) {
            $('#tip').val(r.data.provinceNo + r.data.carrierNo);
            $('#tip').css("color", "green");
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
    for (var i = 1; i <= count; i++) {
        var id = "#dire_tab_" + i;
        $(id).hide();
    }
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
    $("#nav_direcharge").attr("style", "color:green;background:white;");
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
    $("#nav_gratia").attr("style", "color:green;background:white;");
    $("#nav_direcharge").attr("style", "color:black;background:white;");
}   
                        
                            
                            
                                
                            
                    

            
                    
                    
                        
                            
                                
                                    
                                        
                                        
                                    
                                    
                                