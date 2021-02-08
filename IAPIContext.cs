using System;

namespace RequestDecorator
{
    public interface IAPIContextBase
    {
        Guid TraceID { get; }
        LogType EnvironmentLogLevel { get; }
        Func<object, string> SerializerFunc { get; }
        Func<object, Result<string>> TrySerializerFunc { get; }
        Action<LogDataInfoWithInputOutputData> LogRequestInputOutput { get; }
        Action<LogDataInfoWithInputOutputDataAndTiming> LogRequestProcessingTime { get; }
    }
    public interface IAPIContext<out TC> : IAPIContextBase
    {
        TC ContextInfo { get; }
    }

    public class APIContext<TC> : IAPIContext<TC>
    {
        public TC ContextInfo { get; }
        public Guid TraceID { get; }
        public LogType EnvironmentLogLevel { get; }
        public Func<object, string> SerializerFunc { get; }
        public Func<object, Result<string>> TrySerializerFunc => MethodExtension.TryExecuteFunc(SerializerFunc);
        public Action<LogDataInfoWithInputOutputData> LogRequestInputOutput { get; }
        public Action<LogDataInfoWithInputOutputDataAndTiming> LogRequestProcessingTime { get; }
        public APIContext(TC contextInfo
            , Func<object, string> serializerFunc
            , Action<LogDataInfoWithInputOutputData> logRequestInputOutput
            , Action<LogDataInfoWithInputOutputDataAndTiming> logRequestProcessingTime
            )
        {
            ContextInfo = contextInfo;
            LogRequestInputOutput = logRequestInputOutput;
            LogRequestProcessingTime = logRequestProcessingTime;
            SerializerFunc = serializerFunc;
            this.TraceID = Guid.NewGuid();
            this.EnvironmentLogLevel = LogType.Info;
        }
        
    }
}