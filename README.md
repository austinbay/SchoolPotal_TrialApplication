# SchoolPotal_TrialApplication
Trail Application



The Trail Project Documentation
•	The Project Name: SchoolPortal
•	Development Tools: Visual Studio 2019, ASP.Net Core 3.0 and Angular 8,
•	Entity Framework Core (Code First from Database Approach) 
•	Database: SQL Server 2017

Running the Project
•	Clone the source code from GitHub: https://github.com/austinbay/SchoolPotal_TrialApplication.git
•	Open the solution in Visual Studio 2019
•	The solution contains the following 7 projects
o	SchoolPortal.Data
o	SchoolPortal.Models
o	SchoolPortal.Logic
o	SchoolPortal.WebApi
o	SchoolPortal.AngularClient
o	GeneralHelper.Lib
o	MessageManager.Lib

•	Make the following changes according to your settings
o	Open appsettings.json file in SchoolPortal.WebApi and update the ConnectionString, ApiServerUrl and AngularAppUrl  
o	In SchoolPortal.AngularClient project, open environment.ts file in the ClientApp/src/environments folder, and update basePath settings.
•	Run the solution, ensure the WebApi and AngularClient projects are running
•	The Database will be created with a default login details                            (email=“admin@gmail.com” password=”Admin@123”)
•	Create an Account or login using the above details

