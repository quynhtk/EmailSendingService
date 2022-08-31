using EmailMaketing.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailMaketing.SenderEmails
{
    public class SenderWithNavigation
    {
        public SenderEmail SenderEmail { get; set; }
        public Customer Customer { get; set; }
    }
}
