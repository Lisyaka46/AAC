namespace AAC.Classes
{
    public static class Other
    {
        public static Color InvertColor(Color color) =>
            Color.FromArgb(Math.Abs(color.R - 255), Math.Abs(color.G - 255), Math.Abs(color.B - 255));
    }
}
