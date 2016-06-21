using System;

namespace TrainsProblem.Domain
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Error { get; private set; }
        public string LogMessage { get; private set; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string error, string logMessage = null)
        {
            if (isSuccess && error != string.Empty)
                throw new InvalidOperationException();
            if (!isSuccess && error == string.Empty)
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
            LogMessage = logMessage;
        }

        public static Result Fail(string message, string logMessage = null)
        {
            return new Result(false, message, logMessage);
        }

        public static Result<T> Fail<T>(string message, string logMessage = null)
        {
            return new Result<T>(default(T), false, message, logMessage);
        }

        public static Result Ok(string logMessage = null)
        {
            return new Result(true, String.Empty, logMessage);
        }

        public static Result<T> Ok<T>(T value, string logMessage = null)
        {
            return new Result<T>(value, true, string.Empty, logMessage);
        }
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException();
                return _value;
            }
        }

        protected internal Result(T value, bool isSuccess, string error, string logMessage = null)
            : base(isSuccess, error)
        {
            _value = value;
        }
    }
}