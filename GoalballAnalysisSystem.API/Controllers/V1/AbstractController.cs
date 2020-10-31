using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GoalballAnalysisSystem.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoalballAnalysisSystem.API.Controllers.V1
{
    public abstract class AbstractController : ControllerBase
    {
        protected readonly DataContext _context;
        protected readonly IMapper _mapper;

        public AbstractController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
