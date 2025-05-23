using MicroStack.Core.Entitties;
using MicroStack.Core.Repositories;
using MicroStack.Infrastructure.Data;
using MicroStack.Infrastructure.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroStack.Infrastructure.Repository
{
    public class UserRepository : Repository<AppUser>, IUserRepository
    {
        private readonly WebAppContext _context;

        public UserRepository(WebAppContext dbContext)
                : base(dbContext)
        {
            _context = dbContext;
        }
    }
}
