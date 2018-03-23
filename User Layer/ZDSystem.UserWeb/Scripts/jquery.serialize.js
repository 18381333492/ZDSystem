/*jQuery.param=function( a ) {  
    var s = [ ];  
    var encode=function(str){  
        str=escape(str);  
        str=str.replace(/+/g,"%u002B");  
        return str;  
    };  
    function add( key, value ){  
        s[ s.length ] = encode(key) + '=' + encode(value);  
    };  
    // If an array was passed in, assume that it is an array  
    // of form elements  
    if ( jQuery.isArray(a) || a.jquery )  
        // Serialize the form elements  
        jQuery.each( a, function(){  
            add( this.name, this.value );  
        });  
  
    // Otherwise, assume that it's an object of key/value pairs  
    else  
        // Serialize the key/values  
        for ( var j in a )  
            // If the value is an array then the key names need to be repeated  
            if ( jQuery.isArray(a[j]) )  
                jQuery.each( a[j], function(){  
                    add( j, this );  
                });  
            else  
                add( j, jQuery.isFunction(a[j]) ? a[j]() : a[j] );  
  
    // Return the resulting serialization  
    return s.join("&").replace(/%20/g, "+");  
}*/

(function ($) {
    //序列化为QueryString字符串
    $.fn.serialize = function (config) {
        var config = $.extend({
            selector: 'input[name], select[name], textarea[name]'
        }, config || {});
        var s = [];        
        $(config.selector, this).each(function () {
            if (this.disabled || (this.type == 'checkbox' ||
            this.type == 'radio') && !this.checked)
                return;
            if (this.type == 'select-multiple') {
                var name = this.name;
                $('option:selected', this).each(function () {
                    s.push(name + '=' + this.value);
                });
            }
            else
                if (this.tagName == 'INPUT'
                || this.tagName == 'SELECT'
                || this.tagName == 'TEXTAREA')
                    s.push(this.name + '=' + encodeURI(this.value));
                else
                    s.push(this.name + '=' + encodeURI(this.innerText));
        });
        return s.join('&');
    };
//    $.fn.validate = function (config) {
//        var config = $.extend({
//            selector: 'input[name], select[name], textarea[name]'
//        }, config || {});
//        var pass = true;
//        $(config.selector, this).each(function () {
//            if ($(this).attr("req") != null && $(this).val() == "") {
//                pass = false;
//                return false;
//            }
//        });
//        return pass;
//    };
    //将QueryString字符串的值取出来放到界面控件中
    $.fn.deserialize = function (s) {
        var data = s.split("&");
        for (var i = 0; i < data.length; i++) {
            var pair = decodeURIComponent(data[i]).split("=");
            $("[name='" + pair[0] + "']", this).val(pair[1]);
        }
    };
    //将界面的值转换成Json字符串

    //将JSON值,赋值到界面---------------------------------------------------------------------------

    $.fn.json2form = function (data) {
        var elem = null;
        if (typeof (data) == "string")
            elem = eval("(" + data + ")");
        if (typeof (data) == "object")
            elem = data;
        if (elem == null)
            return;
        $("*[name]", this).each(function () {
            var elemType = $(this).attr("type");
            var elemTag = $(this).attr("tagName");
            var elemName = $(this).attr("name");
            var elemData = elem[elemName];
            if (elemType == null) {
                elemType = elemTag;
            }

            if (elemData != null) {
                switch (elemType) {
                    case "text":
                    case "password":
                    case "hidden":
                    case "button":
                    case "reset":
                    case "textarea":
                    case "submit":
                        {
                            $(this).val(elemData);
                            break;
                        }
                    case "SPAN":
                    case "LABEL":
                    case "LI":
                        {
                            $(this).text(elemData);
                        }
                    case "checkbox":
                    case "radio":
                        {
                            $(this).attr("checked", "");
                            if (elemData == $(this).val()) {
                                $(this).attr("checked", true);
                            }
                            break;
                        }
                    case "SELECT":
                    case "select-one":
                    case "select-multiple":
                        {
                            $(this).find("option:selected").attr("selected", false);
                            $(this).find("option[value='" + elemData + "']").attr("selected", true);
                            break;
                        }
                }
            }
        });
    };
    //form2json,将指定对象内的具有Name属性的元素的值拼成JSON串返回
    $.fn.form2json = function (config) {
        var config = $.extend({
            selector: 'input[name], select[name], textarea[name]'
        }, config || {});
        var s = [];       
        $(config.selector, this).each(function () {
            if (this.disabled || (this.type == 'checkbox' ||
            this.type == 'radio') && !this.checked)
                return;
            if (this.type == 'select-multiple') {
                var name = this.name;
                $('option:selected', this).each(function () {
                    s.push(name + ":'" + this.value + "'");
                });
            }
            else {
                if (this.tagName == 'INPUT'
                || this.tagName == 'SELECT'
                || this.tagName == 'TEXTAREA')
                    s.push(this.name + ":'" + this.value + "'");
                else
                    s.push(this.name + ":'" + this.innerText + "'");
            }
        });
        return "{" + s.join(',') + "}";
    };

})(jQuery);

String.prototype.startWith = function (s) {
    if (s == null || s == "" || this.length == 0 || s.length > this.length)
        return false;
    return (this.substr(0, s.length) == s);
}