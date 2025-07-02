namespace HackerNewsApi.Models
{
    public class HackerNewsStory
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Url { get; set; }
        public string? By { get; set; }
        public long Time { get; set; }
        public int Score { get; set; }
        public int Descendants { get; set; }
        public string Type { get; set; } = string.Empty;
        public List<int>? Kids { get; set; }
        
        // Computed properties
        public DateTime TimeStamp => DateTimeOffset.FromUnixTimeSeconds(Time).DateTime;
        public bool HasUrl => !string.IsNullOrEmpty(Url);
    }
}
