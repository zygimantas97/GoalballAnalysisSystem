﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Contracts.V1.Requests
{
    public class TeamRequest
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
    }
}