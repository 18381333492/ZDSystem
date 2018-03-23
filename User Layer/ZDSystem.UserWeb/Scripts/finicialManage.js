//**************上游渠道***************//
//弹出上游渠道加款页面
function popAddUpMoney(id) {
    var html = $.ajax({ url: "/UpFundRecord/AddUpMoney/" + id + "?" + Math.random(), async: false }).responseText;
    art.dialog({
        content: html,
        title: '上游渠道加款',
        lock: true,
        ok: function () {
            $('#mainForm').submit();
            return false;
        },
        okVal: "提交",
        cancelVal: "取消",
        cancel: true
    });
}
// 弹出上游渠道减款页面
function popCutUpMoney(id) {
    var html = $.ajax({ url: "/UpFundRecord/CutUpMoney/" + id + "?" + Math.random(), async: false }).responseText;
    art.dialog({
        content: html,
        title: '上游渠道减款',
        lock: true,
        ok: function () {
            $('#mainForm').submit();
            return false;
        },
        okVal: "提交",
        cancelVal: "取消",
        cancel: true
    });
}
//保存上游渠道加款
function saveAddUpMoney() {
    var data = $("#mainForm").serialize();
    $.ajax({
        type: "post",
        data: data,
        url: "/UpFundRecord/SaveAddUpMoney",
        success: function (data) {
            var _data = $.parseJSON(data);
            if (_data.Status == true) {
                alert(_data.Message);
                $.paging.submit();
                return true;
            }
            else {
                alert(_data.Message + ",请重试");
            }
        },
        error: function () {
            alert("操作失败，请重试");
        }
    });
}
///保存上游渠道减款
function saveCutUpMoney() {
    var data = $("#mainForm").serialize();
    $.ajax({
        type: "post",
        data: data,
        url: "/UpFundRecord/SaveCutUpMoney",
        success: function (data) {
            var _data = $.parseJSON(data);
            if (_data.Status == true) {
                alert(_data.Message);
                $.paging.submit();
                return true;
            }
            else {
                alert(_data.Message + ",请重试");
            }
        },
        error: function () {
            alert("操作失败，请重试");
        }
    });
}

//**************下游渠道***************//


//弹出下游加款页面
function addDownMoney(id) {
    var html = $.ajax({ url: "/DownFundRecord/AddDownMoney/" + id + "?" + Math.random(), async: false }).responseText;
    art.dialog({
        content: html,
        title: '下游渠道加款',
        lock: true,
        ok: function () {
            $('#mainForm').submit();
            return false;
        },
        okVal: "提交",
        cancelVal: "取消",
        cancel: true
    });
}
//弹出下游减款页面
function CutDownMoney(id) {
    var html = $.ajax({ url: "/DownFundRecord/CutDownMoney/" + id + "?" + Math.random(), async: false }).responseText;
    art.dialog({
        content: html,
        title: '下游渠道减款',
        lock: true,
        ok: function () {
            $('#mainForm').submit();
            return false;
        },
        okVal: "提交",
        cancelVal: "取消",
        cancel: true
    });
}
///保存加款
function saveAddDownMoney() {
    var data = $("#mainForm").serialize();
    $.ajax({
        type: "post",
        data: data,
        url: "/DownFundRecord/SaveAddDownMoney",
        success: function (data) {
            var _data = $.parseJSON(data);
            if (_data.Status == true) {
                alert(_data.Message);
                $.paging.submit();
                return true;
            }
            else {
                alert(_data.Message + ",请重试");
            }
        },
        error: function () {
            alert("操作失败，请重试");
        }
    });
}



///保存减款
function saveCutDownMoney() {
    var data = $("#mainForm").serialize();
    $.ajax({
        type: "post",
        data: data,
        url: "/DownFundRecord/SaveCutDownMoney",
        success: function (data) {
            var _data = $.parseJSON(data);
            if (_data.Status == true) {
                alert(_data.Message);
                $.paging.submit();
                return true;
            }
            else {
                alert(_data.Message + ",请重试");
            }
        },
        error: function () {
            alert("操作失败，请重试");
        }
    });
}