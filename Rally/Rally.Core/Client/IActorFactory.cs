using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rally.Core.Client
{
    public interface IActorFactory
    {
        TActorInterface GetActor<TActorInterface>(string actorId) where TActorInterface : class, IActor ;
    }
}
