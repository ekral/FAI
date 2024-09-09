#include "wav.h"

int main()
{
    Wav wav; 
    bool ok = wav.LoadWavFile("file1.wav"); // pouze 16 bit wav

    int left = 0;
    int right = 0;

    wav.CalculateAmplitude(1000, 2, left, right); // max amplitude v case 1000 ms z 2 ms dlouhe casti
}
