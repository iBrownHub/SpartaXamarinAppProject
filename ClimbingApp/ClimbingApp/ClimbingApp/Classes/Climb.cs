using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ClimbingApp.Classes
{
    class Climb
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int ID { get; set; }
        [NotNull]
        public DateTime SessionID { get; set; }
        public string CentreName { get; set; }
        public string AreaInCentre { get; set; }
        public string ClimbGrade { get; set; }
        public int ClimbAttempts { get; set; }
    }
}
