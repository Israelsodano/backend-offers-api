using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Result
{
    public struct Result
    {
        public static Result<TValue> Ok<TValue>(TValue value) where TValue : class => 
            new Result<TValue>(value);
        public static Result<TValue> Ok<TValue>() where TValue : class, new() =>
           new Result<TValue>(new TValue());
        public static Result<TValue> Fail<TValue>(string message, TValue value = null) where TValue : class => 
            new Result<TValue>(value, new Exception(message));
        public static Result<TValue> Fail<TValue>(Exception exception, TValue value = null) where TValue : class =>
            new Result<TValue>(value, exception);
    }

    public struct Result<TValue> where TValue : class
    {
        internal Result(TValue value, Exception exception = null)
        {
            Value = value;
            Exception = exception;
        }

        public TValue Value { get; private set; }
        public Exception Exception { get; set; }
        public bool IsSuccess { get => Exception is null; }
    }
}
