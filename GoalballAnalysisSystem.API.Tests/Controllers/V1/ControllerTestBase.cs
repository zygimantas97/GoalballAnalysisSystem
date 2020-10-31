using AutoMapper;
using GoalballAnalysisSystem.API.Controllers.V1;
using GoalballAnalysisSystem.API.Data;
using GoalballAnalysisSystem.API.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace GoalballAnalysisSystem.API.Tests.Controllers.V1
{
    public class ControllerTestBase : IDisposable
    {
        protected DataContext _context;
        protected IMapper _mapper;
        protected MockRepository mockRepository;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            _context.Database.EnsureCreated();

            var profiles = new List<Profile>();
            var modelToResponseProfile = new ModelToResponseProfile();
            profiles.Add(modelToResponseProfile);
            var requestToModelProfile = new RequestToModelProfile();
            profiles.Add(requestToModelProfile);

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfiles(profiles));
            _mapper = new Mapper(mapperConfiguration);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        protected T CreateController<T>() where T: AbstractController
        {
            var user = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim("id", "test_user")
                    }));

            var controller = (T)Activator.CreateInstance(typeof(T), _context, _mapper);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            return controller;
        }
    }
}
