# Unzip the Project Files
## All project files will be unpacked into the selected folder as a result.

# Open the Project in Visual Studio 2022 (.NET 8)
## Find the.sln (solution) file in the root of the project folder by navigating to the folder where the project was unpacked.

# Set Up the LocalDB
## You must ensure that LocalDB is installed because your project uses it. Usually, Visual Studio comes with it installed by default, but if not, you can use the Visual Studio Installer to install it.

# Configure the Connection String (if necessary)
## Look for the connection string in the project's appsettings.json file. If it seems like this

# Run Database Migrations
## You must apply any pending Entity Framework Core migrations in order to construct the database schema locally before launching the application. They ought to do these things. Navigate to Tools > NuGet Package Manager > Package Manager Console in Visual Studio to launch the Package Manager Console.
