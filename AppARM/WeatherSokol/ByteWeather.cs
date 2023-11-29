using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using AppARM.WeatherSokol;

namespace AppARM.WeatherSokol
{
    public class ByteWeather
    {
        public string ip_adress;
        public string port;
        public string adress;
        public string command;
        public string registerNumber;
        public string firmware_1;//1 – прошивка (82-1.3.0) и тип устройства+номер банка регистров+наличие ошибки в работе сенсоров(подробнее в 90-м регистре)
        public string firmware_2;//2
        public string orderUnixTime_1;//3 - старшие 16 разрядов UNIX TIME
        public string orderUnixTime_2;
        public string juniorUnixTime_1;//4 - младшие 16 разрядов UNIX TIME
        public string juniorUnixTime_2;
        public double temperature; // 5 температура беззнаковое; разрешение 0,01 град, т.е. 09A1 – 2465 или 24.65°С
        public int pressure; //6 - атмосферное давление беззнаковое; разрешение 10Ра; 271D – 10013 т.е. 100130 Па
        public int relativeРumidity;//7 - относительная влажность беззнаковое; разрешение 1%; 19 – 25%
        public double windSpeed; //8 - скорость ветра беззнаковое; разрешение 0.01 m/s
        public int directionWind;//9 - направление ветра беззнаковое; разрешение 1 град; 101 – 257°
        public double precipitationLevel;//10 - Уровень осадков беззнаковое; разрешение 0.1 мм; 2.9
        public double UVlevel; //11 - Уровень ультрафиолетового излучения беззнаковое; разрешение 0.01 W/m2
        public int lightLevel;//12 - Уровень освещенности беззнаковое; разрешение 1 lux; 92 - 146
        public double ultrasonicAnemometerWindspeed;//13 - скорость ветра УЗ анемометра беззнаковое; разрешение 0.01 m/s
        public int ultrasonicAnemometerWindDirection;//14 - направление ветра УЗ анемометра беззнаковое; разрешение 1 град


        CreateJsonRequest createJsonRequest = new CreateJsonRequest();
       
        
        /*                  
        ЕСТЬ ЕЩЕ ПАРАМЕТРЫ
        byte[] obstacleDistance = new byte[2];//14  - Расстояние до препятствия (снежный покров) беззнаковое; разрешение 1 см. ДГВ должен иметь сетевой адрес 160
        byte[] solarRadiation = new byte[2];//15  - Солнечная радиация беззнаковое; разрешение 1 Вт/м2. Пирогелиометр должен иметь сетевой адрес 181
        */

        //перевод из беззнакового формата в знаковый
        public double NumberSign(string _number)
        {

            var t = Convert.ToInt32(_number, 16);
            if (t <= 32767)
            {
                return Convert.ToInt32(_number, 16) * 0.01;
            }
            else
            {
                var result = 65535 - Convert.ToInt32(_number, 16);
                return result * 0.01 * -1;
            }
        }

        // ДОРАБОТАТЬ ПО НЕОБХОДИМОСТИ ! 
        public string HorizonPoints(string _ip, int _directionWind)
        {
            // ДОРАБОТАТЬ ПО НЕОБХОДИМОСТИ ! 
            string horizon = null;

            if ((_directionWind == 0) || (_directionWind == 360)) { horizon = "N"; }

            if (_directionWind == 45) { horizon = "NE"; }

            if (_directionWind == 90) { horizon = "E"; }

            if (_directionWind == 135) { horizon = "SE"; }

            if (_directionWind == 180) { horizon = "S"; }

            if (_directionWind == 225) { horizon = "SW"; }

            if (_directionWind == 270) { horizon = "W"; }

            if (_directionWind == 270) { horizon = "W"; }

            if (_directionWind == 315) { horizon = "NW"; }

            return horizon;


        }

        public ByteWeather(string _ip, string _ipSend, int _portSend, string _location, string _longitude,string _lagatitude, byte _adress, byte _command, byte _registerNumber, byte _firmware_1, byte _firemware_2, byte _orderUnixTime_1, byte _orderUnixTime_2,
            byte _juniorUnixTime_1, byte _juniorUnixTime_2, byte _temperature_1, byte _temperature_2, byte _pressure_1, byte _pressure_2, byte _relativePumidity_1, byte _relativePumidity_2,
            byte _windSpeed_1, byte _windSpeed_2, byte _directionWind_1, byte _directionWind_2, byte _precipitationLevel_1, byte _precipitationLevel_2, byte UVlevel_1, byte UVlevel_2, byte lightLevel_1,
            byte lightLevel_2, byte ultrasonicAnemometerWindspeed_1, byte ultrasonicAnemometerWindspeed_2, byte ultrasonicAnemometerWindDirection_1, byte ultrasonicAnemometerWindDirection_2, bool flag)
        {
            this.ip_adress = _ip;
            this.port = Convert.ToString(_portSend);
            this.adress = Convert.ToString(_adress, 16);
            this.command = Convert.ToString(_command, 16);
            this.registerNumber = Convert.ToString(_registerNumber, 16);
            this.firmware_1 = Convert.ToString(_firmware_1, 16);
            this.firmware_2 = Convert.ToString(_firemware_2, 16);
            this.orderUnixTime_1 = Convert.ToString(_orderUnixTime_1, 16);
            this.orderUnixTime_2 = Convert.ToString(_orderUnixTime_2, 16);
            this.juniorUnixTime_1 = Convert.ToString(_juniorUnixTime_1, 16);
            this.juniorUnixTime_2 = Convert.ToString(_juniorUnixTime_2, 16);
            Console.WriteLine(Convert.ToString(_temperature_1) + "   " + Convert.ToString(_temperature_2));
            string temp = Convert.ToString(_temperature_1, 16) + Convert.ToString(_temperature_2, 16);
            this.temperature = NumberSign(temp);
            temp = (Convert.ToString(_pressure_1, 16) + Convert.ToString(_pressure_2, 16));
            this.pressure = Convert.ToInt32(temp, 16) * 10;
            temp = Convert.ToString(_relativePumidity_1, 16) + Convert.ToString(_relativePumidity_2, 16);
            this.relativeРumidity = Convert.ToInt32(temp, 16);
            this.windSpeed = Convert.ToDouble(Convert.ToString(_windSpeed_1, 16) + "," + Convert.ToString(_windSpeed_2, 16)) * 0.01;
            temp = Convert.ToString(_directionWind_1, 16) + Convert.ToString(_directionWind_2, 16);
            this.directionWind = Convert.ToInt32(temp, 16);
            temp = Convert.ToString(_precipitationLevel_1, 16) + Convert.ToString(_precipitationLevel_2, 16);
            this.precipitationLevel = Convert.ToInt32(temp, 16) * 0.1;
            temp = Convert.ToString(UVlevel_1, 16) + Convert.ToString(UVlevel_2, 16);
            this.UVlevel = Convert.ToInt32(temp, 16) * 0.01;
            temp = Convert.ToString(lightLevel_1, 16) + Convert.ToString(lightLevel_2, 16);
            this.lightLevel = Convert.ToInt32(temp, 16);
            temp = Convert.ToString(ultrasonicAnemometerWindspeed_1, 16) + Convert.ToString(ultrasonicAnemometerWindspeed_2, 16);
            this.ultrasonicAnemometerWindspeed = Convert.ToInt32(temp, 16) * 0.01;
            temp = Convert.ToString(ultrasonicAnemometerWindDirection_1, 16) + Convert.ToString(ultrasonicAnemometerWindDirection_2, 16);
            this.ultrasonicAnemometerWindDirection = Convert.ToInt32(temp, 16);
            Console.WriteLine("adr = {0} cm = {1} , regN = {2} , F1 {3} F2 {4} UNtime1 {5} UnTime2 {6} Температура {7} Давлениие {8} Па Влажность {9}", adress, command, registerNumber, firmware_1, firmware_2, orderUnixTime_1, orderUnixTime_2, temperature, pressure, relativeРumidity);
            Console.WriteLine("Направление ветра {0} , Уровень осадков {1} , Утрафиоле {2} ,уровень света {3}, скорость ветра {4} , направление ветра {5} ", directionWind, precipitationLevel, UVlevel, lightLevel, UVlevel, ultrasonicAnemometerWindDirection);
            if (flag == true)
            {
                createJsonRequest.CreatJSON(_ip, _location, _longitude, _lagatitude, Convert.ToString(temperature), Convert.ToString(windSpeed), Convert.ToString(directionWind), "null");
            }
        }

        public Tuple<string, string, string, string, string> ReturnPartWeater() 
        {
            return Tuple.Create(ip_adress, port, Convert.ToString(temperature), Convert.ToString(windSpeed), Convert.ToString(directionWind));
        }
    }
}
