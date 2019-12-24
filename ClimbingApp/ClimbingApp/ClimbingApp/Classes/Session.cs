using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ClimbingApp.Classes
{
    public class Session
    {
        [PrimaryKey, NotNull]
        public DateTime SessionID { get; set; }
        public string CentreName { get; set; }
        public int AmountOfClimbs { get; set; }
        public string ClimbTime { get; set; }
    }
}
