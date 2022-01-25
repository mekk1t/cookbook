namespace KP.Cookbook.Domain
{
    /// <summary>
    /// Исключение, возникающее в результате нарушения целостности состояния объекта: нарушения его инварианта.
    /// </summary>
    public class InvariantException : Exception
    {
        public InvariantException(string message) : base(message)
        {
        }
    }
}
