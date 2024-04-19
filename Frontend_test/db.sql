SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Message](
	[MessageId] [int] IDENTITY(1,1) NOT NULL,
	[Sender] [int] NULL,
	[Receiver] [int] NULL,
	[Message] [nvarchar](max) NULL,
	[GUID] [nvarchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime2](7) NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_NewTable] PRIMARY KEY CLUSTERED 
(
	[MessageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Message] ADD  CONSTRAINT [DEFAULT_NewTable_GUID]  DEFAULT (newid()) FOR [GUID]
GO

CREATE TABLE [dbo].[Users] (
    [UserId]    BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserName]  NVARCHAR (100) NULL,
    [UserEmail] NVARCHAR (100) NULL,
    [Password]  NVARCHAR (150) NULL,
    [Address]   NVARCHAR (100) NULL,
    [Phone]     NVARCHAR (300) NULL,
    [GUID]      NVARCHAR (MAX) CONSTRAINT [DF_User_GUID] DEFAULT (newid()) NULL,
    [CreatedAt] DATETIME       CONSTRAINT [DF_User_CreatedAt] DEFAULT (getdate()) NULL,
    [UpdatedAt] DATETIME       NULL,
    [Role]      NVARCHAR (50)  NULL,
    [Active]    BIT            NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[User_Login]
@UserEmail NVARCHAR(200),
@Password NVARCHAR(max)
AS
BEGIN
    SELECT UserId,UserName from Users WHERE UserEmail=@UserEmail and [Password] = @Password
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[Usp_IU_Message] 
    @Message nvarchar(250),
	@Sender int,
	@Receiver int
    
    As
    begin transaction
BEGIN
    DECLARE @UserId nvarchar(250)
    set @UserId = (select isnull(max(UserId),0)+1 FROM Users)
    BEGIN
        INSERT INTO [Message] 
        (
            Message,
            Sender,
            Receiver,
            CreatedDate,
            Active,
            CreatedBy
        ) 
        VALUES 
        (
         @Message,
         @Sender,
         @Receiver,
            getdate(),
            1,
            @UserId
        );
    END
END
    IF @@ERROR > 0 
    BEGIN  
        ROLLBACK TRANSACTION  
        select '404' as status, 'Error' as message
    END  
    ELSE  
    BEGIN  
        COMMIT TRANSACTION  
        select '200' as status ,'Success' as message
    END 
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Usp_IU_Users] 
@UserName nvarchar(250),
	@UserEmail nvarchar(250),
	@Password nvarchar(250),
	@Address nvarchar(250),
	@Phone nvarchar(250),
	@Role nvarchar(250),
	@GUID nvarchar(250)
    
    As
    begin transaction
BEGIN
    DECLARE @UserId nvarchar(250)
    set @UserId = (select isnull(max(UserId),0)+1 FROM Users)
    IF EXISTS(SELECT 1 FROM Users WHERE GUID = @GUID)
    BEGIN
        UPDATE Users SET 
        UserName = @UserName,
		UserEmail = @UserEmail,
		Password = @Password,
		Address = @Address,
		Phone = @Phone,
		Role = @Role,
        UpdatedAt = GETDATE()
        WHERE GUID = @GUID;
    END
    ELSE
    BEGIN
        INSERT INTO Users 
        (
            UserName,
			UserEmail,
			Password,
			Address,
			Phone,
			Role,
            CreatedAt,
            Active,
            UserId
        ) 
        VALUES 
        (
            @UserName,
			@UserEmail,
			@Password,
			@Address,
			@Phone,
			@Role,
            getdate(),
            1,
            @UserId
        );
    END
END
    IF @@ERROR > 0 
    BEGIN  
        ROLLBACK TRANSACTION  
        select '404' as status, 'Error' as message,@UserId as UserId
    END  
    ELSE  
    BEGIN  
        COMMIT TRANSACTION  
        select '200' as status ,'Success' as message,@UserId as UserId
    END 
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Usp_S_MessageById]  --[dbo].[Usp_S_MessageById] 1,2
    @receiverId int,
    @senderId int
    AS
BEGIN
    SELECT 
    Sender,
Receiver,
Message,
(select case
when Sender=@SenderId then 'sender'
else 'receiver'
end) as Status
    FROM Message 
    WHERE Active = 1 and Sender in (@senderId,@receiverId) and Receiver in(@senderId,@receiverId)
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Usp_S_UsersById]
    @UserId nvarchar(200),
    @GUID nvarchar(250)
    AS
BEGIN
    SELECT UserName,
	UserEmail,
	Password,
	Address,
	Phone,
	Role,
	GUID
    FROM Users 
    WHERE GUID = @GUID 
    AND Active = 1;
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Usp_S_UsersList] 
    @UserId nvarchar(200)
    AS
BEGIN
    SELECT 
    UserId,
    UserName,
	UserEmail,
	Address,
	Phone,
	Role,
	GUID
    FROM Users 
    WHERE Active = 1 and UserId <> @UserId;
END
GO
