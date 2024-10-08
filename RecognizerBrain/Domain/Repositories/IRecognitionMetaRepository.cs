using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Repositories
{
    public interface IRecognitionMetaRepository
    {
        Task<long> AddRecognitionNode(AddRecognitionNodeModel node);
        Task<long?> FindRecognitionNode(FindRecognitionNodeModel track, int durationDiffThreshold);
    }
}