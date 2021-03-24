import Server
import Client
import smtplib

def main():
	prompt = """Choose which program to run
\t1. Client
\t2. Server
\t3. Send email
"""

	option = input(prompt)

	if option == '1':
		client = Client.Client()
		client.Start()
	elif option == '2':
		server = Server.Server()
		server.Start()
	elif option =='3':
		EMailService()

def EMailService():
	sender = input("\nEnter sender email\n")
	receivers = input("\nEnter receivers email seperated by a ','\n")
	subject = input("\nEnter subject line\n")
	message = input("\nEnter message\n")
	print('')
	final_message = "From: From Person <" + sender + ">\nTo: To Person <" + receivers + ">\nSubject: " + subject + "\n\n" + message + "\n"
	print(final_message)
	domain = sender.split('@')[1]
	try:
		print('domain: ' + domain)
		smtpObj = smtplib.SMTP('mail.' + domain, 25)
		smtpObj.sendmail(sender, receivers, message)
		print ("Successfully sent email")
	except smtplib.SMTPException:
		print ("Error: unable to send email")


main()
