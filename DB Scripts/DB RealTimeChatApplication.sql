USE [master]
GO
/****** Object:  Database [RealTimeChatApplication]    Script Date: 06-10-2024 21:06:50 ******/
CREATE DATABASE [RealTimeChatApplication]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RealTimeChatApplication', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.CYNOSUREDBS\MSSQL\DATA\RealTimeChatApplication.mdf' , SIZE = 51200KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RealTimeChatApplication_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.CYNOSUREDBS\MSSQL\DATA\RealTimeChatApplication_log.ldf' , SIZE = 51200KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
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
/****** Object:  Table [dbo].[ChatMessage]    Script Date: 06-10-2024 21:06:51 ******/
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
PRIMARY KEY CLUSTERED 
(
	[ChatMessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChatUser]    Script Date: 06-10-2024 21:06:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChatUser](
	[ChatUser] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Password] [varchar](30) NOT NULL,
	[ProfilePictureURL] [varchar](100) NULL,
	[Status] [bit] NULL,
	[AuthenticationTime] [smalldatetime] NULL,
	[Gender] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[ChatUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[usp_ChatUser]    Script Date: 06-10-2024 21:06:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_ChatUser]      
(      
    @Mode                INT,      
    @UserName            VARCHAR(30),      
    @Password            VARCHAR(20),      
    @Email               VARCHAR(50),    
    @Gender              VARCHAR(10),    
    @ProfilePictureURL   VARCHAR(100) NULL = '',  
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
        INSERT INTO ChatUser (UserName, Email, Password, AuthenticationTime, Gender, ProfilePictureURL)      
        VALUES (@UserName, @Email, @Password, GETDATE(), @Gender, @ProfilePictureURL);     
  
        -- Return the Last Id Inserted Here  
        SET @ChatUser = SCOPE_IDENTITY();  
    END      
END 
GO
USE [master]
GO
ALTER DATABASE [RealTimeChatApplication] SET  READ_WRITE 
GO
