using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.FurnitoreStore.Shared
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }

        public DateTime BirhtDate { get; set; }
        public string Phone {  get; set; }
        public string Address {  get; set; }

    }
}
