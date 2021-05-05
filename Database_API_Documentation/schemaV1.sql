USE [master]
GO
/****** Object:  Database [bloodbank]    Script Date: 02-May-21 2:38:17 PM ******/
CREATE DATABASE [bloodbank]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'bloodbank', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\bloodbank.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'bloodbank_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\bloodbank_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [bloodbank] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [bloodbank].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [bloodbank] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [bloodbank] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [bloodbank] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [bloodbank] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [bloodbank] SET ARITHABORT OFF 
GO
ALTER DATABASE [bloodbank] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [bloodbank] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [bloodbank] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [bloodbank] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [bloodbank] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [bloodbank] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [bloodbank] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [bloodbank] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [bloodbank] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [bloodbank] SET  DISABLE_BROKER 
GO
ALTER DATABASE [bloodbank] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [bloodbank] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [bloodbank] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [bloodbank] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [bloodbank] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [bloodbank] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [bloodbank] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [bloodbank] SET RECOVERY FULL 
GO
ALTER DATABASE [bloodbank] SET  MULTI_USER 
GO
ALTER DATABASE [bloodbank] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [bloodbank] SET DB_CHAINING OFF 
GO
ALTER DATABASE [bloodbank] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [bloodbank] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [bloodbank] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [bloodbank] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'bloodbank', N'ON'
GO
ALTER DATABASE [bloodbank] SET QUERY_STORE = OFF
GO
USE [bloodbank]
GO
/****** Object:  Table [dbo].[admins_table]    Script Date: 02-May-21 2:38:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[admins_table](
	[fullname] [nchar](25) NOT NULL,
	[email_id] [nchar](25) NOT NULL,
	[password] [nchar](20) NOT NULL,
 CONSTRAINT [PK_admins_table] PRIMARY KEY CLUSTERED 
(
	[email_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[bloods_table]    Script Date: 02-May-21 2:38:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[bloods_table](
	[blood_id] [int] NOT NULL,
	[donor_id] [int] NOT NULL,
	[blood_type] [nchar](5) NOT NULL,
	[unit_of_blood] [int] NOT NULL,
	[date_donated] [date] NOT NULL,
	[expiry_date] [date] NOT NULL,
 CONSTRAINT [PK_bloods_table] PRIMARY KEY CLUSTERED 
(
	[blood_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[donor_request_table]    Script Date: 02-May-21 2:38:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[donor_request_table](
	[donor_id] [int] NOT NULL,
	[request_id] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[donors_table]    Script Date: 02-May-21 2:38:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[donors_table](
	[donor_id] [int] NOT NULL,
	[firstname] [nchar](15) NOT NULL,
	[lastname] [nchar](15) NOT NULL,
	[dob] [date] NOT NULL,
	[gender] [nchar](10) NOT NULL,
	[blood_type] [nchar](5) NOT NULL,
	[unit_of_blood] [int] NOT NULL,
	[last_date_donation] [date] NOT NULL,
	[contact_number] [nchar](15) NOT NULL,
	[email_id] [nchar](25) NULL,
	[city] [nchar](20) NULL,
	[address] [nchar](100) NULL,
 CONSTRAINT [PK_donors_table] PRIMARY KEY CLUSTERED 
(
	[donor_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[requests_table]    Script Date: 02-May-21 2:38:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[requests_table](
	[request_id] [int] NOT NULL,
	[fullname] [nchar](25) NOT NULL,
	[dob] [date] NOT NULL,
	[gender] [nchar](10) NOT NULL,
	[contact_number] [nchar](15) NOT NULL,
	[city] [nchar](20) NULL,
	[date_recieved] [date] NOT NULL,
	[blood_type] [nchar](5) NOT NULL,
	[unit_of_blood] [int] NOT NULL,
 CONSTRAINT [PK_requests_table] PRIMARY KEY CLUSTERED 
(
	[request_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[stocks_table]    Script Date: 02-May-21 2:38:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[stocks_table](
	[blood_type] [nchar](5) NOT NULL,
	[total_unit] [int] NOT NULL,
 CONSTRAINT [PK_stocks_table] PRIMARY KEY CLUSTERED 
(
	[blood_type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [bloodbank] SET  READ_WRITE 
GO
