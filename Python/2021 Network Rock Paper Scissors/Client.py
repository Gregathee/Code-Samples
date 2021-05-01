import socket

class Client(object):
    def Start(self):
        quit = False
        while not quit:
            s = socket.socket()
            host = '138.229.149.71'
            port1 = 12345
            port2 = 23456
            print('Socket created. Connecting to server...')

            s.connect((host, port1))
            print('Connected. Please wait for other player')
            response = s.recv(1024).decode()
            if response == '555':
                s.close()
                s = socket.socket()
                connected = False
                while not connected: 
                    connected = True
                    try:
                        s.connect((host, port2))
                    except:
                        connected = False
            print( s.recv(1024).decode())
            badInput = True
            choice = ""
            while badInput:
                choice = input()
                if choice == '1' or '2' or '3':
                    badInput = False
                else:
                    print('invalid input')
            s.send(choice.encode())

            print(s.recv(1024).decode()) 
            badInput = True
            print("Play again?\n\t1. Yes\n\t2. No\n")
            while badInput:
                inpt = input()
                if inpt == '1' or '2':
                    badInput = False
                else:
                    print('invalid input')

            quit = inpt != '1'
            s.close()