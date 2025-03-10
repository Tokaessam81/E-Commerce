using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Common.Constants
{
    public static class AuthorizationConstants
    {
        // Roles
        public const string AdminRole = "Admin";
        public const string CustomerRole = "Customer";

        // Policies
        public const string AdminPolicy = "AdminPolicy";
        public const string CustomerPolicy = "CustomerPolicy";
    }
}
