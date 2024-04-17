import webbrowser
import socket
import http.server
import socketserver
from threading import Thread
import os
import configparser
from files import script, index

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
# Инициализация сервера
class MyHttpRequestHandler(http.server.SimpleHTTPRequestHandler):
    def do_GET(self):
        if self.path == '/':
            self.path = 'index.html'
        return http.server.SimpleHTTPRequestHandler.do_GET(self)
def start_server():
    handler_object = MyHttpRequestHandler
    PORT = int(config['ADDRESSES']['port'])
    my_server = socketserver.TCPServer(("", PORT), handler_object)
    my_server.serve_forever()
#Функции
def printRez():
    fr = open('draw.js', 'r')
    print(*fr)
    fr.close()
# Отправка запроса
#;33 - получение всех точек
#ip;2222; - получение данных с метеостанции
def tileServerStart():
    os.system('tileserver-gl --file '+config['FILES']['mapName'])
def request(mess, target_host = "127.0.0.1", target_port = 6666):
    client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    client.connect((target_host, target_port))
    client.send(mess.encode())
    response = client.recv(16384).decode().split('\n')
    return response
def draw(response, path = 'draw.js'):
    #Запись в файл
    f = open(path, 'w+')
    
    for i in range(0, len(response)-1):
        resSplited = response[i].split(';')
        print(resSplited) 
        f.write('m'+str(i)+' = new marker('+resSplited[4]+', '+resSplited[5]+', "blue", "<dl><dt><b>Name: </b>'+resSplited[3]+'</dt><dt> <b>X: </b>'+resSplited[4]+'</dt><dt><b>Y: </b>'+resSplited[5]+'</dt><dt><b>Temperature: </b>'+'18'+'</dt><dt><b>Wind Speed: </b>'+'5'+'</dt><dt><b>Direction: </b>'+'30'+'</dt></dl>");\n')
        f.write('c'+str(i)+' = new circle(['+resSplited[4]+', '+resSplited[5]+'], 800, "#D6DF71");\n')
    f.close()

#Тело
try:
    draw(request("33;"))
    response2 = request(config['ADDRESSES']['requestAddress']+";2222;")
    print(response2)
except ConnectionRefusedError:
    print("Не удалось считать информацию с сервера")
#Запуск сервера
serverThread = Thread(target=start_server, args=())
serverThread.start()
tileThread = Thread(target=tileServerStart, args=())
tileThread.start()
print('Сервер запущен')
#printRez()
url = 'http://'+config['ADDRESSES']['host']+':'+config['ADDRESSES']['port']+'/'
webbrowser.open(url,new=2)
serverThread.join()
tileThread.join()
