
import threading
import time

def function1(str):
   time.sleep(120)
   print('Function 21:', str)

def function2():
    for i in range(5):
        print('Function 2:', i)
        time.sleep(1)

thread1 = threading.Thread(target=function1("@22"))
thread2 = threading.Thread(target=function2)

thread1.start()
thread2.start()

thread1.join()
thread2.join()

'''
import fileinput
typeSignal = "1"
typeLocal = "55.7374"

def ReplaceLine(old_line,new_line):
    backup_extension = '.bak'
    for line in fileinput.input("draw.js", inplace=1, backup=backup_extension):
        print(line.replace(old_line, new_line), end='')




if typeSignal == "1":
    print('Тут')
    f = open('draw.js', "r")
    content = f.readlines()
    f.close()
    for contents in content:
        index = content.index(contents)
        temp = str(contents)
        if (temp.find(typeLocal) != -1 ):
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
    print('Тут')
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
                ReplaceLine(temp,temp1)
            if (temp.find("#F00000") != -1):
                temp1 = temp.replace("#F00000", "#D6DF71")
                print(temp1)
                content[index] = temp1
                ReplaceLine(temp,temp1)
    print(type(content))
    print(content)

'''
