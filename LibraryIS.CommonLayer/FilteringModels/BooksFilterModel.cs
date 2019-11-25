using Newtonsoft.Json;

namespace LibraryIS.CommonLayer.FilteringModels
{
    public class BooksFilterModel : FilterModelBase
    {
        public string Title { get; set; }
        public string AuthorName { get; set; }

        public BooksFilterModel()
        {
            Limit = 3;
        }

        public override object Clone()
        {
            var jsonString = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject(jsonString, GetType());
        }
    }
}