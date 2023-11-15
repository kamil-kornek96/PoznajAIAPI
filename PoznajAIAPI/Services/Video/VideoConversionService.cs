using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Streams;
using Microsoft.AspNetCore.SignalR;
using PoznajAI.Hubs;

namespace PoznajAI.Services.Video
{
    public class VideoConversionService : Hub, IVideoConversionService
    {
        private readonly IConfiguration _configuration;


        public VideoConversionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task ConvertVideo(string inputFilePath, CancellationToken cancellationToken = default)
        {
            try
            {
                var outputPath = GetOutputPath(inputFilePath);

                var mediaInfo = await FFmpeg.GetMediaInfo(inputFilePath);

                IStream videoStream = mediaInfo.VideoStreams.FirstOrDefault()?.SetCodec(VideoCodec.h264);
                IStream audioStream = mediaInfo.AudioStreams.FirstOrDefault()?.SetCodec(AudioCodec.aac);

                var conversion = FFmpeg.Conversions.New()
                    .AddStream(audioStream, videoStream)
                    .AddParameter("-re", ParameterPosition.PreInput)  // Dodaj parametr przed wejściem
                    .SetOutput(outputPath);

                // Dodaj dodatkowe parametry konwersji, np. zmniejszenie rozmiaru
                // Pobierz ustawienia z konfiguracji
                var videoBitrate = _configuration.GetValue<int>("VideoConversion:VideoBitrate");
                var audioBitrate = _configuration.GetValue<int>("VideoConversion:AudioBitrate");
                var resolution = _configuration.GetValue<string>("VideoConversion:Resolution");

                if (videoBitrate != 0)
                    conversion.AddParameter($"-b:v {videoBitrate}k");

                if (audioBitrate != 0)
                    conversion.AddParameter($"-b:a {audioBitrate}k");

                if (!string.IsNullOrEmpty(resolution))
                    conversion.AddParameter($"-vf scale={resolution}");

                conversion.OnProgress += async (sender, args) =>
                {
                    var percent = (int)(Math.Round(args.Duration.TotalSeconds / args.TotalLength.TotalSeconds, 2) * 100);

                };

                // Rozpocznij konwersję z obsługą CancellationToken
                await conversion.Start(cancellationToken);
                Log.Information($"Finished conversion file [{Path.GetFileName(inputFilePath)}]");

                File.Delete(inputFilePath);
                Log.Information($"Source file [{Path.GetFileName(inputFilePath)}] deleted.");
            }
            catch (Exception ex)
            {
                // Obsłuż błędy konwersji
                Log.Information($"Error during video conversion: {ex.Message}");
            }
        }

        private string GetOutputPath(string inputFilePath)
        {
            return inputFilePath.Replace("temp","videos");
        }
    }
}
