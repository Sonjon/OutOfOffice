USE OutOfOffice
GO
CREATE TABLE [dbo].[Employee] (
    [ID]                    BIGINT         IDENTITY (1, 1) NOT NULL,
    [Full Name]             NVARCHAR (MAX) NOT NULL,
    [Subdivision]           NVARCHAR (MAX) NOT NULL,
    [Position]              NVARCHAR (MAX) NOT NULL,
    [Status]                VARCHAR (18)   NOT NULL,
    [People Partner]        BIGINT         NULL,
    [Out-of-Office Balance] INT            NOT NULL,
    [Project]               BIGINT         NULL,
    CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Employee__People_Partner_ID] FOREIGN KEY ([People Partner]) REFERENCES [dbo].[Employee] ([ID])
);

CREATE TABLE [dbo].[Leave Request] (
    [ID]             BIGINT        IDENTITY (1, 1) NOT NULL,
    [Employee]       BIGINT        NOT NULL,
    [Absence Reason] VARCHAR (MAX) NOT NULL,
    [Start Date]     DATETIME2 (7) NOT NULL,
    [End Date]       DATETIME2 (7) NOT NULL,
    [Comment]        VARCHAR (MAX) NULL,
    [Status]         VARCHAR (18)  NOT NULL,
    CONSTRAINT [PK_Leave_Request] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Leave_Request__Employee_ID] FOREIGN KEY ([Employee]) REFERENCES [dbo].[Employee] ([ID])
);

CREATE TABLE [dbo].[Approval Request] (
    [ID]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [Approver]      BIGINT        NULL,
    [Leave Request] BIGINT        NOT NULL,
    [Status]        VARCHAR (18)  NOT NULL,
    [Comment]       VARCHAR (MAX) NULL,
    CONSTRAINT [PK_Approval_Request] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Approval_Request__Leave_Request_ID] FOREIGN KEY ([Leave Request]) REFERENCES [dbo].[Leave Request] ([ID]),
    CONSTRAINT [FK_Approval_Request__Approver_ID] FOREIGN KEY ([Approver]) REFERENCES [dbo].[Employee] ([ID])
);

CREATE TABLE [dbo].[Project] (
    [ID]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [Project Type]    VARCHAR (18)  NOT NULL,
    [Start Date]      DATETIME2 (7) NOT NULL,
    [End Date]        DATETIME2 (7) NULL,
    [Project Manager] BIGINT        NOT NULL,
    [Comment]         VARCHAR (MAX) NULL,
    [Status]          VARCHAR (18)  NOT NULL,
    CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Project__Project_Manager_ID] FOREIGN KEY ([Project Manager]) REFERENCES [dbo].[Employee] ([ID])
);

CREATE TABLE [dbo].[Users] (
    [ID]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [EmployeeId] BIGINT        NOT NULL,
    [Username]   VARCHAR (MAX) NOT NULL,
    [Password]   VARCHAR (MAX) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Users_Employee_ID] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employee] ([ID])
);



