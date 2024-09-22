namespace HS.Message.Share.Attributes
{
    public class FieldAttribute : Attribute
    {
        public string Name { get; set; }

        public string EnumRelation { get; set; }

        public FieldAttribute()
        {
        }

        public FieldAttribute(string name)
        {
            Name = name;
        }

        public FieldAttribute(string name, string enumRelation)
        {
            Name = name;
            EnumRelation = enumRelation;
        }
    }
}
