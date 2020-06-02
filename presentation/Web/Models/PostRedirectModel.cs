using System.Collections.Generic;

namespace Web.Models
{
    public class PostRedirectModel
    {
        public string Uri { get; set; }

        public string Description { get; set; }

        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
    }
}
