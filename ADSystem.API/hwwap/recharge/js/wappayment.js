
$(function () {
    //初始化页面
    function initpage() {
        var payTip = localStorage.getItem("payTip");
        if (payTip) {
            var res = JSON.parse(localStorage.getItem("payTip"));
            //支付同步回调
            $.modal({
                text: "请确认支付已完成",
                buttons: [
              {
                  text: "已完成支付", onClick: function () {
                      //查询支付是否已完成
                      client.ajax.ajaxRequest("/HWServer/QueryPayState.ashx", res, function (r) {
                          localStorage.removeItem("payTip");
                          if (r.success) {
                              //支付成功
                              $('.PayContent').hide();
                              $('.PayError').hide();
                              $('.PaySuccess').show();
                          }
                          else {
                              //支付失败
                              $('.PayContent').hide();
                              $('.PayError').show();
                              $('.PaySuccess').hide();
                          }
                      });
                  }
              },
              {
                  text: "未支付重新下单", className: "default", onClick: function () {
                      localStorage.removeItem("payTip");
                      location.href = "wappay.html";
                  }
              }
                    ]
            });
        }
    }
    //页面初始化
    initpage();

    //1-支付宝 2-微信
    //支付事件绑定
    var orderInfo = localStorage.getItem("orderinfo");
    if (orderInfo == null) {
        return $.toast("参数错误", "cancel");
    } else {
        orderInfo = JSON.parse(orderInfo);
        $('.productname').text(orderInfo.product_name);
        $('.mobile').text(orderInfo.mobile);
        $('.price').text(parseFloat(orderInfo.price).toFixed(2) + "元");
    }
    $('.bing').on("click", function () {
        var pay_type = $('input[name=pay]:checked').val();
        if (orderInfo) {
            if (parseFloat(orderInfo.price) < 0.01)
                return $.toast("支付金额错误", "cancel");
            orderInfo.pay_type = pay_type;
            orderInfo.clientType = 2;
            $.showLoading("支付请求中");
            client.ajax.ajaxRequest("/HWServer/PayRequestHandler.ashx", { orderInfo: JSON.stringify(orderInfo) }, function (r) {
                $.hideLoading();
                if (r.success) {
                    localStorage.setItem("payTip", JSON.stringify({ orderno: r.orderno, appid: r.appid, account_type: r.account_type }));
                    if (orderInfo.pay_type == 2) {
                        //微信支付跳转链接
                        location.href = r.url;
                    }
                    else {
                        //支付宝支付表单提交
                        $('#zfb').html(r.url);
                    }
                }
                else {
                    $.toast(r.info, "cancel");
                }
            });
        }
    });
});
