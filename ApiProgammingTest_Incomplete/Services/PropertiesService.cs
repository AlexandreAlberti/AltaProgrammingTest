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

        public bool UpdateProperty(int id, PropertyInfo property, ISet<PropertyUpdatePropietyEnum> updateFields)
        {
            PropertyInfo propertyFromDB = context.Properties.Find(id);

            if (propertyFromDB == null)
            {
                return false;
            }
            foreach (PropertyUpdatePropietyEnum updatePropertyEnum in updateFields)
            {
                switch(updatePropertyEnum){
                    case PropertyUpdatePropietyEnum.BUY_PRICE:
                        propertyFromDB.BuyPrice = property.BuyPrice;
                        break;
                    case PropertyUpdatePropietyEnum.NAME:
                        propertyFromDB.Name = property.Name;
                        break;
                    case PropertyUpdatePropietyEnum.REVENUE_PER_HOUR:
                        propertyFromDB.RevenuePerHour = property.RevenuePerHour;
                        break;
                    case PropertyUpdatePropietyEnum.SELL_PRICE:
                        propertyFromDB.SellPrice = property.SellPrice;
                        break;
                }
            }
            context.Update(propertyFromDB);
            context.SaveChanges();
            return true;
        }

        public bool UpdatePropertyOwnership(int id, bool toPurchase, int ownedBy)
        {
            PropertyInfo propertyFromDB = context.Properties.Find(id);

            if (propertyFromDB == null)
            {
                return false;
            }

            propertyFromDB.OwnedBy = ownedBy;
            propertyFromDB.AvailableForPurchase = toPurchase;

            context.Update(propertyFromDB);
            context.SaveChanges();

            return true;
        }

        public bool DeleteProperty(int id)
        {
            PropertyInfo propertyFromDB = context.Properties.Find(id);

            if (propertyFromDB != null)
            {
                RemoveProperty(propertyFromDB.OwnedBy, id);
                context.Properties.Remove(propertyFromDB);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        private void RemoveProperty(int id, int idProperty)
        {
            AccountInfoDB account = context.Accounts.Find(id);

            if (account == null)
            {
                return;
            }
            account.Purchases.Remove(idProperty);
            context.Update(account);
        }
    }
}