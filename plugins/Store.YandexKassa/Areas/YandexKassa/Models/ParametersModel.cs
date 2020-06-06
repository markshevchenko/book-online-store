using System.Collections.Generic;

namespace Store.YandexKassa.Areas.YandexKassa.Models
{
    public class ParametersModel
    {
        public IReadOnlyDictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();

        public string ReturnUri { get; set; }
    }
}
