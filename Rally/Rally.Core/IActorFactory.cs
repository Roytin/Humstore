using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rally.Core
{
    public interface IActorFactory
    {
        TCellInterface GetCell<TCellInterface>(string cellId) where TCellInterface : class, IActor ;
    }
}
