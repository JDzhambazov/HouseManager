namespace HouseManager.Services.Models
{
    public class PagingModel
    {
        public int CurrentPage { get; set; }

        public int MaxPages { get; set; }

        public int StartPage { get; set; } = 1;

        public int EndPage { get; set; } = 1;
    }
}
