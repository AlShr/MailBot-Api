using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SharedTypes;

namespace BotWcfServiceIIS
{
    public class Authentication
    {
        public static DomainDAL.User Autenticate(AuthenticationArgument args) {
            return DomainDAL.User.Autenticate( args );
        }
    }
}