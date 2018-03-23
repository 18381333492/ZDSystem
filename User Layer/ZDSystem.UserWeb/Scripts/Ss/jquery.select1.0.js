jQuery.fn.sSelect = function (options) {
    return this.each(function () {

        var defaults = {
            defaultText: '==请选择=='
            , defaultWidth: 100 //下拉框宽度，单位:像素(px)
            , itemHeight: 20 //每一项的高度，单位:像素(px)
            , showSize: 15 //显示几个
            , readOnly: false //是否只读
        };

        //初始化
        var opts = $.extend(defaults, options);
        var $input = $(this);
        var $inputText = $('<input type="text" class="js-sSelect_input" />');
        var $containerDivText = $('<div class="js-sSelect_selectedTxt"></div>'); /*selectedTxt*/
        var $newUl = $('<ul class="js-sSelect_optContainer js-l-sSelect_optContainer"></ul>');  /*newList*/
        var $containerDiv = $('<div class="js-sSelect" tabindex="0"></div>'); /*newListSelected*/
        var currentIndex = -1;
        var newListItems = '';

        //创建结构
        var _width = $input.attr("fwidth") || opts.defaultWidth;
        _width = window.parseFloat(_width);
        $inputText.css("width", _width - 36 + 'px'); /*让出右边显示箭头背景的位置*/
        if (opts.readOnly) $inputText.attr("disabled", "disabled");
        $newUl.css("width", _width - 2 + 'px');      /*选项的宽度减去左右1px边框*/
        $containerDiv.css("width", _width + 'px');
        $containerDiv.insertAfter($input);
        $inputText.prependTo($containerDivText);
        $containerDivText.prependTo($containerDiv);
        $newUl.appendTo($containerDiv);
        $input.hide();

        //构建选项
        if ($input.children('optgroup').length == 0) {
            $input.children().each(function (i) {
                var option = $(this).text();
                if ($(this).attr('selected')) {
                    opts.defaultText = option;
                    currentIndex = i;
                    $inputText.val(option);
                    newListItems += '<li class="js-sSelect_option js-sSelect_option-selected">' + option + '</li>';
                } else {
                    newListItems += '<li class="js-sSelect_option">' + option + '</li>';
                }
            });
            $newUl.html(newListItems);
            //缓存
            var $newLi = $newUl.children("li");
            var newLiLength = $newLi.length;
            if (newLiLength > opts.showSize) {
                $newUl.height(opts.itemHeight * opts.showSize);
            }
        } else {
            throw new Error("TODO:select里面包含optgroup的情况还不能处理");
        }

        //选择框朝上还是朝下
        function newUlPos() {
            var containerPosY = $containerDiv.offset().top;
            var containerHeight = $containerDiv.height();
            var scrollTop = $(window).scrollTop();
            var docHeight = $(window).height();
            var newUlHeight = $newUl.height();

            containerPosY = containerPosY - scrollTop;
            if (containerPosY + newUlHeight >= docHeight) {
                $newUl.css('top', '-' + newUlHeight + 'px');
            } else {
                $newUl.css('top', containerHeight + 'px');
            }
        }
        //页面加载、窗口调整时调用
        newUlPos();
        $(window).resize(function (e) {
            newUlPos(e);
        });
        $(window).scroll(function (e) {
            newUlPos(e);
        });
        //点击下拉框
        $containerDivText.click(function () {
            if ($newUl.is(':visible')) {
                $newUl.hide();
                positionHideFix();
                return false;
            }
            popOptions();
        });
        //失焦,隐藏
        $containerDiv.blur(function () {
            if ($newUl.is(':visible')) {
                $newUl.hide();
                positionHideFix();
                return false;
            }
        });
        $containerDivText.hover(function () {
            $(this).addClass('js-sSelect_selectedTxt-hover');
        }, function () {
            $(this).removeClass('js-sSelect_selectedTxt-hover');
        });
        $inputText.bind("click focus", function () {
            popOptions();
            return false;
        });

        //弹出下拉选项并滚动到当前项
        function popOptions() {
            if ($newUl.is(':visible')) { return; }
            $newLi.show();
            $newUl.slideDown("fast", function () {
                if (currentIndex + 1 >= opts.showSize) { //选中项超出层,滚动出来
                    $(this).scrollTop(currentIndex * opts.itemHeight);
                }
            });
            positionFix();
            return;
        }
        //输入框按键
        $inputText.keyup(function (e) {
            //当按下这些键时,不过滤项
            var keyCode = getKeyCode(e).toString();
            var keys = [37, 38, 39, 40, 13, 27];
            var strReg = "^" + keys.join("|") + "$";
            var reg = new RegExp(strReg);
            if (reg.test(keyCode)) return false;
            popOptions();
            $newLi.hide().filter(":contains('" + $(this).val() + "')").show();
        });

        //when keys are pressed
        $containerDiv.get(0).onkeydown = function (e) {
            var keycode = getKeyCode(e);
            switch (keycode) {
                case 40: //down
                case 39: //right
                    incrementList();
                    return false;
                case 38: //up
                case 37: //left
                    decrementList();
                    return false;
                case 13: //Enter
                case 27: //ESC
                    $newUl.hide();
                    positionHideFix();
                    return false;
            }
        }

        //hover选项
        $newLi.hover(function () {
            $(this).addClass('js-sSelect_option-hover');
        }, function () {
            $(this).removeClass('js-sSelect_option-hover');
        });
        //点击选项
        $newLi.click(function (e) {
            var text = $(this).text();
            currentIndex = $newLi.index($(this));
            $newLi.removeClass("js-sSelect_option-selected");
            $(this).addClass("js-sSelect_option-selected");

            setSelectText(text);
            $newUl.hide();
            positionHideFix();
        });

        function setSelectText(text) {
            $input.get(0).selectedIndex = currentIndex;
            $input.change();
            $inputText.val(text);
        }

        function getKeyCode(e) {
            var _e = e || window.event;
            var _currKey = _e.keyCode || _e.which || _e.charCode;
            return _currKey;
        }

        var $newLi_visible = $newLi.filter(":visible");
        var newLi_visible_len = $newLi_visible.length;
        var curr_visible_Index = -1;
        function navigationList(direction) {
            $newLi_visible = $newLi.filter(":visible");
            newLi_visible_len = $newLi_visible.length;

            if (newLi_visible_len <= 0) return;
            var $newLi_visble_selected = $newLi_visible.filter(".js-sSelect_option-selected");
            if ($newLi_visble_selected.length > 0) {
                curr_visible_Index = $newLi_visible.index($newLi_visble_selected);
            } else {
                curr_visible_Index = -1;
            }

            if (direction == "up") --curr_visible_Index;
            else if (direction == "down") ++curr_visible_Index;
            else throw new Error('没有此方向的事件');
            if (curr_visible_Index >= newLi_visible_len) curr_visible_Index = 0;
            else if (curr_visible_Index < 0) curr_visible_Index = newLi_visible_len - 1;

            $newLi.removeClass('js-sSelect_option-selected')
            $newLi_visible.eq(curr_visible_Index).addClass('js-sSelect_option-selected');

            currentIndex = $newLi.index($("li.js-sSelect_option-selected"));

            var text = $newLi_visible.eq(curr_visible_Index).text();
            setSelectText(text);
        }
        //↓向下
        function incrementList() {
            navigationList("down");
            var scrollTop = $newUl.scrollTop();
            var hideSize = Math.round(scrollTop / opts.itemHeight);
            //如果在可见区域，则不滚动
            if (hideSize <= curr_visible_Index && curr_visible_Index < hideSize + opts.showSize - 1) { return; }

            scrollTop = opts.itemHeight * (curr_visible_Index + 2) - opts.showSize * opts.itemHeight;
            if (scrollTop > 0) $newUl.scrollTop(scrollTop);
            else $newUl.scrollTop(0);
        }
        //↑向上
        function decrementList() {
            navigationList("up");
            var scrollTop = $newUl.scrollTop();
            var hideSize = Math.round(scrollTop / opts.itemHeight);
            //如果在可见区域，则不滚动
            if (hideSize < curr_visible_Index + 2 && curr_visible_Index != newLi_visible_len - 1) { return; }

            scrollTop = scrollTop - opts.itemHeight;
            if (scrollTop > 0) $newUl.scrollTop(scrollTop);
            else $newUl.scrollTop((newLi_visible_len - 4) * opts.itemHeight);
        }
        function positionFix() {
            $containerDiv.css('position', 'relative');
        }

        function positionHideFix() {
            $containerDiv.css('position', 'static');
        }
        //OVER!
    });
};