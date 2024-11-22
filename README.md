Local setup guide: 
1) Run atm-create-db script on your SSMS.
2) Open solution, and place your server name in the SqlServerConnection string at the App.Settings file.
3) Run the Update-Database command from the Nuget Package Console Manager on the API.Infrastructure project.
4) Run atm-testdata script.

Docker support in the next iteration. 
