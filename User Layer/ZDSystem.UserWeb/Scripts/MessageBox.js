///****************页面弹出消息提示框*******************///

/*********************顶部消息框***********************/
var messagePop = {
    //msgWin: "#msg_win", //弹窗DIV
    userMsgBox: "#userMsg", //显示未处理的消息数量的box
    popMsgWrapper: "popMsgWrapper", //弹窗内容容器的ID
    centrPopDialog: null, //屏幕中间弹窗
    isPop: false, //是否弹窗

    //消息初始化
    initMessagePop: function () {
        //$(window).resize(function () { that.setTipsLocation(); });
        //$(window).scroll(function () { that.setTipsLocation(); });
        //this.setTipsLocation();
        $(this.userMsgBox).click(function (event) {
            WindowForm.AppendTab("我的消息", $("#MsgUrl").val(), event);
        });
        this.fillMsgBox();
    },
    //弹出在屏幕中间
    popToCentrScreen: function (msg) {
        var conHtml = "<span id='" + this.popMsgWrapper + "'>" + msg + "</span>";
        if (this.centrPopDialog == null) {
            this.centrPopDialog = art.dialog({
                title: '消息提示',
                content: conHtml,
                close: function () {
                    this.hide();
                    return false;
                }
            });
        } else {
            this.centrPopDialog.content(conHtml).show();
        }
    },
    //弹出右下角信息窗口
    popToRightCorner: function (msg) {
        this.preLoadSlipWindow();
        var conHtml = "<span id='" + this.popMsgWrapper + "'>" + msg + "</span>";
        art.dialog.notice({
            title: '您有新消息',
            width: 170, // 必须指定一个像素宽度值或者百分比，否则浏览器窗口改变可能导致artDialog收缩
            content: conHtml,
            icon: 'warning',
            time: 5
        });
    },
    //构造弹窗的内容HTML
    buildMsgContent: function (_msgNum) {
        var contentHTML = "<p id='{0}'>"
            + "<span class='hand' onclick='WindowForm.AppendTab(\"我的消息\",  $(\"#MsgUrl\").val(),event);'>你有消息：<a>{1}</a></span><br/>"
            + "</p>"; //" + this.popMsgWrapper + "
        if (_msgNum > 0) return this.formatStr(contentHTML, this.popMsgWrapper, _msgNum);
        else return null;
    },
    //显示并设置消息箱
    showAndSetMsgBox: function (_msgNum) {
        //待处理消息
        if (_msgNum > 0) $(this.userMsgBox).html("&nbsp;" + _msgNum);
        else $(this.userMsgBox).html("");

        var _msg = this.buildMsgContent(_msgNum);
        if (_msg && this.isPop==0) this.popToRightCorner(_msg);
    },
    //填充消息窗
    fillMsgBox: function () {
        var selfFun = arguments.callee;
        var that = this;
        $.ajax({
            url: "/System/GetMsgSituation/?" + Math.random(),
            success: function (data) {
                if (!that.judgeLoginStatus(data)) { return; }

                var _data = $.parseJSON(data);

                if (!(typeof _data == 'object' && _data != null)) { return; }
                var _msgNum = _data.MsgNum; //待处理消息数
                var _reqestTime = _data.ReqestTime; //重复获取消息时间间隔
                var _recvType = _data.RecvType; //提示类型
                if (!that.isNum(_msgNum, _reqestTime, _recvType)) { return; }
                if (_reqestTime == -1) { return; } //没有该用户，不用再获取它的消息

                that.isPop = _recvType;
                that.showAndSetMsgBox(_msgNum);
                window.setTimeout(function () { selfFun.call(that); }, _reqestTime * 1000);
            },
            error: function () {
                //throw new Error("获取待处理的消息失败，请刷新页面重试。");
            }
        });
    },
    //设置弹窗位置始终在右下角
    setTipsLocation: function () {
        if ($(this.msgWin).is(":hidden")) {
            return;
        }
        var bodyHeight = $(window).height();
        var boxHeight = $(this.msgWin).height();
        var topPosition = bodyHeight - boxHeight;
        var newPosition = $(document).scrollTop() + topPosition;
        $(this.msgWin).css({ "position": "fixed", "_position": "absolute", "top": "" + topPosition + "px", "_top": "" + newPosition + "px" });
    },
    //判断是否已经登录
    judgeLoginStatus: function (data) {
        if (data == 'nologin') {
            return false;
        }
        return true;
    },
    //检查是否是数字
    isNum: function () {
        for (var i in arguments) {
            if (typeof (arguments[i]) != 'number') {
                return false;
            }
        }
        return true;
    },
    //模拟C# Format方法
    formatStr: function (_originalStr) {
        for (var i = 0, len = arguments.length; i < len; i++) {
            _originalStr = _originalStr.replace("{" + i + "}", arguments[i + 1]);
        }
        return _originalStr;
    },
    //预加载右下角滑动弹窗的参数(调用弹窗前必须被先调用)
    preLoadSlipWindow: function () {
        artDialog.notice = function (options) {
            var opt = options || {}, api, aConfig, hide, wrap, top, duration = 800;

            var config = {
                id: 'Notice',
                left: '100%',
                top: '100%',
                fixed: true,
                drag: false,
                resize: false,
                follow: null,
                lock: false,
                init: function (here) {
                    api = this;
                    aConfig = api.config;
                    wrap = api.DOM.wrap;
                    top = parseInt(wrap[0].style.top);
                    hide = top + wrap[0].offsetHeight;

                    wrap.css('top', hide + 'px')
                    .animate({ top: top + 'px' }, duration, function () {
                        opt.init && opt.init.call(api, here);
                    });
                },
                close: function (here) {
                    wrap.animate({ top: hide + 'px' }, duration, function () {
                        opt.close && opt.close.call(this, here);
                        aConfig.close = $.noop;
                        api.close();
                    });

                    return false;
                }
            };

            for (var i in opt) {
                if (config[i] === undefined) config[i] = opt[i];
            };

            return artDialog(config);
        };
    }
};