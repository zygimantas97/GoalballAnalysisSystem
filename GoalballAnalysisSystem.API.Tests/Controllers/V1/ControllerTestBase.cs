using AutoMapper;
using GoalballAnalysisSystem.API.Data;
using GoalballAnalysisSystem.API.Mapping;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.API.Tests.Controllers.V1
{
    public class ControllerTestBase : IDisposable
    {
        protected DataContext _context;
        protected IMapper _mapper;
        protected MockRepository mockRepository;

        public ControllerTestBase()
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


    }
}
