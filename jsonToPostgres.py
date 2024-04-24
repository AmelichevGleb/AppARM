import psycopg2
from psycopg2 import Error
import json
#Вывод json'a
with open('1.json') as json_file:
    data = json.load(json_file)
    print('device_ip '+data['device_ip'])
    print('Name '+data['Name'])
    print('Longitude '+data['Longitude'])
    print('Lagatitude '+data['Lagatitude'])
    print('Temperature '+data['weather']['Temperature'])
    print('Wind_Speed '+data['weather']['Wind_Speed'])
    print('Direction '+data['weather']['Direction'])
    if (data['weather']['Parameter']):
        print('Parameter '+data['weather']['Parameter'])
    else:
        print('Parameter '+'None')
#Запись в БД
try:
    connection = psycopg2.connect(user='postgres', password='11223344', host='localhost', port="5432", database="DB")
    cursor = connection.cursor()
    if (data['weather']['Parameter']):
        param = data['weather']['Parameter']
    else:
        param = ''
    cursor.execute("DELETE FROM public.device")
    cursor.execute("INSERT INTO public.device VALUES ('"+data['device_ip']+"', '"+data['Name']+"', "+data['Longitude']+", "+data['Lagatitude']+", "+data['weather']['Wind_Speed']+", "+data['weather']['Direction']+", '"+param+"')")
    connection.commit()
except (Exception, Error) as error:
    print("Ошибка при работе с PostgreSQL", error)
finally:
    if connection:
        cursor.close()
        connection.close()
        print("Соединение с PostgreSQL закрыто")
