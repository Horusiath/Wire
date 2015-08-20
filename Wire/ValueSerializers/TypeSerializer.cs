﻿using System;
using System.IO;
using System.Text;

namespace Wire.ValueSerializers
{
    public class TypeSerializer : ValueSerializer
    {
        public static readonly TypeSerializer Instance = new TypeSerializer();
        private readonly byte _manifest = 16;

        public override void WriteManifest(Stream stream, Type type, SerializerSession session)
        {
            stream.WriteByte(_manifest);
        }

        public override void WriteValue(Stream stream, object value, SerializerSession session)
        {
            if (value == null)
            {
                stream.WriteInt32(-1);
            }
            else
            {
                var bytes = Encoding.UTF8.GetBytes((string) value);
                stream.WriteLengthEncodedByteArray(bytes);
            }
        }

        public override object ReadValue(Stream stream, SerializerSession session)
        {
            var length = (int) Int32Serializer.Instance.ReadValue(stream, session);
            if (length == -1)
                return null;

            var buffer = session.GetBuffer(length);

            stream.Read(buffer, 0, length);
            var res = Encoding.UTF8.GetString(buffer, 0, length);
            return res;
        }

        public override Type GetElementType()
        {
            return typeof (Type);
        }
    }
}