namespace Serialisation {
    public class Serialiser {
        private enum SerialisedType {
            STRING, INT, LONG, DOUBLE, FLOAT, BOOL, BYTE, NULL, ARRAY
        }

        public static byte[] Serialise(object? obj) {
            if(obj == null) {
                return Array.Empty<byte>();
            }

            MemoryStream memoryStream = new();
            BinaryWriter writer = new(memoryStream);

            Type dataType = obj.GetType();

            writer.Write7BitEncodedInt(dataType.GetFields().Length);

            foreach (var field in dataType.GetFields()) {
                WriteKeyValuePair(writer, field.Name, field.GetValue(obj));
            }

            return memoryStream.ToArray();
        }

        public static T Deserialise<T>(byte[] data) {
            if(data.Length == 0) {
                throw new ArgumentException("Bemeneti halmaz nem lehet 0 hosszú.");
            }

            MemoryStream memoryStream = new(data);
            BinaryReader reader = new(memoryStream);

            Type dataType = typeof(T);
            object createdObject = dataType.GetConstructor(Type.EmptyTypes)!.Invoke(Array.Empty<object?>());

            int fieldCount = reader.Read7BitEncodedInt();

            for(int i = 0; i < fieldCount; i++) {
                if(reader.ReadByte() != 0) {
                    throw new ApplicationException("Hibás adatok.");
                }

                string fieldKey = reader.ReadString();
                SerialisedType fieldType = (SerialisedType)reader.ReadByte();

                object? toSet = null;

                if(fieldType != SerialisedType.NULL) {
                    if(fieldType == SerialisedType.ARRAY) {
                        SerialisedType arrayType = (SerialisedType)reader.ReadByte();
                        int arrayLength = reader.Read7BitEncodedInt();
                        Array array = Array.CreateInstance(GetRuntimeTypeForArrays(arrayType), arrayLength);

                        for(int j = 0; j < array.Length; j++) {
                            if (reader.ReadByte() != 0) {
                                throw new ApplicationException("Hibás halmaz.");
                            }

                            reader.ReadString(); // discard
                            array.SetValue(ReadField(reader, (SerialisedType)reader.ReadByte()), j);

                            if (reader.ReadByte() != 0) {
                                throw new ApplicationException("Hibás halmaz.");
                            }
                        }

                        toSet = array;
                    } else {
                        toSet = ReadField(reader, fieldType);
                    }
                }

                dataType.GetField(fieldKey)!.SetValue(createdObject, toSet);

                if (reader.ReadByte() != 0) {
                    throw new ApplicationException("Hibás adatok.");
                }
            }

            return (T) createdObject;
        }

        private static object? ReadField(BinaryReader reader, SerialisedType type) {
            object? toSet;

            switch (type) {
                case SerialisedType.STRING:
                    toSet = ReadString(reader);
                    break;
                case SerialisedType.INT:
                    toSet = ReadInt(reader);
                    break;
                case SerialisedType.LONG:
                    toSet = ReadLong(reader);
                    break;
                case SerialisedType.DOUBLE:
                    toSet = ReadDouble(reader);
                    break;
                case SerialisedType.FLOAT:
                    toSet = ReadFloat(reader);
                    break;
                case SerialisedType.BOOL:
                    toSet = ReadBool(reader);
                    break;
                case SerialisedType.BYTE:
                    toSet = ReadByte(reader);
                    break;
                default:
                    throw new ApplicationException("Ismeretlen típus.");
            }

            return toSet;
        }

        private static void WriteKeyValuePair(BinaryWriter writer, string key, object? value) {
            writer.Write((byte)0);

            writer.Write(key);

            if(value != null) {
                if(value.GetType().IsArray) {
                    Array valueArray = (value as Array)!;

                    writer.Write((byte)SerialisedType.ARRAY);
                    writer.Write((byte)GetSerialisedTypeForArrays(value.GetType().GetElementType()!));
                    writer.Write7BitEncodedInt(valueArray.Length);

                    foreach(object? element in valueArray) {
                        WriteKeyValuePair(writer, "", element);
                    }
                } else if(value.GetType() == typeof(string)) {
                    WriteString(writer, (string)value);
                } else if(value.GetType() == typeof(int)) {
                    WriteInt(writer, (int)value);
                } else if(value.GetType() == typeof(long)) {
                    WriteLong(writer, (long)value);
                } else if(value.GetType() == typeof(double)) {
                    WriteDouble(writer, (double)value);
                } else if(value.GetType() == typeof(float)) {
                    WriteFloat(writer, (float)value);
                } else if(value.GetType() == typeof(bool)) {
                    WriteBool(writer, (bool)value);
                } else if(value.GetType() == typeof(byte)) {
                    WriteByte(writer, (byte)value);
                } else {
                    throw new ArgumentException("Ismeretlen típus.");
                }
            } else {
                writer.Write((byte)SerialisedType.NULL);
            }

            writer.Write((byte)0);
        }

        private static Type GetRuntimeTypeForArrays(SerialisedType type) {
            Type t;
            
            switch(type) {
                case SerialisedType.STRING:
                    t = typeof(string);
                    break;
                case SerialisedType.INT:
                    t = typeof(int);
                    break;
                case SerialisedType.LONG:
                    t = typeof(long);
                    break;
                case SerialisedType.DOUBLE:
                    t = typeof(double);
                    break;
                case SerialisedType.FLOAT:
                    t = typeof(float);
                    break;
                case SerialisedType.BOOL:
                    t = typeof(bool);
                    break;
                case SerialisedType.BYTE:
                    t = typeof(byte);
                    break;
                default:
                    throw new ApplicationException("Ismeretlen típus.");
            }

            return t;
        }

        private static SerialisedType GetSerialisedTypeForArrays(Type type) {
            SerialisedType t;

            if (type == typeof(string)) {
                t = SerialisedType.STRING;
            } else if (type == typeof(int)) {
                t = SerialisedType.INT;
            } else if (type == typeof(long)) {
                t = SerialisedType.LONG;
            } else if (type == typeof(double)) {
                t = SerialisedType.DOUBLE;
            } else if (type == typeof(float)) {
                t = SerialisedType.FLOAT;
            } else if (type == typeof(bool)) {
                t = SerialisedType.BOOL;
            } else if (type == typeof(byte)) {
                t = SerialisedType.BYTE;
            } else {
                throw new ArgumentException("Ismeretlen típus.");
            }

            return t;
        }

        private static string ReadString(BinaryReader reader) {
            return reader.ReadString();
        }

        private static int ReadInt(BinaryReader reader) {
            return reader.Read7BitEncodedInt();
        }

        private static long ReadLong(BinaryReader reader) {
            return reader.Read7BitEncodedInt64();
        }

        private static double ReadDouble(BinaryReader reader) {
            return reader.ReadDouble();
        }

        private static float ReadFloat(BinaryReader reader) {
            return (float) reader.ReadDouble();
        }

        private static bool ReadBool(BinaryReader reader) {
            return reader.ReadBoolean();
        }

        private static byte ReadByte(BinaryReader reader) {
            return reader.ReadByte();
        }

        private static void WriteString(BinaryWriter writer, string value) {
            writer.Write((byte)SerialisedType.STRING);
            writer.Write(value);
        }

        private static void WriteInt(BinaryWriter writer, int value) {
            writer.Write((byte)SerialisedType.INT);
            writer.Write7BitEncodedInt(value);
        }

        private static void WriteLong(BinaryWriter writer, long value) {
            writer.Write((byte)SerialisedType.LONG);
            writer.Write7BitEncodedInt64(value);
        }

        private static void WriteDouble(BinaryWriter writer, double value) {
            writer.Write((byte)SerialisedType.DOUBLE);
            writer.Write(value);
        }

        private static void WriteFloat(BinaryWriter writer, float value) {
            writer.Write((byte)SerialisedType.FLOAT);
            writer.Write((double)value);
        }

        private static void WriteBool(BinaryWriter writer, bool value) {
            writer.Write((byte)SerialisedType.BOOL);
            writer.Write(value);
        }

        private static void WriteByte(BinaryWriter writer, byte value) {
            writer.Write((byte)SerialisedType.BYTE);
            writer.Write(value);
        }
    }
}