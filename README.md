# The Page Notifier

A .NET Core 3.0 Worker Service that shows a web page once there is an update.

## What it does

The page notifier notifies you about updates on web pages. These pages can be typically the concert web pages of your favorite artists.

## How to configure your favorite web pages

Use your json editor of preference to modify [storage.json](https://github.com/ConnectingApps/PageNotifier/blob/master/src/PageNotifier.Worker/storage.json)

````json
{
  "WebPageDescriptions": [
    {
      "Url": "https://www.luciehorsch.nl/agenda.php",
      "NumberOfAlphaNumericCharacters": 10
    },
    {
      "Url": "https://www.martinfrost.se/concerts",
      "NumberOfAlphaNumericCharacters": 10
    }
  ]
}
````

In the json file shown above, you can see the concert web pages of two great artists. Logically, in case of an update on one of these pages, you want to be notified as soon as possible. This is exactly what the page notifier does. It regularly reads a the web pages, saves the number of alphanumeric characters and if it significantly differs from the known number, it beeps and opens the page for you so you can see the the changes in your default browser.

## What problem it solves

If you are a great fan of an artist, you do not need to check their website every hour to see if a new concert ticket can be bought. The service does that for you. You only need to read the concert page if there is a recent update on it.

## Supported platforms

Currently, only windows is supported. Since this software has been developed with .NET Core 3.0, we can support more platforms in the future.

## Get started

You need to have git and the [.NET Core 3.0](https://dotnet.microsoft.com/download/dotnet-core/3.0) SDK (for development) or Runtime (for running) to get started with this application. After that, use the following command line instructions.

````bash
git clone https://github.com/ConnectingApps/PageNotifier.git
cd PageNotifier/src/PageNotifier.Worker
dotnet restore
dotnet build
cd bin
mkdir publish
cd ..
dotnet publish -o bin/publish
cd bin/publish
PageNotifier.Worker.exe
````
