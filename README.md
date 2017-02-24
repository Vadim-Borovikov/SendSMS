# SendSMS
Web service for sending SMS, viewing sent SMS and aggregated statistics.
The data is stored in a relational database.
During send an SMS the mobile country code of the receiver will be identified and stored within the SMS record.
This service is able to return the responses in JSON and XML formats.

# Configure and install
- Replace connection string in _Web.config_ with a connection string to your MySql Data Base.
- At IIS Manager:
  - Create new Application Pool v4.0.
  - Add new Application on your site with this pool and path were project was downloaded/published to.

# Services
Service | Example | Description
--- | --- | ---
countries | countries.json | Gets the countries list.
sms/send | sms/send.json?from=The+Sender&to=%2B4917421293388&text=Hello+World | Sends the SMS.
sms/sent | sms/sent.json?dateTimeFrom=2015-03-01T11:30:20&dateTimeTo=2015-03-02T09:20:22&skip=100&take=50 | Gets the SMS sent earlier.
statistics | statistics.json?dateFrom=2015-03-01&dateTo=2015-03-05&mccList=262,232 | Gets the statistics for days and counties.
More detailed information and examples are available on Web.API Help Page once API is running.

# Actual SendSMS implementation
While this project is a WebAPI/DB wrapper around sending SMS, it can not send real SMS.
To make it actually work, one should add an `ISMSSender` implementation and replace `DummySMSSender` creation in _SendSMS.WebAPI/BusinessLogic/DataManager.cs_ with it.

# Technology stack
- C# 6.0
- .NET Web API (Framework 4.5)
- ReSharper
- Entity Framework Code First
- MySQL

# Possible optimizations
- DataBase may perform better after replacing LINQ queries in _SendSMS.Data/DataProvider.cs_ with Stored Procedures.
- _SendSMS.Data_ project may be extracted to its own solution with internal API for better scalability

# Difficulties I've been through
- Since DB SMS price should have 3 decimals, I needed to adjust DB decimal format. Since I decided follow the Code First approach, it took a dedicated NuGet package (EFAttributeConfig) to make an annotation required.
- API SMS price, on the other hand, should have 2 decimals, and I have no idea how real-world companies compute their bills, so I just `Math.Round` prices for single text and for statistics record.
- .json/.xml extensions for associated result formats in Web.API wasn't obvious for me. I did it with `config.Formatters.JsonFormatter.AddUriPathExtensionMapping` in `WebApiConfig` after some research.
