using Microsoft.AspNetCore.SignalR;
using PoznajAI.Hubs;
using Serilog;
using Xabe.FFmpeg;

namespace PoznajAI.Services.Video
{
    public class VideoConversionService : Hub, IVideoConversionService
    {
        private readonly IConfiguration _configuration;
        private readonly IHubContext<VideoConversionHub> _hubContext;


        public VideoConversionService(IConfiguration configuration, IHubContext<VideoConversionHub> hubContext)
        {
            _configuration = configuration;
            _hubContext = hubContext;
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
                    .AddParameter("-re", ParameterPosition.PreInput)
                    .SetOutput(outputPath);

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
                    var percent = (int)((args.Duration.TotalSeconds / args.TotalLength.TotalSeconds) * 100);
                    var filename = Path.GetFileName(inputFilePath);
                    await _hubContext.Clients.All.SendAsync(filename, new { FileName = filename, Progress = percent });

                };

                await conversion.Start(cancellationToken);
                await _hubContext.Clients.All.SendAsync(Path.GetFileName(inputFilePath), new { FileName = Path.GetFileName(inputFilePath), Progress = -1 });
                Log.Information($"Finished conversion file [{Path.GetFileName(inputFilePath)}]");

                File.Delete(inputFilePath);
                Log.Information($"Source file [{Path.GetFileName(inputFilePath)}] deleted.");
            }
            catch (Exception ex)
            {
                Log.Error($"Error during video conversion: {ex.Message}");
            }
        }

        private string GetOutputPath(string inputFilePath)
        {
            return inputFilePath.Replace("temp", "videos");
        }
    }
}
