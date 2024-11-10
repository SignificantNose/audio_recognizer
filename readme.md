# Recognizer — audio metadata at your fingertips

Recognizer is a project that strives to make audio metadata fetching easier by utilizing audio fingerprinting algorithms and making use of extensive audio metadata storages. The primary goal of this project was to get familiar with audio recognition algorithms and techniques.  

Recognizer project is composed of 4 key components:
- Gateway — the microservice that is available to the end users
- Brain — microservice containing recognition engine that utilizes the [Chromaprint library]
- Covers — microservice for fetching the URIs to the recognized audio tracks
- Metadata — track metadata itself

There's also an Aspire project present in the repository, however the application launch procedure does not fully depend on Aspire, so it is just another option to launch the application. 

The inspiration for the project comes from [this article][chromaprint article], that goes deep into audio recognition algorithms that formed the basis of the following web APIs this project took implementation details from:
- [AcoustID] — recognize audio by its fingerprint
- [MusicBrainz] — fetch audio metadata by its ID, relies on AcoustID
- [CoverArtArchive] — fetch link to audio cover art, relied on MusicBrainz/AcoustID

## Disclaimer

> Although **there ARE solutions to the problem** provided by AcoustID and MusicBrainz themselves (e.g. [MusicBrainz Picard]), our project's goal was to — again — get familiar with the audio fingerprinting algorithms and build something that can be used as a convenient mean of providing metadata to audio files, analagous to solutions provided by MusicBrainz and AcoustID.  
Our goal was not to build a counterpart OR competitor to these solutions by any means, as our solution strongly relies on their services and their audio fingerprinting algorithms. Neither was our goal to monetise the application, even though it is allowed on a paid basis.  
Same goes to the rewritten [open-source chromaprint library][original chromaprint library], which is basically the implementation of the audio fingerprinting algorithms described in [this article][chromaprint article]. The goal was to get familiar with the algorithms, and we believe that the best way to learn is to practise.

# Table of contents

- [How to launch](#how-to-launch)
    - [Manual build](#manual-build)
    - [Docker Compose application launch](#docker-compose-application-launch)
    - [.NET Aspire application](#net-aspire-application)
- [User manual](#user-manual)
    1. [Track recognition](#1-track-recognition)
    2. [Add artist metadata](#2-add-artist-metadata)
    3. [Add album metadata](#3-add-album-metadata)
    4. [Add track metadata](#4-add-track-metadata)
    - [Notes about application functionality](#notes-about-application-functionality)
- [Notes & features](#notes--features)
    - [Authentication](#authentication)
- [Dev notes](#dev-notes)
    - [Overview of the structure of the microservices](#overview-of-the-structure-of-the-microservices)
    - [About Clean Architecture implementation](#about-clean-architecture-implementation)
    - [About tests](#about-tests)
    - [Aspire project notes](#aspire-project-notes)
    - [Rich and anemic models](#rich-and-anemic-models)
    - [About migrations](#about-migrations)
    - [Goals for the future](#goals-for-the-future)
- [Conclusion](#conclusion)

# How to launch
The application can be launched with the following ways:
- Manually build all the microservices and launch them on your machine
- Launch Docker Compose application
- Launch .NET Aspire application
- Deploy the project to Azure *(under maintenance)*

Currently no configuration parameters are required to be provided to the application. You can consider the project to be in development state.

## Manual build

In order to build the Brain, a reference to [our Chromaprint library][Chromaprint library] package must be provided. Currently, the `Chromaprint.dll` is provided in the project files under ```/RecognizerBrain/Application/Packages/Chromaprint.dll```, as the corresponding `Application.csproj` suggests. In case the version of the library changes and the application needs to be up to date to the new algorithms, you must provide the compiled library to the appropriate location.

In order for the microservices to successfully launch, they need to be provided a database (except for Gateway project). To simplify the process, a corresponding compose file is provided in each of those microservices to launch a database with a microservice.

> Note: the gateway project must be built with the ```docker build``` command called in the root dir, as it references other projects. So the build command would be the following:
    
    docker build -t tag --file ./RecognizerGateway/Dockerfile .

> WARNING: for now, all the images of the microservices must be built in the root directory using the command provided above. The reason for that is the dependency on the `RecognizerAspire.ServiceDefaults` project, which is not mandatory to be present when the project is launched outside of the Aspire project. The other solution would be to pull the images from the docker hub registry.

## Docker Compose application launch

Docker Compose launch methods are easier in comparison to launching the manually built application. A simpler version of the compose file currenly lies in the root of the project. 

The other, more advanced and more preferred option would be to use the docker compose with a load balancer and metrics collection introduced for the Brain microservice (as it is expected that the recognition procedure takes most of the time). The compose file lies under ```./RecognizerAspire/NginxLoadBalancing/docker-compose.yaml```. To launch the application using this compose file, use:

    docker compose up --scale svcbrain=N


Provide the amount of running Brain instances under N.

> WARNING: for now, all the images of the microservices must be built in the root directory using the command provided above. The reason for that is the dependency on the `RecognizerAspire.ServiceDefaults` project, which is not mandatory to be present when the project is launched outside of the Aspire project. The other solution would be to pull the images from the docker hub registry.

## .NET Aspire application

The Aspire project was mainly introduced to simplify the process of collecting the metrics. It also allows to visualize metrics through their own Dashboards mechanism. Launch the project under ```RecognizerAspire/RecognizerAspire.AppHost/RecognizerAspire.AppHost.csproj```. The project still requires Docker to be installed on your machine.

# User manual
The user can access the application through its gateway, other services are called through the gateway, not by user. The application allows for several functions:

## 1. Track recognition 

The application allows all users to recognize the tracks under ```/recognizeTrack``` endpoint. It is a POST endpoint. The main reason for that is that, apart from the fingerprint (which is usually a large Base64-encoded string), the microservice must receive additional information. Currently it must also receive the duration of the audio track. In the future it is planned to introduce the ability to choose what properties to return about the audio. The microservice expects to receive a JSON. An example of a valid request is the following:

    POST http://gateway/RecognizerEndpoint/recognizeTrack
    Content-Type: application/json
    Accept: */*
    
    {
        "fingerprint": "string",
        "duration": 0
    }    

### Request properties
- `fingerprint` — Base64-encoded string, an audio fingerprint generated using Chromaprint library. Mean of recognizing the audio file, cannot be an empty string.
- `duration` — integer, audio file duration, in seconds. Mean of recognizing the audio file.

### Application response 
In case there were no internal errors, the response will be structured similarly to this:

    HTTP/1.1 200 OK
    content-type: application/json; charset=utf-8 
    date: Mon,01 Jan 2024 00:00:00 GMT 
    server: Kestrel 
    transfer-encoding: chunked 

    {
        "trackId": 5,
        "title": "mirror",
        "artists": [
            {
                "artistId": 2,
                "stageName": "Kendrick Lamar"
            }
        ],
        "releaseDate": "2022-05-13",
        "album": {
            "albumId": 3,
            "title": "Mr. Morale & The Big Steppers"
        },
        "coverUri": "http://coverarts..."
    }
    

### Response properties
The response properties are put into a JSON object, and are pretty self-explanatory. In case the track has not been recognized, the response will be empty, with an OK status code.

In case the recognition service was down, the response status code will be 503: Internal error. However, if any other service was down, the response code will be 200: OK, as the track has been recognized, and the track metadata can be fetched later on.

## 2. Add artist metadata
[Privileged](#notes-about-application-functionality) users are allowed to add metadata about artists under ```/add/artist``` endpoint. It is a POST endpoint. An example of a valid request is the following (the parameters are passed in the request URL):

    POST http://gateway/RecognizerEndpoint/add/artist?stageName=Don%20Toliver&realName=Caleb%20Zackery%20Toliver
    Content-Type: application/json
    x-api-key: abc...
    Accept: */*
    

### Request properties
The request parameters are self-explanatory: `stageName` being the staeg name of the artist and `realName` being the real name of the artist

### Application response
In case there were no internal errors, the response will be structured similarly to this:

    HTTP/1.1 200 OK
    content-type: application/json; charset=utf-8 
    date: Mon,01 Jan 2024 00:00:00 GMT 
    server: Kestrel 
    transfer-encoding: chunked 

    10

### Response properties
The only returned value is an integer value representing the ID of the added artist in the database.

In case the metadata service was down, the response status code will be 503: Internal error.


## 3. Add album metadata
[Privileged](#notes-about-application-functionality) users are allowed to add metadata about artists under ```/add/artist``` endpoint. It is a POST endpoint. An example of a valid request is the following (in the example most parameters are passed in the request URL):

    POST http://gateway/RecognizerEndpoint/add/album?title=Novacane&releaseDay=12&releaseMonth=1&releaseYear=2010
    Content-Type: application/json
    x-api-key: abc...
    Accept: */*
    
    [
        3, 4
    ]

### Request properties
- `title` — a string representing the title of the album
- `releaseDay`, `releaseMonth`, `releaseYear` — integer properties representing the release date
- `artistIds` — a collection of artist IDs that are supposed to represent the authors of the album 

### Application response
In case there were no internal errors, the response will be structured similarly to this:

    HTTP/1.1 200 OK
    content-type: application/json; charset=utf-8 
    date: Mon,01 Jan 2024 00:00:00 GMT 
    server: Kestrel 
    transfer-encoding: chunked 

    10

### Response properties
The only returned value is an integer value representing the ID of the added album in the database.

In case the metadata service was down, the response status code will be 503: Internal error.

## 4. Add track metadata
[Privileged](#notes-about-application-functionality) users are allowed to add metadata about artists under ```/add/artist``` endpoint. It is a POST endpoint. An example of a valid request is the following(in the example most parameters are passed in the request URL):

    POST http://gateway/RecognizerEndpoint/add/track?title=NOID&releaseDay=15&releaseMonth=1&releaseYear=2024&albumId=2&coverArtId=4
    Content-Type: application/json
    x-api-key: abc...
    Accept: */*
    
    [
        3, 4
    ]

### Request properties
- `title` — a string representing the title of the track
- `releaseDay`, `releaseMonth`, `releaseYear` — integer properties representing the release date
- `albumId` — ID of the album this track is present in; nullable (e.g. null when a track is released as a single)
- `coverArtId` — ID of the cover art belonging to the track; nullable (e.g. null when the cover is unknown or not present in the service) 
- `artistIds` — a collection of artist IDs that are supposed to represent the authors of the track

### Application response
In case there were no internal errors, the response will be structured similarly to this:

    HTTP/1.1 200 OK
    content-type: application/json; charset=utf-8 
    date: Mon,01 Jan 2024 00:00:00 GMT 
    server: Kestrel 
    transfer-encoding: chunked 

    10

### Response properties
The only returned value is an integer value representing the ID of the added track in the database.

In case the metadata service was down, the response status code will be 503: Internal error.


## Notes about application functionality

The addition of metadata is protected and cannot be performed by regular users. Users must be authenticated to perform this action. See [notes](#authentication) for more information.

# Notes & features

## Authentication 
As it was mentioned before, some request require users to be authenticated. While it is expected that anyone can use the main function of the application — the recognition of the tracks — only privileged users are allowed to contribute to the knowledge stored in the application. 

Currently API key authentication is implemented. The API key must be provided in the ```X-Api-Key``` header of the request, otherwise the requests that require authentication will fail.

In case the API key is missing or invalid, the return status code will be 401: Unauthorized.

# Dev notes
## Overview of the structure of the microservices
The gateway of the application uses REST API as a main mean of communication with the client, while other microservices use GRPC to communicate with each other. That's why the gateway will accept regular REST HTTP requests and send GRPC requests to other microservices to acquire the required data.

All microservices (except for gateway) were implemented with Clean Architecture in mind. That's why almost in each service you'll see the following layers:

- Domain layer — main business logic, types that are common to the enterprise; does not reference other layers
- Infrastructure layer — communication with external services; (in  case of this application only communication with database is present)
- Application layer — orchestration of the use-cases that operate on the domain; contains application-specific types and interfaces

See more notes on Clean Architecture implementation details [here](#about-clean-architecture-implementation). 

Apart from the layers of the Clean Architecture, you can also see the following projects in each of the microservice:

- Application itself — ASP.NET core application providing endpoints for communication
- Tests — unit and integration tests of the microservice
- Proto — a class library providing access to the GRPC service, message and client classes

> The Proto project is introduced as a separate project so that other services that must communicate with the GRPC microservice should not reference the whole microservice project. 

## About Clean Architecture implementation
### Domain layer
The Domain layer contains the classes for entities that are stored in the database under the Entities directory. However, it is often not required to operate with all the data that is stored in the database (e.g. IDs of the entities are not always required). That's why some classes were introduced to wrap only the data that is needed to satisfy a user request (e.g. add an entity, or fetch brief data on some track, which might be useful when displaying a list of tracks). The classes are:

- Models — classes that wrap the data present in the request
- Projections — classes that wrap the returned data

Domain layer also contains the implementation of the Result pattern, which is used in the Application layer. The Result pattern is introduced here, as the Domain layer can be referenced by all of the outer layers.

### Application layer
The application layer mainly just operates on the provided models. Also the main responsibility of the Application layer is to wrap the result of the operation in the Result class.

### Infrastructure layer
The Application communicates with an application using the Infrastructure layer. A Repository pattern has been introduced to simplify communication with the database and make it independent of the underlying implementation. 

Migrations are also present in the Infrastructure layer. Migrations are run using FluentMigrations NuGet package.

The DBMS used in the project is PostgreSQL. Corresponding interfaces for establishing communication with the DBMS have been introduced. The connection string is provided using configuration of ASP.NET application.

> WARNING: the interface that provides the connection to the database (PgRepository) uses the obsolete method of acquiring the connection: ```new NpgSqlConnection(conn_string)```. A more modern and recommended solution to this is using data sources, as the documentation of NpgSql [suggests][NPG usage of DataSource].

## About tests
There are unit tests present in Brain, Metadata, Covers and Gateway project. Unit tests cover Mapping tests (e.g. from Domain classes to GRPC classes and the other way around) and tests for application layers.

Integration tests are present in Metadata and Aspire projects. Tests in Aspire project are testing the system as a whole. Tests in Metadata projects are divided into tests with a real DBMS present on the machine the tests are run on and tests with containerized DBMS.

### Real DBMS integration tests
Integration tests run with the real DBMS do not change the state of the database, therefore there aren't any tests that add or remove data. 

### Containerized DBMS integration tests
Integration tests run with a DBMS run in a container use Testcontainer and Respawn NuGet packages. Respawn package was not applicable in the real DBMS, as it deletes all the data from the tables. VersionInfo table is ignored in the tests, so that migrations are not run each time.

## Aspire project notes
Somehow persisting volumes do not want to work in Aspire when multiple instances of DBMS are run and try to persist the data: they conflict with each other. Maybe the persisted volume is instantiated once for all, and is dedicated only for one running instance.

Considering Aspire name resolution: for some reason GRPC does not support name resolution, like HttpClient does. https://github.com/dotnet/aspire/issues/2896 states that you can use `ServiceEndPointResolver` class, I tried to implement it but stumbled into not being able to introduce it in the building stage -> did not implement it and left with the localhost names in the gateway.

## Rich and anemic models
The projections with collections are not rich - they're anemic. This should be considered when continuing developing the project: it must be considered as soon as possible.

## About migrations
There was a thought to move the migrations into a separate project (so that in case multiple instances are run, they do not conflict with each other on the point of migrations and just wait until the migrations project finishes working). This would've led to a new image that must co-exist with each microservice, meaning more complexity added to the project. 

A decision has been made to make it a part of functionality of the microservices. Now, the appilcation reads the configuration, and in case it finds a `MigrateUp` configuration value set to `true`, the migrations will run. That way, other instances of the microservice can wait for the one instance running the migration to start running, and then start. 

The approach described above is implemented in the compose file, which lies under `./RecognizerAspire/NginxLoadBalancing/docker-compose.yaml`. All microservices have the `-migr` suffix added to them, describing that this instance will run migrations on startup.

However, the implementation in the compose file is far from being ideal: in case of Brain microservice, there is a separate migrations microservice, which is a working instance, and the service configuration is pretty much the same for them both. Therefore, if some configuration value must be changed for Brain microservice, it must be changed in both entries. I haven't found a way to make this more elegant, so this problem exists for now. Also, the amount of instances in the `svcbrain` service is the amount of ADDITIONAL instances. For now, it cannot be 0, as the load balancer will not be able to resolve the hostname and will fail to start.

The 'separate migrations project' approach is not ideal as well: another problem that would've occurred when moving migrations into a separate project is that the connection string must've been shared (in order to avoid duplicating the connection string in both projects). It is possible with some workarounds described in [this][Shared config asp net] and [this][Include linked files from outside project asp net] articles, however it still leaves us with the added complexity and a new image that must be included into each deployment environment.

## Goals for the future
There are plenty things to implement in the project. The following is just a small part of the bigger picture:

- Implement rich errors in GRPC microservices (present as todo in the source code): https://learn.microsoft.com/en-us/aspnet/core/grpc/error-handling?view=aspnetcore-8.0#reading-rich-errors-by-a-client
- Implement the message broker
- Make multiple return types in the recognition microservice
- Add the ability for user to choose the fetched track metadata
- Remake the cover art database schema. The PK must be TrackID+Cover_Type, with a string value denoting the path to the image
- Modify the structure of Brain, Covers, Metadata microservices to be composed of several components: add another HTTP endpoint for metrics collection, possibly protected and requiring an API key.


# Conclusion
Considering this repository, it can be said that, as a result, an application has been created that is flexible due to the features ASP.NET Core provides. The microservices can also be modified easily due to the usage of Clean Architecture, and the communication interface that these APIs present can be considered all-purpose, as they are easy to follow.


[Chromaprint library]: <https://github.com/0TheThing0/Chromaprint_lib>
[Original chromaprint library]: <https://github.com/acoustid/chromaprint/tree/master>
[Chromaprint article]: <https://oxygene.sk/2011/01/how-does-chromaprint-work/>

[AcoustID]: <https://acoustid.org/>
[AcoustID register application]: <https://acoustid.org/new-application>

[MusicBrainz]: <https://musicbrainz.org/>
[MusicBrainz UserAgent strings]: <https://musicbrainz.org/doc/MusicBrainz_API/Rate_Limiting#Provide_meaningful_User-Agent_strings>

[CoverArtArchive]: <https://coverartarchive.org/>

[MusicBrainz Picard]: <https://picard.musicbrainz.org/>

[NPG usage of DataSource]: <https://www.npgsql.org/doc/basic-usage.html>

[Shared config asp net]: <https://andrewlock.net/sharing-appsettings-json-configuration-files-between-projects-in-asp-net-core/>
[Include linked files from outside project asp net]: <https://andrewlock.net/including-linked-files-from-outside-the-project-directory-in-asp-net-core/>