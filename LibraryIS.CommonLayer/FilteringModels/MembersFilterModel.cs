using Newtonsoft.Json;

namespace LibraryIS.CommonLayer.FilteringModels
{
    public class MembersFilterModel : FilterModelBase
    {
        public string Name { get; set; }

        public MembersFilterModel()
        {
            Limit = 5;
        }

        public override object Clone()
        {
            var jsonString = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject(jsonString, GetType());
        }
    }
}