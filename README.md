# Go!Link ASP.NET

This is an ASP.NET web application that allows you to create and manage short URLs (aka Go! links) that redirect users to different destinations based on an ID provided. It also saves all visitors to a database table for further analysis.

![Screenshot](Documents/screenshot.png)


## Usage

To create a short URL, insert a long URL in the database table `golinks.links` with your preferred database client. You can also specify a custom ID for the short URL, such as `go.sv-foster.com/fwlink/?linkid=741`, by entering it manually, or just use autoincrement.

All visitors are stored in the table `golinks.requests`, grab the data from it for analysis.


## Building and Installing

To install the Go!Link application, you need to have the following prerequisites:
* Windows Server with IIS (Internet Information Services) enabled
* MySQL Server running
* Microsoft Visual Studio 2022 or Visual Web Developer Express


To install the application, follow these steps:
1. In the SQL Server, run the `init database.SQL` script to create the tables and user. Update databese name, user name and password with your own values if needed.
2. Open Microsoft Visual Studio or Microsoft Visual Web Developer Express and open the `Go!Links.sln` file.
3. In the `config.xml` file update the `MySQLConnectionParameters` string to point to the SQL Server and database you have just created. By default, named pipe `\\.\PIPE\MySQL` is used for the connection.
4. In the Solution Explorer, right-click on the go project and select Publish. Choose a publish profile, such as File System, and specify a target location, such as `C:\inetpub\wwwroot\go`. Click Publish to deploy the application files to the target location.
5. In the IIS Manager, create a new website or application for the Go!Link application. Set the physical path to the target location, such as `C:\inetpub\wwwroot\go`. Choose a port number and a host name for the website or application, such as `80` and `go.sv-foster.com`.

You have successfully installed the Go!Link application on your server!

To change the advanced settings of the Go!Link application edit the config.xml file in the target location, such as `C:\inetpub\wwwroot\go`. The `config.xml` file is an XML file that contains the configuration settings for the Go!Link ASP.NET web application. You can use any text editor to edit the file, but be careful not to introduce any syntax errors or invalid values.


## Authors

This program was written and is maintained by SV Foster.


## License

This program is available under EULA, see [EULA text file](EULA.txt) for the complete text of the license. This program is free for personal, educational and/or non-profit usage.
