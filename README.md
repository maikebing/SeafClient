# SeafileClient

This project is a client implementation for the [Seafile](https://www.seafile.com) Web API  for .Net Standard 2.0 .
 
The aim is to create a library to easily access a Seafile server and the files stored there through a .Net application in a strong-typed fashion (no custom JSON parsing and with meaningful error messages, etc.) The library uses async/await methods for requests to the Seafile server.

The current stable release of the SeafileClient library is available on NuGet [here](https://www.nuget.org/packages/SeafileClient/).

 

## Usage example (C#)

```C#
using SeafileClient;

async Task Demo()
{
    var serverUri = new Uri("https://seacloud.cc", UriKind.Absolute);
    var username = "testuser@internet.com";
    var password = new [] { 't', 'e', 's', 't' };

    try
    {
        // authenticate with the Seafile server and retrieve a Session
        var session = await SeafSession.Establish(serverUri, username, password);

        // connection was successful
        // The password array has been overwritten with zeroes as soon as the authentication request was sent
        // now retrieve some information about the account
        var accountInfo = await session.CheckAccountInfo();
        Debug.WriteLine("Nickname: {0}\nUsed Storage: {1:d} Bytes\nQuota: {2}", accountInfo.Nickname, accountInfo.Usage, accountInfo.Usage);

        // get the url of the user avatar
        var userAvatar = await session.GetUserAvatar(128);
        Debug.WriteLine("Url to user's avatar: " + userAvatar.Url);

        // get the libraries
        var libraries = await session.ListLibraries();
        foreach (var lib in libraries)
            Debug.WriteLine(lib.Name + " " + lib.Timestamp);

        // list root contents of the first library
        var firstLib = libraries.FirstOrDefault();
        if (firstLib != null)
        {
            var content = await session.ListDirectory(firstLib, "/");
            foreach (var dirEntry in content)
                switch (dirEntry.Type)
                {
                    case SeafClient.Types.DirEntryType.File:
                        Debug.WriteLine(dirEntry.Name + " - " + dirEntry.Size + " Bytes");
                        break;
                    default:
                        Debug.WriteLine(dirEntry.Name);
                        break;
                }
        }
    }
    catch (SeafException ex)
    {
        Debug.WriteLine($"An error occured: {ex.Message} (ErrorCode: {ex.SeafError.SeafErrorCode} ({ex.SeafError.HttpStatusCode}))");
    }
}
```

## Currently implemented requests
See the [official Seafile Web API documentation](https://manual.seafile.com/develop/web_api_v2.1.html) for a list of all available requests. The following requests are currently implemented:

* Authentication
* Ping (with and without authentication)
* Get Server Info
* Check Account Info
* Get User Avatar
* Get Default Library
* Get Library Info
* Create Library (unencrypted and encrypted)
* List Libraries / List Shared Libraries
* List Directory Entries
* Create Directory
* Delete Directory / File
* Rename Directory / File
* Copy / Move File
* Get Thumbnail Image
* Get Download Link
* Get Upload Link / Upload file
* Get Update link / Update file
* Starred files (list / star / unstar)
* Groups (list / create / delete)
* Group members (add / bulk add / remove)
* Decrypt an encrypted library (allowing to download files from the encrypted library)
