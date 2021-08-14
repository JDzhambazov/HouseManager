# HouseManager
Condominium management platform (bg version only for now). Designed for professional house managers with many sites and individual clients. Allows easy tracking of obligations, income, expenses, occupants of the property, notification by email for payment, etc. Automatic addition of liabilities for the current month when changing the month. Automatic recalculation of liabilities when changing the number of inhabitants or the amount of monthly fees from the date of the changes. 

# How to Install
Download or clone the repository. In appsettings.json set a value for DefaultConnection and SendGrid ApiKey .Start the project.Done 

# How to use
Register user
Register address data
Enter the monthly fees set for the address
Enter a description for each property in the address
Add occupants (users) for each property
ATTENTION If you add an occupant (user) who is already registered in the platform, the data about him will not be changed and will remain as they were when he first registered. 

# Back End
ASP.NET Core 5.0
Entity Framework Core
MS SQL Server

# Front End
Bootstrap
HTML & CSS

# Email Sender
SendGrid

# Template Autors
Nikolay Kostov
Vladislav Karamfilov

# Test Library
Ivaelo Kenov
