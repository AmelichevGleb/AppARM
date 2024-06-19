#HTTP Сервер отрисовывает объекты / делает запросы серверу / запрашивает погоду

import configparser
import socket
import http.server
import socketserver
from files import script, index
import fileinput
import json
from io import BytesIO
import time

#Получение информации из файла настроек
config = configparser.ConfigParser()
config.read('settings.conf')


class MyHttpRequestHandler(http.server.SimpleHTTPRequestHandler):
    #Обработка Get_запроса
    def do_GET(self):
        if self.path == '/':
            self.path = 'index.html'
        return http.server.SimpleHTTPRequestHandler.do_GET(self)

    #Обработка Post_запроса
    def do_POST(self):
        content_lenght = int(self.headers['Content-Length'])
        body = self.rfile.read(content_lenght)
        self.send_response(200)
        self.end_headers()
        response = BytesIO()
        response.write(b'This is POST request.\n')
        response.write(b'Received as a test data: ')
        response.write(body)
        response.write(b'\nPredicted by nn model:')
        print(response.getvalue())
        data = json.loads(body)
        print(data)
        typeSignal = data["Type"]
        ipSignal = data["IP"]
        print(type(typeSignal))
        functionCircle(typeSignal,ipSignal)

    #Функция поиска и замены текста
    def search_and_replace(file_path, search_text, replace_text):
        with fileinput.FileInput(file_path, inplace=True, backup='.bak') as file:
            for line in file:
                print(line.replace(search_text, replace_text), end='')

    try:
        search_and_replace('file.txt', 'old_line', 'new_line')
    except Exception as e:
        print(f"При работе произошла ошибка: {e}")


def ReplaceLine(old_line, new_line):
    backup_extension = '.bak'
    for line in fileinput.input("draw.js", inplace=1, backup=backup_extension):
        print(line.replace(old_line, new_line), end='')


    #Функция изменяет цвет области на красный или зеленый;
    #в зависимости от команды
def functionCircle(typeSignal, typeLocal):
    if typeSignal == "1":
        print('Тут 1')
        f = open('draw.js', "r")
        content = f.readlines()
        f.close()
        for contents in content:
            index = content.index(contents)
            temp = str(contents)
            if (temp.find(typeLocal) != -1):
                if (temp.find("blue") != -1):
                    temp1 = temp.replace("blue", "red")
                    print(temp1)
                    content[index] = temp1
                    ReplaceLine(temp, temp1)
                if (temp.find("#D6DF71") != -1):
                    temp1 = temp.replace("#D6DF71", "#F00000")
                    print(temp1)
                    content[index] = temp1
                    ReplaceLine(temp, temp1)
        print(type(content))
        print(content)

    if typeSignal == "2":
        print('Тут 2')
        f = open('draw.js', "r")
        content = f.readlines()
        f.close()
        for contents in content:
            index = content.index(contents)
            temp = str(contents)
            if (temp.find(typeLocal) != -1):
                if (temp.find("red") != -1):
                    temp1 = temp.replace("red", "blue")
                    print(temp1)
                    content[index] = temp1
                    ReplaceLine(temp, temp1)
                if (temp.find("#F00000") != -1):
                    temp1 = temp.replace("#F00000", "#D6DF71")
                    print(temp1)
                    content[index] = temp1
                    ReplaceLine(temp, temp1)
        print(type(content))
        print(content)


def start_server():
    handler_object = MyHttpRequestHandler
    PORT = int(config['ADDRESSES']['port'])
    my_server = socketserver.TCPServer(("", PORT), handler_object)
    my_server.serve_forever()
