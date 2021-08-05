using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoGamingAPI.Model
{
    public class LandMark
    {
        public int Id { get; set; }

        public int Index { get; set; }

        public string Name { get; set; }

        public bool Visited { get; set; }
        
        public LandMark()
        {

        }

        public LandMark(int _index, string _name)
        {
            Index = _index;
            Name = _name;
        }

    }
}
