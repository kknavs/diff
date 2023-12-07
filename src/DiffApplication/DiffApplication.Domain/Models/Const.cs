namespace DiffApplication.Domain.Models
{
    public static class Const
    {
        public enum DiffType
        {
            Left,
            Right
        }

        public static class DiffResultType
        {
            public const string DiffEquals = "Equals";
            public const string SizeDoNotMatch = "SizeDoNotMatch";
            public const string ContentDoNotMatch = "ContentDoNotMatch";
        }
    }
}
