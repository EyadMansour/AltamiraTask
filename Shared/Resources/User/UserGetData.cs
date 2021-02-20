using Shared.Resources.Permission;
using Shared.Resources.Role;
using System.Collections.Generic;

namespace Shared.Resources.User
{
    public class UserGetData
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public UserDetailGetData Detail { get; set; }=new UserDetailGetData();
        public string DateCreated { get; set; }

    }
}
