using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Reposatories
{
    public interface IUnitOfWork :IDisposable
    {
        IGenaricReposatiory<TEntity> reposatiory<TEntity>() where TEntity : BaseEntity;
        Task<int> Complete();
    }
}
