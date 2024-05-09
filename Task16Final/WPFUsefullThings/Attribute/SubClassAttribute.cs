namespace WPFUsefullThings
{
    public class SubClassAttribute : Attribute
    {
        public bool IsSubClass { get; set; }

        public SubClassAttribute(bool isSubClass = true)
        {
            IsSubClass = isSubClass;
        }
    }
}
