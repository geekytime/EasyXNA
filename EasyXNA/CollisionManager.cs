using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyXNA
{
    public class CollisionManager
    {
        Dictionary<string, Action<EasyGameComponent, EasyGameComponent>> handlers = new Dictionary<string, Action<EasyGameComponent, EasyGameComponent>>();

        public void AddHandler(string categoryOne, string categoryTwo, Action<EasyGameComponent, EasyGameComponent> anonymousAction)
        {
            string key = keyify(categoryOne, categoryTwo);            
            handlers[key] = anonymousAction;        
        }

        private string keyify(string categoryOne, string categoryTwo)
        {
            return categoryOne + ":" + categoryTwo;
        }

        public bool FireEvent(EasyGameComponent componentOne, EasyGameComponent componentTwo)
        {
            if (componentOne.IsRemoved || componentTwo.IsRemoved)
            {
                return false;
            }

            //The order of the keys matters, because the game loop will be expecting to get the components in the right order
            string keyOneFirst = keyify(componentOne.Category, componentTwo.Category);
            if (handlers.ContainsKey(keyOneFirst))
            {
                handlers[keyOneFirst].Invoke(componentOne, componentTwo);
                return true;
            }

            string keyTwoFirst = keyify(componentTwo.Category, componentOne.Category);
            if (handlers.ContainsKey(keyTwoFirst))
            {
                handlers[keyTwoFirst].Invoke(componentTwo, componentOne);
                return true;
            }

            return false;
        }     

        
    }
}
