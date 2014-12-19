using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids_Rebirth
{
    [Serializable]
    class record
    {
        public int Pos { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }

        public DateTime Date { get; set; }

        public record(string name, int score, DateTime date)
        {
            this.Name = name;
            this.Score = score;
            this.Date = date;
        }
    }
}
