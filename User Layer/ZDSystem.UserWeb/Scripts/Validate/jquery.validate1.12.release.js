// JScript source code
/* ========================================
 * 表单验证，模仿jQUERY.Validate 版本：1.12
 * ========================================
 */
var Validate = {
    form: null,  //form的选择器
    config: {}, //配置参数集合
    init: function(form, config) {
        var defaultOptions = { //默认参数
            rules: {},
            messages: {},
            infos: {},
            submitHandler: function() { },
            succeed: "vdt-ok",
            focusInvalid: true,
            submit: true
        };

        for (var item in defaultOptions) {
            this.config[item] = this.getSafeValue(config[item], defaultOptions[item]);
        }
        this.form = form;
        this.bind();
    },
    //转换值
    getSafeValue: function(_arg, _ref) {
        var _type = typeof _arg;
        if (/(object)|(undefined)|(string)/.test(_type)) {
            return _arg || _ref;
        }
        return _arg;
    },
    //绑定所有控件验证，绑定表单提交验证
    bind: function() {
        var _rules = this.config.rules;
        var __this = this;

        for (var _name in _rules) {
            var $input = $(":input[name=" + _name + "]", this.form);
            if ($input.length == 0) {
                ProgrameEx.chuck("控件错误,原因:没有名为\"$0\"的控件".Format(_name));
                return false;
            }
            var _type = $input.attr('type');
            var _tag = $input.get(0).nodeName.toLowerCase();
            var _bind_event = Controlls.BindType[_type] || Controlls.BindType[_tag];
            if (!_bind_event) {
                throw new Error("待绑定控件未定义Controlls.BindType(绑定事件类型)");
            }
            $input.bind(_bind_event, function() {
                __this.check(this.name);
            });

            if (this.config.infos.hasOwnProperty(_name)) { //显示提示信息
                TipMsgHandle.init($input);
                TipMsgHandle.showInfo(this.config.infos[_name]);
            }
        }
        $(this.form).submit(function() {
            var result = __this.checkAll();
            if (!result) { //验证错误的第一个控件聚焦
                if (typeof __this.config.focusInvalid == 'undefined' || __this.config.focusInvalid) {
                    var _focus_name = $(".vdt-error[generate=true]:visible").attr("related");
                    $(":input[name=" + _focus_name + "]", this).focus();
                }
                return false;
            }
            if (typeof __this.config.submitHandler == 'function') {
                __this.config.submitHandler.call(__this.config);
            }
            return __this.config.submit; //是否提交
        });
    },
    //验证obj并显示错误
    check: function(input_name) {
        var $obj = $(":input[name=" + input_name + "]", this.form);
        var _name = $obj.attr('name');
        var _rule = this.config.rules[_name];
        var _message = this.config.messages[_name];
        if (!_rule) return true; //没有该控件的规则，直接返回true
        if (!_message) _message = {};

        Verify.init($obj);
        for (var item in _rule) {
            TipMsgHandle.init($obj, item, _message[item], _rule[item]);
            if (typeof Verify[item] != 'function') {
                ProgrameEx.chuck("无法执行验证规则:\"$0\",原因:该规则未定义".Format(item));
                return false;
            }
            if (!Verify[item](_rule[item])) {
                TipMsgHandle.showError();
                return false;
            } else {
                TipMsgHandle.hideTip();
                if (this.config.succeed) TipMsgHandle.showSucceed(this.config.succeed);
            }
        }
        return true;
    },
    //遍历rules
    //return boolean
    checkAll: function() {
        var _rules = this.config.rules;
        var _error_Counter = 0;
        var $_chking_obj = null;
        for (var _name in _rules) {
            $_chking_obj = $(":input[name=" + _name + "]", this.form);
            if ($_chking_obj.length == 0) continue; //没有找到控件，就不验证它
            if (!this.check(_name)) {
                _error_Counter++;
            }
        }
        return _error_Counter == 0;
    }
}

/*+ 错误处理类 +*/
var TipMsgHandle = {
    $input: '',
    inputName: '',
    tipMessage: '',
    init: function($input, rule_name, err_msg, rule_value) { //TODO:这里的msgArgs没取好，它是rule的值，值可能的范围：function，string, array, boolean等
        this.$input = $input;
        this.inputName = $input.attr('name');
        var _error = err_msg || ErrorMessage[rule_name] || '无此类型的错误提示消息可用';
        if (rule_value instanceof Array) {  //参数化错误消息，暂时只支持两个数字的数组和一个数字的变量
            this.tipMessage = _error.Format(rule_value[0], rule_value[1]);
        } else {
            this.tipMessage = _error.Format(rule_value);
        }
    },
    //获取消息
    getTip: function() {
        return $("[generate=true][related=" + this.inputName + "]");
    },
    //定位消息
    setTipPosition: function($error) {
        var input_type = this.$input.attr("type").toLowerCase();
        if ('checkbox' == input_type || 'radio' == input_type || 'select-one' == input_type) { //radio或者checkbox，错误显示在最后
            this.$input.parent().append($error);
        } else {
            this.$input.after($error);
        }
    },
    //创建消息并定位
    createTip: function() {
        var $error = $("<span generate='true' style='display:inline-block' related='$0'>$1</span>".Format(this.inputName, this.tipMessage));
        this.setTipPosition($error);
        return this.getTip();
    },
    //显示提示信息
    showInfo: function(_info) {
        this.tipMessage = _info;
        var $info = this.getTip();
        if ($info.length == 0) {
            $info = this.createTip();
        }
        $info.attr("class", "vdt-info").html(_info).show();
    },
    //显示错误消息
    showError: function() {
        var $error = this.getTip();
        if ($error.length == 0) {
            $error = this.createTip();
        }
        $error.attr("class", "vdt-error").html(this.tipMessage).show();
    },
    //显示成功消息
    showSucceed: function(_class) {
        var $succeed = this.getTip();
        if ($succeed.length == 0) {
            $succeed = this.createTip();
        }
        $succeed.attr("class", _class).html("&nbsp;").show();
    },
    //隐藏消息
    hideTip: function() {
        var $tip = this.getTip();
        if ($tip.length > 0) $tip.hide();
    }
};
/*+ 验证规则类 +*/
/*TODO：验证规则需要支持function，selector，和jQUERY的表达式*/
var Verify = { $element: null
    , evalue: ''
    , etype: ''
    , ename: ''
    , init: function($obj) {
        this.$element = $obj;
        this.evalue = $.trim($obj.val());   //注意，value被截掉了首尾空格
        this.etype = $obj.attr("type");
        this.ename = $obj.attr("name");
    }
    , required: function(param) {
        if (!param) return true; //参数空值，不验证
        if (this.etype == 'checkbox' || this.etype == 'radio') { //单选框、复选框个数大于0
            return this.$element.filter(":checked").length > 0;
        }
        if (typeof param == 'function') {
            return param.call(window, this.evalue);
        }
        return this.evalue != '';
    }
    , number: function() { //纯数字（字符串类）
        return this.evalue == '' || /^\d+$/.test(this.evalue);
    }
    , decimal: function() { //标准金额类数字
        return this.evalue == '' || /(^[-]?[1-9]\d*$)|(^0$)|(^[-]?[0][\.][0-9]+$)|(^[-]?[1-9]\d*[\.]\d+$)/.test(this.evalue);
    }
    , digits: function() { //标准整数类
        return this.evalue == '' || /(^[-]?[1-9]\d*$)|(^0$)/.test(this.evalue);
    }
    , rangelength: function(arr) { //字符串长度限制
        return this.evalue == '' || (this.evalue.length >= arr[0] && this.evalue.length <= arr[1]);
    }
    , minlength: function(param) { //字符串最小长度 
        return this.evalue == '' || this.evalue.length >= param;
    }
    , maxlength: function(param) { //字符串最大长度
        return this.evalue == '' || this.evalue.length <= param;
    }
    , range: function(arr) { //数字范围限制
        if (this.evalue == '') return true;
        var _temp = parseFloat(this.evalue);
        return _temp >= arr[0] && _temp <= arr[1];
    }
    , min: function(param) { //最小值限制（仅数字，checkbox个数）
        if (typeof param == 'function') {
            param = param.call(window, this.evalue);
        }
        if (this.etype == 'checkbox') {
            return this.$element.filter(":checked").length >= param;
        } else if (this.etype == 'text') {
            return this.evalue == '' || parseFloat(this.evalue) >= param;
        }
        return false;
    }
    , max: function(param) { //最大值限制（仅数字，checkbox个数）
        if (typeof param == 'function') {
            param = param.call(window, this.evalue);
        }
        if (this.etype == 'checkbox') {
            return this.$element.filter(":checked").length <= param;
        } else if (this.etype == 'text') {
            return this.evalue == '' || parseFloat(this.evalue) <= param;
        }
        return false;
    }
    , equalTo: function(selector) {
        return this.evalue == '' || this.evalue == $(selector).val();
    }
    /**
    * 验证文件路径的后缀
    * str：[string] 格式：docx?|txt|pdf|xlsx?
    */
    , accept: function(str) {
        if (this.evalue == '') return true;
        var _arr = str.split('|');
        for (var i = 0, len = _arr.length; i < len; i++) {
            _arr[i] = "[\.]" + _arr[i] + "$";
        }
        var reg = new RegExp(_arr.join('|'));
        return reg.test(this.evalue);
    }
    /**
    * 自定义验证规则
    * --------------
    *  参数：
    *    _rule:[string] 规则名称
    *    _func:[function(value,element,param)] 验证的函数
    *          |- 参数:
    *          |-     value:  被验证控件的值
    *          |-   element:  被验证控件(JQuery对象)
    *          |-     param:  验证规则(_rule)的值
    *    _msg:[string] 验证错误时显示的消息(可为空)
    */
    , addMethod: function(_rule, _func, _msg) {
        var __this = this;
        if (this.hasOwnProperty(_rule)) {
            ProgrameEx.chuck('验证规则"$0"添加失败,原因:该验证已存在'.Format(_rule));
            return;
        }
        if (_msg && !ErrorMessage.hasOwnProperty(_rule)) {
            ErrorMessage[_rule] = _msg;
        }
        this[_rule] = function(_param) {
            return _func.call(__this, __this.evalue, __this.$element, _param);
        }
    }
    /** 一些特殊的验证 **/
    , mobile: function() { //手机
        return this.evalue == '' || /^1[3458]\d{9}$/.test(this.evalue);
    }
    , phone: function() { //座机
        return this.evalue == '' || /^0[1-9]\d{1,2}[-]?\d{7,8}$/.test(this.evalue);
    }
    , email: function() { //邮箱
        return this.evalue == '' || /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(this.evalue);
    }
    , url: function() { //链接
        return this.evalue == '' || /^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/.test(this.evalue);
    }
    , idcard: function() { //身份证
        return this.evalue == '' || /(^\d{15}$)|(^\d{18}$)|(^\d{17}[Xx]$)/.test(this.evalue);
    }
    , username: function() { //注册账号名,过滤特殊字符
        return this.evalue == '' || /^[A-Za-z][A-Za-z1-9_\u4e00-\u9fa5]{4,15}$/.test(this.evalue);
    }
};
/*+ 验证错误消息类 +*/
var ErrorMessage = { required: '该项不能为空'
    , number: '请填写数字'
    , decimal: '请填写有效的数字（支持小数）'
    , digits: '请填写有效的整数'
    , mobile: '请填写手机号码'
    , phone: '请填写座机,格式:010-87654321'
    , email: '请填写邮箱'
    , url: '请填写链接,格式以http://开头'
    , rangelength: '请输入$0到$1个字符'
    , minlength: '最少$0个字符'
    , maxlength: '最多$0个字符'
    , range: '数字范围在$0-$1之间'
    , min: '不能小于最小值$0'
    , max: '不能大于最大值$0'
    , equalTo: '两个值不相等'
    , accept: '文件类型必须为:$0'
    , idcard: '身份证号码填写有误'
    , username: '以字母开头,由5-16个汉字、字母、数字和下划线组成'
};

/**
* 通用类:控件类型
*/
var Controlls = {
    //不同控件的绑定事件类型
    BindType: {
        text: "blur keyup",
        password: "blur keyup",
        radio: "blur click",
        checkbox: "blur click",
        file: "blur",
        select: "blur change",
        textarea: "blur keyup"
    }
}

/**
* 异常类
* 在调用过程中，避免抛出系统异常而导致提交
* 调用chuck显示异常错误
*/
var ProgrameEx = {
    messages: [],
    //加入异常信息并显示
    chuck: function(msg) {
        if (this.messages.length >= 5) {
            this.messages.shift();
        }
        this.messages[this.messages.length] = msg;
        if ($("form:first div.pe").length == 0) {
            $("form:first").prepend("<div class='pe'></div>");
        }
        var _e = "<h3>验证程序出现异常,具体如下:</h3><ol>";
        for (var i = 0, len = this.messages.length; i < len; i++) {
            _e += "<li>" + this.messages[i] + "</li>";
        }
        _e += "</ol>";
        $("form:first div.pe").html(_e);
    }
};