using System;
using System.Collections.Generic;
using System.Text;

namespace WeChatPlatform.Config
{
    public class UrlConfig
    {
        #region 请求的根域
        public static readonly string BaseApiURL = "api.weixin.qq.com/";
        public static readonly string BaseApi2URL = "api2.weixin.qq.com/";
        #endregion
        #region 获取token的部分网址
        public static readonly string TokenURL = "cgi-bin/token?";
        #endregion
        #region 获取IP地址
        public static readonly string IPURL = "cgi-bin/getcallbackip?";
        #endregion
        #region 消息URL
        public static readonly string SetIndustry = "cgi-bin/template/api_set_industry?access_token=";//设置行业URL
        public static readonly string GetIndustry = "cgi-bin/template/get_industry?access_token=";//获取行业URL 
        public static readonly string GetTemplateID = "cgi-bin/template/api_add_template?access_token=";//获取模板ID
        public static readonly string GetTemplateList = "cgi-bin/template/get_all_private_template?access_token=";//获取模板list
        public static readonly string DelTemplate = "cgi-bin/template/del_private_template?access_token=";//删除模板
        public static readonly string SendTemplateMsg = "cgi-bin/message/template/send?access_token=";//发送模板消息
        #endregion
        #region 用户信息URL
        public static readonly string GetUserList = "cgi-bin/user/get?access_token=";//获取用户list
        #endregion
        #region 用户标签
        public static readonly string CreateUserTag = "cgi-bin/tags/create?access_token=";//创建用户管理tag
        public static readonly string GetUserTag = "cgi-bin/tags/get? access_token=";//获取用户标签
        public static readonly string EditUserTag = "cgi-bin/tags/update?access_token=";//编辑用户标签
        public static readonly string DelUserTag = "cgi-bin/tags/delete?access_token=";//删除用户标签
        public static readonly string GetFansList = "cgi-bin/user/tag/get?access_token=";//获取粉丝列表
        public static readonly string SetUserTag = "tags/members/batchtagging?access_token=";//批量为用户打标签
        public static readonly string CancelUserTag = "cgi-bin/tags/members/batchuntagging?access_token=";//批量取消用户标签
        public static readonly string GetUserTagList = "cgi-bin/tags/getidlist?access_token=";//获取用户身上的标签列表
        #endregion

    }
}
