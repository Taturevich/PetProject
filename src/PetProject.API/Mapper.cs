using System;
using System.Reflection;

namespace PetProject
{
    public class Mapper
    {
        public static TEntity MapToEntity<TMapped, TEntity>(TMapped objectToConvert, TEntity entity)
            where TEntity : class
        {
            if (objectToConvert == null || entity == null)
            {
                return null;
            }

            var BusinessObjectType = entity.GetType();
            var BusinessPropList = BusinessObjectType.GetProperties();

            var EntityObjectType = objectToConvert.GetType();
            var EntityPropList = EntityObjectType.GetProperties();

            foreach (var businessPropInfo in BusinessPropList)
            {
                foreach (var entityPropInfo in EntityPropList)
                {
                    if (entityPropInfo.Name != businessPropInfo.Name || entityPropInfo.GetGetMethod().IsVirtual ||
                        businessPropInfo.GetGetMethod().IsVirtual)
                    {
                        continue;
                    }

                    businessPropInfo.SetValue(entity, entityPropInfo.GetValue(objectToConvert, null), null);
                    break;
                }
            }

            return entity;
        }
    }
}
