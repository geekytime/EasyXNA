using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyXNA
{
    public class CustomProperties
    {
        Dictionary<string, object> props = new Dictionary<string, object>();

        public void setValue(string name, object value)
        {
            props[name] = value;
        }

        public object getValue(string name)
        {
            return props[name];
        }

        public bool getBool(string name)
        {
            return (bool)props[name];
        }

        public int getInt(string name)
        {
            return (int)props[name];
        }

    }
}
