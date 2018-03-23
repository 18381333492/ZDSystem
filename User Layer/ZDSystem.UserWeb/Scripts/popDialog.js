//删除信息
function popDelDialog(url, data, obj, func) {
    if (!confirm("你确认要删除该信息吗?")) {
        return false;
    }
    $.ajax({
        type: "post",
        data: data,
        url: url,
        success: function (data) {
            if (typeof (func) === "function") {
                func(data);
            } else {
                var msg = $.parseJSON(data);
                alert(msg.Message);
                if (msg.Status) {
                    $(obj).parent().parent().remove();
                }
            }
        },
        error: function () {
            alert("调用失败，请重试");
        }
    });
}

//弹出编辑页面
function popEditDialog(url, data, title) {
    var html = $.ajax({ url: url + "?" + data + "&" + Math.random(), async: false }).responseText;
    art.dialog({
        content: html,
        title: title,
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