/****** Object:  Database [RealTimeChatApplication]    Script Date: 13-11-2024 07:27:15 ******/
CREATE DATABASE [RealTimeChatApplication]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RealTimeChatApplication', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.TRILOKSQL\MSSQL\DATA\RealTimeChatApplication.mdf' , SIZE = 51200KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RealTimeChatApplication_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.TRILOKSQL\MSSQL\DATA\RealTimeChatApplication_log.ldf' , SIZE = 51200KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [RealTimeChatApplication] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RealTimeChatApplication].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RealTimeChatApplication] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RealTimeChatApplication] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RealTimeChatApplication] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RealTimeChatApplication] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RealTimeChatApplication] SET ARITHABORT OFF 
GO
ALTER DATABASE [RealTimeChatApplication] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RealTimeChatApplication] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RealTimeChatApplication] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RealTimeChatApplication] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RealTimeChatApplication] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RealTimeChatApplication] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RealTimeChatApplication] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RealTimeChatApplication] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RealTimeChatApplication] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RealTimeChatApplication] SET  DISABLE_BROKER 
GO
ALTER DATABASE [RealTimeChatApplication] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RealTimeChatApplication] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RealTimeChatApplication] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RealTimeChatApplication] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RealTimeChatApplication] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RealTimeChatApplication] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RealTimeChatApplication] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RealTimeChatApplication] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [RealTimeChatApplication] SET  MULTI_USER 
GO
ALTER DATABASE [RealTimeChatApplication] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RealTimeChatApplication] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RealTimeChatApplication] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RealTimeChatApplication] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RealTimeChatApplication] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RealTimeChatApplication] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [RealTimeChatApplication] SET QUERY_STORE = OFF
GO
USE [RealTimeChatApplication]
GO
/****** Object:  Table [dbo].[ChatMessage]    Script Date: 13-11-2024 07:27:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChatMessage](
	[ChatMessageID] [int] IDENTITY(1,1) NOT NULL,
	[SenderID] [int] NOT NULL,
	[ReceiverID] [int] NOT NULL,
	[ChatMessage] [varchar](200) NOT NULL,
	[TimeStamp] [smalldatetime] NOT NULL,
	[Latitude] [varchar](30) NULL,
	[Longitude] [varchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[ChatMessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChatUser]    Script Date: 13-11-2024 07:27:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChatUser](
	[ChatUserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[FullName] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Password] [varchar](30) NOT NULL,
	[ProfilePictureURL] [varchar](100) NULL,
	[Status] [bit] NULL,
	[AuthenticationTime] [smalldatetime] NULL,
	[Gender] [varchar](10) NULL,
	[FCMToken] [varchar](350) NULL,
PRIMARY KEY CLUSTERED 
(
	[ChatUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SendNotificationMessage]    Script Date: 13-11-2024 07:27:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SendNotificationMessage](
	[NotificationID] [int] IDENTITY(1,1) NOT NULL,
	[SenderID] [int] NULL,
	[ReceiverID] [int] NULL,
	[MessageData] [varchar](200) NULL,
	[TimeStamp] [date] NULL,
	[IsNotificationSend] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[NotificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserConnectionList]    Script Date: 13-11-2024 07:27:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserConnectionList](
	[ConnectionID] [int] IDENTITY(1,1) NOT NULL,
	[RequestID] [int] NOT NULL,
	[AcceptID] [int] NOT NULL,
	[TIMESTAMP] [datetime] NOT NULL,
	[IsRequestAccepted] [bit] NOT NULL,
	[Accept] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ConnectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserLoginData]    Script Date: 13-11-2024 07:27:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLoginData](
	[UserLoginDataID] [int] IDENTITY(1,1) NOT NULL,
	[ChatUserID] [int] NOT NULL,
	[LastLoginTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserLoginDataID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserConnectionList] ADD  DEFAULT ((0)) FOR [Accept]
GO
/****** Object:  StoredProcedure [dbo].[usp_ChatMessage]    Script Date: 13-11-2024 07:27:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usp_ChatMessage]                            
( @Mode               INT,                            
  @UserName           VARCHAR = '',                          
  @ChatUserID         INT = '',              
  @RequestID          INT = 0,              
  @AcceptID           INT = 0,
  @SearchUserID       INT = 0,
  @SearchUserName     VARCHAR(50) =''
)                            
 AS                            
 BEGIN                            
 SET NOCOUNT ON;                            
  IF(@Mode=1)                            
  BEGIN    
		WITH RankedConnections AS (
    SELECT 
        u.UserName,         
        u.FullName,
        u.ProfilePictureURL,
        u.Gender,   
        u.ChatUserID,
        ucl.ConnectionID,
        ucl.RequestID,
        ucl.AcceptID,
        ucl.IsRequestAccepted, 
        ucl.Accept,
        ucl.TIMESTAMP,
        ROW_NUMBER() OVER (PARTITION BY u.ChatUserID ORDER BY ucl.TIMESTAMP DESC) AS RowNum
    FROM 
        dbo.ChatUser u
    LEFT JOIN 
        dbo.UserConnectionList ucl 
        ON (ucl.RequestID = @SearchUserID OR ucl.AcceptID = @SearchUserID)
        AND (ucl.RequestID = u.ChatUserID OR ucl.AcceptID = u.ChatUserID)
    WHERE 
        u.UserName LIKE '%' + @SearchUserName + '%'  
)
SELECT 
    UserName,         
    FullName,
    ProfilePictureURL,
    Gender,   
    ChatUserID,
    CASE 
        WHEN ConnectionID IS NOT NULL THEN RequestID  
        ELSE NULL 
    END AS RequesterID,  
    CASE 
        WHEN ConnectionID IS NOT NULL THEN AcceptID  
        ELSE NULL 
    END AS AccepterID,   
    CASE 
        WHEN ConnectionID IS NOT NULL THEN IsRequestAccepted 
        ELSE NULL
    END AS IsRequestAccepted, 
    CASE 
        WHEN ConnectionID IS NOT NULL THEN 
            CASE 
                WHEN IsRequestAccepted = 1 THEN 'Accepted'
                WHEN IsRequestAccepted = 0 AND Accept = 0 THEN 'Canceled'
                ELSE 'Pending'
            END
        ELSE NULL
    END AS RequestStatus, 
    CASE 
        WHEN ConnectionID IS NOT NULL THEN TIMESTAMP
        ELSE NULL
    END AS ConnectionTimestamp 
FROM RankedConnections
WHERE RowNum = 1 
ORDER BY UserName;
		

    END
  IF(@Mode=2)                          
  BEGIN                          
         Select UserName,ProfilePictureURL,Email,ChatUserID,Gender from ChatUser Where ChatUserID = @ChatUserID                          
  END                
  --To check the user that this User is Connected or not.              
  IF(@Mode=3)              
  BEGIN              
     Select * from UserConnectionList Where (RequestID = @RequestID AND AcceptID = @AcceptID) or (RequestID = @AcceptID AND AcceptID = @RequestID) AND IsRequestAccepted = 1              
  END              
            
  --TO Get the Last Login Time of the User            
  IF(@Mode=4)            
    BEGIN            
   Select Top(1) LastLoginTime from UserLoginData Where  ChatUserID = @ChatUserID Order by 1 desc            
   END            
     --To check is Connected or Not;      
   IF(@Mode=5)    
    BEGIN    
   Select * from UserConnectionList where (RequestID = @ChatUserID AND AcceptID =@AcceptID) OR( RequestID = @AcceptID AND AcceptID =@ChatUserID)    
    END    
  
 --To get the UserInfo DATA  here  
 IF(@Mode=6)  
 BEGIN  
   SELECT ChatUserID,FullName,UserName,Email,Gender FROM ChatUser where ChatUserID = @ChatUserID  
 END  
END     
    
GO
/****** Object:  StoredProcedure [dbo].[usp_ChatUser]    Script Date: 13-11-2024 07:27:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_ChatUser]                  
(                  
    @Mode                INT,                  
    @UserName            VARCHAR(30) ='',                  
    @Password            VARCHAR(20)='',                 
    @Email               VARCHAR(50) ='',                
    @Gender              VARCHAR(10) ='',                
    @ProfilePictureURL   VARCHAR(100) NULL = '',      
    @FCMToken    VARCHAR(350) = '',      
    @FullName            VARCHAR(50) ='',       
	@ChatUserUpdate           INT = 0,
    @ChatUser            INT OUTPUT  -- Changed to OUTPUT parameter            
)                  
AS                   
BEGIN                   
    SET NOCOUNT ON;                  
            
    DECLARE @EmailMessage VARCHAR(150);              
    DECLARE @UserMessage VARCHAR(150);              
            
    -- Insert                  
    IF (@Mode = 1)                  
    BEGIN                
        -- Check the Email availability              
        IF EXISTS (SELECT 1 FROM ChatUser WHERE Email = @Email)              
        BEGIN              
            SET @EmailMessage = 'Email ID already exists. Please use a new email to register or login with the same email.';              
            RAISERROR(@EmailMessage, 16, 1);              
            RETURN;              
        END              
              
        -- Check the UserName availability              
        IF EXISTS (SELECT 1 FROM ChatUser WHERE UserName = @UserName)              
        BEGIN              
            SET @UserMessage = 'Username already exists. Please use a new username to register or login with the same username.';              
            RAISERROR(@UserMessage, 16, 1);              
            RETURN;              
        END              
              
        -- Insert the Data for Creating New User              
        INSERT INTO ChatUser (UserName, Email, Password, AuthenticationTime, Gender, ProfilePictureURL, FullName, FCMToken)                  
        VALUES (@UserName, @Email, @Password, GETDATE(), @Gender, @ProfilePictureURL, @FullName , @FCMToken);                 
              
        -- Return the Last Id Inserted Here              
        SET @ChatUser = SCOPE_IDENTITY();              
    END          
 --For Login          
 IF (@Mode = 2)           
 BEGIN          
      SELECT * FROM ChatUser WHERE UserName=@UserName AND Password = @Password          
    
   Declare @ChatUserID INT;    
   SET @ChatUserID = ( SELECT ChatUserID FROM ChatUser WHERE UserName=@UserName AND Password = @Password )    
    
 --Insert the Login time when User is Login Every Time    
   IF(@ChatUserID<>'')    
   BEGIN    
    INSERT INTO UserLoginData(ChatUserID, LastLoginTime)    
    Values(@ChatUserID,GETDATE())    
   END    
 END   
 
 --For Update
   IF(@Mode=3)
   BEGIN
     SELECT ChatUserID,UserName,FullName,Email,ProfilePictureURL,Gender FROM ChatUser WHERE ChatUserID = @ChatUserUpdate
   END
END 
GO
/****** Object:  StoredProcedure [dbo].[usp_ConnectionRequest]    Script Date: 13-11-2024 07:27:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_ConnectionRequest]      
(      
    @Mode          INT,       
    @UserID        INT=0,    
 @AcceptID      INT=0,    
 @RequestID      INT=0    
     
)      
AS       
BEGIN      
    SET NOCOUNT ON;      
    IF (@Mode = 1)      
    BEGIN      
        -- Retrieve pending connection requests      
        SELECT       
            CS.ChatUserID,       
            CS.FullName,       
            CS.ProfilePictureURL      
        FROM       
            UserConnectionList UCL      
        INNER JOIN       
            ChatUser CS ON UCL.RequestID = CS.ChatUserID       
        WHERE       
            UCL.IsRequestAccepted = 0       
            AND UCL.AcceptID = @UserID;      
    END      
    ELSE IF (@Mode = 2)      
    BEGIN      
        -- Retrieve accepted connection requests 
          Select CS.ChatUserID,CS.FullName,CS.ProfilePictureURL, CS.Gender,CS.UserName from UserConnectionList UCL 
          INNER Join ChatUser CS on (CS.ChatUserID = UCL.AcceptID OR CS.ChatUserID = UCL.RequestID)
          where (RequestID = @UserID or AcceptID = @UserID) AND IsRequestAccepted = 1 AND cs.ChatUserID <> @UserID
    END      
 --To Accept the Request    
    ELSE IF(@Mode=3)    
 BEGIN    
  Update UserConnectionList SET IsRequestAccepted=1 Where RequestID =@RequestID AND AcceptID = @AcceptID     
     
 END    
 --To Reject the Request    
 ELSE IF(@Mode=4)    
 BEGIN    
   DELETE FROM UserConnectionList WHERE RequestID =@RequestID AND AcceptID = @AcceptID     
 END    
 ELSE    
    BEGIN      
        -- Optional: handle invalid mode      
        RAISERROR('Invalid mode specified. Please use 1 for pending requests or 2 for accepted requests.', 16, 1);      
    END      
END 
GO
/****** Object:  StoredProcedure [dbo].[usp_MessageRecord]    Script Date: 13-11-2024 07:27:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_MessageRecord]                  
(                  
    @Mode             INT = 0,                  
    @SenderID         INT = 0,                  
    @ReceiverID       INT = 0,                  
    @Message          VARCHAR(100) = '',                  
    @ChatMessageID    INT = 0        
         
)                  
AS                  
BEGIN                  
    SET NOCOUNT ON;                  
    DECLARE @LoginID INT;              
              
    -- Insert a new chat message                  
    IF (@Mode = 1)                  
    BEGIN                  
        INSERT INTO ChatMessage (SenderID, ReceiverID, ChatMessage, TimeStamp)                  
        VALUES (@SenderID, @ReceiverID, @Message, GETDATE());                  
    END                  
                  
    -- Chat History List                  
    IF (@Mode = 2)                  
    BEGIN              
                     
           SELECT DISTINCT ChatUserID          
           INTO #TempChatUsers          
           FROM ChatUser          
           WHERE ChatUserId IN (          
               -- Get distinct ReceiverIDs where the logged-in user is the Sender          
               SELECT DISTINCT ReceiverID FROM ChatMessage WHERE SenderID = @ChatMessageID          
                         
               UNION          
                         
               -- Get distinct SenderIDs where the logged-in user is the Receiver          
               SELECT DISTINCT SenderID FROM ChatMessage WHERE ReceiverID = @ChatMessageID          
                );          
                     
             -- Now select user details for those distinct user IDs          
                SELECT UserName, FullName, ProfilePictureURL, ChatUserID,Gender FROM ChatUser          
                WHERE ChatUserID IN (SELECT ChatUserID FROM #TempChatUsers);          
                     
              -- Clean up the temporary table          
                 DROP TABLE #TempChatUsers;              
    END                  
                  
    -- Chat Record between the Users                  
    IF (@Mode = 3)                  
    BEGIN                  
        IF (@SenderID = @ReceiverID)                  
        BEGIN                  
            SELECT *                   
            FROM ChatMessage                   
            WHERE                   
                (SenderID = @SenderID AND ReceiverID = @ReceiverID);                  
        END                  
                  
        IF (@SenderID <> @ReceiverID)                  
        BEGIN                  
            SELECT *                   
            FROM ChatMessage                   
            WHERE                   
                (SenderID = @SenderID OR ReceiverID = @SenderID) AND                   
                (ReceiverID = @ReceiverID OR SenderID = @ReceiverID);                  
        END                  
    END         
 IF (@Mode=4)        
  BEGIN        
     INSERT INTO UserConnectionList (RequestID, AcceptID, TIMESTAMP, IsRequestAccepted)        
     values(@SenderID, @ReceiverID,GETDATE(),0)        
         
     Select FCMToken from ChatUser where ChatUserID = @ReceiverID      
  END     
 IF (@Mode=5)  
  BEGIN  
  DELETE FROM UserConnectionList WHERE RequestID=@SenderID AND AcceptID = @ReceiverID AND IsRequestAccepted =0  
  END  
END 
GO
/****** Object:  StoredProcedure [dbo].[usp_NotificationRecord]    Script Date: 13-11-2024 07:27:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE Procedure [dbo].[usp_NotificationRecord]
(
  @Mode						INT,
  @SenderID					INT,
  @ReceiverID				INT,
  @MessageData				VARCHAR(200),
  @IsNotificationSend       BIT
)
AS 
BEGIN
SET NOCOUNT ON;

		IF(@Mode=1)
		BEGIN
			Insert into SendNotificationMessage(SenderID,ReceiverID,MessageData,TimeStamp,IsNotificationSend)
			Values (@SenderID,@ReceiverID,@MessageData, GETDATE(), @IsNotificationSend)
		END

END
GO
USE [master]
GO
ALTER DATABASE [RealTimeChatApplication] SET  READ_WRITE 
GO
