$(function () {
    var id = $("#hd_id").val();
    $(".menu_word").find("a[tag="+id+"]").addClass("menu_word_hover");
});

$(function () {
    $(".leftmenu_title").click(function () {
        var MenuLi = $(this).next("div");
        if ($(MenuLi).is(":hidden")) {
            $(MenuLi).show();
            $(this).removeClass("leftmenu_title_bg2");
            $(this).addClass("leftmenu_title_bg1");
        }
        else {
            $(MenuLi).hide();
            $(this).removeClass("leftmenu_title_bg1");
            $(this).addClass("leftmenu_title_bg2");
        }
    });
    var hid = $("#hm_id").val();
    var pnl = $("a[tag=" + hid + "]");
    pnl.parent().parent().show();
    pnl.attr("class", "a_col40");
    pnl.parent().addClass("leftmenu_li_bg");
    pnl.parent().parent().prev().removeClass("leftmenu_title_bg2");
    pnl.parent().parent().prev().addClass("leftmenu_title_bg1");
    $("input[title]").each(function () {
        $(this).val($(this).attr("title"));
        $(this).focus(function () {
            if ($(this).val() == $(this).attr("title")) {
                $(this).val("");
            }
        });
        $(this).blur(function () {
            if ($(this).val() == "") {
                $(this).val($(this).attr("title"));
            }
        });
    });
})