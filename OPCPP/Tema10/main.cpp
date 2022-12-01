#include <fstream>

struct WavFile
{
	WavFile(WavFile&) = delete;

	WavFile() : Data(nullptr)
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

	bool LoadWavFile(const char* filePath)
	{
		FILE* fp;

		errno_t err = fopen_s(&fp, filePath, "rb");

		if (fp == nullptr)
		{
			return false;
		}

		ReadId(RiffChunkId, fp);
		ReadValue(&RiffChunkSize, fp);
		ReadId(RiffFormat, fp);

		ReadId(FmtSubchunk1Id, fp);
		ReadValue(&FmtSubchunk1Size, fp);
		ReadValue(&FmtAudioFormat, fp);
		ReadValue(&FmtNumChannels, fp);
		ReadValue(&FmtSampleRate, fp);
		ReadValue(&FmtByteRate, fp);
		ReadValue(&FmtBlockAlign, fp);
		ReadValue(&FmtBitsPerSample, fp);

		do
		{
			ReadId(Id, fp);
			ReadValue(&Size, fp);

			if (Id[0] == 'd' && Id[1] == 'a' && Id[2] == 't' && Id[3] == 'a')
			{
				Data = new uint8_t[Size];
				ReadData(Data, Size, fp);
			}
			else
			{
				fseek(fp, Size, SEEK_CUR);
			}

		} while (Data == nullptr);

		if (fp != nullptr)
		{
			fclose(fp);
		}

		return true;
	}

	~WavFile()
	{
		if (Data != nullptr)
		{
			if (Data != nullptr) delete[] Data;
		}
	}

private:
	void ReadId(uint8_t* p, FILE* fp)
	{
		fread(p, sizeof(uint8_t), 4, fp);

	}

	void ReadValue(uint32_t* p, FILE* fp)
	{
		fread(p, sizeof(uint32_t), 1, fp);
	}

	void ReadValue(uint16_t* p, FILE* fp)
	{
		fread(p, sizeof(uint16_t), 1, fp);
	}

	void ReadData(uint8_t* p, int count, FILE* fp)
	{
		fread(p, sizeof(uint8_t), count, fp);
	}
};

int main()
{
	// Doporuceny soubor pro testovani:
	// https://freewavesamples.com/ensoniq-zr-76-01-dope-77-bpm

	WavFile wav;
	bool ok = wav.LoadWavFile("C:\\Users\\erik\\Downloads\\file.wav");

	if (!ok)
	{
		printf("Nepovedlo se nacist soubor.\n");
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

	int frames = wav.Size / (wav.FmtNumChannels * wav.FmtBlockAlign);
	
	int16_t* p2 = (int16_t*)wav.Data;

	for (int i = 0; i < frames; i++)
	{
		int16_t leftSample = *p2;
		double d = (double)leftSample / (INT16_MAX + 1);
		
		newPosition = (int)((d + 1) * n / 2);

		printf("%10d ", i);

		retezec[oldPosition] = ' ';
		retezec[newPosition] = 'x';

		puts(retezec);

		oldPosition = newPosition;

		p2 += 2;
	}

	int znak = getchar();
}
