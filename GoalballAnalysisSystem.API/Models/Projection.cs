﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GoalballAnalysisSystem.API.Models
{
    public class Projection
    {
        [Key]
        public long Id { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public double Speed { get; set; }
        public long GameId { get; set; }
        public long? GamePlayerId { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; }
        [ForeignKey(nameof(GamePlayerId))]
        public GamePlayer GamePlayer { get; set; }
    }
}