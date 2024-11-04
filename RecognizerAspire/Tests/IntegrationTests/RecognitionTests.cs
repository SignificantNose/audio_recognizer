using System.Net.Http.Json;
using Aspire.Hosting;
using Aspire.Hosting.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using RecognizerGateway.Projections;

namespace Tests.IntegrationTests;

public class RecognitionTests
{
    [Fact]
    // meaning, recognize track that exists in the seed 
    // data of the microservices
    public async Task RecognizeExistingTrack_Test(){
        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.RecognizerAspire_AppHost>();

        await using var app = await appHost.BuildAsync();
        await app.StartAsync();
        
        var httpClient = app.CreateHttpClient("gateway");
        JsonContent requestParams = JsonContent.Create(
            new Dictionary<string, string> { 
                { "fingerprint", "string" },
                { "duration", "10" }
            }
        );

        var response = await httpClient.PostAsync(
            "/RecognizerEndpoint", 
            requestParams
            );

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        response.Content.ReadAsStringAsync().Should().Be(string.Empty);
    }

    [Fact]
    public async Task RecognizeMissingTrack_Test(){
        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.RecognizerAspire_AppHost>();

        await using var app = await appHost.BuildAsync();
        await app.StartAsync();
        
        var httpClient = app.CreateHttpClient("gateway");
        JsonContent requestParams = JsonContent.Create(
            new Dictionary<string, string> { 
                { "fingerprint", "AQAD6JmaZJHCJIK-PEW_HM3l4vnxQF3G4Dm0E36GcCX25MXz4JpKcMyOG_fRJUQT8cSdDk-bBNfRLA9RiS0-5UpwVOSOMB_UJzqehFeEUrzh93CSJTlMc6iaSMVl3CLyw1N0xN-GKs4FP-gzlDEqKVPwLcJz_Cn84NxTPOvxRWET-FCtXMh1NGN2NKGkzcgpLcEfbNXxaAlUScwkTNecQsukxDjCB9UWCnn3HOcnvB2aLMqLrocqJRd02eiXjEbKHqESsvjy480jSG8KDx85PBEfPDl0JSKmHCGPcVGGPMsTnE5xOM6hJtKS4lF4gTcFfcmRRUejZEQsKVFKPA90Hn7weLieptCjnHiSDWGO2sHzpHh0vA1-BVlS8egsD-qWPBouDmcW5EeY_eiNk6IGeY5xBY12lH1QGq8GZwvepPiSHZo1HGKF58KTHeqW7dCooxkzI35YNFIchGxkwSkP3cMfOCriJFfwhBrCdGegXeiFWFmMcCGDM3yOX_g7gUp8XIlNPPlxZkLIB012PFIWxM1z_B2aF6HUCM9TXC9OZUKdEM095A_05HhWxG1UmNONf0I2XzEaGzpztBPyNfiO-wj1CFryC8-JeA-FSzyYRxVSz9A8In_wRQ_xLFnA0URzVCkuKg_yiN6gL1mKox-aaEohKz2uW8KPxppRSgyePNBePCnSNsYb1BN-ND2q6oijHP_RH1rOI3sVNIuCXIo-o_ka_CcaDRrn4opHjLrhR9Aj5BGeGNeSy3h5XGgedJmOK1GL6HtwxUdISjnCZD8eSTLUJqSEv_Ae5MtxqT2eo1cFV0kRPvjBJspE3HqCJ4eYNMOJi8o0_EfuVPiExslRnsgmmsGflNBChQ8i3Xhu_BFh58iVFPoRXuiXRrh6PMo99LiOp5exsbrQZwE93WgeIfyIa8shfwYVMTfyFN8FJpPyEP2CXVLa4InwVAkuJccThN6P5hdU7cLVV7iYD2FOCyXD47zRjugf5HiUDspeE9-RG_Z25B2kozziKVLwKcqU4HoGX5jEo2uO8ce5Co6D7iVi5cel0NBzLdgDVzxyZik-HselRItQrsGUa3iU47zAXBbC5FDj5chv_Efto3FaPD4-o9mPiq-QM7qg60ecj2jXwj1RJxl-_AitLGiaSFCRwy8eBl8cCn0eXMSRX8d1nA4JHc2J8T86D2KWI24yrMWDVtrxa5i-o7gx7keurmhOClqVOEJ1hHFoXMtwx2hyXD2cPDnOzGgYCS96NG_xPBFOH-EO40wOSxQuwpNaCdWWQ87hB8-OmEKzFz_SOMuhxhF-2Fo6aN0R8jEa58apYP3RJ4efoytOqtnwZDli8YF8TAmd48rxa_CLB-aOp0cTfsgZ_EZDnSgpPMtRnTgRHg85ol_QPHi2ozlRodyDMzqSH-Vx9Xj045SRJZOO8tCbgpGyaGjEHHqImC5uvDoe8UNPNNMuvEHieBaaS0aeHFR2vDqah4jHHI_qQMuPPlOF-EbKF2_xIsxRGXmGHxJ_XDP6HBZrnDkeSOcRPk1xFnx2XKGO-DDVCvrD4PgDLZaDS3vwvCgvQlfwDqGVo4l4XGikb-iP_IN45IxgXcRPeriFskOTjTEehL-hpU7wo0fz4DmqC2Z2QY_moImDcFfQiJGORxdyCo30HPVydLShLBSeCM4DbWLQhBV6HteWD81RCw--H3eio1E7GeFzHNVXpMfR9LiUfGieUdCeonlK1M6hLruMRg8DTVoyXFKh6AmancF5AayW4--Q5TkeykOa_figZxWoaknR5zilxGijzIPPjOh7aIoP53hy4Rmu44e7qUUPtWHAzIMY7viIp0OpD9Kz4JF6XDilCmGeUJ1QkjmOMJH2obt4dAxzNFx2aG9OMMG547vQ5xIap8TF5EFXNBt16NCaE81jXMrQiGGOSjL6BVOyI29ONBVjVC3y_Wg-1Phx4sen6PBn5EUlHc0_5EyOwk-OnxBNRcdPOHyhTQsu1Wjyo45wKzy2BzlL5OCPK6xQGXwOJ8kP6oZ2zXgSfMGPNrrQjFmOlws0ObCe4oO36DjOb3haVNShC2eyHJrhMWB-cBJaHt-G8CYeJR9CWoeZ7Xh6-HlQWfKhO4cfIZfCIJdBXUkb_Jxw57jMQMuWHD9ScUM4C8-L5kE14jaaSYeiZzX6QBwj1JKChipq5ei1I0yOZ8d5xAt19FLRDN-hfUez4z_RJYf41fCDC6IoVYImccK-7MYXiG2IM1HQjoSU7Pnw9MKeWLhSHw-PkFwifEvQRxuuTUf4zPAZoQ90Mhp65hrSHz5-we7R5_ioC-FT4VlJiHFm7JFi5Mvw48wCnwOfGJdxZvilBllS_dAVHWFSH7WOfTkuxQgzhsfp48nxvnCPaZFOvGGOPwUjh2ii6dB-iDr6HXqFhj9K7qiakEIzHV8Q6UVzoVZ2-MYVDU2cUccz4kbOHNB7NNZQ3igZWnAadAmh-XCOJzv64UqOZwkn4ZZ-7BHR7EVPPFGYo6eRJbmCtZGPH3l25KmYoxKDL0ceOE1yaJSY45nxqQya6R3CMcHDCdNFjFOCkO_w5KjExcKUB7IWxce9xOiR8uiTBcx3PDP8FnaiLjEcqgcvEeF6yM2CMKGPUhF2BnmO6ilc9Fqg9OjRrCeuE9KcVRlaC2LE7MSfw_bxHc2O_yjLoxFU__iSo4kPOczgtOgPfUdv6IT24MepPLAYBio3LuhToWGMD9eOuoJ45IF_uEpc9HgkEQ0q8cGTD6GhJ0LOBPkOh9qHjjZu6ISf48EVJYeW6-hzNBzjos9ktNER5i-u6Wj0FFXD49nR9OjxLEdOkaioHb2y4z5xH80_9GijtCnSMziah0d-NN0WBq2ESpKK5sinKHjaonYMH31q5CFU9ngyXBoNbb3QHN-H7wjDHGeO9DWhZseZg6J6aIqWg-GNJm3wHX3wo1cO4dJhSQlXnHugM4drHNceNEwTQzuPJk6GUKlFhFaOxj50BeGPhnmK68Jlojn6HAcihACMCJCYRAYRQYQFRiBEDRBACUEMAAIgwSxhRAhHhFTAWICEAsIAIRARDACWAAAAKGAQEAYJAQwAQChCDAMEUEAAQEwgBA0CQAiiNCPGAAGQQMQJgghQwAhEmGUEOASEgJAhBQQCACBDBAAIAQaAMAABCAgjCCmDDADECSCFU4gYAAQCRAnBgBEiAAKkYYAgxYhgyipNBCFOGCCAAAQgIARSkgAiAEcCIOAEUEQgAAggQiGiCAFACkYMAMYAIoTQgBggDVGWGAEMQwoIIAAiwACCjDBKKEKkBAQYJQwAQDFCiHeCCUkEdQII4ggwSDBBjHBOGOAIMoQQY5AAhhFgoBNCAAEUIc5AggAVAAEgBDDKADAMEIAahJgRQBiBBKJACUCIAowAwogBhiImiEQASSYMIEIhgAACiAAFEFOMAKoUFYAhI6wgQBChDCEAAgCYYEQYBAEhgCgAAEIAESAMsMIIZ6WCTgAGEBMMCMUIEUICAZpBgBilLTJQEOQEEgAAKgihTAChpTFAMGEAIMwAQAxihAFABGHCOoAAAAAIgpAxwAAkoBCAICEIAAAiJhQkSHFgKCLKEAMANQQAAoADgAkBCFAIEUAAMIYI5awCVkChGDBCAMKMcQgQAoigRBOJiBKAAIkIMUIRZyRQjDGBmEBEMCQIFcAAQgCQQhBAjAFCCKSUUEQwYYDQgiBjhAOAOAIkAIQIBIhBQEGgABBAAAAEUgAoBYBQglhKCGGQAYEAMUACwwgAijAAhSAKIYEMQEAI5YwAQBkhgLDCOCYZEQALAAQigDDhAFGEIMCIFIAZIARhBBEgARCSAYAMMIoZQAwQBAghjCPQAGCQIAgxRgxDgkCAFFiCKQU0QIpAQQQAEAmFAAECCKYAMEQwBTgTQgAgAEBKAAACooAEZQYgChiFCGGMGOCIFUYggiwjADhgEDALCKMAERYoQrSwDA" },
                { "duration", "256" }
            }
        );

        var response = await httpClient.PostAsync(
            "/RecognizerEndpoint", 
            requestParams
            );

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var responseValue = await response.Content.ReadFromJsonAsync<RecognizerResponse>();
        responseValue.Should().NotBeNull();
        responseValue.Title.Should().Be("mirror");
    }
}