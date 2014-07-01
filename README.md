Scarf
=====
Scarf is security audit, access and action logging framework for .NET

Many times software come with non functional requirements to track access to resources, to audit events or to simply keep track of who did what in the system. While you can use traditional logging frameworks to save this information, Scarf was specifically designed to log this type of information and to make it effortless and natural to use.

With scarf, you can log audit events, access events and actions performed by the user. Scarfs should work with any type of .NET application but it was mainly intended for MVC and WebApi applications that need to keep track of such events. The integration modules allow you to add attributes to your MVC and WebApi action methods to initiate logging.

Logs are saved in a database or in files (or you can create custom data sources if you want). They contain a lot of information beyond what has happened: HTTP Server headers, query strings variables, POST field values, cookies and even MVC model state values.