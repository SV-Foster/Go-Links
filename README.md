# Go!Link ASP.NET

This is an ASP.NET web application that allows you to create and manage short URLs that redirect users to different destinations based on an ID provided. It also stores data in a MySQL database and saves all visitors to a database table for further analysis.


## Usage

To use the Go!Link application, you need to create and use short URLs, view and analyze statistics. You can also perform other tasks, such as editing, deleting, or disabling short URLs, exporting or importing settings, and backing up or restoring the database.

To create a short URL, you need to enter a long URL in the database table. You can also specify a custom ID for the short URL, such as go.sv-foster.com/fwlink/?linkid=741, by entering it.


## Building and Installation

To install the Go!Link application, you need to have the following prerequisites:
* Windows Server with IIS (Internet Information Services) enabled
* MySQL Server
* Visual Studio 2022 or Visual Web Developer Express


To install the application, follow these steps:
1. Extract the zip file to a folder on your server, such as C:\inetpub\wwwroot\go.
2. Open Visual Studio or Visual Web Developer Express and open the go.sln file in the folder.
3. In the Solution Explorer, right-click on the go project and select Publish. Choose a publish profile, such as File System, and specify a target location, such as C:\inetpub\wwwroot\go. Click Publish to deploy the application files to the target location.
4. In the IIS Manager, create a new website or application for the Go!Link application. Set the physical path to the target location, such as C:\inetpub\wwwroot\go. Choose a port number and a host name for the website or application, such as 80 and go.sv-foster.com.
5. In the MySQL Server, create a new database for the Go!Link application, such as goDB. Run the "init database.SQL" script in the folder to create the tables and stored procedures for the database.
6. In the config.xml file in the target location, update the connection string to point to the database you created.
7. Restart the IIS and the SQL Server services.

You have successfully installed the Go!Link application on your server!

To change the advanced settings of the Go!Link application, you need to edit the config.xml file in the target location, such as C:\inetpub\wwwroot\go. The config.xml file is an XML file that contains the configuration settings for the ASP.NET web application. You can use any text editor to edit the file, but you need to be careful not to introduce any syntax errors or invalid values.


## Authors

This program was written and is maintained by SV Foster.


## License

This program is available under EULA, see [EULA file](EULA.txt) for the complete text of the license. This program is free for personal, educational and/or non-profit usage.
