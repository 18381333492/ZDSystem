var VDT_nmsg = {
    '*': '任意字符',
    'UN': '中文，字母，数字的组合',
    'DM': '小数',
    'NUM': '整数',
    'CHR': '字母，数字',
    'EML': '电子邮件',
    'QQ': '4位以上数字',
    'ICD': '15或18位身份证号码',
    'CHN': '中文字符',
    'IP': 'IP地址(只支持IPV4)'
};


$(function () {
    $("input[vdt_type]").each(function () {
        var nmsg = $(this).attr("label");
        if ($(this).attr("info") == null)
            $(this).attr("info", "请输入" + nmsg);
        if ($(this).attr("error") == null)
            $(this).attr("error", "请输入" + VDT_nmsg[$(this).attr("vdt_type")]);
    });
    var nmsg = $("#__nmsg").val();
    if (nmsg != null && nmsg != "") {
        alert(nmsg);
    }
    $.verify.bind();
    $.paging.bind();
});

function check_input(msg) {
    var smsg = "";
    if (nmsg == null) {
        nmsg = "未选择任何项";
    }
    else {
        nmsg = "请选择" + msg;
        smsg = "是否提交" + msg;
    }
    var status = true;

    if ($("input[type=radio]").length == 0
    && $("input[type=checkbox]").length == 0) {
        return true;
    }

    $("input[type=radio]").each(function () {
        var name = $(this).attr("name");
        if ($("input[name=" + name + "][checked]").length == 0) {
            status = false;
            return status;
        }
    });
    if (!status) {
        alert(nmsg);
        return false;
    }
    count = $("input[type=checkbox]").length;
    checked = $("input[type=checkbox][checked]").length;
    if (count > 0 && checked == 0) {
        alert(nmsg);
        return false;
    }
    return confirm("是否提交" + smsg+"?");
}