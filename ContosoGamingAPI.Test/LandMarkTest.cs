using ContosoGamingAPI.Controllers;
using ContosoGamingAPI.Model;
using ContosoGamingAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ContosoGamingAPI.Test
{
    public class LandMarkTest
    {
        RoutesController _routesController;
        ILandMarkService _iLandMarkService;
        LandMarkDBContext _dbContext;

        public LandMarkTest()
        {
            _iLandMarkService = new LandMarkServiceMock();
            _dbContext = _dbContext ?? GetInMemoryDBContext();
            InitMockData();
            _routesController = new RoutesController(_iLandMarkService, _dbContext);
        }

        public void InitMockData()
        {
            LandMark A = new LandMark(0, "A") { Id = 1 };
            LandMark B = new LandMark(1, "B") { Id = 2 };
            LandMark C = new LandMark(2, "C") { Id = 3 };
            LandMark D = new LandMark(3, "D") { Id = 4 };
            LandMark E = new LandMark(4, "E") { Id = 5 };

            _dbContext.LandMarks.Add(A);
            _dbContext.LandMarks.Add(B);
            _dbContext.LandMarks.Add(C);
            _dbContext.LandMarks.Add(D);
            _dbContext.LandMarks.Add(E);

            _dbContext.RouteConnections.Add(new RouteConnection(A, B, 3));
            _dbContext.RouteConnections.Add(new RouteConnection(B, C, 9));
            _dbContext.RouteConnections.Add(new RouteConnection(C, D, 3));
            _dbContext.RouteConnections.Add(new RouteConnection(D, E, 6));
            _dbContext.RouteConnections.Add(new RouteConnection(A, D, 4));
            _dbContext.RouteConnections.Add(new RouteConnection(D, A, 5));
            _dbContext.RouteConnections.Add(new RouteConnection(C, E, 2));
            _dbContext.RouteConnections.Add(new RouteConnection(A, E, 4));
            _dbContext.RouteConnections.Add(new RouteConnection(E, B, 1));

            _dbContext.SaveChanges();
        }

        protected LandMarkDBContext GetInMemoryDBContext()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
            var builder = new DbContextOptionsBuilder<LandMarkDBContext>();
            var options = builder.UseInMemoryDatabase("LandMarks").UseInternalServiceProvider(serviceProvider).Options;
            LandMarkDBContext dbContext = new LandMarkDBContext(options);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            return dbContext;
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            LandMark testItem = new LandMark()
            {
                Name = "A",
            };
            // Act
            var createdResponse = _routesController.Post(testItem);
            // Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }

        [Fact]
        public void Delete_ExistingIdPassed_ReturnsOkResult()
        {
            // Arrange
            var exisitngId = 1;
            // Act
            var okResponse = _routesController.Delete(exisitngId);
            // Assert
            Assert.IsType<OkResult>(okResponse);
        }


        [Fact]
        public void Add_ValidRouteConnectionPassed_ReturnsOkResponse()
        {
            // Arrange
            string testItem = "AB31, BC9, CD3, DE6, AD4, DA5, CE2, AE4, EB1";

            // Act
            var createdResponse = _routesController.AddRoutes(testItem);
            // Assert
            Assert.IsType<OkObjectResult>(createdResponse);
        }

        [Fact]
        public void Get_DistanceViaRoutePassed_ReturnsOkResponse()
        {
            // Arrange
            string testItem = "A-E-B-C-D";

            // Act
            var createdResponse = _routesController.GetDistanceViaRoute(testItem);
            // Assert
            Assert.IsType<OkObjectResult>(createdResponse);
        }

        [Fact]
        public void Get_AllLandmarks_ReturnsOkResponse()
        {
            // Arrange

            // Act
            var createdResponse = _routesController.GetAllLandmarks();
            // Assert
            Assert.IsType<OkObjectResult>(createdResponse);
        }

        [Fact]
        public void Get_AllRouteConnections_ReturnsOkResponse()
        {
            // Arrange

            // Act
            var createdResponse = _routesController.GetAllRouteConnections();
            // Assert
            Assert.IsType<OkObjectResult>(createdResponse);
        }

        [Fact]
        public void Get_AllRouteWithDefaultStops_ReturnsOkResponse()
        {
            // Arrange
            string landmarkStart = "A";
            string landmarkEnd = "C";

            // Act
            var createdResponse = _routesController.GetRoutes(landmarkStart, landmarkEnd);
            // Assert
            Assert.Equal(2, ((OkObjectResult)createdResponse).Value);
        }

        [Fact]
        public void Get_AllRouteWith5Stops_ReturnsOkResponse()
        {
            // Arrange
            string landmarkStart = "A";
            string landmarkEnd = "C";
            int stops = 5;

            // Act
            var createdResponse = _routesController.GetRoutes(landmarkStart, landmarkEnd, stops);
            // Assert
            Assert.Equal(3, ((OkObjectResult)createdResponse).Value);
        }
    }
}
