using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoGamingAPI.Model;
using ContosoGamingAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContosoGamingAPI.Controllers
{
    [Route("Routes")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        ILandMarkService _iLandMarkService;

        private readonly LandMarkDBContext _dbContext;
        public RoutesController(ILandMarkService iLandMarkService, LandMarkDBContext dbContext)
        {
            _iLandMarkService = iLandMarkService;
            _dbContext = dbContext;

            _iLandMarkService.DBContext = _dbContext;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Welcome.");
        }

        [Route("getalllandmarks")]
        [HttpGet]
        public ActionResult GetAllLandmarks()
        {
            try
            {
                return Ok(_iLandMarkService.DBContext.LandMarks);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [Route("getallRouteConnections")]
        [HttpGet]
        public ActionResult GetAllRouteConnections()
        {
            try
            {
                return Ok(_iLandMarkService.DBContext.RouteConnections);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] LandMark value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = _iLandMarkService.Add(value);
            return CreatedAtAction("Get", new { id = item.Id }, item);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existingItem = _iLandMarkService.GetById(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            _iLandMarkService.Delete(id);
            return Ok();
        }

        [Route("addroutes/{routeDefination}")]
        [HttpPost]
        public ActionResult AddRoutes(string routeDefination)
        {
            try
            {
                string[] allRoutes = routeDefination.Replace(" ", "").Split(",");

                int landMarkIndex = 0;

                _iLandMarkService.ClearAllLandMarks();
                _iLandMarkService.ClearAllRouteConnections();

                foreach (var route in allRoutes)
                {
                    if (route.Length < 3)
                        throw new Exception("Invalid Routes");

                    var landMarkOneValue = route[0];
                    var landMarkTwoValue = route[1];
                    var distance = route.Substring(2, route.Length - 2);

                    if (landMarkOneValue == landMarkTwoValue)
                        throw new Exception("The starting and ending landmark cannot be the same for a given route");

                    var landmarkOneExist = _iLandMarkService.GetLandMarkByName(landMarkOneValue.ToString());
                    var landmarkTwoExist = _iLandMarkService.GetLandMarkByName(landMarkTwoValue.ToString());

                    LandMark landMarkOne = null, landMarkTwo = null;

                    if (landmarkOneExist == null)
                    {
                        landMarkOne = new LandMark(landMarkIndex++, landMarkOneValue.ToString());
                        _iLandMarkService.Add(landMarkOne);
                    }
                    else
                        landMarkOne = landmarkOneExist;

                    if (landmarkTwoExist == null)
                    {
                        landMarkTwo = new LandMark(landMarkIndex++, landMarkTwoValue.ToString());
                        _iLandMarkService.Add(landMarkTwo);
                    }
                    else
                        landMarkTwo = landmarkTwoExist;

                    _iLandMarkService.AddRoute(landMarkOne, landMarkTwo, Convert.ToInt32(distance.ToString()));

                }
                return Ok("DataStore added successfully.");
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [Route("getdistanceviaroute/{routeDefination}")]
        [HttpGet]
        public ActionResult GetDistanceViaRoute(string routeDefination)
        {
            try
            {
                string _returnValue = string.Empty;
                string[] allLandmarks = routeDefination.Replace(" ", "").Split("-");

                if (_iLandMarkService.GetAllLandMarks().Count() <= 0)
                    throw new Exception("There are no Landmarks in store.");

                if (_iLandMarkService.GetAllRouteConnection().Count() <= 0)
                    throw new Exception("There are no Routes in store.");

                Dictionary<string, string> pairs = new Dictionary<string, string>();

                for (int i = 0; i < allLandmarks.Count() - 1; i++)
                {
                    pairs.Add(allLandmarks[i], allLandmarks[i + 1]);
                }

                int _totalDistance = 0;
                foreach (var item in pairs)
                {
                    string Value = _iLandMarkService.GetDistanceFromTo(_iLandMarkService.GetAllLandMarks().ToList().Find(l => l.Name == item.Key),
                        _iLandMarkService.GetAllLandMarks().ToList().Find(l => l.Name == item.Value));

                    if (Value == "Path not Found")
                    {
                        _totalDistance = 0;
                        _returnValue = "Path not Found";
                        break;
                    }
                    else
                        _totalDistance += Convert.ToInt32(Value);
                }

                if (_totalDistance > 0)
                    _returnValue = Convert.ToString(_totalDistance);

                return Ok(_returnValue);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [Route("getroutes/{landMarkOne}/{landMarkTwo}/{stops}")]
        [HttpGet]
        public ActionResult GetRoutes(string landMarkOne, string landMarkTwo, int stops = 2)
        {
            try
            {
                string _returnValue = string.Empty;

                var landmarkOneExist = _iLandMarkService.GetLandMarkByName(landMarkOne);
                var landmarkTwoExist = _iLandMarkService.GetLandMarkByName(landMarkTwo);

                if(landmarkOneExist == null || landmarkTwoExist == null)
                {
                    throw new Exception("There are no Landmarks in store.");
                }

                RouteEngine _engine = new RouteEngine(_iLandMarkService.GetAllLandMarks().ToList());
                _engine.MaxStop = stops;

                foreach (var route in _iLandMarkService.GetAllRouteConnection())
                {
                    _engine.AddRoutePath(route.LandMarkOne, route.LandMarkTwo);
                }

                var routeCount = _engine.GetAllRoutePaths(landmarkOneExist.Index, landmarkTwoExist.Index);
                return Ok(routeCount);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }
    }
}
