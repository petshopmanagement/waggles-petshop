using System.Collections.Generic;

namespace PetManagementSystem.Web.Models
{
    public class CustomerDashboardViewModel
    {
        public dynamic Profile { get; set; }
        public IEnumerable<dynamic> Transactions { get; set; }
    }
}
