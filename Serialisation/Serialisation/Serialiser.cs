namespace Serialisation {
    public class Serialiser {
        private enum SerialisedType {
            STRING, INT, LONG, DOUBLE, FLOAT, BOOL, BYTE, NULL, ARRAY, ENUM
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

                        Type elementType = arrayType == SerialisedType.ENUM
                            ? Type.GetType(reader.ReadString())!
                            : GetRuntimeTypeForArrays(arrayType);

                        int arrayLength = reader.Read7BitEncodedInt();
                        Array array = Array.CreateInstance(elementType, arrayLength);

                        for(int j = 0; j < array.Length; j++) {
                            if (reader.ReadByte() != 0) {
                                throw new ApplicationException("Hibás halmaz.");
                            }

                            reader.ReadString(); // discard
                            object? readValue = ReadField(reader, (SerialisedType)reader.ReadByte());
                            
                            array.SetValue(readValue, j);

                            if (reader.ReadByte() != 0) {
                                throw new ApplicationException("Hibás halmaz.");
                            }
                        }

                        toSet = array;
                    } else if (fieldType == SerialisedType.ENUM) {
                        toSet = ReadEnum(reader);
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

        private static object? ReadField(BinaryReader reader, SerialisedType type)
        {
            object? toSet = type switch {
                SerialisedType.STRING => ReadString(reader),
                SerialisedType.INT => ReadInt(reader),
                SerialisedType.LONG => ReadLong(reader),
                SerialisedType.DOUBLE => ReadDouble(reader),
                SerialisedType.FLOAT => ReadFloat(reader),
                SerialisedType.BOOL => ReadBool(reader),
                SerialisedType.BYTE => ReadByte(reader),
                SerialisedType.ENUM => ReadEnum(reader),
                _ => throw new ApplicationException("Ismeretlen típus.")
            };

            return toSet;
        }

        private static void WriteKeyValuePair(BinaryWriter writer, string key, object? value) {
            writer.Write((byte)0);

            writer.Write(key);

            if(value != null) {
                if(value.GetType().IsArray) {
                    Array valueArray = (value as Array)!;

                    writer.Write((byte)SerialisedType.ARRAY);
                    if (value.GetType().GetElementType()!.IsEnum) {
                        writer.Write((byte)SerialisedType.ENUM);
                        writer.Write(value.GetType().GetElementType()!.AssemblyQualifiedName!);
                    } else {
                        writer.Write((byte)GetSerialisedTypeForArrays(value.GetType().GetElementType()!));
                    }

                    writer.Write7BitEncodedInt(valueArray.Length);

                    foreach(object? element in valueArray) {
                        WriteKeyValuePair(writer, "", element);
                    }
                } else if (value.GetType().IsEnum) {
                    WriteEnum(writer, value);
                } else switch (value) {
                    case string s:
                        WriteString(writer, s);
                        break;
                    case int i:
                        WriteInt(writer, i);
                        break;
                    case long l:
                        WriteLong(writer, l);
                        break;
                    case double d:
                        WriteDouble(writer, d);
                        break;
                    case float f:
                        WriteFloat(writer, f);
                        break;
                    case bool b:
                        WriteBool(writer, b);
                        break;
                    case byte value1:
                        WriteByte(writer, value1);
                        break;
                    default:
                        throw new ArgumentException("Ismeretlen típus.");
                }
            } else {
                writer.Write((byte)SerialisedType.NULL);
            }

            writer.Write((byte)0);
        }

        private static Type GetRuntimeTypeForArrays(SerialisedType type)
        {
            Type t = type switch {
                SerialisedType.STRING => typeof(string),
                SerialisedType.INT => typeof(int),
                SerialisedType.LONG => typeof(long),
                SerialisedType.DOUBLE => typeof(double),
                SerialisedType.FLOAT => typeof(float),
                SerialisedType.BOOL => typeof(bool),
                SerialisedType.BYTE => typeof(byte),
                _ => throw new ApplicationException("Ismeretlen típus.")
            };

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

        private static object ReadEnum(BinaryReader reader) {
            string typeName = reader.ReadString();
            int numericValue = reader.Read7BitEncodedInt();
                        
            Type runtimeType = Type.GetType(typeName)!;
            return Enum.ToObject(runtimeType, numericValue);
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

        private static void WriteEnum(BinaryWriter writer, object value) {
            writer.Write((byte)SerialisedType.ENUM);
            writer.Write(value.GetType().AssemblyQualifiedName!);
            writer.Write7BitEncodedInt((int)value);
        }
    }
}