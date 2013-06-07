﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.ModelConfiguration.Conventions
{
    using System.Data.Entity.Config;
    using System.Data.Entity.ModelConfiguration.Configuration.Types;
    using System.Data.Entity.ModelConfiguration.Mappers;
    using System.Data.Entity.ModelConfiguration.Utilities;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    ///     Base class for conventions that process CLR attributes found on properties of types in the model.
    /// </summary>
    /// <remarks>
    ///     Note that the derived convention will be applied for any non-static property on the mapped type that has
    ///     the specified attribute, even if it wasn't included in the model.
    /// </remarks>
    /// <typeparam name="TAttribute"> The type of the attribute to look for. </typeparam>
    public abstract class PropertyAttributeConfigurationConvention<TAttribute>
        : Convention
        where TAttribute : Attribute
    {
        private readonly AttributeProvider _attributeProvider = DbConfiguration.GetService<AttributeProvider>();

        protected PropertyAttributeConfigurationConvention()
        {
            Types().Configure(
                ec =>
                    {
                        foreach (var propertyInfo in ec.ClrType.GetProperties(PropertyFilter.DefaultBindingFlags))
                        {
                            foreach (var attribute in _attributeProvider.GetAttributes(propertyInfo).OfType<TAttribute>())
                            {
                                Apply(propertyInfo, ec, attribute);
                            }
                        }
                    });
        }

        public abstract void Apply(PropertyInfo memberInfo, LightweightTypeConfiguration configuration, TAttribute attribute);
    }
}