using System;

namespace PieceOfCake.Shared.ViewModels
{
    public class Envelope<T>
    {
        public T Result { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime TimeGenerated { get; set; }

        #warning Sparation Of Concerns violation
        //required by JsonSerializar along with public properties setters.
        public Envelope()
        {

        }

        protected internal Envelope(T result, string errorMessage)
        {
            Result = result;
            ErrorMessage = errorMessage;
            TimeGenerated = DateTime.UtcNow;
        }
    }

    public class Envelope : Envelope<string>
    {
        public Envelope()
        {

        }

        protected Envelope(string errorMessage)
            : base(string.Empty, errorMessage)
        {
        }

        public static Envelope<T> Ok<T>(T result)
        {
            return new Envelope<T>(result, string.Empty);
        }

        public static Envelope Ok()
        {
            return new Envelope(string.Empty);
        }

        public static Envelope Error(string errorMessage)
        {
            return new Envelope(errorMessage);
        }
    }
}
