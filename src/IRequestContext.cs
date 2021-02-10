using System;
using System.Collections.Generic;
using System.Text;

namespace RequestDecorator
{
    public interface IRequestContext<TI, TR, TC>
    {
        IAPIContext<TC> Context { get; }
        IRequest<TI, TR, TC> RequestInfo { get; }
    }
    
}
