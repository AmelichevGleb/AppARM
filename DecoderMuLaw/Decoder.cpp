#include <cstdint>
#define DecoderMulaw  _declspec(dllexport)
#include <iostream>

using namespace std;


extern "C"
{
	DecoderMulaw void Test1(int _y)
	{
		cout << 11 << endl;
	}
	
	DecoderMulaw int16_t MuLaw_Decode(int8_t number)
	{
		const uint16_t MULAW_BIAS = 33;
		uint8_t sign = 0, position = 0;
		int16_t decoded = 0;
		number = ~number;
		if (number & 0x80)
		{
			number &= ~(1 << 7);
			sign = -1;
		}
		position = ((number & 0xF0) >> 4) + 5;
		decoded = ((1 << position) | ((number & 0x0F) << (position - 4))
			| (1 << (position - 5))) - MULAW_BIAS;
		return (sign == 0) ? (decoded) : (-(decoded));
	}


}