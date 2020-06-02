using System;
using System.Collections.Generic;

namespace Store.Contractors
{
    public abstract class Field
    {
        public string Label { get; }

        public string Name { get; }

        public string Value { get; }

        protected Field(string label, string name, string value)
        {
            if (string.IsNullOrWhiteSpace(label))
                throw new ArgumentException(nameof(label));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

            Label = label;
            Name = name;
            Value = value;
        }
    }

    public class HiddenField : Field
    {
        public HiddenField(string label, string name, string value)
            : base(label, name, value)
        { }
    }

    public class StringField : Field
    {
        public StringField(string label, string name, string value)
            : base(label, name, value)
        { }
    }

    public class TextField : Field
    {
        public TextField(string label, string name, string value)
            : base(label, name, value)
        { }
    }

    public class SelectionField : Field
    {
        public IReadOnlyDictionary<string, string> Items { get; }

        public SelectionField(string label, string name, string value, IReadOnlyDictionary<string, string> items)
            : base(label, name, value)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            Items = items;
        }
    }
}
