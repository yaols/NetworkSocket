﻿using NetworkSocket.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NetworkSocket.Http
{
    /// <summary>
    /// 表示属性的Setter
    /// </summary>
    internal class PropertySetter
    {
        /// <summary>
        /// Api行为的方法成员调用委托
        /// </summary>
        private Func<object, object[], object> methodInvoker;

        /// <summary>
        /// 获取属性的名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 获取属性的类型
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// 属性的Setter
        /// </summary>       
        /// <param name="property">属性</param>        
        public PropertySetter(PropertyInfo property)
        {
            this.methodInvoker = MethodReflection.CreateInvoker(property.GetSetMethod());
            this.Name = property.Name;
            this.Type = property.PropertyType;
        }

        /// <summary>
        /// 设置属性的值
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="value">属性的值</param>
        /// <returns></returns>
        public void SetValue(object instance, object value)
        {
            this.methodInvoker.Invoke(instance, new object[] { value });
        }

        /// <summary>
        /// 字符串显示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// 类型属性的Setter缓存
        /// </summary>
        private static ConcurrentDictionary<Type, PropertySetter[]> cached = new ConcurrentDictionary<Type, PropertySetter[]>();

        /// <summary>
        /// 从类型的属性获取Setter
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static PropertySetter[] FromPropertiesOf(Type type)
        {
            PropertySetter[] setters;
            if (cached.TryGetValue(type, out setters) == false)
            {
                setters = type.GetProperties().Where(p => p.CanWrite && IsSimpleType(p.PropertyType)).Select(p => new PropertySetter(p)).ToArray();
                cached.TryAdd(type, setters);
            }
            return setters;
        }

        /// <summary>
        /// 类型是否为所支持的简单类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private static bool IsSimpleType(Type type)
        {
            if (typeof(IConvertible).IsAssignableFrom(type) == true)
            {
                return true;
            }

            if (typeof(Guid) == type)
            {
                return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var argTypes = type.GetGenericArguments();
                if (argTypes.Length == 1)
                {
                    return IsSimpleType(argTypes.First());
                }
            }
            return false;
        }
    }
}
