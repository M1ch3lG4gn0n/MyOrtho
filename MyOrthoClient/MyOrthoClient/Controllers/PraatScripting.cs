﻿using System;
using System.Threading.Tasks;
using System.IO;

namespace MyOrthoClient.Controllers
{
    class PraatScripting
    {
        string baseScript = "sound = Read from file: \"{0}\"\r\npitch = {1}\r\nintensity = {2}\r\ntmin = Get start time\r\ntmax = Get end time\r\nTo Pitch: 0.01, 30, pitch\r\nRename: \"pitch\"\r\nselectObject: sound\r\nTo Intensity: intensity, 0.01\r\nRename: \"intensity\"\r\nn = Get number of frames\r\ntimes=0\r\ni=1\r\nwhile times=0\r\n	if i<=n\r\n		intensity = Get value in frame: i\r\n		if intensity > 40\r\n			times = Get time from frame: i\r\n		endif\r\n	endif\r\n	i= i+1\r\nendwhile\r\n\r\nresultats$ = \"{3}\"\r\nfor i to (tmax-tmin)/0.01\r\n    time = tmin + i * 0.01\r\n    selectObject: \"Pitch pitch\"\r\n    pitch = Get value at time: time, \"Hertz\", \"Linear\"\r\n    selectObject: \"Intensity intensity\"\r\n    intensity = Get value at time: time, \"Cubic\"\r\n    appendFileLine: resultats$, fixed$ (time, 2), \" \", fixed$ (pitch, 3), \" \", fixed$ (intensity, 3)\r\nendfor";
        string localAppData;

        public PraatScripting(string exerciceName)
        {
            localAppData = Environment.GetEnvironmentVariable("LocalAppData") + "\\MyOrtho";
            if (!Directory.Exists(localAppData))
            {
                Directory.CreateDirectory(localAppData);
            }
        }

        public async Task<string> WriteScript(string wavPath, int pitch, int intensity, string resultPath)
        {
            if (!File.Exists(wavPath))
            {
                throw new FileNotFoundException("Wav file not found");
            }
            if (!File.Exists(resultPath))
            {
                throw new FileNotFoundException("Result file not found");
            }

            var script = string.Format(baseScript, wavPath, pitch, intensity, resultPath);
            var path = string.Format(localAppData+"\\{0}.praat", Guid.NewGuid().ToString("N"));

            File.WriteAllText(path, script);

            return await Task.FromResult<string>(path);
        }

    }
}
