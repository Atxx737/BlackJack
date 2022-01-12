USE [master]
GO
/****** Object:  Database [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF]    Script Date: 28/6/2021 6:08:33 AM ******/
CREATE DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Blackjack', FILENAME = N'C:\Users\Admin\Documents\Blackjack.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Blackjack_log', FILENAME = N'C:\Users\Admin\Documents\Blackjack_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET ARITHABORT OFF 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET AUTO_SHRINK ON 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET  ENABLE_BROKER 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET  MULTI_USER 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET DB_CHAINING OFF 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET QUERY_STORE = OFF
GO
USE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF]
GO
/****** Object:  Table [dbo].[Player]    Script Date: 28/6/2021 6:08:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Player](
	[ID] [nvarchar](5) NOT NULL,
	[Playername] [nvarchar](100) NOT NULL,
	[Username] [nvarchar](30) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Money] [int] NOT NULL,
	[VIP] [int] NULL,
	[RoomID] [int] NULL,
 CONSTRAINT [PK__Player__3214EC27FD374E08] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Room]    Script Date: 28/6/2021 6:08:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Room](
	[RoomID] [varchar](50) NOT NULL,
	[MinBet] [int] NULL,
	[IPAddress] [varchar](50) NULL,
	[Port] [int] NULL,
 CONSTRAINT [PK__RoomID__32863919B6980B4D] PRIMARY KEY CLUSTERED 
(
	[RoomID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sp_Player_TusinhID]    Script Date: 28/6/2021 6:08:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sp_Player_TusinhID]
@result nvarchar(5) OUT
as
begin
declare @ma_next nvarchar(5)
declare @max int
--Khởi tạo 2 biến @ma_next là mã tự sinh và max là tổng số bản ghi hiện có
select @max = COUNT(ID) + 1 from Player where ID like '#'
select @ma_next = '#' + RIGHT('000' + CAST(@max as nvarchar(4)),4)
while(exists(select ID from Player where ID = @ma_next))
begin
	set @max = @max + 1
	set @ma_next = '#' + RIGHT('000' + CAST(@max as nvarchar(4)),4)
end
set @result = @ma_next
end
GO
USE [master]
GO
ALTER DATABASE [C:\USERS\ADMIN\DOCUMENTS\BLACKJACK.MDF] SET  READ_WRITE 
GO
