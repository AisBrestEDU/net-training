﻿using System;
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
            Assembly assemblyUnit = Assembly.Load(assemblyName);
            IEnumerable < string> types2 = assemblyUnit.GetTypes().Where( a=> a.IsClass && a.IsPublic)
                .Where(b=>b.GetCustomAttributes().Any(a=>a.GetType().Name.Equals("ObsoleteAttribute"))).Select(a=> a.Name);

            return types2;
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
            //string[] simpleProp = propertyPath.Split('.');

            //// var managerObj = obj;

            //if (propertyPath.Contains('.'))
            //{
            //    var managerPath = simpleProp.Last();
            //    var managerObj = obj.GetType().GetPropertyValue<object>(simpleProp.First());
            //    GetPropertyValue<T>(managerObj, managerPath);

            //}
            //var simpleResult = obj.GetType().GetProperty(propertyPath);
            //return (T)simpleResult.GetValue(obj, null);
            var result = obj;
            var properties = propertyPath.Split('.');

            foreach (var item in properties)
            {
                result = result.GetType().GetProperty(item).GetValue(result);
            }
            return (T)result;

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
            var result = obj;
            var properties = propertyPath.Split('.');

            if (properties.Length > 1)
                result = result.GetPropertyValue<object>(string.Join(".", properties.Take(properties.Length - 1)));

            var type = result.GetType();
            var setProperty = type.GetProperty(properties.Last());
            while (type.BaseType != null && !setProperty.CanWrite)
            {
                type = type.BaseType;
                setProperty = type.GetProperty(properties.Last());
            }

            setProperty.SetValue(result, value, null);
        }


    }
}
