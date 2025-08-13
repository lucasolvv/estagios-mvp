using System.Net;

namespace PlataformaEstagios.Exceptions.ExceptionBase
{
    public abstract class AppBaseException : SystemException
    {
        protected AppBaseException(string message) : base(message) 
        { }

        public abstract IList<string> GetErrorMessages();
        public abstract HttpStatusCode GetStatusCode();
    }
}
