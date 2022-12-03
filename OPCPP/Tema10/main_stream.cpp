// verze s C++

#include <fstream>
#include <array>

struct Wav
{
	Wav(Wav&) = delete;

	Wav() : Data(nullptr)
	{
	}

	std::array<uint8_t, 4> RiffChunkId{};
	uint32_t RiffChunkSize{};
	std::array<uint8_t, 4> RiffFormat{};

	std::array<uint8_t, 4> FmtSubchunk1Id{};
	uint32_t FmtSubchunk1Size{};
	uint16_t FmtAudioFormat{};
	uint16_t FmtNumChannels{};
	uint32_t FmtSampleRate{};
	uint32_t FmtByteRate{};
	uint16_t FmtBlockAlign{};
	uint16_t FmtBitsPerSample{};

	std::array<uint8_t,4> Id{};
	uint32_t Size{};
	uint8_t* Data;

	bool Load(const char* filePath)
	{
		try
		{
			std::basic_ifstream<uint8_t> input;
			input.exceptions(std::ios::failbit);

			input.open(filePath, std::ios::in | std::ios::binary);

			input.read(RiffChunkId.data(), RiffChunkId.size());
			if (RiffChunkId != std::array<uint8_t, 4> {'R', 'I', 'F', 'F'}) return false;

			input.read(reinterpret_cast<uint8_t*>(&RiffChunkSize), sizeof(RiffChunkSize));

			input.read(RiffFormat.data(), RiffFormat.size());
			if (RiffFormat != std::array<uint8_t, 4> {'W', 'A', 'V', 'E'}) return false;

			input.read(FmtSubchunk1Id.data(), FmtSubchunk1Id.size());
			if (FmtSubchunk1Id != std::array<uint8_t, 4> {'f', 'm', 't', ' '}) return false;

			input.read(reinterpret_cast<uint8_t*>(&FmtSubchunk1Size), sizeof(FmtSubchunk1Size));
			input.read(reinterpret_cast<uint8_t*>(&FmtAudioFormat), sizeof(FmtAudioFormat));
			input.read(reinterpret_cast<uint8_t*>(&FmtNumChannels), sizeof(FmtNumChannels));
			input.read(reinterpret_cast<uint8_t*>(&FmtSampleRate), sizeof(FmtSampleRate));
			input.read(reinterpret_cast<uint8_t*>(&FmtByteRate), sizeof(FmtByteRate));
			input.read(reinterpret_cast<uint8_t*>(&FmtBlockAlign), sizeof(FmtBlockAlign));
			input.read(reinterpret_cast<uint8_t*>(&FmtBitsPerSample), sizeof(FmtBitsPerSample));

			do
			{
				input.read(Id.data(), Id.size());
				input.read(reinterpret_cast<uint8_t*>(&Size), sizeof(Size));

				if (Id == std::array<uint8_t, 4> {'d', 'a', 't', 'a'})
				{
					Data = new uint8_t[Size];
					input.read(reinterpret_cast<uint8_t*>(Data), Size);
				}
				else
				{
					input.seekg(Size, std::ios_base::cur);
				}

			} while (Data == nullptr);
		}
		catch (const std::ios_base::failure& e)
		{
			return false;
		}
		
		return Data == nullptr ? false : true;
	}

	~Wav()
	{
		if (Data != nullptr)
		{
			delete[] Data;
		}
	}
};

int main()
{
	// Doporuceny soubor pro testovani:
	// https://freewavesamples.com/ensoniq-zr-76-01-dope-77-bpm

	Wav wav;
	if (!wav.Load("C:\\Users\\erik\\Downloads\\file.wav"))
	{
		printf("Chyba pri cteni souboru.\n");
		return -1;
	}

	int16_t* p2 = (int16_t*)wav.Data;

	for (int i = 0; i < 10000; i++)
	{
		int16_t x = *p2;
		double d = (double)x / (INT16_MAX + 1);
		int y = (d + 1) * 40;

		printf("%10d ", i);

		for (size_t i = 0; i < y; i++)
		{
			putchar(' ');
		}

		putchar('x');
		putchar('\n');

		p2 += 2;
	}

	auto znak = getchar();
}
