﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Invoices
{
    class EnumBindingSourceExtension : MarkupExtension
    {
        #region FIELDS
        private Type _enumType;
        #endregion

        #region PROPERTIES
        /// <summary>
        /// Gets the type of the enum.
        /// </summary>
        /// <value>
        /// The type of the enum.
        /// </value>
        /// <exception cref="ArgumentException">Type must be an Enum.</exception>
        public Type EnumType
        {
            get => _enumType;
            private set
            {
                if (_enumType == value)
                    return;

                var enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (enumType.IsEnum == false)
                    throw new ArgumentException("Type must be an Enum.");

                _enumType = value;
            }
        }
        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBindingSourceExtension"/> class.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <exception cref="ArgumentNullException">enumType</exception>
        public EnumBindingSourceExtension(Type enumType)
        {
            EnumType = enumType ?? throw new ArgumentNullException(nameof(enumType));
        }
        #endregion

        #region PUBLIC

        #endregion

        #region PRIVATE

        #endregion

        #region OVERRIDES
        /// <summary>
        /// Provides the value.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var enumValues = Enum.GetValues(EnumType);

            return (
                from object enumValue in enumValues
                select new EnumerationMember
                {
                    Value = enumValue,
                }).ToArray();
        }
        #endregion

        #region  NESTED CLASS
        /// <summary>
        /// Enumeration Member Class for static object.
        /// </summary>
        public class EnumerationMember
        {
            public object Value { get; set; }
        }
        #endregion
    }
}
