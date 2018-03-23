using System;
using System.Collections.Generic;
using System.Text;
using Lib4Net.Comm;
using Lib4Net.ORM;


namespace ZDSystem.Entity
{

    ///<summary>
    ///枚举状态:错误码
    ///</summary>
    public class ErrorCode
    {
        #region 公共提示
        ///<summary>
        ///成功
        ///</summary>
        public readonly static int Success = 100;
        ///<summary>
        ///人工审核
        ///</summary>
        public readonly static int Manual = 999;
        ///<summary>
        ///处理失败
        ///</summary>
        public readonly static int Failure = 200;

        /// <summary>
        /// 参数为空
        /// </summary>
        public readonly static int ParamIsError = 250;
        /// <summary>
        /// 签名错误
        /// </summary>
        public readonly static int SignError = 251;

        /// <summary>
        /// 订单已经存在
        /// </summary>
        public readonly static int IsExist = 252;
        /// <summary>
        /// 商家信息错误
        /// </summary>
        public readonly static int PartnerError = 253;
        /// <summary>
        /// 金额错误
        /// </summary>
        public readonly static int AmountError = 254;
        /// <summary>
        /// 卡规则错误
        /// </summary>
        public readonly static int CardError = 255;

        /// <summary>
        /// 不支持
        /// </summary>
        public readonly static int NotSupport = 256;

        /// <summary>
        /// 加解密错误
        /// </summary>
        public readonly static int DecryptyError = 257;


        /// <summary>
        /// IP禁止访问
        /// </summary>
        public readonly static int IPNOTALLOW = 273;

        /// <summary>
        /// 订单不存在
        /// </summary>
        public readonly static int IsNotExist = 258;
        /// <summary>
        /// 保存推广人渠道业务关系时出错
        /// </summary>
        public readonly static int SaveSCBRelationError = 301;
        /// <summary>
        /// 保存推广人渠道业务关系提成策略时出错
        /// </summary>
        public readonly static int SaveSCBPolicyError = 302;
        /// <summary>
        /// 区服映射失败
        /// </summary>
        public readonly static int ServerMapError = 307;

        #endregion

        #region 账号管理类提示
        /// <summary>
        /// 验证码错误
        /// </summary>
        public readonly static int ValidateCodeError = 401;
        /// <summary>
        /// 用户被禁用
        /// </summary>
        public readonly static int UserDisabled = 402;
        /// <summary>
        /// 登录密码有误
        /// </summary>
        public readonly static int PasswordError = 403;
        /// <summary>
        /// 修改密码时原始密码有误
        /// </summary>
        public readonly static int OldPwdError = 404;
        /// <summary>
        /// 用户名已经被使用
        /// </summary>
        public readonly static int UserNameExists = 405;
        #endregion

        #region 纯系统提示
        /// <summary>
        /// 外部商家号已经存在
        /// </summary>
        public readonly static int PartnerIDExists = 501;
        /// <summary>
        /// 运营商编号已经存在
        /// </summary>
        public readonly static int CarrierNoExists = 502;
        #endregion
    }
}
