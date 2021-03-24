import socket

class Server(object):
	def __init__(self):
		self.p1_socket = socket.socket()
		self.p2_socket = socket.socket()
		self.host = ''
		self.port1 = 12345
		self.port2 = 23456
		self.options = {'1': "Rock", '2':"Paper", '3':"Scissors"}

		self.p1_socket.bind((self.host, self.port1))
		self.p2_socket.bind((self.host, self.port2))

		print('Created socket. Awaiting connections...')

		prompt = "Select an option.\n"
		op1 = "\t1. Rock\n"
		op2 = "\t2. Paper\n"
		op3 = "\t3. Scissors"
		self.gameMessage = prompt + op1 + op2 + op3

	def Start(self):
		while True:
			connection1, connection2 = self.__connect_players__()

			welcome1 = "Welcome Player 1 "
			welcome2 = "Welcome Player 2 "
			
			connection1.send((welcome1 + self.gameMessage).encode())
			response1 = connection1.recv(1024).decode()
			print (response1)
			
			connection2.send((welcome2 +  self.gameMessage).encode())
			response2 = connection2.recv(1024).decode()

			draw, p1_wins = self.__determine_outcome__(response1, response2)
			self.__send_results__(response1, response2, draw, p1_wins, connection1, connection2)

			self.p1_socket.close()
			self.p2_socket.close()
			self.__init__()

	def __connect_players__(self):
		self.p1_socket.listen(5)
		print('Player 1 connected, rerouting...')
		connection1, addr1 = self.p1_socket.accept()
		connection1.send('555'.encode())
		self.p1_socket.close()
		self.p1_socket = socket.socket()
		self.p1_socket.bind((self.host, self.port1))
		self.p1_socket.listen(5)
		connection1, addr1 = self.p1_socket.accept()
		connection1.send('666'.encode())
		print('Player 2 connected.')

		self.p2_socket.listen(5)
		connection2, addr2 = self.p2_socket.accept()
		print('Player 1 rerouted..')

		return connection1, connection2

	def __determine_outcome__(self, response1, response2):
		draw = False
		p1_wins = True

		if response1 == response2:
			draw = True
		elif response1 == '1' and response2 == '2':
			p1_wins = False
		elif response1 == '1' and response2 == '3':
			pass
		elif response1 == '2' and response2 == '1':
			pass
		elif response1 == '2' and response2 == '3':
			p1_wins = False
		elif response1 == '3' and response2 == '1':
			p1_wins = False
		elif response1 == '3' and response2 == '2':
			pass

		return draw, p1_wins

	def __send_results__(self, response1, response2, draw, p1_wins, connection1, connection2):
		m =  "Player 1 chose " + self.options[response1] + ".\nPlayer 2 chose " + self.options[response2] + ".\n"
		if draw:
			m = m + "Draw!"
			print(m + "Draw!")
		elif p1_wins:
			m = m + "Player 1 Wins!"
			print(m + "Player 1 Wins!")
		else:
			m = m + "Player 2 Wins!"
			print(m + "Player 2 Wins!")
				
		connection1.send(m.encode())
		connection2.send(m.encode())
