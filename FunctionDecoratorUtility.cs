using System;
using System.Diagnostics;
using System.Threading.Tasks;
using RequestDecorator.Functional;
using static RequestDecorator.Functional.MayBeExtension;

namespace RequestDecorator
{
    public static class FunctionDecoratorUtility
    {
        

        public static Func<IRequestContext<TI, TR,TC>, Task<Result<TR>>> DecorateWithExecutionTimeLogger<TI, TR,TC>(this Func<IRequestContext<TI, TR,TC>, Task<Result<TR>>> funcToBeDecorated) =>
            funcToBeDecorated.PipeLineDecorateFunc<Stopwatch, IRequestContext<TI, TR,TC>, Task<Result<TR>>>(
                (input) =>
                {
                    var sw = new Stopwatch();
                    return sw;
                }
                , (sw, input) =>
                {
                    sw.Start();
                    return new MayBe<Task<Result<TR>>>(MayBeDataState.DataNotPresent);
                }
                , (sw, input, previousResultValue) =>
                {
                    var elapsedMillisecond = sw.ElapsedMilliseconds;
                    sw.Stop();
                    var logData = new LogDataInfoWithInputOutputDataAndTiming(input.Context,input.RequestInfo, elapsedMillisecond);
                    input.Context.LogRequestProcessingTime(logData);
                    return previousResultValue.GetValueThrowExceptionIfExceptionPresent();
                });


        
        public static Func<IRequestContext<TI, TR, TC>, Task<Result<TR>>> DecorateRequestWithInputOutputLogging<TI, TR, TC>(
            this Func<IRequestContext<TI, TR, TC>, Task<Result<TR>>> funcToBeDecorated,
            Func<object,string> serializeFunc)
            =>
                funcToBeDecorated.PipeLineDecorateFunc<int, IRequestContext<TI, TR, TC>, Task<Result<TR>>>(
                    (input) => 0
                    , (sw, input) => GetNothingMaybe<Task<Result<TR>>>()
                    , (sw, input, previousResultValue) =>
                    {
                        var serializedInputData = serializeFunc(input.RequestInfo);
                        if (previousResultValue.TryGetResult(out var previousResultTask))
                        {
                            var taskResult = previousResultTask.Result;
                            if (taskResult.TryGetResult(out var finalResultValue))
                            {
                                var logData = new LogDataInfoWithInputOutputData(input.Context, input.RequestInfo, finalResultValue,null);
                                input.Context.LogRequestInputOutput(logData);
                            }
                            else
                            {
                                var logData = new LogDataInfoWithInputOutputData(input.Context, input.RequestInfo, finalResultValue, null);
                                input.Context.LogRequestInputOutput(logData);
                            }
                        }
                        return previousResultValue.GetValueThrowExceptionIfExceptionPresent();
                    }
                );

    }
}
