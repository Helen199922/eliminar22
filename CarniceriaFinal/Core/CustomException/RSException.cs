using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core.CustomException
{
    public class RSException : Exception
    {
        public List<String> MessagesError { get; set; }
        public string? TypeError { get; set; }
        public int Code { get; set; }
        public Object DataError { get; set; }

        public RSException(string typeError, int code) : base()
        {
            this.MessagesError = new List<string>();
            this.TypeError = typeError;
            this.Code = code;
        }
        public RSException(string typeError, int code, List<string> messages) : base()
        {
            this.MessagesError = messages;
            this.TypeError = typeError;
            this.Code = code;
        }

        public RSException(string typeError, int code, string message) : base()
        {
            this.MessagesError = new();
            this.MessagesError.Add(message);
            this.TypeError = typeError;
            this.Code = code;
        }
        public RSException SetMessage(string messages)
        {
            this.MessagesError = new List<string>();
            this.MessagesError.Add(messages);
            return this;
        }
        public static RSException NoData(string message)
        {
            RSException except = new RSException("No Data", 404);
            except.MessagesError.Add(message);
            return except;
        }
        public static RSException BadRequest(string message)
        {
            RSException except = new RSException("Hay inconsistencias en los datos envíados", 400);
            except.MessagesError.Add(message);
            return except;
        }
        public static RSException WithData(string typeError, int code, List<string> messages, Object DataError)
        {
            RSException except = new RSException("No Data", 404);
            except.MessagesError = messages;
            except.TypeError = typeError;
            except.Code = code;
            except.DataError = DataError;
            return except;
        }
        public static RSException ErrorQueryDB(string db)
        {
            RSException except = new RSException("No Data", 500);
            except.MessagesError.Add("Ha ocurrido un error al consultar la base de datos: " + db);
            return except;
        }
        public static RSException Unauthorized(string message)
        {
            RSException except = new RSException("No autorizado", 401);
            except.MessagesError.Add(message);
            return except;
        }
    }
}
