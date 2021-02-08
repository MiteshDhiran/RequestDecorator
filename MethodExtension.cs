using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using RequestDecorator.Functional;

namespace RequestDecorator
{
    public static class MethodExtension
    {
        /// <summary>
        /// Enables decoration of function with before and after behavior
        /// </summary>
        /// <typeparam name="TS"></typeparam>
        /// <typeparam name="TI"></typeparam>
        /// <typeparam name="TO"></typeparam>
        /// <param name="funcToBeDecorated"></param>
        /// <param name="funcToInitializeState"></param>
        /// <param name="funcToBeExecutedBefore"></param>
        /// <param name="funcToBeExecutedAfter"></param>
        /// <returns></returns>
        public static Func<TI, TO> PipeLineDecorateFunc<TS, TI, TO>(this Func<TI, TO> funcToBeDecorated
            , Func<TI, TS> funcToInitializeState
            , Func<TS, TI, MayBe<TO>> funcToBeExecutedBefore
            , Func<TS, TI, Result<TO>, TO> funcToBeExecutedAfter)
        {
            return (i) =>
            {
                var state = funcToInitializeState(i);
                try
                {
                    var maybeResult = funcToBeExecutedBefore != null ? funcToBeExecutedBefore(state, i) : new MayBe<TO>(MayBeDataState.DataNotPresent);
                    var retVal = maybeResult.TryGetValue(out var res1) ? res1 : funcToBeDecorated(i);
                    var res = new Result<TO>(retVal);
                    funcToBeExecutedAfter(state, i, res);
                    return retVal;
                }
                catch (Exception ex)
                {
                    return funcToBeExecutedAfter(state, i, new Result<TO>(ex));
                    throw;
                }
            };
        }

        public static Func<T,Result<TO>> TryExecuteFunc<T,TO>(Func<T, TO> func)
        {
            return (T input) =>
            {
                try
                {
                    var res = func(input);
                    return new Result<TO>(res);
                }
                catch (Exception e)
                {
                    return new Result<TO>(e);
                }
            };
        }

        public static string TrySerialize(object objectToSerialize, Func<object, Result<string>> trySerializeFunc)
        {
            if (objectToSerialize == null)
            {
                return "NULL";
            }
            else
            {
                 return trySerializeFunc(objectToSerialize).Select((data) => data
                     , (ex) => $"Error Occurred While Serializing object of type: {objectToSerialize.GetType().FullName}");
            }
        }

    }

    public static class SerializeDeserializeHelper
    {
        public static string GetJSONSerializedObject(object value)
        {
            if (value != null)
            {
                var serializer = new DataContractJsonSerializer(value.GetType());
                using var ms = new MemoryStream();
                serializer.WriteObject(ms, value);
                ms.Flush();
                ms.Position = 0;
                return Encoding.UTF8.GetString(ms.GetBuffer());
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
