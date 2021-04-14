﻿using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;

namespace SpaceshipGame2.Sound {
	class CachedSound {
		public float[] AudioData { get; private set; }
		public WaveFormat WaveFormat { get; private set; }
		public CachedSound(string audioFileName) {
			using (var audioFileReader = new AudioFileReader(audioFileName)) {
				// [TODO] add resampling as required
				WaveFormat = audioFileReader.WaveFormat;
				var wholeFile = new List<float>((int)(audioFileReader.Length / 4));
				var readBuffer = new float[audioFileReader.WaveFormat.SampleRate * audioFileReader.WaveFormat.Channels];
				int samplesRead;
				while ((samplesRead = audioFileReader.Read(readBuffer, 0, readBuffer.Length)) > 0) {
					wholeFile.AddRange(readBuffer.Take(samplesRead));
				}
				AudioData = wholeFile.ToArray();
			}
		}
	}
}
