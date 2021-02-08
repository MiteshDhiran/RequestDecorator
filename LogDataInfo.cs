using System;
using System.Collections.Generic;
using System.Text;

namespace RequestDecorator
{
    public enum LogType
    {
        Info,
        Error,
        Warning,
        Debug,
        Trace
    }

    public class LogDataInfoWithInputOutputData 
    {
        public IAPIContextBase ContextBase { get; }
        public object RequestData{ get; }
        public object Output { get; }
        public Exception Exception { get; }
        public string RequestTypeName { get; }
        public LogDataInfoWithInputOutputData(IAPIContextBase contextBase, object requestData, object output, Exception exception)
        {
            ContextBase = contextBase ?? throw new ArgumentNullException(nameof(contextBase)); 
            RequestTypeName = requestData?.GetType()?.FullName ?? string.Empty;
            RequestData = requestData;
            Output = output;
            Exception = exception;
        }

        

        public override string ToString()
        {
            if (Exception == null)
            {
                return $"Request Type:{RequestTypeName}. Input Value: {ContextBase.TrySerializerFunc(RequestData)} OutputValue: {ContextBase.TrySerializerFunc(Output)}";
                
            }
            else 
            {
                return
                    $"TraceID: {ContextBase.TraceID.ToString()} Request Type:{RequestTypeName}. Input Value: {ContextBase.TrySerializerFunc(RequestData)} Exception: {Exception?.Message}";
            }
        }
    }

    public class LogDataInfoWithInputOutputDataAndTiming
    {
        public IAPIContextBase ContextBase { get; }
        public string RequestTypeName { get; }
        public object RequestData { get; }
        public long ExecutionTimeInMillisecond { get; }
        public LogDataInfoWithInputOutputDataAndTiming(IAPIContextBase contextBase, object requestData, long executionTimeInMillisecond)
        {
            ContextBase = contextBase;
            RequestTypeName = requestData?.GetType()?.FullName ?? string.Empty;
            RequestData = requestData;
            ExecutionTimeInMillisecond = executionTimeInMillisecond;
        }

        public override string ToString()
        {
            return
                $"TraceID: {ContextBase.TraceID.ToString()} Request Type:{RequestTypeName}. Input Value: {ContextBase.TrySerializerFunc(RequestData)} Time: {ExecutionTimeInMillisecond}ms";
        }
    }

}
