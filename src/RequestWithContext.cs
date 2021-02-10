using System;
using System.Collections.Generic;
using System.Text;

namespace RequestDecorator
{
    public class RequestWithContext<TI, TR, TC> : IRequestContext<TI, TR, TC>
    {
        public RequestWithContext(IAPIContext<TC> context, IRequest<TI, TR, TC> requestInfo)
        {
            Context = context;
            RequestInfo = requestInfo;
        }

        public IAPIContext<TC> Context { get; }
        public IRequest<TI, TR, TC> RequestInfo { get; }
    }
}
