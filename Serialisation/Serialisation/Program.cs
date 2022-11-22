namespace Serialisation {
    internal class TestClass
    {
        public string TesztString = "ABCABCABCABCŰŰŰŰÁÉ";
        public int TesztInt = 53;
        public long TesztLong = 62;
        public double TesztDouble = 62.4d;
        public float TesztFloat = 2.3f;
        public byte TesztByte = 234;
        public bool TesztBool = true;

        public int[] TesztIntHalmaz = { 1, 2, 3, 4, 5 };
        public byte[] TesztByteHalmaz = { 55, 66, 77, 88, 99, 255 };
        public string[] TesztStringHalmaz = { "ABC", "DEF", "GHI", "JKL" };

        public override string ToString()
        {
            return $"{nameof(TesztString)}: {TesztString}, {nameof(TesztInt)}: {TesztInt}, {nameof(TesztLong)}: {TesztLong}, {nameof(TesztDouble)}: {TesztDouble}, {nameof(TesztFloat)}: {TesztFloat}, {nameof(TesztByte)}: {TesztByte}, {nameof(TesztBool)}: {TesztBool}, {nameof(TesztIntHalmaz)}: [{string.Join(", ", TesztIntHalmaz)}], {nameof(TesztByteHalmaz)}: [{string.Join(", ", TesztByteHalmaz)}], {nameof(TesztStringHalmaz)}: [{string.Join(", ", TesztStringHalmaz)}]";
        }
    }

    internal class Program {
        static void Main(string[] args) {
            TestClass menteni = new();
            menteni.TesztString = "Nem az alap ami a classba van írva";
            menteni.TesztBool = false;
            menteni.TesztFloat = 6.9f;
            menteni.TesztStringHalmaz[2] = "XYZ";
            
            byte[] data = Serialiser.Serialise(menteni);
            Console.WriteLine(menteni);

            TestClass betoltott = Serialiser.Deserialise<TestClass>(data);
            Console.WriteLine(betoltott);
        }
    }
}