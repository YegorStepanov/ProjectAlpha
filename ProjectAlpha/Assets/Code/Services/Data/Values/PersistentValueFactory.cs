using UnityEngine;

namespace Code.Services.Data
{
    public static class PersistentValueFactory
    {
        public static PersistentValueWriter<int> CreateInt(int defaultValue, string key)
        {
            PersistentIntWriter writer = new(defaultValue, key);
            writer.Restore();
            return writer;
        }

        public static PersistentValueWriter<bool> CreateBool(bool defaultValue, string key)
        {
            PersistentBoolWriter writer = new(defaultValue, key);
            writer.Restore();
            return writer;
        }

        private sealed class PersistentIntWriter : PersistentValueWriter<int>
        {
            public PersistentIntWriter(int defaultValue, string key) : base(defaultValue, key) { }

            public override void Restore() =>
                Value = PlayerPrefs.GetInt(Key, DefaultValue);

            protected override void Save(int value) =>
                PlayerPrefs.SetInt(Key, value);
        }

        private sealed class PersistentBoolWriter : PersistentValueWriter<bool>
        {
            public PersistentBoolWriter(bool defaultValue, string key) : base(defaultValue, key) { }

            public override void Restore() =>
                Value = ToBool(PlayerPrefs.GetInt(Key, ToInt(DefaultValue)));

            protected override void Save(bool value) =>
                PlayerPrefs.SetInt(Key, ToInt(value));

            private static bool ToBool(int value) =>
                value == 1;

            private static int ToInt(bool value) =>
                value ? 1 : 0;
        }
    }
}
