# Unzip the Project Files
## All project files will be unpacked into the selected folder as a result.

# Open the Project in Visual Studio 2022 (.NET 8)
## Find the.sln (solution) file in the root of the project folder by navigating to the folder where the project was unpacked.

# Set Up the LocalDB
## You must ensure that LocalDB is installed because your project uses it. Usually, Visual Studio comes with it installed by default, but if not, you can use the Visual Studio Installer to install it.
## Copy exactly this code:
## Add-Migration InitialMigration
## Update-Database


# Configure the Connection String (if necessary)
## Look for the connection string in the project's appsettings.json file. If it seems like this

# Run Database Migrations
## You must apply any pending Entity Framework Core migrations in order to construct the database schema locally before launching the application. They ought to do these things. Navigate to Tools > NuGet Package Manager > Package Manager Console in Visual Studio to launch the Package Manager Console.

# Run the Project
## You can start the project after the database has been moved. Here's how:
## To begin debugging and executing the project, press F5 or select the green play button at the top.

# References 
## M3 Programming. (2023, June 26). C# Programming - CRUD with Local Database (MDF) [Video]. YouTube. https://www.youtube.com/watch?v=YtTVmiamdpc
## Fox Learn. (2019, July 24). C# Tutorial - How to Connect and Use Local Database in Visual Studio 2019 | FoxLearn [Video]. YouTube. https://www.youtube.com/watch?v=mgtfxtjKoaA
## Simplilearn. (2022, June 24). C# MVC CRUD Tutorial | Asp.net MVC Full CRUD Operation Using Entity Framework | Simplilearn [Video]. YouTube. https://www.youtube.com/watch?v=xFeDCD4si3U
## Web Dev Simplified. (2019, April 3). MVC Explained in 4 Minutes [Video]. YouTube. https://www.youtube.com/watch?v=DUg2SWWK18I
## NetSecProf. (2021, October 10). ASP.NET Core MVC - Intro to Controllers and Views [Video]. YouTube. https://www.youtube.com/watch?v=qagdAYZfD04
## Daniel Wood. (2020, March 26). HTML & CSS 2020 Tutorial 10 - Styling your website with external stylesheets (CSS) [Video]. YouTube. https://www.youtube.com/watch?v=WuvujA5facU
