using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Interfaces
{
    public interface IUserResolverService
    {
        public Guid? GetUserId();
    }
}
