using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RequestDecorator.Functional;

namespace RequestDecorator.Test
{
    [TestClass]
    public class RequestDecoratorTest
    {
        public APIContext<Context> GetAPIContext()
        {
            var apiContext = new APIContext<Context>(new Context(), SerializeDeserializeHelper.GetJSONSerializedObject
                , (l) =>
                {
                    Console.WriteLine(l.ToString());
                }, (l) =>
                {
                    Console.WriteLine(l.ToString());
                });
            return apiContext;
        }

        [TestMethod]
        public void TestFunctionDecoration()
        {
            var apiContext = GetAPIContext();
            var queryRequest = new QueryRequest(new QueryData(1));
            var model = queryRequest.Process(apiContext).Result;
            Assert.IsTrue(model.ID == queryRequest.Data.ID);
        }
    }

    
    public class QueryData
    {
        public QueryData(int? id)
        {
            ID = id;
        }

        public int? ID { get; }
    }

    public class Model
    {

        public Model(int id)
        {
            ID = id;
        }

        public int ID { get; }
    }

    public class Context
    {

    }

    public class QueryRequest : IRequest<QueryData,Model,Context>
    {
        public QueryRequest(QueryData data)
        {
            Data = data;
        }
        public QueryData Data { get; }

        [JsonIgnore]
        public Func<IRequestContext<QueryData, Model, Context>, Task<Result<Model>>> ProcessRequestFunc =>
            (r) =>
            {
                var model = new Model(r.RequestInfo.Data.ID ?? -1);
                return Task.FromResult(new Result<Model>(model));
            };

        public async Task<Model> Process(IAPIContext<Context> context)
        {
            var decoratedFunc = this.ProcessRequestFunc
                .DecorateRequestWithInputOutputLogging(SerializeDeserializeHelper.GetJSONSerializedObject)
                .DecorateWithExecutionTimeLogger();
            var res = await decoratedFunc(new RequestWithContext<QueryData, Model, Context>(context, this));
            var retVal = res.GetValueThrowExceptionIfExceptionPresent();
            return retVal;
        }
    }
}
