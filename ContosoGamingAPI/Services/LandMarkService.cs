using ContosoGamingAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoGamingAPI.Services
{
    public class LandMarkService : ILandMarkService
    {
        LandMarkDBContext _dbContext = null;
        LandMarkDBContext ILandMarkService.DBContext
        {
            get
            {
                return _dbContext;
            }
            set
            {
                _dbContext = value;
            }
        }

        //List<LandMark> _dbContext.LandMarks;
        //List<RouteConnection> _dbContext.RouteConnections;

        public LandMarkService()
        {
            
        }

        public LandMark Add(LandMark landMark)
        {
            if (_dbContext.LandMarks.Count() > 0)
                landMark.Id = _dbContext.LandMarks.Max(x => x.Id) + 1;
            else
                landMark.Id = 1;

            _dbContext.LandMarks.Add(landMark);
            _dbContext.SaveChanges();
            return landMark;
        }

        public string AddRoute(LandMark landMarkOne, LandMark landMarkTwo, int Disatnce)
        {
            string returnValue = null;
            RouteConnection newRoute = null;
            if (_dbContext.RouteConnections.Any(c => c.LandMarkOne == landMarkOne && c.LandMarkTwo == landMarkTwo) == false)
            {
                newRoute = new RouteConnection(landMarkOne, landMarkTwo, Disatnce);

                if (_dbContext.RouteConnections.Count() > 0)
                    newRoute.Id = _dbContext.RouteConnections.Max(x => x.Id) + 1;
                else
                    newRoute.Id = 1;

                _dbContext.RouteConnections.Add(newRoute);
                _dbContext.SaveChanges();
                returnValue = "Success";
            }
            else
            {
                throw new Exception("Duplicate route found.");
            }
            return returnValue;
        }

        public void ClearAllLandMarks()
        {
            foreach (var item in _dbContext.LandMarks)
                _dbContext.LandMarks.Remove(item);
            _dbContext.SaveChanges();
        }

        public void ClearAllRouteConnections()
        {
            foreach (var item in _dbContext.RouteConnections)
                _dbContext.RouteConnections.Remove(item);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _dbContext.LandMarks.First(a => a.Id == id);
            _dbContext.LandMarks.Remove(existing);
        }

        public IEnumerable<LandMark> GetAllLandMarks()
        {
            return _dbContext.LandMarks;
        }

        public IEnumerable<RouteConnection> GetAllRouteConnection()
        {
            return _dbContext.RouteConnections;
        }

        public LandMark GetById(int id)
        {
            return _dbContext.LandMarks.First(x => x.Id == id);
        }

        public LandMark GetLandMarkByName(string name)
        {
            return _dbContext.LandMarks.Count() > 0 ? _dbContext.LandMarks.Any(x => x.Name == name) ? _dbContext.LandMarks.First(x=>x.Name == name) : null : null;
        }

        public string GetDistanceFromTo(LandMark _startLandMark, LandMark _endLandMark)
        {
            var _selectedConnections = _dbContext.RouteConnections.First(x => x.LandMarkOne == _startLandMark && x.LandMarkTwo == _endLandMark);

            if (_selectedConnections != null)
                return Convert.ToString(_selectedConnections.Distance);
            else
                return "Path not Found";
        }
    }
}
