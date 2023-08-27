using System.Collections.Generic;

namespace ApiProgrammingTest.Services
{
    public interface IPropertiesService
    {
        public IEnumerable<PropertyInfo> Get();
        public PropertyInfo Get(int id);
        public List<PropertyInfo> Get(List<int> list);
        public void CreateProperty(PropertyInfo property);
        public bool UpdateProperty(int id, string name, decimal buyPrice, decimal sellPrice, decimal revenue, bool toPurchase, int ownedBy);
        public bool DeleteProperty(int id);
    }
}