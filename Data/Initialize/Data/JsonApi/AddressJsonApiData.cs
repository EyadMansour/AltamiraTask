using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Initialize.Data.JsonApi
{
    public class AddressJsonApiData
    {
        public string Street { get; set; }
        public string Suite { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public GeoJsonApiData Geo { get; set; }
    }
}
