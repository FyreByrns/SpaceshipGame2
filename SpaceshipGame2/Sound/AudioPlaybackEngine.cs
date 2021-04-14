﻿using System;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace SpaceshipGame2.Sound {
	class AudioPlaybackEngine : IDisposable {
		private readonly IWavePlayer outputDevice;
		private readonly MixingSampleProvider mixer;

		public void PlaySound(string name) {
			AudioFileReader input = new AudioFileReader(name);
			AddMixerInput(new AutoDisposeFileReader(input));
		}

		public void PlaySound(CachedSound sound) {
			AddMixerInput(new CachedSoundSampleProvider(sound));
		}

		private void AddMixerInput(ISampleProvider input) {
			mixer.AddMixerInput(ConvertToRightChannelCount(input));
		}

		private ISampleProvider ConvertToRightChannelCount(ISampleProvider input) {
			if (input.WaveFormat.Channels == mixer.WaveFormat.Channels) {
				return input;
			}
			if (input.WaveFormat.Channels == 1 && mixer.WaveFormat.Channels == 2) {
				return new MonoToStereoSampleProvider(input);
			}
			throw new NotImplementedException("Not yet implemented this channel count conversion");
		}

		public AudioPlaybackEngine(int sampleRate = 44100, int channelCount = 2) {
			outputDevice = new WaveOutEvent();
			mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
			mixer.ReadFully = true;
			outputDevice.Init(mixer);
			outputDevice.Play();
		}

		public void Dispose() {
			outputDevice.Dispose();
		}
	}
}
