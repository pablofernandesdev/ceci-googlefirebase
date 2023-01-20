namespace CeciGoogleFirebase.Domain.DTO.Commons
{
    public class QueryFilter
    {
        public QueryFilter()
        {
            Page = 1;
            PerPage = 10;
        }

        public string Search { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
    }
}
