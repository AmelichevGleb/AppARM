using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppARM.WeatherSokol
{
    internal class ByteRequestMeteo
    {
        private byte[] bytesBuffer = new byte[6]; // буферный массив байт 
        private byte[] CRC = new byte[1]; //массив для контрольной суммы 
        private byte[] EndBytesMassive = new byte[8]; //итоговый массив байт для создания запроса метеостанции 

        public byte adress; // адрес
        public byte codeCommand; // код команды
        public byte registerNumber_1; // номер регистра, начиная с которого запрашивается количество регистров
        public byte registerNumber_2; // ??? номер регистра, начиная с которого запрашивается количество регистров
        public byte numberOfRequested_1; //количество запрашиваемых регистров  
        public byte numberOfRequested_2; //количество запрашиваемых регистров (5А – 90 регистров; B4 - 180 регистров) 
        public byte CRC_16_Modbus_1; // CRC-16 Modbus с обратным порядком байтов
        public byte CRC_16_Modbus_2; // CRC-16 Modbus с обратным порядком байтов

        public ByteRequestMeteo(byte _adress, byte _codeCommand, byte _registerNumber_1, byte _registerNumber_2, byte _numberOfRequested_1, byte _numberOfRequested_2)
        {
            bytesBuffer[0] = _adress;
            bytesBuffer[1] = _codeCommand;
            bytesBuffer[2] = _registerNumber_1;
            bytesBuffer[3] = _registerNumber_2;
            bytesBuffer[4] = _numberOfRequested_1;
            bytesBuffer[5] = _numberOfRequested_2;
            CRC = ModbusCRC16Calc(bytesBuffer);
            bytesBuffer.CopyTo(EndBytesMassive, 0);
            EndBytesMassive[6] = CRC[0];
            EndBytesMassive[7] = CRC[1];
        }

        //Алгоритм ModbusCRC16Calc
        public static byte[] ModbusCRC16Calc(byte[] Message)
        {
            //выдаваемый массив CRC
            byte[] CRC = new byte[2];
            ushort Register = 0xFFFF; // создаем регистр, в котором будем сохранять высчитанный CRC
            ushort Polynom = 0xA001; //Указываем полином, он может быть как 0xA001(старший бит справа), так и его реверс 0x8005(старший бит слева, здесь не рассматривается), при сдвиге вправо используется 0xA001

            for (int i = 0; i < Message.Length; i++) // для каждого байта в принятом\отправляемом сообщении проводим следующие операции(байты сообщения без принятого CRC)
            {
                Register = (ushort)(Register ^ Message[i]); // Делим через XOR регистр на выбранный байт сообщения(от младшего к старшему)

                for (int j = 0; j < 8; j++) // для каждого бита в выбранном байте делим полученный регистр на полином
                {
                    if ((ushort)(Register & 0x01) == 1) //если старший бит равен 1 то
                    {
                        Register = (ushort)(Register >> 1); //сдвигаем на один бит вправо
                        Register = (ushort)(Register ^ Polynom); //делим регистр на полином по XOR
                    }
                    else //если старший бит равен 0 то
                    {
                        Register = (ushort)(Register >> 1); // сдвигаем регистр вправо
                    }
                }
            }
            CRC[1] = (byte)(Register >> 8); // присваеваем старший байт полученного регистра младшему байту результата CRC (CRClow)
            CRC[0] = (byte)(Register & 0x00FF); // присваеваем младший байт полученного регистра старшему байту результата CRC (CRCHi) это условность Modbus — обмен байтов местами.

            return CRC;
        }

        //Тестовая работа с байтами
        public void ShowTest()
        {
            byte[] bt = new byte[5] { 0x01, 0x02, 0x03, 0x04, 0x05 };
            byte[] bt2 = new byte[8];
            bt.CopyTo(bt2, 0);
            Console.WriteLine("ТЕСТ BT 2 Array Output, Size: {0} Data: " + BitConverter.ToString(bt2), bt2.Length);
            bt2[6] = 0x02;
            Console.WriteLine("ТЕСТ BT 2 Array Output, Size: {0} Data: " + BitConverter.ToString(bt2), bt2.Length);

            Console.WriteLine("ТЕСТ Array Output, Size: {0} Data: " + BitConverter.ToString(CRC), CRC.Length);

            Console.WriteLine("РЕЗУЛЬТАТ !!!!   Array Output, Size: {0} Data: " + BitConverter.ToString(EndBytesMassive), EndBytesMassive.Length);
            Console.WriteLine(EndBytesMassive);
        }
    }
}
