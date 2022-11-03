# Warframe Manager API
A web service keeps track of a user's resources for [Warframe MMOs](https://www.warframe.com/), allows user profile information and items in MongoDB and is an API service to interact with [Warframe Manager](https://github.com/michaelenoroexe/warframeManager).
## Getting Started

Clone the repository
Before using the web service it is necessary to deploy a Mongo database, for this in the root of the project there is a DB folder and Json dump files of basic collections from the database, they can be used to create your own Mongo database, or use the ready deployed in Mongo atlasian by a standard connection string.

-------------------------------------
The structure of the database should look as follows
Database name: Warframe Manager
Collection names (Collection names can be changed in the Startup file):
 - The Components collection: Contains a list of all in-game items;
 - The Planets collection: Contains all the planets available in the game;
 - Types collection: Contains a list of the types of items available;
 - Users collection: Contains the username/password of the user;
 - UsersInfo collection: Contains information about the user's profile;
 - UsersResources collection: Contains a list of all items entered by the user.
Next, you can use an environment variable named "MongoClientUrl" to connect to the new database, or in the appsettings.json file, replace the value of the MongoUri variable with its connection string.
--------------------------------------
Knock down the API project.
The service is up and running and ready to receive enquiries at http://localhost:5132 and https://localhost:7132
Makes a simple request to the service for performance testing.

 ![2022-11-03_15-44-49](https://user-images.githubusercontent.com/86874761/199787459-1c9fbf2b-4741-4d8f-b660-bb41e102562f.png)

## Basic functionality of the web service

### **Controller for receiving data api/GetData**
> #### The main task is to provide the user with data
The controller handles the following GET requests:
- ResourcesList query - Retrieves a list of resources that are involved in the creation of other resources, but are not created themselves; 
- Request All Items List (ItemsList) - Retrieves a list of all items excluding resources;
- TypesList query - Retrieves all possible types of items (Primary, Secondary, Melee, etc.);
- Request a list of all planets in the game (Planets) - Retrieves all planets available in the game;
- Query all user resources (UserResourcesList);
- Query all user items (UserItemsList);
- Query all user items (UserItemsList);
- Query the number of user credits (UserCredits);
- Query user profile information (UserInfo).

```json
{
        "creationTime": 43200,
        "credits": 25000,
        "neededResources": {
            "Amesha Harness": 1,
            "Amesha Systems": 1,
            "Amesha Wings": 1,
            "Nitain Extract": 4
        },
        "id": {
            "timestamp": 1661285454,
            "machine": 9465958,
            "pid": 19592,
            "increment": 15071783,
            "creationTime": "2022-08-23T20:10:54Z"
        },
        "name": "Amesha",
        "location": null,
        "type": [
            "62d8682daeef469267d8083f"
        ],
        "mastery": true,
        "owned": 0,
        "stringID": "6305344e9070664c88e5fa27"
    }
```

**The structure of the returned item.**
> **"creationTime"** - Time of creation of an item in seconds; (Item) <br>
> **"credits"** - Item creation price in credits; (I) <br>
> **"neededResources"** - Components needed to create the item; (I) <br>
> **"id"** - Serialised id of the item in the database; (Item/Resource) <br>
> **"name"** - The name of the item;	(I/R) <br>
> **"location"** - Only available for resources and shows the planet where they are received; (I/R) <br>
> **"type"** - List of item types; (I/R) <br>
> **"mastery"** - Is it possible to gain profile experience for an item; (I/R) <br>
> **"owned"** - The number of items the user has, the standard is 0 (I/R) <br>
> **"stringID"** - String representation of item id from the database (I/R) <br>

### **Controller of data change api/ProfUp**
> #### The main task is to edit user data.

The controller handles the following Post requests:
- Request to change item quantity(api/ProfUp) - the request takes data in the form: {Resource: string (string id of item), Number: int(new owned number), Type: string(resource or item depends on user needed item)}; 
- Request to change the number of credits (creds) - the request takes data in the form of {Number: int};
- UserInfo query - the query takes data in the form of {Rank: int(User rank number)}: {Rank: int(User rank number), Image: int(User picture number)};

### **User administration controller api**
> #### The main task is to administer users.

The controller handles the following Post requests:
- Registration request - accepts {Login: string, Password: string}; 
- Signin request - takes {Login: string, Password: string} returns JWT token to user;
- Request to change user's password(passChange) - accepts data in the form: {OldPassword, NewPassword};
- Request to delete user(delUser) - accepts account password as {Password: string}.

