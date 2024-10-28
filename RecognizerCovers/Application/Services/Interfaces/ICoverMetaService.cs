using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.Models;
using Domain.Shared;

namespace Application.Services.Interfaces
{
    public interface ICoverMetaService
    {
        Task<Result<long>> AddCoverMeta(AddCoverMetaModel cover);
        Task<Result<string>> GetCoverMeta(long coverId, CoverType coverType);   
    }
}