using System;
using Application.Models;
using Application.Services.Interfaces;
using Chromaprint;
using Chromaprint.Utilities;
using Domain.Models;
using Domain.Repositories;

namespace Application.Services;

public class RecognitionService : IRecognitionService
{
    private const int DURATION_DIFF_THRESHOLD = 5;
    private readonly IRecognitionMetaRepository _recognitionRepository;

    public RecognitionService(IRecognitionMetaRepository recognition)
    {
        _recognitionRepository = recognition;
    }



    public async Task<long> AddRecognitionNode(AddRecognitionNodeModel node)
    {
        return await _recognitionRepository.AddRecognitionNode(node);
    }

    public async Task<long?> RecognizeTrack(RecognizeTrackModel recognitionData)
    {
        byte[] acquiredData = ChromaBase64.ByteEncoding.GetBytes(recognitionData.Fingerprint);
        int[] fingerprintData = IFileChromaContext.DecodeFingerprint(acquiredData, true, out _);
        uint fpHash = SimHash.Compute(fingerprintData);

        long? recognizedTrack = await _recognitionRepository.FindRecognitionNode(new FindRecognitionNodeModel{
            IdentificationHash = fpHash,
            Duration = recognitionData.Duration
        }, DURATION_DIFF_THRESHOLD);

        return recognizedTrack;
    }
}
