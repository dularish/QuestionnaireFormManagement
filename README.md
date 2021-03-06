# Questionnaire Form Management

Explored a simple form management problem with the following technology stack

1. Blazor WebAssembly Frontend framework
2. .NET CORE WebAPI
3. Entity Framework Core ORM for datastore
4. ASP.NET core identity for authentication management
5. MS-SQL database

## Demo
  ![Demo Gif](DemoGifs/BlazorQuestionnaireAppDemo1_1in8_ReducedImageSize.gif)

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
      The SuperAdmin Account Email is fixed as "admin@gmail.com".
      
3. Update the FormsWeb.Server project appSettings.json for the connectionstring properties "AuthConnection", "QuestionnaireConnection"
4. Run the following commonds in the powershell/cmd :
      ```powershell
      dotnet ef database update --context "AuthDbContext"
      dotnet ef database update --context "QuestionnaireDbContext"
      ```
5. Run the "FormsWeb.Server" project
