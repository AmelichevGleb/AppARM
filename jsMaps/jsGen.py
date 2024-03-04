import webbrowser
import socket

#Функции
def printRez():
    fr = open('E:/openstreetmap/AppARM/jsMaps/draw.js', 'r')
    # print(*fr)
    fr.close()
# Получить местоположение всех точек
target_host = "127.0.0.1"
target_port = 6666
client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
client.connect((target_host, target_port))
# Отправка данных
mess = "33;"
client.send(mess.encode())
# Получение ответа
response = client.recv(16384).decode().split('\n')
print(response)

#Запись в файл
f = open('E:/openstreetmap/AppARM/jsMaps/draw.js', 'w+')
for i in range(0, len(response)-1):
    resSplited = response[i].split('; ')
    print(resSplited) 
    f.write('m'+str(i)+' = new marker('+resSplited[4]+', '+resSplited[5]+', "blue", "<dl><dt><b>Name: </b>'+resSplited[3]+'</dt><dt> <b>X: </b>'+resSplited[4]+'</dt><dt><b>Y: </b>'+resSplited[5]+'</dt><dt><b>Temperature: </b>'+'18'+'</dt><dt><b>Wind Speed: </b>'+'5'+'</dt><dt><b>Direction: </b>'+'30'+'</dt></dl>");\n')
    f.write('c'+str(i)+' = new circle(['+resSplited[4]+', '+resSplited[5]+'], 800, "#D6DF71");\n')
#f.write('addCircle(55.738172, 37.185, 500);\n')
#f.write('c = new circle([55.738172, 37.185], 400);\n')
f.close()


client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
client.connect((target_host, target_port))
# Отправка данных
mess = "127.0.0.1;2222;"
client.send(mess.encode())
# Получение ответа
response = client.recv(16384).decode()
print(response)



#printRez()
url = "file://E:/openstreetmap/AppARM/jsMaps/index.html"
webbrowser.open(url,new=2)
