﻿using System.Collections.Generic;
using System.Linq;
using Wire.Converters;

namespace Wire
{
    public class SerializerOptions
    {
        internal static readonly Surrogate[] EmptySurrogates = new Surrogate[0];
        internal static readonly ValueSerializerFactory[] EmptyValueSerializerFactories = new ValueSerializerFactory[0];

        private static readonly ValueSerializerFactory[] DefaultValueSerializerFactories =
        {
            new ToSurrogateSerializerFactory(),
            new FromSurrogateSerializerFactory(),
            new ArraySerializerFactory(),
            new EnumerableSerializerFactory()
        };

        internal readonly bool PreserveObjectReferences;
        internal readonly Surrogate[] Surrogates;
        internal readonly ValueSerializerFactory[] ValueSerializerFactories;
        internal readonly bool VersionTolerance;

        public SerializerOptions(bool versionTolerance = false, IEnumerable<Surrogate> surrogates = null,
            bool preserveObjectReferences = false, IEnumerable<ValueSerializerFactory> serializerFactories = null)
        {
            VersionTolerance = versionTolerance;
            Surrogates = surrogates?.ToArray() ?? EmptySurrogates;

            //use the default factories + any user defined
            ValueSerializerFactories =
                DefaultValueSerializerFactories.Concat(serializerFactories?.ToArray() ?? EmptyValueSerializerFactories)
                    .ToArray();

            PreserveObjectReferences = preserveObjectReferences;
        }
    }
}