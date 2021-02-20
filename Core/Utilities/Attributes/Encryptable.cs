using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Attributes
{
    [AttributeUsage(AttributeTargets.All,AllowMultiple =false,Inherited =false)]
    public class Encryptable : Attribute
    {
    }
}
