using System;
using System.IO;

namespace MyOrthoClient.Controllers
{
    class PraatScripting
    {
        string baseScript = "sound = Read from file: {0}\npitch = {1}\nintensity = {3}\ntmin = Get start time\ntmax = Get end time\nTo Pitch: 0.01, 30, pitch\nRename: \"pitch\"\nselectObject: sound\nTo Intensity: intensity, 0.01\nRename: \"intensity\"\nn = Get number of frames\ntimes=0\ni=1\nwhile times=0\n	if i<=n\n		intensity = Get value in frame: i\n		if intensity > 40\n			times = Get time from frame: i\n		endif\n	endif\n	i= i+1\nendwhile\nresultats$ = {4}\nfor i to (tmax-tmin)/0.01\n    time = tmin + i * 0.01\n    selectObject: \"Pitch pitch\"\n    pitch = Get value at time: time, \"Hertz\", \"Linear\"\n    selectObject: \"Intensity intensity\"\n    intensity = Get value at time: time, \"Cubic\"\n    appendFileLine: resultats$, fixed$ (time, 2), \" \", fixed$ (pitch, 3), \" \", fixed$ (intensity, 3)\nendfor";
        
        public PraatScripting(string exerciceName)
        {
            if (!Directory.Exists(@"%localappdata%\MyOrtho"))
            {
                Directory.CreateDirectory(@"%localappdata%\MyOrtho");
            }
        }

        public string WriteScript(string wavPath, int pitch, int intensity, string resultPath)
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
            var path = string.Format("%localappdata%\\MyOrtho\\{0}.txt", Guid.NewGuid().ToString("N"));
            
            File.WriteAllText(path, script);
            
            return string.Empty;
        }

    }
}
