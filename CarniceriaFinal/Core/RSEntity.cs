using CarniceriaFinal.Core.CustomException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core
{
    public class RSEntity<T>
    {
        public string? TypeError { get; set; }
        public List<string> Message { get; set; }
        public T Data { get; set; }


        public RSEntity<T> Send(T data)
        {
            this.Message = new List<string>();
            this.Data = data;
            this.Message.Add("Proceso realizado correctamente");
            this.TypeError = "";
            return this;
        }
        public RSEntity<T> Fail(string message)
        {

            this.Message = new List<string>();
            this.Message.Add(message);
            this.TypeError = "ERROR";
            return this;
        }
        public RSEntity<T> Fail(List<string> message)
        {

            this.Message = message;
            this.TypeError = "ERROR";
            return this;
        }
        public RSEntity<T> FailWithData(List<string> message, T data)
        {
            this.Message = message;
            this.TypeError = "ERROR";
            this.Data = data;
            return this;
        }
        public RSEntity<T> Fail(RSException err)
        {
            this.Message = err.MessagesError;
            this.TypeError = err.TypeError;
            return this;
        }
        public static RSEntity<T> RS()
        {
            return new RSEntity<T>();
        }
    }
}
