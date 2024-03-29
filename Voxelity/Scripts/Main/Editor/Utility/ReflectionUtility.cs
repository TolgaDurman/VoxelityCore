using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Voxelity.Editor
{
    public static class ReflectionUtility
    {
        public static TypeInfo[] GetTypesWith<T>(BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) where T : Attribute
        {
            List<TypeInfo> typesWithAttribute = new List<TypeInfo>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.GetCustomAttribute<T>() != null)
                        {
                            typesWithAttribute.Add(type.GetTypeInfo());
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    Debug.LogWarning($"Failed to load types from assembly '{assembly.FullName}': {ex.Message}");
                    foreach (var loaderException in ex.LoaderExceptions)
                    {
                        Debug.LogWarning($"  {loaderException.Message}");
                    }
                }
            }
            return typesWithAttribute.ToArray();
        }

        public static MemberInfo[] GetMembersWith<T>(BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) where T : Attribute
        {
            List<MemberInfo> members = new List<MemberInfo>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type typ in types)
                {
                    if (!typ.IsClass)
                        continue;

                    MemberInfo[] memberInfos = typ.GetMembers(flags);
                    foreach (MemberInfo member in memberInfos)
                    {
                        if (member.CustomAttributes.ToArray().Length > 0)
                        {
                            T attribute = member.GetCustomAttribute<T>();
                            if (attribute != null)
                                members.Add(member);
                        }
                    }
                }
            }
            return members.ToArray();
        }
        public static MethodInfo[] GetMethodsWith<T>(BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) where T : Attribute
        {
            List<MethodInfo> methods = new List<MethodInfo>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type typ in types)
                {
                    if (!typ.IsClass)
                        continue;

                    MethodInfo[] methodInfos = typ.GetMethods(flags);
                    foreach (MethodInfo method in methodInfos)
                    {
                        if (method.CustomAttributes.ToArray().Length > 0)
                        {
                            T attribute = method.GetCustomAttribute<T>();
                            if (attribute != null)
                                methods.Add(method);
                        }
                    }
                }
            }
            return methods.ToArray();
        }
        public static FieldInfo[] GetFieldsWith<T>(BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) where T : Attribute
        {
            List<FieldInfo> fields = new List<FieldInfo>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type typ in types)
                {
                    if (!typ.IsClass)
                        continue;

                    FieldInfo[] fieldInfos = typ.GetFields(flags);
                    foreach (FieldInfo field in fieldInfos)
                    {
                        if (field.CustomAttributes.ToArray().Length > 0)
                        {
                            T attribute = field.GetCustomAttribute<T>();
                            if (attribute != null)
                                fields.Add(field);
                        }
                    }
                }
            }
            return fields.ToArray();
        }
        public static MethodInfo[] GetMethodsWithInterface<T>(BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) where T : class
        {
            List<MethodInfo> methods = new List<MethodInfo>();
            var type = typeof(T);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));
            foreach (var typ in types)
            {
                if (!typ.IsClass)
                    continue;
                Debug.Log(typ.Name);

                MethodInfo[] methodInfos = typ.GetMethods(flags);
                foreach (MethodInfo method in methodInfos)
                {
                    if (method.IsPublic && method.DeclaringType != typeof(object))
                    {
                        methods.Add(method);
                    }
                }
            }
            return methods.ToArray();
        }
    }
}
