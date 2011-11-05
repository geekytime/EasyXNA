using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyXNA
{
    public class ComponentRegistry_OLD : Dictionary<String, ComponentMapByGuid>
    {
        ComponentMapByGuid allComponents = new ComponentMapByGuid();

        public void Add(EasyGameComponent component)
        {
            //allComponents.Add(component.ObjectId, component);

            //string key = component.Category;
            //if (this.ContainsKey(key) == false)
            //{
            //    ComponentMapByGuid newMap = new ComponentMapByGuid();
            //    newMap.Add(component.ObjectId, component);
            //    this.Add(key, newMap);
            //}
            //else
            //{
            //    ComponentMapByGuid map = this[key];
            //    map.Add(component.ObjectId, component);
            //}
        }

        public List<EasyGameComponent> GetComponentsForClass(String objectClassName)
        {
            return this[objectClassName].Values.ToList<EasyGameComponent>();
        }

        
    }

    public class ComponentMapByGuid : Dictionary<Guid, EasyGameComponent>
    {
    }

}
