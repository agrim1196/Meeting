USE [master]
GO
/****** Object:  Database [Enterprise]    Script Date: 08-03-2024 23:43:04 ******/
CREATE DATABASE [Enterprise]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Enterprise', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Enterprise.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Enterprise_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Enterprise_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Enterprise] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Enterprise].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Enterprise] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Enterprise] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Enterprise] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Enterprise] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Enterprise] SET ARITHABORT OFF 
GO
ALTER DATABASE [Enterprise] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Enterprise] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Enterprise] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Enterprise] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Enterprise] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Enterprise] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Enterprise] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Enterprise] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Enterprise] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Enterprise] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Enterprise] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Enterprise] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Enterprise] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Enterprise] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Enterprise] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Enterprise] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Enterprise] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Enterprise] SET RECOVERY FULL 
GO
ALTER DATABASE [Enterprise] SET  MULTI_USER 
GO
ALTER DATABASE [Enterprise] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Enterprise] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Enterprise] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Enterprise] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Enterprise] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Enterprise] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Enterprise', N'ON'
GO
ALTER DATABASE [Enterprise] SET QUERY_STORE = ON
GO
ALTER DATABASE [Enterprise] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Enterprise]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 08-03-2024 23:43:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[ename] [nvarchar](max) NOT NULL,
	[eid] [int] NOT NULL,
	[client] [nvarchar](max) NULL,
	[bu] [nvarchar](max) NULL,
	[password] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MeetingRooms]    Script Date: 08-03-2024 23:43:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MeetingRooms](
	[id] [int] NOT NULL,
	[roomno] [int] NOT NULL,
	[capacity] [int] NOT NULL,
	[isOccupied] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MeetingsPlanned]    Script Date: 08-03-2024 23:43:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MeetingsPlanned](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoomNo] [int] NOT NULL,
	[NoOfParticipants] [int] NULL,
	[MeetingScheduledOn] [datetime] NULL,
	[MeetingScheduledFor] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 08-03-2024 23:43:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[uid] [numeric](18, 0) NOT NULL,
	[user_email] [nvarchar](max) NOT NULL,
	[user_password] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Employees] ([ename], [eid], [client], [bu], [password]) VALUES (N'Agrim', 1, N'Tesco', N'Digital Media', N'$2a$04$4WPGAbMyhYIA2t7Dm6k6Iu9znkP4wYu2JkDoXgi6gO57kuxteoqNS')
INSERT [dbo].[Employees] ([ename], [eid], [client], [bu], [password]) VALUES (N'Priya', 2, N'Kone', N'MFSI', N'$2a$04$4WPGAbMyhYIA2t7Dm6k6Iu9znkP4wYu2JkDoXgi6gO57kuxteoqNS')
GO
INSERT [dbo].[MeetingRooms] ([id], [roomno], [capacity], [isOccupied]) VALUES (1, 1, 10, 1)
INSERT [dbo].[MeetingRooms] ([id], [roomno], [capacity], [isOccupied]) VALUES (2, 2, 20, 1)
INSERT [dbo].[MeetingRooms] ([id], [roomno], [capacity], [isOccupied]) VALUES (3, 3, 25, 1)
INSERT [dbo].[MeetingRooms] ([id], [roomno], [capacity], [isOccupied]) VALUES (4, 4, 2, 0)
GO
SET IDENTITY_INSERT [dbo].[MeetingsPlanned] ON 

INSERT [dbo].[MeetingsPlanned] ([Id], [RoomNo], [NoOfParticipants], [MeetingScheduledOn], [MeetingScheduledFor]) VALUES (1, 1, 8, CAST(N'2024-01-01T14:27:51.000' AS DateTime), 1)
INSERT [dbo].[MeetingsPlanned] ([Id], [RoomNo], [NoOfParticipants], [MeetingScheduledOn], [MeetingScheduledFor]) VALUES (2, 2, 10, CAST(N'2024-01-01T14:27:51.000' AS DateTime), 2)
INSERT [dbo].[MeetingsPlanned] ([Id], [RoomNo], [NoOfParticipants], [MeetingScheduledOn], [MeetingScheduledFor]) VALUES (3, 3, 0, CAST(N'2024-01-01T14:27:51.000' AS DateTime), 1)
INSERT [dbo].[MeetingsPlanned] ([Id], [RoomNo], [NoOfParticipants], [MeetingScheduledOn], [MeetingScheduledFor]) VALUES (7, 1, 1, CAST(N'2024-01-03T14:27:51.000' AS DateTime), 1)
INSERT [dbo].[MeetingsPlanned] ([Id], [RoomNo], [NoOfParticipants], [MeetingScheduledOn], [MeetingScheduledFor]) VALUES (8, 1, NULL, CAST(N'2023-12-22T14:01:10.437' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[MeetingsPlanned] OFF
GO
INSERT [dbo].[Users] ([uid], [user_email], [user_password]) VALUES (CAST(1 AS Numeric(18, 0)), N'test@test.com', N'5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8')
GO
/****** Object:  Index [UK_Password]    Script Date: 08-03-2024 23:43:04 ******/
ALTER TABLE [dbo].[MeetingRooms] ADD  CONSTRAINT [UK_Password] UNIQUE NONCLUSTERED 
(
	[roomno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MeetingsPlanned]  WITH CHECK ADD  CONSTRAINT [FK_roomNo] FOREIGN KEY([RoomNo])
REFERENCES [dbo].[MeetingRooms] ([roomno])
GO
ALTER TABLE [dbo].[MeetingsPlanned] CHECK CONSTRAINT [FK_roomNo]
GO
USE [master]
GO
ALTER DATABASE [Enterprise] SET  READ_WRITE 
GO
