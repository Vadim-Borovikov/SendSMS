# SendSMS
Web service for sending SMS, viewing sent SMS and aggregated statistics.
The data is stored in a relational database.
During send an SMS the mobile country code of the receiver will be identified and stored within the SMS record.
This service is able to return the responses in JSON and XML formats.

# Services
Service | Example | Description
--- | --- | ---
countries | countries.json | Gets the countries list.
sms/send | sms/send.json?from=The+Sender&to=%2B4917421293388&text=Hello+World | Sends the SMS.
sms/sent | sms/sent.json?dateTimeFrom=2015-03-01T11:30:20&dateTimeTo=2015-03-02T09:20:22&skip=100&take=50 | Gets the SMS sent earlier.
statistics | statistics.json?dateFrom=2015-03-01&dateTo=2015-03-05&mccList=262,232 | Gets the statistics for days and counties.
More detailed information and examples are available on Web.API help page once API is published.

# Technology stack
- C# 6.0
- .NET Web API (Framework 4.5)
- Entity Framework Code First
- MySQL

# Configure and install
- At Visual Studio
  - Open the _SendSMS.sln_ solution
  - Replace connection string in _SendSMS.WabAPI_ project's _Web.config_ with a connection string to your MySql Data Base.
  - Publish _SendSMS.WebAPI_.
- At IIS Manager
  - Create new Application Pool v4.0.
  - Add new Application on your site with this pool and path were project was published to.
