#include <fstream>

struct Wav
{
	Wav(Wav&) = delete;

	Wav() : Data(nullptr)
	{
	}

	uint8_t RiffChunkId[4]{};
	uint32_t RiffChunkSize{};
	uint8_t RiffFormat[4]{};

	uint8_t FmtSubchunk1Id[4]{};
	uint32_t FmtSubchunk1Size{};
	uint16_t FmtAudioFormat{};
	uint16_t FmtNumChannels{};
	uint32_t FmtSampleRate{};
	uint32_t FmtByteRate{};
	uint16_t FmtBlockAlign{};
	uint16_t FmtBitsPerSample{};

	uint8_t Id[4]{};
	uint32_t Size{};
	uint8_t* Data;

	uint32_t FramesCount()
	{
		return Size / (FmtNumChannels * FmtBlockAlign);
	}

	bool LoadWavFile(const char* filePath)
	{
		FILE* fp;

		errno_t err = fopen_s(&fp, filePath, "rb");

		if (fp == nullptr)
		{
			return false;
		}

		size_t count = 0;

		count = fread(&RiffChunkId[0], sizeof(RiffChunkId), 1, fp);
		if (count != 1) goto exit;
		
		count = fread(&RiffChunkSize, sizeof(RiffChunkSize), 1, fp);
		if (count != 1) goto exit;

		count = fread(&RiffFormat[0], sizeof(RiffFormat), 1, fp);
		if (count != 1) goto exit;

		count = fread(&FmtSubchunk1Id[0], sizeof(FmtSubchunk1Id), 1, fp);
		if (count != 1) goto exit;

		count = fread(&FmtSubchunk1Size, sizeof(FmtSubchunk1Size), 1, fp);
		if (count != 1) goto exit;

		count = fread(&FmtAudioFormat, sizeof(FmtAudioFormat), 1, fp);
		if (count != 1) goto exit;

		count = fread(&FmtNumChannels, sizeof(FmtNumChannels), 1, fp);
		if (count != 1) goto exit;

		count = fread(&FmtSampleRate, sizeof(FmtSampleRate), 1, fp);
		if (count != 1) goto exit;

		count = fread(&FmtByteRate, sizeof(FmtByteRate), 1, fp);
		if (count != 1) goto exit;

		count = fread(&FmtBlockAlign, sizeof(FmtBlockAlign), 1, fp);
		if (count != 1) goto exit;

		count = fread(&FmtBitsPerSample, sizeof(FmtBitsPerSample), 1, fp);
		if (count != 1) goto exit;

		do
		{
			count = fread(&Id[0], sizeof(Id), 1, fp);
			if (count != 1) goto exit;

			count = fread(&Size, sizeof(Size), 1, fp);
			if (count != 1) goto exit;

			if (Id[0] == 'd' && Id[1] == 'a' && Id[2] == 't' && Id[3] == 'a')
			{
				int n = Size;
				Data = new uint8_t[n];

				count = fread(Data, sizeof(uint8_t), n, fp);
				if (count != Size) goto exit;
			}
			else
			{
				int count = fseek(fp, Size, SEEK_CUR);
				if (count != Size) return false;
			}

		} while (Data == nullptr);

	exit:
		fclose(fp);
		return Data == nullptr ? false : true;
	}

	~Wav()
	{
		if (Data != nullptr)
		{
			if (Data != nullptr) delete[] Data;
		}
	}
};

struct Frame
{
	int16_t leftSample;
	int16_t rightSample;
};

int main()
{
	// Doporuceny soubor pro testovani:
	// https://freewavesamples.com/ensoniq-zr-76-01-dope-77-bpm

	Wav wav;
	bool ok = wav.LoadWavFile("C:\\Users\\erik\\Downloads\\file.wav");

	if (!ok)
	{
		printf("Nepovedlo se nacist soubor.\n");
		return -1;
	}

	if (wav.FmtAudioFormat != 1 && wav.FmtNumChannels != 2 && wav.FmtBitsPerSample != 16)
	{
		printf("Nespravny format wav souboru..\n");
		return -1;
	}

	// kvuli rychlejsimu vypisu pouzivam uz cely retezec
	constexpr int n = 80;
	char retezec[n + 1]{};

	for (size_t i = 0; i < n; i++)
	{
		retezec[i] = ' ';
	}

	retezec[n] = 0;

	int oldPosition = 0;
	int newPosition = 0;

	Frame* p = (Frame*)wav.Data;

	for (size_t i = 0; i < wav.FramesCount(); i++)
	{
		Frame frame = *p;
		double d = (double)frame.leftSample / (INT16_MAX + 1);

		newPosition = (int)((d + 1) * n / 2);

		printf("%10lld ", i);

		retezec[oldPosition] = ' ';
		retezec[newPosition] = 'x';

		puts(retezec);

		oldPosition = newPosition;

		++p;
	}

	int znak = getchar();
}
