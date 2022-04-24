﻿using System;

namespace Code.Project
{
    public readonly struct Address: IComparable<Address>, IEquatable<Address>
    {
        public readonly string Key;

        private Address(string key) => Key = key;

        public int CompareTo(Address other) =>
            string.Compare(Key, other.Key, StringComparison.Ordinal);

        public bool Equals(Address other) =>
            Key == other.Key;

        public override bool Equals(object obj) =>
            obj is Address other && Equals(other);

        public override int GetHashCode() =>
            Key.GetHashCode();

        // public static bool operator ==(Address left, Address right) =>
        //     left.Equals(right);
        //
        // public static bool operator !=(Address left, Address right) =>
        //     !left.Equals(right);
        
        public static implicit operator Address(string key) => new(key);
    }
}