function toggleHourChkBox(dp) {
    if ($("#e").val() == getCurrentDate()) { $("#h").parent().show().next('label').show(); }
    else { $("#h").removeAttr("checked").parent().hide().next('label').hide(); }
}
$(function () {
    //        $("#btnExport").click(function () {
    //            if ($("#e").val() == "") {
    //                alert("必须选择查询起始日期");
    //                return false;
    //            }
    //            exportExcel();

    //        });
    $(".Wdate").val(getCurrentDate());

    var order = new MRCenterOrder();
    order.bind();
    toggleHourChkBox();
    if (Request.QueryString("pi") && !Request.QueryString("h")) $("#h").removeAttr("checked");
    else $("#h").attr("checked", "checked");

    //鼠标点击该行背景色
    RowClickBackColor();
});

function partOrder() {
    if ($("#w").is(":checked")) {
        $(".operation").text("-");
    }
}
//备注
function Remark(id) {
    var html = $.ajax({ url: "/MainOrder/Remark/" + id + "?" + Math.random(), async: false }).responseText;
    art.dialog({
        content: html,
        title: '备注',
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

//添加亏损
function SetLoss(id) {
    var html = $.ajax({ url: "/MainOrder/SetLoss/" + id + "?" + Math.random(), async: false }).responseText;
    art.dialog({
        content: html,
        title: '添加亏损',
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

//投诉/重新投诉
function ComplaintAdd(id) {
    var html = $.ajax({ url: "/MainOrder/ComplaintAdd/" + id + "?" + Math.random(), async: false }).responseText;
    art.dialog({
        content: html,
        title: '投诉',
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

//处理
function ComplaintDeal(id) {
    var html = $.ajax({ url: "/MainOrder/ComplaintDeal/" + id + "?" + Math.random(), async: false }).responseText;
    art.dialog({
        content: html,
        title: '处理',
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

function submitDialog2(controller, action, data, id) {
    if (!data)
    { data = { id: id }; }
    $.ajax({
        type: "post",
        data: data,
        url: "/" + controller + "/" + action,
        success: function (data) {
            var obj = eval("(" + data + ")");
            if (obj.Status) {
                alert(obj.Message);
                $("#liftcard").text("---");
                return true;
            }
            else {
                alert("操作失败");
            }
        },
        error: function () {
            alert("调用操作失败");
        }
    });
}

//移库
function move(id, obj) {
    $.ajax({
        type: "post",
        data: "orderNo=" + id,
        url: "/MainOrder/Move",
        success: function (data) {
            data = $.parseJSON(data);
            if (data.Status) {
                $(this).remove();
            }
            alert(data.Message);
        },
        error: function () {
            alert("调用操作失败");
        }
    });
}