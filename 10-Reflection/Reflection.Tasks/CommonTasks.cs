using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Reflection.Tasks
{
    public static class CommonTasks
    {

        /// <summary>
        /// Returns the lists of public and obsolete classes for specified assembly.
        /// Please take attention: classes (not interfaces, not structs)
        /// </summary>
        /// <param name="assemblyName">name of assembly</param>
        /// <returns>List of public but obsolete classes</returns>
        public static IEnumerable<string> GetPublicObsoleteClasses(string assemblyName) {
            // TODO : Implement GetPublicObsoleteClasses method
            // throw new NotImplementedException();

            return Assembly.Load(assemblyName).GetTypes()
                .Where(t => t.IsClass)
                .Where(t => t.IsDefined(typeof(ObsoleteAttribute))).Select(t=>t.Name);
        }

        /// <summary>
        /// Returns the value for required property path
        /// </summary>
        /// <example>
        ///  1) 
        ///  string value = instance.GetPropertyValue("Property1")
        ///  The result should be equal to invoking statically
        ///  string value = instance.Property1;
        ///  2) 
        ///  string name = instance.GetPropertyValue("Property1.Property2.FirstName")
        ///  The result should be equal to invoking statically
        ///  string name = instance.Property1.Property2.FirstName;
        /// </example>
        /// <typeparam name="T">property type</typeparam>
        /// <param name="obj">source object to get property from</param>
        /// <param name="propertyPath">dot-separated property path</param>
        /// <returns>property value of obj for required propertyPath</returns>
        public static T GetPropertyValue<T>(this object obj, string propertyPath) {
            // TODO : Implement GetPropertyValue method
            //throw new NotImplementedException();

            Type type;
            PropertyInfo field;
            Object buff = obj;

            var propertys = propertyPath.Split('.');
            foreach (var p in propertys.Take(propertys.Count() - 1))
            {

                type = obj.GetType();
                field = type.GetProperty(p, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                obj = field.GetValue(obj);
            }

            type = obj.GetType();
            field = type.GetProperty(propertys.Last(), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            while (!field.CanRead)
            {
                type = type.BaseType;
                field = type.GetProperty(propertys.Last(), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            }
            return (T)field.GetValue(obj);
        }


        /// <summary>
        /// Assign the value to the required property path
        /// </summary>
        /// <example>
        ///  1)
        ///  instance.SetPropertyValue("Property1", value);
        ///  The result should be equal to invoking statically
        ///  instance.Property1 = value;
        ///  2)
        ///  instance.SetPropertyValue("Property1.Property2.FirstName", value);
        ///  The result should be equal to invoking statically
        ///  instance.Property1.Property2.FirstName = value;
        /// </example>
        /// <param name="obj">source object to set property to</param>
        /// <param name="propertyPath">dot-separated property path</param>
        /// <param name="value">assigned value</param>
        public static void SetPropertyValue(this object obj, string propertyPath, object value) {
            // TODO : Implement SetPropertyValue method
            //throw new NotImplementedException();

            Type type;
            PropertyInfo field;
            Object buff = obj;

            var propertys = propertyPath.Split('.');
            foreach (var p in propertys.Take(propertys.Count() - 1))
            {
                
                type = obj.GetType();
                field = type.GetProperty(p, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                obj = field.GetValue(obj);
            }

            type = obj.GetType();
            field = type.GetProperty(propertys.Last(), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            while (true)
            {
                if (field.CanWrite)
                {
                    field.SetValue(obj, value);
                    break;
                }

                type=type.BaseType;
                field = type.GetProperty(propertys.Last(), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            }
        }


    }
}
