namespace KitProjects.Cookbook.UI
{
    public static class Extensions
    {
        private readonly static DecimalFormatProvider _formatProvider = new();

        public static string ToCustomString(this decimal value) =>
            string.Format(_formatProvider, "{0}", value);
    }
}
