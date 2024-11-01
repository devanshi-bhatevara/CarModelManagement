USE [master]
GO
/****** Object:  Database [CarModelManagementDB]    Script Date: 27-10-2024 18:32:17 ******/
CREATE DATABASE [CarModelManagementDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CarModelManagementDB', FILENAME = N'D:\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\CarModelManagementDB.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CarModelManagementDB_log', FILENAME = N'D:\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\CarModelManagementDB_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [CarModelManagementDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CarModelManagementDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CarModelManagementDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CarModelManagementDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CarModelManagementDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CarModelManagementDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CarModelManagementDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [CarModelManagementDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CarModelManagementDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CarModelManagementDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CarModelManagementDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CarModelManagementDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CarModelManagementDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CarModelManagementDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CarModelManagementDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CarModelManagementDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CarModelManagementDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CarModelManagementDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CarModelManagementDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CarModelManagementDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CarModelManagementDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CarModelManagementDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CarModelManagementDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CarModelManagementDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CarModelManagementDB] SET RECOVERY FULL 
GO
ALTER DATABASE [CarModelManagementDB] SET  MULTI_USER 
GO
ALTER DATABASE [CarModelManagementDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CarModelManagementDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CarModelManagementDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CarModelManagementDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CarModelManagementDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CarModelManagementDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'CarModelManagementDB', N'ON'
GO
ALTER DATABASE [CarModelManagementDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [CarModelManagementDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [CarModelManagementDB]
GO
/****** Object:  UserDefinedTableType [dbo].[TVP_CarModelImage]    Script Date: 27-10-2024 18:32:18 ******/
CREATE TYPE [dbo].[TVP_CarModelImage] AS TABLE(
	[ImageData] [varbinary](max) NULL,
	[ImageName] [nvarchar](255) NULL,
	[ImageType] [nvarchar](100) NULL
)
GO
/****** Object:  Table [dbo].[CarModel]    Script Date: 27-10-2024 18:32:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CarModel](
	[CarModelId] [int] IDENTITY(1,1) NOT NULL,
	[Brand] [nvarchar](100) NOT NULL,
	[Class] [nvarchar](100) NOT NULL,
	[ModelName] [nvarchar](100) NOT NULL,
	[ModelCode] [nvarchar](10) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Features] [nvarchar](max) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[DateOfManufacturing] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CarModelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CarModelImage]    Script Date: 27-10-2024 18:32:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CarModelImage](
	[CarModelImageId] [int] IDENTITY(1,1) NOT NULL,
	[CarModelId] [int] NOT NULL,
	[ImageData] [varbinary](max) NOT NULL,
	[ImageName] [nvarchar](255) NOT NULL,
	[ImageType] [nvarchar](100) NOT NULL,
	[UploadDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CarModelImageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[CarModelImage] ADD  DEFAULT (getdate()) FOR [UploadDate]
GO
ALTER TABLE [dbo].[CarModelImage]  WITH CHECK ADD FOREIGN KEY([CarModelId])
REFERENCES [dbo].[CarModel] ([CarModelId])
ON DELETE CASCADE
GO
/****** Object:  StoredProcedure [dbo].[AddCarModelWithImages]    Script Date: 27-10-2024 18:32:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddCarModelWithImages]
    @Brand NVARCHAR(100),
    @Class NVARCHAR(100),
    @ModelName NVARCHAR(100),
    @ModelCode NVARCHAR(10),
    @Description NVARCHAR(MAX),
    @Features NVARCHAR(MAX),
    @Price DECIMAL(18, 2),
    @DateOfManufacturing DATETIME,
    @IsActive BIT,
    @SortOrder INT,
    @Images TVP_CarModelImage READONLY  -- Accepts a variable number of images
AS
BEGIN
    DECLARE @CarModelId INT;

    BEGIN TRANSACTION;

    -- Insert into CarModel
    INSERT INTO CarModel (Brand, Class, ModelName, ModelCode, Description, Features, Price, DateOfManufacturing, IsActive, SortOrder)
    VALUES (@Brand, @Class, @ModelName, @ModelCode, @Description, @Features, @Price, @DateOfManufacturing, @IsActive, @SortOrder);

    SET @CarModelId = SCOPE_IDENTITY();

    -- Insert images from TVP
    INSERT INTO CarModelImage (CarModelId, ImageData, ImageName, ImageType, UploadDate)
    SELECT @CarModelId, ImageData, ImageName, ImageType, GETDATE()
    FROM @Images;

    COMMIT TRANSACTION;
END;
GO
USE [master]
GO
ALTER DATABASE [CarModelManagementDB] SET  READ_WRITE 
GO
