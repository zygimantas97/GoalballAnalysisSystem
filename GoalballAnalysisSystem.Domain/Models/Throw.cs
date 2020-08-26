using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.Domain.Models
{
    public class Throw
    {
        public int Id { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public double Speed { get; set; }
        public int Game { get; set; }
        public int? GamePlayer { get; set; }

        public Game GameNavigation { get; set; }
        public GamePlayer GamePlayerNavigation { get; set; }
    }
}
