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
                stringBuilder.Append(value is string || value is DateTime ?
                    $" {name} = '{value}' \t" :
                    $" {name} = {value} \t");
                stringBuilder.Append(',');
            }
            if(stringBuilder.Length > 0)
                stringBuilder.Remove(stringBuilder.Length-1, 1);
            return stringBuilder.ToString();
        }

        public string GetPropNames()
        {
            var stringBuilder = new StringBuilder();
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
            {
                stringBuilder.Append(descriptor.Name + '\t');
                stringBuilder.Append(',');
            }
            if (stringBuilder.Length > 0)
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            return stringBuilder.ToString();
        }
        public string GetPropValues()
        {
            var stringBuilder = new StringBuilder();
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
            {
                var value = descriptor.GetValue(this);
                stringBuilder.Append(value is string || value is DateTime ?
                    "'"+value+"'"+ '\t' :
                    value?.ToString() + '\t');
                stringBuilder.Append(',');
            }
            if (stringBuilder.Length > 0)
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            return stringBuilder.ToString();
        }
    }
}
