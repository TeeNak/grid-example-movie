# method

## Get

http://localhost:40221/Movies

http://localhost:40221/Movies(1)

## POST

Request

POST
http://localhost:40221/Movies/

    {
         "Code":"tt1000001",
         "Name":"Test Movie",
         "Version":1
    }

Response
"Id" is automatically filled. "Id" in request will be ignored.

    {
        "@odata.context": "http://localhost:40221/$metadata#Movies/$entity",
        "Id": 12,
        "Code": "tt1000001",
        "Name": "Test Movie",
        "Version": 1
    }

## Patch

Request

PATCH 
http://localhost:40221/Movies(12)/

    {
         "Code":"tt1000001",
         "Name":"Test Movie 2",
         "Version":1
    }

Response
204 No Content




## Update All

POST
http://localhost:40221/Movies/Action.UpdateAll/


    {
        "value":[
            {
                "Id":1,
                "Code":"tt0094675",
                "Name":"Ariel",
                "Version":1
            },
            {
                "Id":2,
                "Code":"tt0092149",
                "Name":"Varjoja paratiisissa",
                "Version":1
            },
            {
                "Id":3,
                "Code":"tt0113101",
                "Name":"Four Rooms",
                "Version":1
            }
        ]
    }