namespace WPFUsefullThings
{
    public class DisplayNamesAttribute : Attribute
    {
        public string Singular { get; private set; }
        public string Plural { get; private set; }

        public DisplayNamesAttribute(string singular, string plural)
        {
            Singular = singular;
            Plural = plural;
        }
    }
}
