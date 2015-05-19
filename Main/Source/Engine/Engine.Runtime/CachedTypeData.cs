using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mud.Engine.Runtime
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Represents a Type, its properties and attributes
    /// </summary>
    internal class CachedTypeData
    {
        /// <summary>
        /// The attributes bag
        /// </summary>
        private readonly ConcurrentDictionary<PropertyInfo, IEnumerable<Attribute>> propertyAttributesBag;

        /// <summary>
        /// The properties bag
        /// </summary>
        private ConcurrentBag<PropertyInfo> propertiesBag;

        /// <summary>
        /// A thread-safe collection of attributes for a Type
        /// </summary>
        private ConcurrentBag<Attribute> typeAttributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedTypeData"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        internal CachedTypeData(Type type)
        {
            ExceptionFactory.ThrowIf<ArgumentNullException>(type == null, "Type is required.");
            this.Type = type;
            this.propertiesBag = new ConcurrentBag<PropertyInfo>();
            this.propertyAttributesBag = new ConcurrentDictionary<PropertyInfo, IEnumerable<Attribute>>();
            this.typeAttributes = new ConcurrentBag<Attribute>();
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        internal Type Type { get; private set; }

        /// <summary>
        /// Gets all of the attributes for the Type that is associated with an optional property and optionally filtered.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a collection of Attributes</returns>
        internal IEnumerable<Attribute> GetAttributes(PropertyInfo property = null, Func<Attribute, bool> predicate = null)
        {
            this.SetupAttributesBag();

            // If property is null, then return all attributes.
            var attributes = new List<Attribute>();
            if (property == null)
            {
                // build a collection of attributes
                foreach (var pair in this.propertyAttributesBag)
                {
                    attributes.AddRange(pair.Value);
                }

                attributes.AddRange(this.typeAttributes);

                // Return all attributes for the Type, filtered if needed
                return predicate == null
                    ? attributes
                    : attributes.Where(predicate);
            }

            // Fetch attributes for the given property
            attributes.AddRange(this.propertyAttributesBag.GetOrAdd(
                property,
                prop => prop.GetCustomAttributes(true).Cast<Attribute>()));

            // Return all attributes for property or filtered by predicate
            return predicate == null
                ? attributes
                : attributes.Where(predicate);
        }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns an Attribute</returns>
        internal Attribute GetAttribute(PropertyInfo property = null, Func<Attribute, bool> predicate = null)
        {
            return this.GetAttributes(property, predicate).FirstOrDefault();
        }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="property">The property.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a collection of Attributes matching T</returns>
        internal IEnumerable<TAttribute> GetAttributes<TAttribute>(PropertyInfo property = null, Func<TAttribute, bool> predicate = null)
            where TAttribute : Attribute
        {
            this.SetupAttributesBag();

            if (property == null)
            {
                // If property is null, then return all attributes.
                var filteredAttributes = new List<TAttribute>();

                // build a collection of attributes
                foreach (var pair in this.propertyAttributesBag)
                {
                    filteredAttributes.AddRange(pair.Value.OfType<TAttribute>());
                }

                filteredAttributes.AddRange(this.typeAttributes.OfType<TAttribute>());

                // Return all attributes for the Type, filtered if needed
                return predicate == null
                    ? filteredAttributes
                    : filteredAttributes.Where(predicate);
            }

            // Fetch attributes for the given property
            var attributes = this.propertyAttributesBag.GetOrAdd(
                property,
                prop => prop.GetCustomAttributes(true).Cast<Attribute>());

            // Return all attributes for property or filtered by predicate
            return predicate == null
                ? attributes.OfType<TAttribute>()
                : attributes.OfType<TAttribute>().Where(predicate);
        }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="property">The property.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns an Attribute matching T</returns>
        internal TAttribute GetAttribute<TAttribute>(PropertyInfo property = null, Func<TAttribute, bool> predicate = null) where TAttribute : Attribute
        {
            return this.GetAttributes<TAttribute>(property, predicate).FirstOrDefault();
        }

        /// <summary>
        /// Gets all of the properties.
        /// </summary>
        /// <returns>Returns a collection of PropertyInfo instances</returns>
        internal IEnumerable<PropertyInfo> GetProperties()
        {
            this.SetupPropertiesBag();
            return this.propertiesBag;
        }

        /// <summary>
        /// Gets the properties matching the predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a collection of PropertyInfo instances</returns>
        internal IEnumerable<PropertyInfo> GetProperties(Func<PropertyInfo, bool> predicate)
        {
            this.SetupPropertiesBag();
            return this.propertiesBag.Where(predicate);
        }

        /// <summary>
        /// Gets a property matching the predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Returns a PropertyInfo</returns>
        internal PropertyInfo GetProperty(Func<PropertyInfo, bool> predicate)
        {
            this.SetupPropertiesBag();
            return this.propertiesBag.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Setups the attributes bag.
        /// </summary>
        private void SetupAttributesBag()
        {
            if (this.propertyAttributesBag.IsEmpty)
            {
                this.SetupPropertiesBag();
                foreach (PropertyInfo property in this.propertiesBag)
                {
                    this.propertyAttributesBag.AddOrUpdate(
                        property,
                        info => property.GetCustomAttributes(true).Cast<Attribute>(),
                        (propertyInfo, currentBag) => currentBag);
                }

                this.typeAttributes = new ConcurrentBag<Attribute>(
                    this.Type.GetTypeInfo().GetCustomAttributes(true).Cast<Attribute>());
            }
        }

        /// <summary>
        /// Setups the properties bag.
        /// </summary>
        private void SetupPropertiesBag()
        {
            if (this.propertiesBag.IsEmpty)
            {
                this.propertiesBag = new ConcurrentBag<PropertyInfo>(this.Type.GetRuntimeProperties());
            }
        }
    }
}
