﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkSocket.Http;
using Autofac;
using NetworkSocket.Core;

namespace Autofac.NetworkSocket
{
    /// <summary>
    /// Autofac服务行为特性过滤器提供者
    /// </summary>
    internal class HttpFilterAttributeProvider : FilterAttributeProvider
    {
        /// <summary>
        /// 解析提供者
        /// </summary>
        private DependencyResolver dependencyResolver;

        /// <summary>
        /// Autofac服务行为特性过滤器提供者
        /// </summary>
        /// <param name="dependencyResolver">解析提供者</param>
        public HttpFilterAttributeProvider(DependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        /// <summary>
        /// 获取服务行为的特性过滤器   
        /// 并进行属性注入
        /// </summary>
        /// <param name="fastAction">服务行为</param>
        /// <returns></returns>
        public override IEnumerable<IFilter> GetActionFilters(HttpAction fastAction)
        {
            var filters = base.GetActionFilters(fastAction);
            var lifetimeScope = this.dependencyResolver.CurrentLifetimeScope;

            if (lifetimeScope == null)
            {
                return filters;
            }
            return filters.Select(filter => lifetimeScope.InjectProperties(filter));
        }
    }
}
