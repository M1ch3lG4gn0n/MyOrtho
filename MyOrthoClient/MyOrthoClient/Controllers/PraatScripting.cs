using System;
using System.Threading.Tasks;
using System.IO;

namespace MyOrthoClient.Controllers
{
    class PraatScripting
    {
        string intensityFrenquencyScript = "sound = Read from file: \"{0}\"\r\npitch = 0\r\nintensity = 0\r\ntmin = Get start time\r\ntmax = Get end time\r\nTo Pitch: 0.01, {1}, {2}\r\nRename: \"pitch\"\r\nselectObject: sound\r\nTo Intensity: 75, 0.01\r\nRename: \"intensity\"\r\nn = Get number of frames\r\ntimes=0\r\ni=1\r\nwhile times=0\r\n	if i<=n\r\n		intensity = Get value in frame: i\r\n		if intensity > {3}\r\n			times = Get time from frame: i\r\n		endif\r\n	endif\r\n	i= i+1\r\nendwhile\r\n\r\nresultats$ = \"{4}\"\r\nfor i to (tmax-tmin)/0.01\r\n    time = tmin + i * 0.01\r\n    selectObject: \"Pitch pitch\"\r\n    pitch = Get value at time: time, \"Hertz\", \"Linear\"\r\n    selectObject: \"Intensity intensity\"\r\n    intensity = Get value at time: time, \"Cubic\"\r\n    appendFileLine: resultats$, fixed$ (time, 2), \" \", fixed$ (pitch, 3), \" \", fixed$ (intensity, 3)\r\nendfor";
        string jitterScript = "pitch_min = {1}\r\npitch_max = {2}\r\ntime_step = 0.3\r\nsilence_threshold = -25\r\nmin_pause = 0.1\r\nmin_voiced = 0.1\r\ntier = 1\r\nsound = Read from file: \"{0}\"\r\ntextgrid = To TextGrid (silences): pitch_min, time_step, \r\n  ... silence_threshold, min_pause, min_voiced, \"silent\", \"sounding\"\r\ntotal_intervals = Get number of intervals: tier\r\nselectObject: sound\r\npitch = To Pitch: 0, pitch_min, pitch_max\r\nselectObject: sound\r\npulses = To PointProcess (periodic, cc): pitch_min, pitch_max\r\nresultats$ = \"{3}\"\r\nfor i to total_intervals\r\n  selectObject: textgrid\r\n  label$ = Get label of interval: tier, i\r\n  if label$ == \"sounding\"\r\n    start = Get start point: tier, i\r\n    end   = Get end point: tier, i\r\n    selectObject: sound, pitch, pulses\r\n    report$ = Voice report: start, end,\r\n      ... pitch_min, pitch_max, 1.3, 1.6, 0.03, 0.45\r\n    jitter = extractNumber (report$, \"Jitter (local): \")\r\n	appendFileLine: resultats$, , fixed$ (start, 2), \" \", fixed$ (end, 2), \" \", percent$ (jitter, 3)\r\n  endif\r\nendfor\r\nremoveObject: textgrid, pitch, pulses\r\n";
        string localAppData;

        public PraatScripting(string exerciceName)
        {
            localAppData = Environment.GetEnvironmentVariable("LocalAppData") + "\\MyOrtho";
            if (!Directory.Exists(localAppData))
            {
                Directory.CreateDirectory(localAppData);
            }
        }

        public string WriteIntensityFrequencyScript(string wavPath, int pitchMin, int pitchMax, int intensityThreshold, string resultPath)
        {
            if (!File.Exists(wavPath))
            {
                throw new FileNotFoundException("Wav file not found");
            }
            if (!File.Exists(resultPath))
            {
                throw new FileNotFoundException("Result file not found");
            }

            var script = string.Format(intensityFrenquencyScript, wavPath, pitchMin, pitchMax, intensityThreshold, resultPath);
            var path = string.Format(localAppData+"\\{0}.praat", Guid.NewGuid().ToString("N"));

            File.WriteAllText(path, script);

            return path;
        }

        public string WriteJitterScript(string wavPath, int pitchMin, int pitchMax, string resultPath)
        {
            if (!File.Exists(wavPath))
            {
                throw new FileNotFoundException("Wav file not found");
            }
            if (!File.Exists(resultPath))
            {
                throw new FileNotFoundException("Result file not found");
            }

            var script = string.Format(jitterScript, wavPath, pitchMin, pitchMax, resultPath);
            var path = string.Format(localAppData + "\\{0}.praat", Guid.NewGuid().ToString("N"));

            File.WriteAllText(path, script);

            return path;
        }

    }
}
