namespace Automix.Models.Domain
{
    public class Pager
    {
        public int TotalItems { get; set; } 
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public int StartPage { get; set; }

        public int EndPage { get; set; }

        public Pager()
        {

        }

        public Pager(int totalItems,int page, int pageSize = 5)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            int currentPage = page;

            int startPage = currentPage;
            int endPage = totalPages; 

            if(startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }

            if(endPage > totalPages)
            {
                endPage = totalPages;
                startPage = endPage - 9;
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;   
            StartPage = startPage;
            EndPage = endPage;
         
        }
    }
}
