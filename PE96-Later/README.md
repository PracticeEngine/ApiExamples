# PE 9.6 and Later Example

- NOTE: this example is not used for systems running 9.5 and prior

Starting with Practice Engine 9.6, our Api System has changed to used Open ID Connect for Authentication via our 'Auth' server (typically installed at /auth url).

To consume our WebApi there are a couple steps :

1. Add the IdentityModel nuget package (this provides OpenIDConnect protocol clients).
1. Use the DiscoClient to Discover the endpoints from the /auth site (aka auth server).
1. Use the TokenClient to Request an Access Token from the auth server's TokenEndoint using your AppID and AppKey.
1. Set the BearerToken on a HttpClient.
1. Make calls to Post/Get via the API.

Our sample applications included here provide a PracEngApi class that wraps this all up - feel free to use this class in your code, to simplify your code.

*Future* - we will be adding Swagger and other technologies that will make it easier to consume calls, and build requests in upcoming versions of PE. Star or Follow this repository to get information when that is available.
