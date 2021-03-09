import requests
import json

client_id = "282069153345916"
redirect_uri = "https://www.facebook.com/connect/login_success.html"

scope = "CREATE_CONTENT, pages_manage_metadata, pages_manage_posts, pages_manage_read_engagement, pages_show_list"

access_token = "EAAEAilev8XwBAGNqesVPHZC0SIlxeZAslcZBcQA4hipFg9VIZA92DPEZBHxw7HbpBECW8P0ywZBtnDj2ZBdXJ9JKuB5MByCOut4yMu59r3qQtO2V6IjJtbDYINAktgFXwbOTXUCqUViGX7CzyTMampOKHZBoTzyZCJ96YNl5JFnNpRJYMgteZCby1vsFZCYZA9YKyM65ooE2gRAlmSdaAUBldZA1ZCRoBGnaEZC4TYby9ZCSkaecaQZDZD"

url = "https://graph.facebook.com/3975544335816956/accounts?access_token=" + access_token

def get_page_ID():
	url = "https://graph.facebook.com/3975544335816956/accounts?access_token=" + access_token

	response = requests.get(url)

	if response.status_code == 200:
		#print(response.content)
		return json.loads(response.content.decode('utf-8'))
	else:
		print("************fail***********")
		print("status code: " + str(response.status_code))
		print(response.content)
		return None

def post_to_page(id, message, token):
	url = "https://graph.facebook.com/{0}/feed?message={1}&access_token={2}".format(id, message, token)

	print('\n\n{}\n\n'.format(url))

	response = requests.post(url)

	if response.status_code == 200:
		return json.loads(response.content.ddecode('utf-8'))
	else:
		print("************fail***********")
		print("status code: " + str(response.status_code))
		print(response.content)
		return None

page_id = get_page_ID()
id = 0

id = page_id['data'][0]['id']
token = page_id['data'][0]['access_token']

post_to_page(id, "This is a message sent from python", token)







