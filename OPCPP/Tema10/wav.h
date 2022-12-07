#pragma once

#include <cstdint>
#include <fstream>
#include <cmath>

struct Wav
{
	Wav(Wav&) = delete;

	Wav() : data(nullptr)
	{
	}

	uint8_t riffChunkId[4]{};
	uint32_t riffChunkSize{};
	uint8_t riffFormat[4]{};

	uint8_t fmtSubchunk1Id[4]{};
	uint32_t fmtSubchunk1Size{};
	uint16_t fmtAudioFormat{};
	uint16_t fmtNumChannels{};
	uint32_t fmtSampleRate{};
	uint32_t fmtByteRate{};
	uint16_t fmtBlockAlign{};
	uint16_t fmtBitsPerSample{};

	uint8_t id[4]{};
	uint32_t size{};
	uint8_t* data;
	uint32_t framesCount{};

	bool LoadWavFile(const char* filePath)
	{
		FILE* fp;

		errno_t err = fopen_s(&fp, filePath, "rb");

		if (fp == nullptr)
		{
			return false;
		}

		size_t count = 0;

		count = fread(&riffChunkId[0], sizeof(riffChunkId), 1, fp);
		if (count != 1) goto exit;

		count = fread(&riffChunkSize, sizeof(riffChunkSize), 1, fp);
		if (count != 1) goto exit;

		count = fread(&riffFormat[0], sizeof(riffFormat), 1, fp);
		if (count != 1) goto exit;

		count = fread(&fmtSubchunk1Id[0], sizeof(fmtSubchunk1Id), 1, fp);
		if (count != 1) goto exit;

		count = fread(&fmtSubchunk1Size, sizeof(fmtSubchunk1Size), 1, fp);
		if (count != 1) goto exit;

		count = fread(&fmtAudioFormat, sizeof(fmtAudioFormat), 1, fp);
		if (count != 1) goto exit;

		count = fread(&fmtNumChannels, sizeof(fmtNumChannels), 1, fp);
		if (count != 1) goto exit;

		count = fread(&fmtSampleRate, sizeof(fmtSampleRate), 1, fp);
		if (count != 1) goto exit;

		count = fread(&fmtByteRate, sizeof(fmtByteRate), 1, fp);
		if (count != 1) goto exit;

		count = fread(&fmtBlockAlign, sizeof(fmtBlockAlign), 1, fp);
		if (count != 1) goto exit;

		count = fread(&fmtBitsPerSample, sizeof(fmtBitsPerSample), 1, fp);
		if (count != 1) goto exit;

		do
		{
			count = fread(&id[0], sizeof(id), 1, fp);
			if (count != 1) goto exit;

			count = fread(&size, sizeof(size), 1, fp);
			if (count != 1) goto exit;

			if (id[0] == 'd' && id[1] == 'a' && id[2] == 't' && id[3] == 'a')
			{
				int n = size;
				data = new uint8_t[n];

				count = fread(data, sizeof(uint8_t), n, fp);
				if (count != size) goto exit;
			}
			else
			{
				int ret = fseek(fp, size, SEEK_CUR);
				if (ret != 0) return false;
			}

		} while (data == nullptr);

	exit:
		fclose(fp);

		if (data != nullptr)
		{
			framesCount = size / fmtBlockAlign;
			return true;
		}
		
		return false;
	}

	bool CalculateAmplitude(size_t timeMilliseconds, size_t widthMilliseconds, int& leftResult, int& rightResult)
	{
		size_t offset = (timeMilliseconds * fmtSampleRate) / 1000;

		if (offset >= framesCount)
		{
			leftResult = 0;
			rightResult = 0;

			return false;
		}

		size_t width = (widthMilliseconds * fmtSampleRate) / 1000;
		size_t end = (offset + width);

		if (end >= framesCount)
		{
			end = framesCount;
		}

		int16_t* const ptBegin = reinterpret_cast<int16_t*>(data);
		int16_t* const ptStart = ptBegin + (2 * offset);
		int16_t* const ptEnd = ptBegin + (2 * end);

		int lMax = 0;
		int rMax = 0;

		for (int16_t* p = ptStart; p < ptEnd;)
		{
			int16_t left = *p;
			++p;

			int16_t right = *p;
			++p;

			lMax = std::max<int>(lMax, std::abs(left));
			rMax = std::max<int>(rMax, std::abs(right));
		}

		leftResult = lMax;
		rightResult = rMax;

		return true;
	}

	~Wav()
	{
		if (data != nullptr)
		{
			if (data != nullptr) delete[] data;
		}
	}
};

