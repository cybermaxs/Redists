# RedisTS

[![Build status](https://ci.appveyor.com/api/projects/status/vd1il43mgwx66ti8?svg=true)](https://ci.appveyor.com/project/Cybermaxs/redists)
[![Nuget](https://img.shields.io/nuget/dt/redists.svg)](http://nuget.org/packages/redists)
[![Nuget](https://img.shields.io/nuget/v/redists.svg)](http://nuget.org/packages/redists)

A very compact representation of a list of samples, usually referred as time series. Inspired from https://github.com/antirez/redis-timeseries

## Installing via NuGet
---
```
Install-Package Redists
```
# Show me the code !
_You can review tests or open the sample RandomMonitor to see how to implement it._

###Setup your TimeSeriesClient
Redists will not create a new StackExchange.Redis connection. You have to pass an existing connection to the main factory.
```csharp
	//db is and instance of ConnectionMultiplexer.GetDatabase()
    var tsOptions = new TimeSeriesOptions(3600 * 1000, 1, TimeSpan.FromDays(1));
    var client = TimeSeriesFactory.New(db, "msts", tsOptions);
```

###Append data

```csharp
    await client.AddAsync(DateTime.UtcNow, 123456789);
```

###Get the data

```csharp
    await client.RangeAsync(DateTime.UtcNow.AddHours(-1), DateTime.UtcNow);
```

# Acknowledgements
+ Salvatore Sanfilippo (@antirez) : Creator of Redis
+ Marc Gravell(@marcgravell) : Creator of [StackExchange.Redis](https://github.com/StackExchange)) is a high performance general purpose redis client for .NET languages

# License
Licensed under the terms of the [MIT License](http://opensource.org/licenses/MIT)

Want to contribute ?
------------------
- Beginner => Download, Star, Comment/Tweet, Kudo, ...
- Amateur => Ask for help, send feature request, send bugs
- Pro => Pull request, promote

Thank you
