using Shared.Resources.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Entities.Companies
{
    public interface ICompanyService : IBaseService<CompanyGetData,CompanyData,int>
    {
    }
}
