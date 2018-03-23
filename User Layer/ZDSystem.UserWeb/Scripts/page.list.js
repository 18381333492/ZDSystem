$(function () {
    $.paging.bind();
    $("input[name=qsubmit]").click(function () {
            $.paging.submit();
    });
});
//获取URL传递参数
Request = {
    QueryString: function (item) {
        var svalue = location.search.match(new RegExp("[\?\&]" + item + "=([^\&]*)(\&?)", "i"));
        return svalue ? svalue[1] : svalue;
    }
}