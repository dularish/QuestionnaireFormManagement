# Questionnaire Form Management

Explored a simple form management problem with the following technology stack

1. Blazor WebAssembly Frontend framework
2. .NET CORE WebAPI
3. Entity Framework Core ORM for datastore
4. ASP.NET core identity for authentication management
5. MS-SQL database


## Setup Instructions :

1. Clone
2. Update the FormsWeb.Server project with the following settings included
      ```json
      {
        "SuperAdminPassword": <YourSuperAdminPassword>,
        "EmailSenderId": <YourEmailSenderAddressForSendingAuthenticationEmails>,
        "EmailSenderPassword": <YourEmailSenderAddressPassword>
      }
      ```
      
3. Run the "FormsWeb.Server" project
