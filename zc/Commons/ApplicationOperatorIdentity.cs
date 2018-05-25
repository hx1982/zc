using System;
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
    public class OperatorIdentity : IIdentity
    {
        private string _name;
        private _operator _operatorObj;

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

        public _operator OperatorObj
        {
            get
            {
                return _operatorObj;
            }
        }

        public OperatorIdentity(_operator oper)
        {
            this._operatorObj = oper;
            this._name = oper.oper_name;
        }

        public OperatorIdentity()
        {
        }
    }

    /// <summary>
    /// 鉴权相关: 会员Principal
    /// </summary>
    public class OperatorPrincipal : IPrincipal
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

        public OperatorPrincipal(_operator oper)
        {
            this._identity = new OperatorIdentity(oper);
        }
    }
}