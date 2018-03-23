//用于提供基础的页面数据验证
//作者:杨磊
//时间:2011/9/17
(function ($) {
    var __paging = function () {
        this.config = { enablepaging: true, pageid: '#pager', selector: 'input:submit', form: "form", pageSize: 10,
            pageIndexId: "#pi", recrdCountId: "#count", pageSizeId: "#ps", query: { pageIndexId: "pi" },
            onsubmit: function (t, o) { return true; }
        };
        this.pageIndex = 0;
        this.pageCount = 0;
        this.recrdCount = 0;
        //根据页面或URL参数初始化
        this.init = function () {
            $.each($.query.keys, function (key, value) {
                var obj = $($paging.config.form).find("[name=" + key + "]");
                if (value != null && value != "" && obj.attr("type") != null &&
                        obj.attr("type").toLowerCase() == "checkbox") {
                    var valArry = value.split(',');
                    $(obj).each(function () {
                        if ($.inArray($(this).val(), valArry) >= 0) {
                            $(this).attr("checked", "checked");
                        }
                    });
                }
                else if (value != null && value != "" && obj.attr("type") != null &&
                    obj.attr("type").toLowerCase() == "radio") {
                    var index = 0;
                    $(obj).each(function () {
                        index++;
                        if ($(this).val() == value) {
                            return false;
                        }
                    });
                    $(obj).eq(index - 1).attr("checked", true);
                }
                else {
                    obj.val($.query.get(key));
                }
            });
            var pi = $.query.get(this.config.query.pageIndexId);
            if (pi != "")
                this.pageIndex = isNaN(parseInt(pi)) ? 0 : parseInt(pi);
            else
                this.pageIndex = isNaN(parseInt($(this.config.pageIndexId))) ?
                0 : parseInt($(this.config.pageIndexId));
            this.config.pageSize = $(this.config.pageSizeId).val();
            this.recrdCount = $(this.config.recrdCountId).val();
            this.recrdCount = isNaN(parseInt(this.recrdCount)) ? 0 : parseInt(this.recrdCount);
            this.pageCount = Math.ceil(this.recrdCount / this.config.pageSize);

            $($paging.config.form).find("input:submit,input[submit]").click(function () {
                $paging.submit();
            });
        }
        this.submit = function () {
            $(this.config.pageIndexId).val("0");
            $(this.config.form).submit();
        };
        this.bind = function (config) {
            this.config = $.extend(this.config, config || {});
            this.init();
            $('select').each(function () {
                if ($(this).attr("id") == "ddlps") {
                    SelectHelper.init();
                }
                else {
                    if ($(this).css('display') == "none") {
                    }
                    else {
                        $(this).sSelect();
                    }
                }
            });
            if (!this.config.enablepaging || this.pageCount == 0)
                return;
            $(this.config.pageid).pagination(this.pageCount, {
                num_display_entries: 8,
                callback: this.paging,
                items_per_page: 1,
                current_page: this.pageIndex > this.pageCount ? this.pageCount - 1 : this.pageIndex
            });
        }
        this.paging = function (pageindex, jq, async) {
            $($paging.config.pageIndexId).val(pageindex);
            $($paging.config.form).submit();
        }
    }
    var $paging = new __paging();
    $.extend({ paging: $paging });
})(jQuery);