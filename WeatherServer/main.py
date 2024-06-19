# This is a sample Python script.

# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.

import ClassRequestData
import ServerTileMap
import UpdateWeathers
import configparser
import webbrowser
from files import script, index
from threading import Thread


# Чтение конфига
config = configparser.ConfigParser()
config.read('settings.conf')
# Файлы
fileGen = open('script.js', 'w')
fileGen.write(script(config['ADDRESSES']['host'], config['ADDRESSES']['tileport']))
fileGen.close()
fileGen = open('index.html', 'w')
fileGen.write(index)
fileGen.close()

# Функции
def printRez():
    fr = open('draw.js', 'r')
    print(*fr)
    fr.close()


updateThread = Thread(target=UpdateWeathers.updateWeather,args=())
updateInfoThread = Thread(target=UpdateWeathers.updateInfo, args=()) #Получение данных пи первом запуске
tileThread = Thread(target=ServerTileMap.tileServerStart, args=())
serverThread = Thread(target=ClassRequestData.start_server, args=())
updateWeather = Thread(target=UpdateWeathers.updateWeather,args=())

updateThread.start()
#updateInfoThread.start()
#serverThread.start()
#tileThread.start()
#updateWeather.start()

print('Сервер запущен')
printRez()

#Запуск браузера
url = 'http://' + config['ADDRESSES']['host'] + ':' + config['ADDRESSES']['port'] + '/#55.74605,37.60895,11z'
webbrowser.open(url, new=2)

updateThread.join()
#updateInfoThread.join()
#serverThread.join()
#tileThread.join()
#updateWeather.join()


