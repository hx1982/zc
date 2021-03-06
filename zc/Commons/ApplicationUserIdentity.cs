﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using zc.Models;

namespace zc.Commons
{
    // TODO: 待完善, 后期加入权限控制有用

    /// <summary>
    /// 鉴权相关: 会员身份标识
    /// </summary>
    public class UserIdentity : IIdentity
    {
        private string _name;
        private user _userObj;

        public string AuthenticationType
        {
            get
            {
                return "Form";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return true;
            }
        }

        public string Name
        {
            get { return this._name; }
        }

        public user UserObj
        {
            get
            {
                return _userObj;
            }

            set
            {
                _userObj = value;
            }
        }

        public UserIdentity(user user)
        {
            this._userObj = user;
            this._name = user.user_id.ToString();
        }

        public UserIdentity()
        {

        }
    }

    /// <summary>
    /// 鉴权相关: 会员Principal
    /// </summary>
    public class UserPrincipal : IPrincipal
    {

        private IIdentity _identity;

        public IIdentity Identity
        {
            get
            {
                return _identity;
            }
        }

        public bool IsInRole(string role)
        {
            return true;
        }

        public UserPrincipal(user user)
        {
            this._identity = new UserIdentity(user);
        }
    }
}