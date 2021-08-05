using ContosoGamingAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoGamingAPI.Services
{
    public interface ILandMarkService
    {
        LandMarkDBContext DBContext { get; set; }
        IEnumerable<RouteConnection> GetAllRouteConnection();
        string AddRoute(LandMark landMarkOne, LandMark landMarkTwo, int Disatnce);
        IEnumerable<LandMark> GetAllLandMarks();
        LandMark Add(LandMark landMark);
        LandMark GetById(int id);
        LandMark GetLandMarkByName(string name);
        void Delete(int id);
        void ClearAllLandMarks();
        void ClearAllRouteConnections();
        string GetDistanceFromTo(LandMark _startLandMark, LandMark _endLandMark);
    }
}
