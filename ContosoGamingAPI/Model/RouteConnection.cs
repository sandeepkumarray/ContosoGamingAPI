using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoGamingAPI.Model
{
    public class RouteConnection
    {
        public int Id { get; set; }
        public LandMark LandMarkOne { get; set; }
        public LandMark LandMarkTwo { get; set; }
        public int Distance { get; set; }
        public RouteConnection()
        { }
        public RouteConnection(LandMark _landMarkOne, LandMark _landMarkTwo, int _distance)
        {
            LandMarkOne = _landMarkOne;
            LandMarkTwo = _landMarkTwo;
            Distance = _distance;
        }
    }
}
