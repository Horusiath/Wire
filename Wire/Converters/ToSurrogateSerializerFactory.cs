﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using Wire.ValueSerializers;

namespace Wire.Converters
{
    public class ToSurrogateSerializerFactory : ValueSerializerFactory
    {
        public override bool CanSerialize(Serializer serializer, Type type)
        {
            var surrogate = serializer.Options.Surrogates.FirstOrDefault(s => s.From.IsAssignableFrom(type));
            return surrogate != null;
        }

        public override bool CanDeserialize(Serializer serializer, Type type)
        {
            return false;
        }

        public override ValueSerializer BuildSerializer(Serializer serializer, Type type,
            ConcurrentDictionary<Type, ValueSerializer> typeMapping)
        {
            var surrogate = serializer.Options.Surrogates.FirstOrDefault(s => s.From.IsAssignableFrom(type));
            ValueSerializer objectSerializer = new ObjectSerializer(surrogate.To);
            var toSurrogateSerializer = new ToSurrogateSerializer(surrogate.ToSurrogate, surrogate.To, objectSerializer);
            typeMapping.TryAdd(type, toSurrogateSerializer);

            CodeGenerator.BuildSerializer(serializer, surrogate.To, (ObjectSerializer) objectSerializer);
            return toSurrogateSerializer;
        }
    }
}