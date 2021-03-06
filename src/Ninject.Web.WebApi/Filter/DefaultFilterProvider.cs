﻿// -------------------------------------------------------------------------------------------------
// <copyright file="DefaultFilterProvider.cs" company="Ninject Project Contributors">
//   Copyright (c) 2007-2010 Enkari, Ltd. All rights reserved.
//   Copyright (c) 2010-2017 Ninject Project Contributors. All rights reserved.
//
//   Dual-licensed under the Apache License, Version 2.0, and the Microsoft Public License (Ms-PL).
//   You may not use this file except in compliance with one of the Licenses.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//   or
//       http://www.microsoft.com/opensource/licenses.mspx
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// -------------------------------------------------------------------------------------------------

namespace Ninject.Web.WebApi.Filter
{
    using System.Collections.Generic;
    using System.Linq;

    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    /// <summary>
    /// Provider for the default filters.
    /// </summary>
    public class DefaultFilterProvider : IFilterProvider
    {
        private readonly IKernel kernel;
        private readonly IEnumerable<IFilterProvider> filterProviders;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultFilterProvider"/> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <param name="filterProviders">The filter providers.</param>
        public DefaultFilterProvider(IKernel kernel, DefaultFilterProviders filterProviders)
        {
            this.kernel = kernel;
            this.filterProviders = filterProviders.DefaultFilters;
        }

        /// <summary>
        /// Gets and injects the default filters.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="actionDescriptor">The action descriptor.</param>
        /// <returns>The default filters.</returns>
        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            var filters = this.filterProviders.SelectMany(fp => fp.GetFilters(configuration, actionDescriptor)).ToList();
            foreach (var filter in filters)
            {
                this.kernel.Inject(filter.Instance);
            }

            return filters;
        }
    }
}