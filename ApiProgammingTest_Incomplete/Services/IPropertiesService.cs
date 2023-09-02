using System.Collections.Generic;

namespace ApiProgrammingTest.Services
{
    public interface IPropertiesService
    {
        public IEnumerable<PropertyInfo> Get();
        public PropertyInfo Get(int id);
        public List<PropertyInfo> Get(List<int> list);
        public void CreateProperty(PropertyInfo property);
        public bool UpdateProperty(int id, PropertyInfo property, ISet<PropertyUpdatePropietyEnum> updateFields);
        public bool UpdatePropertyOwnership(int id, bool toPurchase, int ownedBy);
        public bool DeleteProperty(int id);
    }
}