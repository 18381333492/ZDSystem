$.extend({
    //去除二端空白
    trim: function (str) {
        var m = str.match(/^\s*(\S+(\s+\S+)*)\s*$/);
        return (m == null) ? "" : m[1];
    },
    //判断是否手机号
    isMobile: function (str) {
        return (/^(13[0-9]|15[0-9]|18[0-9])\d{8}$/.test($.trim(str)));
    },
    //判断是否电话号码
    isTel: function (str) {
        return (/^[\d|\-|\(|\)]{0,20}$/.test($.trim(str)));
    },
    //判断是否手机或电话号码
    isMobileTel: function (str) {
        return $.isTel(str) || $.isMobile(str);
    },
    //判断是否邮箱
    isEmail: function (str) {
        return (/^(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)$/.test($.trim(str)));
    },
    //判断是否为数字
    isNumber: function (str) {
        return (!isNaN($.trim(str)));
    },
    //判断正整数
    isPositive: function (str) {
        return (/^[0-9]*[1-9][0-9]*$/.test($.trim(str)));
    },
    //判断正浮点数
    isNaFloat: function (str) {
        return (/^\d+(\.\d+)?$/.test($.trim(str)));
    },
    //判断邮政编码
    isPostalCode: function (str) {
        return (/^\d{6}$/.test($.trim(str)));
    },
    //是否身份证号码
    isCardID: function (str) {
        return (/^\d{15}$|^\d{18}$/.test($.trim(str)));
    },
    //是否QQ
    isQQ: function (str) {
        return (/[1-9][0-9]{4,}/.test($.trim(str)));
    },
    //是否图片
    isPicture: function (str) {
        return (/\.(jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga)/.test($.trim(str)));
    }
})

String.prototype.startWith = function (str) {
    if (str == null || str == "" || this.length == 0 || str.length > this.length)
        return false;
    if (this.substr(0, str.length) == str)
        return true;
    else
        return false;
    return true;
}
String.prototype.endWith = function (str) {
    if (str == null || str == "" || this.length == 0 || str.length > this.length)
        return false;
    if (this.substring(this.length - str.length) == str)
        return true;
    else
        return false;
    return true;
}
//刷新当前页面
function refreshCurrentPage() {
    var path = window.location.toString();
    if (path.endWith("#")) {
        path = path.substr(0, path.length - 1);
    }
    window.location = path;
}
//刷新当前Iframe
function refreshInframe() {
    if ($('iframe[contag]:visible').length > 0) {
        var _iframe = $('iframe[contag]:visible');
        _iframe.attr("src", _iframe.attr("src"));
    }
}
//**********Page Validation************//
//验证码更换图片
function reloadcode() {
    document.getElementById("authImage").src = '/ajax/ValidateCode.ashx?' + (new Date()).getTime();
}
//格式化日期
Date.prototype.format = function (format) {
    /*
    * eg:format="YYYY-MM-dd hh:mm:ss";
    */
    var o = {
        "M+": this.getMonth() + 1,  //month
        "d+": this.getDate(),     //day
        "h+": this.getHours(),    //hour
        "m+": this.getMinutes(),  //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter
        "S": this.getMilliseconds() //millisecond
    }

    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}

//获取当前时间(到分钟)
function getCurrentMinute() {
    var date = new Date();
    return date.format("yyyy-MM-dd hh:mm");
}

//获取上半小时时间(分钟)
function getPreviousMinute(pval) {
    var date = new Date();
    date.setMinutes(date.getMinutes() - pval)
    return date.format("yyyy-MM-dd hh:mm");
}

///获取当前日期
function getCurrentDate() {
    var date = new Date();
    return date.format("yyyy-MM-dd");
}

///获取昨天的日期
function getPreviousDate() {
    var date = new Date();
    date.setDate(date.getDate() - 1)
    return date.format("yyyy-MM-dd");
}

//复制功能
function dbcopy(obj) {
    var copycontent = $.trim($(obj).text());
    if (window.clipboardData) {
        window.clipboardData.clearData();
        window.clipboardData.setData("Text", copycontent);
    } else if (navigator.userAgent.indexOf("Opera") != -1) {
        window.location = copycontent;
    } else if (window.netscape) {
        try {
            netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
        } catch (e) {
            alert("您的当前浏览器设置已关闭此功能！请按以下步骤开启此功能！\n新开一个浏览器，在浏览器地址栏输入about:config并回车。\n然后找到'signed.applets.codebase_principal_support'项，双击后设置为'true'。\n声明：本功能不会危极您计算机或数据的安全！");
        }
        var clip = Components.classes['@mozilla.org/widget/clipboard;1'].createInstance(Components.interfaces.nsIClipboard);
        if (!clip) return false;
        var trans = Components.classes['@mozilla.org/widget/transferable;1'].createInstance(Components.interfaces.nsITransferable);
        if (!trans) return false;
        trans.addDataFlavor('text/unicode');
        var str = new Object();
        var len = new Object();
        var str = Components.classes["@mozilla.org/supports-string;1"].createInstance(Components.interfaces.nsISupportsString);
        var copytext = copycontent;
        str.data = copytext;
        trans.setTransferData("text/unicode", str, copytext.length * 2);
        var clipid = Components.interfaces.nsIClipboard;
        if (!clip) return false;
        clip.setData(trans, null, clipid.kGlobalClipboard);
    }
    return true;
}

//获取URL传递参数
Request = {
    QueryString: function (item) {
        var svalue = location.search.match(new RegExp("[\?\&]" + item + "=([^\&]*)(\&?)", "i"));
        return svalue ? svalue[1] : svalue;
    }
}

// 为所有数据列表添加鼠标滑过效果
$(function () {
    RowClickBackColor();
    // 获取背景色(用获取背景色再还原回去是有问题的：因原来的TR没有设置背景)
    // 获取页面中的table元素的tr元素,并且tr元素中不包含th元素的hover事件
    // tr[tag=fixed]时,忽略此行
    $('tr', 'div.list_content table').hover(function () {
        if ($(this).is("[tag=fixed]")) return false;
        var _color = $(this).parents("table").attr('hovercolor') || '#ebf5f8';
        $(this).css('background-color', _color);
    }, function () {
        if ($(this).is("[tag=fixed]")) return false;
        $(this).css('background-color', '#FFFFFF');
    });

    //    //页面中所有a链接 targe=_blank的都打开新标签，而不是新窗口
    //    $('a[target="_blank"][href]').click(function (event) {
    //        if ($(this).attr('blank')) { return true; }
    //        window.parent.WindowForm.AppendTab($(this).text(), this.href, event);
    //        return false;
    //    });


    $('a[target="_blank"][href]').live('click', function (event) {
        if ($(this).is('a[target="_blank"][tag="blank"]')) {
            return true;
        }
        else {
            window.parent.WindowForm.AppendTab($.trim($(this).text()) || $(this).attr('title'), this.href, event);
            return false;
        }
    });
    //table的th浮动
    if ($("table[tag=float_th]").length > 0) {
        var $ths = $("table[tag=float_th] tr th");
        var $tds = $("table[tag=float_th] tr:eq(1) td");
        if ($ths.length > 0 && $tds.length > 0) {
            $tds.each(function (i) {
                var _width = $(this).width();
                $(this).css("width", _width);
                $ths.eq(i).css("width", _width);
            });
            var _offsetTop = $ths.offset().top;
            $(window).scroll(function () {
                var _scrollTop = $(window).scrollTop();
                if (_scrollTop >= _offsetTop) {
                    $("table[tag=float_th] tr:first").addClass("float_th");
                }
                else {
                    $("table[tag=float_th] tr:first").removeClass("float_th");
                }
            });
        }
    }
    //为Table增加虚拟编号，编号可连续
    //需要为Table增加属性VirSeq="true"，
    //可选属性：ThClass（虚拟编号列头样式）、TdClass（虚拟编号列样式）、ThTxt(TH列头文本)
    var pIndex = Request.QueryString("pi") || 0;
    var pSize = Request.QueryString("ps") || 10;
    $('table[VirSeq="true"]').each(function (tabIndex) {
        //已经添加了序号了
        if ($(this).attr('added') == 'true') return;

        var thClass = $(this).attr("ThClass") || "";
        var tdClass = $(this).attr("TdClass") || "";
        var thText = $(this).attr("ThTxt") || "编号";
        $(this).find("th").first().before("<th class='" + thClass + "'>" + thText + "</th>");
        $(this).find("tr").each(function (tdIndex) {
            $(this).find("td").first().before("<td class='" + tdClass + "'>" + (pIndex * pSize + tdIndex) + "</td>");
        });
        $(this).attr('added', 'true');
    });
});

function changeDataSelectType() {
    var lastMonth = new Date();
    lastMonth.setMonth(lastMonth.getMonth() - 1);
    var selectedDate = new Date();
    selectedDate.setYear($dp.cal.date.y);
    selectedDate.setMonth($dp.cal.date.M - 1);
    selectedDate.setDate($dp.cal.date.d);
    if (lastMonth > selectedDate) {
        $("#DataSelectType").attr("selectedIndex", 1);
    }
    else {
        $("#DataSelectType").attr("selectedIndex", 0);
    }
}

//鼠标点击该行背景色
function RowClickBackColor() {
    $("div.list_content tr").click(function () {
        //            if ($(this).is("[tag=fixed]")) // 点击的是它自己
        //                return false;
        if ($(this).siblings("[tag=fixed]").length > 0) {
            $(this).siblings("[tag=fixed]")
                    .css("background-color", "#FFFFFF")
                    .removeAttr("tag");
        }
        $(this).css("background-color", "#D3FF93").attr("tag", "fixed");
    })
}