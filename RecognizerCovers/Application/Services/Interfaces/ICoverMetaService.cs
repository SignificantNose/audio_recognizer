using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.Models;

namespace Application.Services.Interfaces
{
    public interface ICoverMetaService
    {
        Task<long> AddCoverMeta(AddCoverMetaModel cover);
        Task<string?> GetCoverMeta(long coverId, CoverType coverType);   
    }
}