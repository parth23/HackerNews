namespace HackerNewsApi.Models
{
    public class StoryQueryParameters
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; } = string.Empty;

        public bool HasSearch => !string.IsNullOrEmpty(Search);
    }
}
