﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkSocket.Http
{
    /// <summary>
    /// 表示路由规则
    /// 不可继承
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class RouteAttribute : Attribute
    {
        /// <summary>
        /// 获取路由规则
        /// </summary>
        public string Route { get; private set; }

        /// <summary>
        /// 表示路由规则
        /// 以/开始
        /// </summary>
        /// <param name="route">路由规则</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public RouteAttribute(string route)
        {
            if (string.IsNullOrEmpty(route))
            {
                throw new ArgumentNullException();
            }
            if (route.StartsWith("/") == false)
            {
                throw new ArgumentException("route必须以/开始");
            }
            this.Route = route;
        }
    }
}
