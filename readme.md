# WeatherCal

This projekt was developed during the hackathon weekend at [Obungi](http://www.obungi.com/) in March 2018.

## General Idea

As a user I want to get a calendar entry for a specific weather condition. The weather condition can be definied by a location (name, latitude, logitude), wind speed (minimum and maximum) and wind bearing (angle from to).

## Architecture

This is the diagram which we draw during the hackathon:

![Architecture](/Arch.jpg "Architecture")

We identified the terms of a feed and subscrption. A feed is a collection of subscription and a subscription is the notification for a location with specific conditions.

The application is split into 3 parts:

- API App
  - Management of feeds and subscriptions
  - [Swagger](https://weathercalazureapi.azurewebsites.net/swagger/ui/index#/Feed)
- Azure Function as a scheduler
  - CRON Job, runs once a day
  - Fetches all subscribed locations from the darksky API
  - Filter result by wind speed and bearing conditions
  - Pushes a message per feed to a topic in a Azure Service Bus
- Webjob
  - Retrieves the message from the Service Bus and transforms the result of the scheduler function into a calendar event. 

## Used Azure products

- Azure Web Api
- Azure Functions
- Azure Webjobs
- Azure Service Bus
- Azure Table Storage
