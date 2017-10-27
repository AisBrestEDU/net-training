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
            Assembly assembly = Assembly.Load(assemblyName);

            IEnumerable <string> classes = assembly.GetTypes().
                Where(type => type.IsClass && type.IsPublic && type.GetCustomAttributes().
                Any(attr => attr.GetType().Name.Equals("ObsoleteAttribute"))).
                Select(type => type.Name);
            return classes;
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
        public static T GetPropertyValue<T>(this object obj, string propertyPath)
        {
            string[] partsOfPropertyPath = propertyPath.Split('.');
            string path = propertyPath;
            object root = obj;


            if (partsOfPropertyPath.Length > 1)
            {
                //set the current path to the last one of arrays of pathes
                path = partsOfPropertyPath[partsOfPropertyPath.Length - 1];
                //get values from array except the last one
                partsOfPropertyPath = partsOfPropertyPath.TakeWhile((p, i) => i < partsOfPropertyPath.Length - 1)
                    .ToArray();
                //create the new path
                string path2 = String.Join(".", partsOfPropertyPath);
                //invoke method while one element left in the path
                root = obj.GetPropertyValue<object>(path2);
            }
            //return the value of specified property
            var sourceType = root.GetType();
            return (T) sourceType.GetProperty(path)?.GetValue(root, null);
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
        public static void SetPropertyValue(this object obj, string propertyPath, object value)
        {
            var property = obj.GetType().BaseType.GetProperty(propertyPath, BindingFlags.Instance | BindingFlags.NonPublic);
            property?.SetValue(obj, value);
        }


    }
}
