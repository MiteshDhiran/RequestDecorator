using System;
using System.Collections.Generic;
using System.Text;

namespace RequestDecorator.Functional
{
    public enum MayBeDataState
    {
        DataPresent,
        DataNotPresent
    }

    public class MayBe<T>
    {
        private T Data { get; }

        private MayBeDataState DataState { get; }


        public MayBe(T data)
        {
            Data = data;
            DataState = MayBeDataState.DataPresent;
        }


        public MayBe(MayBeDataState dataAbsent)
        {
            if (dataAbsent != MayBeDataState.DataNotPresent)
                throw new InvalidOperationException($"Parameter should always be MayBeDataState.DataNotPresent");
            DataState = MayBeDataState.DataNotPresent;
        }


        public bool TryGetValue(out T result)
        {
            if (DataState == MayBeDataState.DataPresent)
            {
                result = Data;
                return true;
            }
            else
            {
                result = default(T);
                return false;
            }
        }
    }

    public static class MayBeExtension
    {
        public static MayBe<T> GetNothingMaybe<T>() => new MayBe<T>(MayBeDataState.DataNotPresent);
    }
}
