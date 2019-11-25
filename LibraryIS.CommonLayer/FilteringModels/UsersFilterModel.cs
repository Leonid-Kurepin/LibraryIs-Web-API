using Newtonsoft.Json;

namespace LibraryIS.CommonLayer.FilteringModels
{
    public class UsersFilterModel : FilterModelBase
    {
        public string Name { get; set; }

        public UsersFilterModel()
        {
            Limit = 10;
        }

        public override object Clone()
        {
            var jsonString = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject(jsonString, GetType());
        }
    }
}