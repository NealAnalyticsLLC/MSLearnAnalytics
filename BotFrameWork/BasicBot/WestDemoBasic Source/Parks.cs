using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuisBot
{
    public class Parks
    {
        public Parks()
        {

        }
        public Parks(string name,string address,string rating,string distance)
        {
            Name = name;
            Address = address;
            Rating=rating;
            Distance = distance;
        }
        public string Name { get; set; }

        public string Address { get; set; }

        public string Rating { get; set; }

      
        public string Distance { get; set; }

        
    }
}