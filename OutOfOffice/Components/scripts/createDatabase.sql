USE OutOfOffice
GO
CREATE TABLE [dbo].[Employee] (
    [ID]                    BIGINT        IDENTITY (1, 1) NOT NULL,
    [Full Name]             VARCHAR (MAX)        NOT NULL,
    [Subdivision]           VARCHAR (MAX)        NOT NULL,
    [Position]              VARCHAR (MAX)        NOT NULL,
    [Status]                VARCHAR (18)  NOT NULL,
    [People Partner]        BIGINT        NULL,
    [Out-of-Office Balance] INTEGER  NOT NULL,
    CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Employee__People_Partner_ID] FOREIGN KEY ([People Partner]) REFERENCES [dbo].[Employee] ([ID])
);

CREATE TABLE [dbo].[Leave Request] (
    [ID]                BIGINT        IDENTITY (1, 1) NOT NULL,
    [Employee]          BIGINT        NOT NULL,
    [Absence Reason]    VARCHAR        NOT NULL,
    [Start Date]        DATETIME2 (7) NOT NULL,
    [End Date]          DATETIME2 (7) NOT NULL,
    [Comment]           VARCHAR (MAX) NULL,
    [Status]            VARCHAR (18)  NOT NULL,
    CONSTRAINT [PK_Leave_Request] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Leave_Request__Employee_ID] FOREIGN KEY ([Employee]) REFERENCES [dbo].[Employee] ([ID])
);

CREATE TABLE [dbo].[Approval Request] (
    [ID]                BIGINT        IDENTITY (1, 1) NOT NULL,
    [Approver]          BIGINT        NOT NULL,
    [Leave Request]     BIGINT        NOT NULL,
    [Status]            VARCHAR (18)  NOT NULL,
    [Comment]           VARCHAR (MAX) NULL,
    CONSTRAINT [PK_Approval_Request] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Approval_Request__Approver_ID] FOREIGN KEY ([Approver]) REFERENCES [dbo].[Employee] ([ID]),
    CONSTRAINT [FK_Approval_Request__Leave_Request_ID] FOREIGN KEY ([Leave Request]) REFERENCES [dbo].[Leave Request] ([ID])
);

CREATE TABLE [dbo].[Project] (
    [ID]                BIGINT        IDENTITY (1, 1) NOT NULL,
    [Project Type]      VARCHAR (18)  NOT NULL,
    [Start Date]        DATETIME2 (7) NOT NULL,
    [End Date]          DATETIME2 (7) NULL,
    [Project Manager]   BIGINT        NOT NULL,
    [Comment]           VARCHAR (MAX) NULL,
    [Status]            VARCHAR (18)  NOT NULL,
    CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Project__Project_Manager_ID] FOREIGN KEY ([Project Manager]) REFERENCES [dbo].[Employee] ([ID])
);

