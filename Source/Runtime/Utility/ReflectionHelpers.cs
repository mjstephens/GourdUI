using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace GourdUI
{
    public static class ReflectionHelpers
    {
        /// From:
        /// https://answers.unity.com/questions/983125/c-using-reflection-to-automate-finding-classes.html
        /// 
        /// Sample usage:
        /// var types = System.AppDomain.CurrentDomain.GetAllDerivedTypes(typeof(YourBaseClass));
        public static System.Type[] GetAllDerivedTypes(this System.AppDomain appDomain, System.Type aType)
        {
            var result = new List<System.Type>();
            var assemblies = appDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsSubclassOf(aType))
                        result.Add(type);
                }
            }
            return result.ToArray();
        }
        
        /// From:
        /// https://answers.unity.com/questions/983125/c-using-reflection-to-automate-finding-classes.html
        /// 
        /// Sample usage:
        /// Type t = System.AppDomain.CurrentDomain.GetTypeByName("typeName");
        public static System.Type GetTypeByName(this System.AppDomain appDomain, string typeName)
        {
            System.Type result = null;
            var assemblies = appDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.Name == typeName)
                        result = type;
                }
            }
            return result;
        }

        public static void SetTypeFieldValue(
            GameObject targetObject, 
            Type targetType, 
            string fieldName, 
            object value)
        {
            Component[] components = targetObject.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                Component co = components[i];
                if (co != null)
                {
                    Type t = co.GetType();
                    if (t == targetType)
                    {
                        System.Reflection.FieldInfo[] fieldInfo = t.GetFields(
                            BindingFlags.Instance | BindingFlags.NonPublic);
                        foreach (System.Reflection.FieldInfo info in fieldInfo)
                        {
                            if (info.Name == fieldName)
                            {
                                info.SetValue(co, value);
                            }
                        }
                    }
                }
            }
        }

        /// From:
        /// https://stackoverflow.com/questions/52797/how-do-i-get-the-path-of-the-assembly-the-code-is-in
        public static string GetPathOfAssemblyForType(Type type)
        {
            string fullPath = System.Reflection.Assembly.GetAssembly(type).Location;
            return Path.GetDirectoryName( fullPath );
        }
    }
}