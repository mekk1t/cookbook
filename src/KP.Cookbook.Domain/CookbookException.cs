namespace KP.Cookbook.Domain
{
    public class CookbookException : Exception
    {
        public CookbookErrorCode ErrorCode { get; }

        public CookbookException(CookbookErrorCode errorCode)
        {
            ErrorCode = errorCode;
        }
    }

    public enum CookbookErrorCode
    {
        AccessDenied = 1
    }
}
