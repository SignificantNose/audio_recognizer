using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.Models;

namespace Domain.Repositories
{
    public interface ICoverMetaRepository
    {
        Task<long> AddCoverMeta(AddCoverMetaModel cover);
        Task<string?> GetCoverUri(long id, CoverType coverType);       
    }
}