# Bitcoin Price API

Bitcoin Price API is a web platform built with ASP.NET Core Web API that fetches Bitcoin prices from multiple sources and presents them to the users. It allows users to retrieve Bitcoin prices for specific sources and currencies, as well as view the history of Bitcoin prices stored in the database.

## Installation

### Prerequisites
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/mahdi751/Nova4Task.git
   

Additional Information
This project uses Entity Framework Core for data access and SQL Server as the database.
Caching is implemented using ASP.NET Core's in-memory cache.
For production use, consider replacing in-memory cache with a distributed cache provider.
