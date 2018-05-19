using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using zc.Models;

namespace zc.Commons
{

    // UNDONE: 可选操作, 未完成
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
            throw new NotImplementedException();
        }

        public UserPrincipal(user user)
        {
            this._identity = new UserIdentity(user);
        }
    }
}