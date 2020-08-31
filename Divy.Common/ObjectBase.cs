using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Divy.Common
{
    public class ObjectBase
    {
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
            {
                var name = descriptor.Name;
                var value = descriptor.GetValue(this);
                stringBuilder.Append($" {name} = {value} \t");

            }
            return stringBuilder.ToString();
        }
    }
}
