using System;
using System.Collections.Generic;
using System.Text;

namespace Rally.Core
{
    public class ActorKey
    {
        public string Id { get; set; }
        public Type InterfaceType { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is ActorKey address)
            {
                return address.InterfaceType == this.InterfaceType
                    && address.Id == this.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public static string ParseToKey(Type type, string id)
        {
            if(!type.IsInterface)
            {
                throw new Exception($"can't find interface {type.FullName}");
            }
            return $"{type.Name}_{id}";
        }
    }
}
