using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ClimbingApp.Classes
{
    class Session
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        [NotNull]
        public DateTime SessionID { get; set; }
        public int AmountOfClimbs { get; set; }
        public string ClimbTime { get; set; }
    }
}
