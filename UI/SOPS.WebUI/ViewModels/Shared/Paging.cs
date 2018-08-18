using System.Collections.Generic;

namespace SOPS.WebUI.ViewModels.Shared
{
    public class Paging
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfElements { get; set; }

        public int NumberOfPages
        {
            get
            {
                return NumberOfElements / PageSize;
            }
        }

        public int? Previous
        {
            get
            {
                if (CurrentPage == 0)
                    return null;

                return CurrentPage - 1;
            }
        }

        public int? Next
        {
            get
            {
                if (CurrentPage == NumberOfPages || CurrentPage + 1 == NumberOfPages)
                    return null;

                return CurrentPage + 1;
            }
        }

        public IEnumerable<int> Pages
        {
            get
            {
                int startPage = 0,
                    endPage = 0;

                bool firstPart = CurrentPage <= 3;
                bool secondPart = NumberOfPages - CurrentPage <= 3;


                if (CurrentPage <= 3)
                {
                    startPage = 0;
                    int end = 7 - NumberOfPages;

                    endPage = end <= 0 ? 7 : 7 - end + 1;
                }

                if (NumberOfPages - CurrentPage <= 3)
                {
                    endPage = NumberOfPages;
                    int start = endPage - 7 + 1;

                    startPage = start <= 0 ? 0 : start;
                } 

                if (firstPart == false && secondPart == false)
                {
                    startPage = CurrentPage - 3;
                    endPage = CurrentPage + 4;
                }

                for (int i = startPage; i < endPage; i++)
                {
                    yield return i;
                }
            }
        }
    }
}