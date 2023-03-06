namespace Rubik.API.Models
{
    public class RankingEntityDto
    {
        public int Position { get; set; }
        public string Username { get; set; }
        public int Time { get; set; }

        public DateTime Date { get; set; }

        public RankingEntityDto(int position, string username, int time, DateTime date)
        {
            Position = position;
            Username = username;
            Time = time;
            Date = date;
        }
    }
}