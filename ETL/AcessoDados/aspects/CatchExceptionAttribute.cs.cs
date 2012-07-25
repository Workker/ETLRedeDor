using System;
using System.Diagnostics;
using System.Reflection;
using PostSharp.Aspects;
using AcessoDados;

namespace AcessoDados
{
    [Serializable]
    public sealed class CatchExceptionAttribute : OnExceptionAspect
    {
        //[NonSerialized]
        //private readonly Type exceptionType;

        //public CatchExceptionAttribute()
        //{
        //}

        //public CatchExceptionAttribute(Type exceptionType)
        //{
        //    this.exceptionType = exceptionType;
        //}

        //public override Type GetExceptionType(MethodBase targetMethod)
        //{
        //    return this.exceptionType;
        //}

        public override void OnException(MethodExecutionArgs args)
        {
            // Create a unique identifier for this exception.
            Guid guid = Guid.NewGuid();

            //Log de erro
            var logger = log4net.LogManager.GetLogger("LogInFile");

            logger.Error(args.Exception.Message);

            // Replace the exception by a BusinessException.
            args.Exception =
                new BusinessException(string.Format("{0} {1} ",
                                                      guid, args.Exception.Message), args.Exception.InnerException);
            args.FlowBehavior = FlowBehavior.ThrowException;
        }
    }
}