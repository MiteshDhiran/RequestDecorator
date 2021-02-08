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

    /*
    public class LogDataInfo
    {
        public IAPIContextBase ContextBase { get; } 
        public LogType LogType { get; }
        public string Message { get; }
        public string StackTrace { get; }
        public Exception Exception { get; }
        public Dictionary<string, string> Data { get; }
        public Type RequestType { get; }
        public object RequestData { get; }
        public LogDataInfo(LogType logType, IAPIContextBase contextBase, Type requestType,string message, Exception exception, Dictionary<string, string> data, string stackTrace, object requestData)
        {
            ContextBase = contextBase;
            LogType = logType;
            Message = message ?? string.Empty;
            Exception = exception ;
            Data = data ;
            RequestType = requestType;
            StackTrace = stackTrace ?? string.Empty;
            RequestData = requestData;
        }

        public LogDataInfo(LogType logType, IAPIContextBase contextBase, Type requestType, string message, object requestData) : this(logType, contextBase, requestType,message, null,null,null, requestData)
        {
        }

        public LogDataInfo(LogType logType, IAPIContextBase contextBase, Type requestType,Exception exception, object requestData) : this(logType, contextBase, requestType,string.Empty, exception ?? throw new ArgumentNullException(nameof(exception)), null, null, requestData)
        {
        }
    }
    */
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
