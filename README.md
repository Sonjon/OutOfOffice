# OutOfOffice

## Create application
  1.Open solution in Visual Studio
  2.Go to **Tools > NuGet Package Manager >Package Manager Console**
  3. Type : _**dotnet publish -c Release**_
  4. After that close Visual Studio.

## Prepare database
  1. Go to _**OutOfOffice\Components\scripts**_
  2. Run script _**InitDatabase.sql**_
  3. Run script _**createDatabase.sql**_
  4. Run script _**InitData.sql**_


## Run application
  1. Go to _**OutOfOffice\bin\Release\net8.0\publish**_
  2. Start _**OutOfOffice.exe**_
  3. Open browser and type address from new opened consol. It should be _**http://localhost:5000**_
  4. Log in to appliaction:

      ◦ **Administrator**:
     
          ▪ Login: Admin
          ▪ Password: Admin
     
      ◦ **HR Manager**:
     
          ▪ Login: HR
          ▪ Password: HR
     
      ◦ **Project Manager**:
     
          ▪ Login: PR
          ▪ Password: PR
      ◦ **Employee**:
     
          ▪ Login: EMP
          ▪ Password: EMP
  5. Click _**Lists**_ on right side menu to see options
  6. Options:
     
        ◦ _**Employees**_ – Open employees list. You can managed employees there.
     
        ◦ _**Leave Requests**_ – Open leave requests list. You can managed leave requests there.
     
        ◦ _**Approval Requests**_ – Open approval requests list. You can managed leave requests there.
     
        ◦ _**Project**_ – Open projects list. You can managed projects there.

              
