using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RequestDecorator.Functional;

namespace RequestDecorator
{
    public interface IRequest<TI, TR, TC>
    {
        
        Func<Func<IRequestContext<TI, TR, TC>, Task<Result<TR>>>, Func<IRequestContext<TI, TR, TC>, Task<Result<TR>>>>
            FunctionDecorator => (Func<IRequestContext<TI, TR, TC>, Task<Result<TR>>> inputFunc) => inputFunc;

        TI Data { get; }


        
        Func<IRequestContext<TI, TR, TC>, Task<Result<TR>>> ProcessRequestFunc { get; }

        Task<TR>  Process(IAPIContext<TC> context);
        
    }

    
}
