using System;
using System.Collections.Generic;
using System.Text;

namespace RequestDecorator
{
    public class Result<T>
    {
        private T Data { get; set; }

        private Exception Exception { get; set; }

        public bool TryGetResult(out T res)
        {
            if (this.Exception == null)
            {
                res = Data;
                return true;
            }
            else
            {
                res = default(T);
                return false;
            }
        }

        public Result(T data)
        {
            Data = data;
            Exception = null;
        }

        public static Result<T> GetResultFromData(T data)
        {
            return new Result<T>(data);
        }

        public Result(Exception ex)
        {
            this.Exception = ex ?? throw new ArgumentNullException(nameof(ex));
            Data = default(T);
        }

        public void Match(Action<T> onSuccessAction, Action<Exception> onExceptionAction)
        {
            if (Exception == null)
            {
                onSuccessAction(Data);
            }
            else
            {
                onExceptionAction(this.Exception);
            }
        }


        public TO Select<TO>(Func<T, TO> onSuccessFunc, Func<Exception, TO> onExceptionFunc)
        {
            return Exception == null ? onSuccessFunc(Data) : onExceptionFunc(this.Exception);
        }

        public T GetValueThrowExceptionIfExceptionPresent()
        {
            if (Exception == null)
            {
                return Data;
            }
            else
            {
                throw this.Exception;
            }
        }

        public Result<T> FlattenResult(Result<Result<T>> result)
        {
            Result<T> retVal = result.Select((Result<T>  r) => r.Select<Result<T>>(rr => new Result<T>(rr)
                ,(exx) => new Result<T>(exx))  , (ex) => new Result<T>(ex));
            return retVal;
        }
    }
}
