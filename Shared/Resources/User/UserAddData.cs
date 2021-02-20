using System.Collections.Generic;

namespace Shared.Resources.User
{
    public class UserAddData : UserRegisterData
    {
        public List<string> Roles { get; set; }
        public List<string> DirectivePermissions { get; set; }


        public UserAddData()
        {
            Roles = new List<string>();
            DirectivePermissions = new List<string>();
        }

    }
}
