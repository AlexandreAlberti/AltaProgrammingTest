using System.Collections.Generic;

namespace ApiProgrammingTest.Services
{
    public class PropertiesService : IPropertiesService
    {
        readonly PropertyMogulContext context;

        public PropertiesService(PropertyMogulContext context)
        {
            this.context = context;
        }

        public IEnumerable<PropertyInfo> Get()
        {
            return context.Properties;
        }
        public List<PropertyInfo> Get(List<int> list)
        {
            List<PropertyInfo> listReturn = new List<PropertyInfo>();
            foreach(int id in list)
            {
                listReturn.Add(context.Properties.Find(id));
            }
            return listReturn;
        }

        public PropertyInfo Get(int id)
        {
            return context.Properties.Find(id);
        }

        public void CreateProperty(PropertyInfo property)
        {
            context.Properties.Add(property);
            context.SaveChanges();
        }

        public bool UpdateProperty(int id, string name, decimal buyPrice, decimal sellPrice, decimal revenue, bool toPurchase, int ownedBy)
        {
            var existing = context.Properties.Find(id);

            if (existing == null)
            {
                return false;
            }

            existing.Name = name;
            existing.BuyPrice = buyPrice;
            existing.SellPrice = sellPrice;
            existing.RevenuePerHour = revenue;
            existing.AvailableForPurchase = toPurchase;
            existing.OwnedBy = ownedBy;

            context.Update(existing);
            context.SaveChanges();

            return true;
        }

        public bool DeleteProperty(int id)
        {
            var existing = context.Properties.Find(id);

            if (existing != null)
            {
                RemoveProperty(existing.OwnedBy, id);
                context.Properties.Remove(existing);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        private void RemoveProperty(int id, int idProperty)
        {
            var existing = context.Accounts.Find(id);

            if (existing == null)
            {
                return;
            }
            existing.Purchases.Remove(idProperty);
            context.Update(existing);
        }

    }
}