//******************系统框架及菜单设置**********************//

//点击头部的模块切换
function changeModule(moduleID, _a) {
    $.cookie('moduleID', moduleID);

    $("#leftMenu .con_left_menu").hide();
    $("#leftMenu .con_left_menu[module=" + moduleID + "]").show();

    $(".nav_list_right a.nav_select").attr("class", "nav1");
    $(_a).attr("class", "nav_select");
}

//调整窗口大小
function resizeWindow() {
    var h = $(window).height() - 110;
    var content_body_h = $(window).height() - 137;
    $(".con_left").height(h);
    $(".content_body").height(content_body_h);
}


//获取当前时间
function getCurrentDateTime() {
    var dayarray = new Array("星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六")
    var montharray = new Array("1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月")

    var mydate = new Date();
    var year = mydate.getYear();
    if (year < 1000) year += 1900;
    var day = mydate.getDay();
    var month = mydate.getMonth();
    var daym = mydate.getDate();
    if (daym < 10) daym = "0" + daym;
    var hours = mydate.getHours();
    var minutes = mydate.getMinutes();
    var seconds = mydate.getSeconds();
    if (hours < 10) hours = "0" + hours;
    if (minutes <= 9) minutes = "0" + minutes;
    if (seconds <= 9) seconds = "0" + seconds;
    var cdate = "日期:" + year + "年" + montharray[month] + daym + "日 " + dayarray[day] + " " + hours + ":" + minutes + ":" + seconds;
    return cdate;
}
var WindowForm = {
    cookie: 'navSequence',
    navTab: '#navTab',
    menuTab: '.con_left_nav',
    maxNum: 10,
    url: 'url', //href被"void(0)"替换掉，避免URL跳转 (匹配查找对应的标签也将使用此属性)
    InitTab: function () {
        $.cookie(this.cookie, null);
        var that = this;
        //顶部标签
        $(this.navTab + " div.first_li_span2 span").live({
            click: function () { //单击获取焦点
                that.ShowTab($(this).parent().parent());
            },
            dblclick: function () { //双击关闭本标签
                that.DeleteCurrentTab();
            }
        });
        //左边导航链接
        $(this.menuTab + " li a").each(function () {
            $(this).attr(that.url, $(this).attr('href')).attr("href", "javascript:void(0)");
            this.onclick = function (event) {
                that.AppendTab($(this).text(), $(this).attr(that.url), event);
                return;
            };
        });
        //默认第一个标签是选中状态
        //$("li.only_one_li span").trigger("click");
    },
    //插入一个Tab
    AppendTab: function (title, src, eve) { //内部ifrmae中的a[target=_blank]显式调用了此方法
        //没有按shift键，检查是否已经存在同一个URL的标签
        if (!this.IsShiftKeyPreDown(eve)) {
            var theSameTab = $(this.navTab).find("span[url=" + src + "]");
            if (theSameTab.length > 0) { //存在，获得焦点，并返回
                this.ShowTab($(theSameTab).parent().parent());
                return;
            }
        }
        //否则插入新标签
        var _ul = this.navTab + " ul";
        if ($(_ul + " li").length >= this.maxNum) {
            //$.cookie(this.cookie, null);
            this.RemoveOldTab();
            //alert("不能再开更多的标签啦");
            //return;
        }
        var _li = $(_ul + " li").eq(0).clone(true);
        _li.find("span").text(title).attr(this.url, src);
        $(_ul).append(_li);

        var _iframe = $("div.content_body div.frame_content").eq(0).clone(true);
        _iframe.find("iframe").attr("src", src);
        $(".content_body").append(_iframe);

        this.ShowTab(_li);
    },
    tabList: new Array("/mainorder/mainorderlist"),
    filterTabList: function (path) {
        for (var i = 0; i < this.tabList.length; i++) {
            if (WindowForm.tabList[i] == path) {
                WindowForm.tabList.splice(i, 1);
            }
        }
    },
    appendTabList: function (path) {
        if (!path) return;
        this.filterTabList(path);
        this.tabList.push(path);
    },
    RemoveTab: function (index) {
        var frameContent = $(".content_body>div").eq(index);
        frameContent.remove();
        $(this.navTab + " ul li").eq(index).remove();
    },
    //删除最早未点击标签
    RemoveOldTab: function (path) {
        if (path) {
            this.filterTabList(path);
        } else {
            path = this.tabList.shift();
        }
        var index = $(this.navTab).find("span[url=" + path + "]").closest('li').index();
        $(".content_body>div").eq(index).remove();
        $(this.navTab + " ul li").eq(index).remove();
        //$(this.frame_content).find("iframe[src='" + path + "']").remove();
        //$(this.navTab).find("span[url=" + path + "]").closest('li').remove();
        this.PopNaviArr();
    },

    //删除第一个标签
    DeleteFirstTab: function () {
        var _lis = $(this.navTab + " ul li");
        if (_lis.length <= 1) { return; }
        var firstTab = $(_lis).eq(0);
        var firstTabUrl = $(".content_body>div").eq(0);
        firstTab.remove(); //移除第一个标签
        firstTabUrl.remove();
        this.PopNaviArr();
        if (this.GetNaviArrLength() > 0) {
            this.ShowTab($(this.navTab + " ul li:has(span[url='" + this.GetNaviArrFirstMember() + "'])"));
            return;
        }
    },

    //删除
    DeleteCurrentTab: function () {
        var _lis = $(this.navTab + " ul li");
        if (_lis.length <= 1) { return; }
        var curLi = $(this.navTab + " ul li[class$='_select']");
        var _pos = curLi.index();
        var frameContent = $(".content_body>div").eq(_pos);
        frameContent.remove();
        curLi.remove(); //移除当前选中项
        this.PopNaviArr();
        if (this.GetNaviArrLength() > 0) {
            this.ShowTab($(this.navTab + " ul li:has(span[url='" + this.GetNaviArrLastMember() + "'])"));
            return;
        }
        if (_pos == 0) { //他是第一个
            this.ShowTab(_lis.eq(1));
        } else {
            this.ShowTab(_lis.eq(_pos - 1));
        }
        this.filterTabList($(curLi).find('span').attr('url'));
    },
    //切换
    SwitchTab: function (direction) {
        var curLi = $(this.navTab + " ul li[class$='_select']");
        if (direction == "left") {
            if (curLi.prev().length == 0) return;
            this.ShowTab(curLi.prev());
            this.filterTabList($(curLi).prev().find('span').attr('url'));
        } else if (direction == "right") {
            if (curLi.next().length == 0) return;
            this.ShowTab(curLi.next());
            this.filterTabList($(curLi).next().find('span').attr('url'));
        }
    },
    //显示第几个
    ShowTab: function (showTab) {
        this.appendTabList($(showTab).find('span').attr('url'));
        var index = this.GetIndex(showTab);
        this.PushNaviArr(showTab.find("span[url]").attr('url')); //存入选项卡顺序
        this.Show(index, showTab);
        //高亮焦点标签及显示对应的所有应该显示的项
        this.HightLight(showTab);
    },
    Show: function (index, showTab) {
        var length = $(showTab).parent().children().length;
        $(showTab).parent().find("li").each(function (i) {
            if (i == 0 && length == 1) {
                $(this).attr("class", "only_one_li");
            }
            else {
                //选中
                if (index == i) {
                    if (i == 0) {//第一个
                        $(this).attr("class", "first_li_select");
                    }
                    if (i > 0 && i < length - 1) {//中间元素
                        $(this).attr("class", "middle_li_select");
                    }
                    if (i == length - 1) {//最后一个
                        $(this).attr("class", "last_li_select");
                    }
                } //未选中
                else {
                    if (i == 0) {
                        $(this).attr("class", "first_li");
                    }
                    if (i > 0 && i < length - 1) {
                        $(this).attr("class", "middle_li_unselect");
                    }
                    if (i == length - 1) {
                        $(this).attr("class", "last_li");
                    }
                }
            }
        });
        $(".content_body>.frame_content").hide();
        $(".content_body>.frame_content").eq(index).show();
    },
    //高亮菜单项并展示它对应的父项和关联项
    HightLight: function (curTab) {
        var _hlClass = "text_select"; //Hight Light Class
        $(this.menuTab + " li a." + _hlClass).removeClass(_hlClass);
        var _href = curTab.find('span').attr(this.url) || "";
        var _curSelectMenu = $(this.menuTab + " li a[" + this.url + "='" + _href + "']");
        if (_curSelectMenu.length > 0) _curSelectMenu.addClass(_hlClass);
        if (_curSelectMenu.is(":hidden")) { //把它和它父籍显示出来
            _curSelectMenu
                .parentsUntil("div.con_left").show()
                .last().attr("TempSign", "1")
                .siblings("div.con_left_menu").hide(); //隐藏其他Module
            var _moduelTxt = $("div.con_left_menu[TempSign]").removeAttr("TempSign").children().eq(0).text();

            $(".nav_list_right a.nav_select").attr("class", "nav1");
            if (_moduelTxt) $(".nav_list_right a:contains('" + _moduelTxt + "')").attr("class", "nav_select");
        }
    },
    GetIndex: function (input) {
        return $(input).index();
    },

    //判断Shift键是否被按下
    IsShiftKeyPreDown: function (e) {
        var eve = window.event || e;
        if (eve.shiftKey) return true;
        return false;
    },
    //判断Alt键是否被按下
    IsAltKeyPreDown: function (e) {
        var eve = window.event || e;
        if (eve.altKey) return true;
        return false;
    },
    //判断Ctrl键是否被按下
    IsCtrlKeyPreDown: function (e) {
        var eve = window.event || e;
        if (eve.ctrlKey) return true;
        return false;
    },
    PushNaviArr: function (val) {
        var _cookie = $.cookie(this.cookie);
        if (_cookie && typeof _cookie == 'string') {
            var _arr = _cookie.split(',');
            for (var i = 0, len = _arr.length; i < len; i++) {
                if (_arr[i] == val) {
                    _arr.splice(i, 1);
                    break;
                }
            }
            _arr.push(val);
            $.cookie(this.cookie, _arr.join(','));
        }
        else {
            $.cookie(this.cookie, val);
        }
    },
    PopNaviArr: function () {
        var _cookie = $.cookie(this.cookie);
        if (_cookie && typeof _cookie == 'string') {
            var _arr = _cookie.split(',');
            _arr.pop();
            $.cookie(this.cookie, _arr.join(','));
        }
    },
    GetNaviArrLength: function () {
        var _cookie = $.cookie(this.cookie);
        if (_cookie && typeof _cookie == 'string') {
            return _cookie.split(',').length;
        }
        return 0;
    },
    GetNaviArrLastMember: function () {
        var _cookie = $.cookie(this.cookie);
        if (_cookie && typeof _cookie == 'string') {
            var _arr = _cookie.split(',');
            return _arr[_arr.length - 1];
        }
        return null;
    }
};