import requests, json
from pprint import pprint

servurl = 'https://<server address>'
authurl = servurl+'/auth'
apiurl = servurl+'/pe/api/StaffMember/Me'
appid = '<appid>'
appkey = '<appkey>'

respDisc = requests.get(authurl+'/.well-known/openid-configuration')
tokenurl = respDisc.json()['token_endpoint']
auth = (appid,appkey)
authtype = {'grant_type': 'client_credentials', 'scope': 'pe.api'}
resptoken = requests.post(tokenurl, data=authtype, auth=auth)
token = resptoken.json()['access_token']
apiheader = {'Authorization': 'Bearer ' + token}
# add the apiheader to PE api requests

respapi = requests.get(apiurl, headers=apiheader)
pprint(respapi.json())
