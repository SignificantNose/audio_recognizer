using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.Interfaces;
using Domain.Enums;
using Domain.Models;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Services
{
    public class CoverMetaService : ICoverMetaService
    {
        private readonly ICoverMetaRepository _coverRepository;

        public CoverMetaService(ICoverMetaRepository coverRepository)
        {
            _coverRepository = coverRepository;
        }


        public async Task<Result<long>> AddCoverMeta(AddCoverMetaModel cover)
        {
            return await _coverRepository.AddCoverMeta(cover);
        }

        public async Task<Result<string>> GetCoverMeta(long coverId, CoverType coverType)
        {
            return await _coverRepository.GetCoverUri(coverId, coverType);
        }
    }
}