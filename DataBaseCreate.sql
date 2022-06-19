USE [master]
GO
/****** Object:  Database [PersonnelTrackingDb]    Script Date: 19.06.2022 03:44:12 ******/
CREATE DATABASE [PersonnelTrackingDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'dogantest', FILENAME = N'D:\data\PersonnelTrackingDb.mdf' , SIZE = 38912KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'dogantest_log', FILENAME = N'D:\data\PersonnelTrackingDb_log.ldf' , SIZE = 13632KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [PersonnelTrackingDb] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PersonnelTrackingDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PersonnelTrackingDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PersonnelTrackingDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PersonnelTrackingDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PersonnelTrackingDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PersonnelTrackingDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [PersonnelTrackingDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PersonnelTrackingDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PersonnelTrackingDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PersonnelTrackingDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PersonnelTrackingDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PersonnelTrackingDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PersonnelTrackingDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PersonnelTrackingDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PersonnelTrackingDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PersonnelTrackingDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PersonnelTrackingDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PersonnelTrackingDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PersonnelTrackingDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PersonnelTrackingDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PersonnelTrackingDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PersonnelTrackingDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PersonnelTrackingDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PersonnelTrackingDb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [PersonnelTrackingDb] SET  MULTI_USER 
GO
ALTER DATABASE [PersonnelTrackingDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PersonnelTrackingDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PersonnelTrackingDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PersonnelTrackingDb] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [PersonnelTrackingDb] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'PersonnelTrackingDb', N'ON'
GO
ALTER DATABASE [PersonnelTrackingDb] SET QUERY_STORE = OFF
GO
USE [PersonnelTrackingDb]
GO
/****** Object:  User [finger]    Script Date: 19.06.2022 03:44:12 ******/
CREATE USER [finger] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  DatabaseRole [gd_execprocs]    Script Date: 19.06.2022 03:44:12 ******/
CREATE ROLE [gd_execprocs]
GO
ALTER ROLE [db_owner] ADD MEMBER [finger]
GO
/****** Object:  Schema [dogantestdb]    Script Date: 19.06.2022 03:44:13 ******/
CREATE SCHEMA [dogantestdb]
GO
/****** Object:  Table [dbo].[Ayarlar]    Script Date: 19.06.2022 03:44:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ayarlar](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Value] [nvarchar](50) NULL,
	[Key] [bit] NULL,
	[Saats] [time](7) NULL,
 CONSTRAINT [PK_Ayarlar] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Departman]    Script Date: 19.06.2022 03:44:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departman](
	[DepartmanID] [int] IDENTITY(1,1) NOT NULL,
	[DepartmanName] [nchar](20) NULL,
 CONSTRAINT [PK_Departman] PRIMARY KEY CLUSTERED 
(
	[DepartmanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Hareketler20200310]    Script Date: 19.06.2022 03:44:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hareketler20200310](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[KayitId] [int] NULL,
	[Tarih] [datetime] NULL,
	[Islem] [nchar](10) NULL,
	[operation] [int] NULL,
	[Tipi] [bit] NULL,
	[SehirDisi] [bit] NULL,
	[MusteriBİlgisi] [nvarchar](50) NULL,
	[SourceId] [int] NULL,
	[MusteriTipiDateTime] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Izinler]    Script Date: 19.06.2022 03:44:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Izinler](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[KId] [int] NULL,
	[BaslangicTarihi] [datetime] NULL,
	[BitisTarihi] [datetime] NULL,
	[IzinTuru] [nvarchar](50) NULL,
	[Aciklama] [nvarchar](max) NULL,
 CONSTRAINT [PK_Izınler] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 19.06.2022 03:44:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[IpAdress] [nvarchar](50) NULL,
	[PcName] [nvarchar](50) NULL,
	[MacAdress] [nvarchar](50) NULL,
	[PageName] [nvarchar](50) NULL,
	[Operation] [nvarchar](50) NULL,
	[Date] [datetime] NULL,
	[UserName] [nchar](10) NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Login]    Script Date: 19.06.2022 03:44:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Login](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[KAdi] [nvarchar](50) NULL,
	[Sifre] [nvarchar](50) NULL,
	[uyeTuru] [int] NULL,
 CONSTRAINT [PK__Login__3214EC07FB479D56] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dogantestdb].[Hareketler]    Script Date: 19.06.2022 03:44:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dogantestdb].[Hareketler](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[KayitId] [int] NULL,
	[Tarih] [datetime] NULL,
	[Islem] [nchar](10) NULL,
	[operation] [int] NULL,
	[Tipi] [bit] NULL,
	[SehirDisi] [bit] NULL,
	[MusteriBİlgisi] [nvarchar](50) NULL,
	[SourceId] [int] NULL,
	[MusteriTipiDateTime] [datetime] NULL,
 CONSTRAINT [PK_Personel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dogantestdb].[HareketleryDK]    Script Date: 19.06.2022 03:44:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dogantestdb].[HareketleryDK](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[KayitId] [int] NULL,
	[Tarih] [datetime] NULL,
	[Islem] [nchar](10) NULL,
	[operation] [int] NULL,
	[Tipi] [bit] NULL,
	[SehirDisi] [bit] NULL,
	[MusteriBİlgisi] [nvarchar](50) NULL,
	[SourceId] [int] NULL,
	[MusteriTipiDateTime] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dogantestdb].[KayitliPersonel]    Script Date: 19.06.2022 03:44:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dogantestdb].[KayitliPersonel](
	[KId] [int] IDENTITY(1,1) NOT NULL,
	[KartID] [nvarchar](50) NULL,
	[PAdSoyad] [nvarchar](50) NULL,
	[DogumTarihi] [date] NULL,
	[EPosta] [nvarchar](50) NULL,
	[CepNum] [nvarchar](50) NULL,
	[Adres] [nvarchar](max) NULL,
	[IseBaslamaTarihi] [date] NULL,
	[FotoYol] [nchar](100) NULL,
	[FingerID] [int] NULL,
	[DepartmanID] [int] NULL,
	[Silindi] [bit] NULL,
	[QRPath] [nvarchar](max) NULL,
	[MobilePass] [nvarchar](50) NULL,
 CONSTRAINT [PK_KayitliPersonel_1] PRIMARY KEY CLUSTERED 
(
	[KId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dogantestdb].[Notlar]    Script Date: 19.06.2022 03:44:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dogantestdb].[Notlar](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[KayitId] [int] NOT NULL,
	[Tarih] [datetime] NULL,
	[Not] [varchar](max) NULL,
 CONSTRAINT [PK_Notlar] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Departman]  WITH CHECK ADD  CONSTRAINT [FK_Departman_Departman] FOREIGN KEY([DepartmanID])
REFERENCES [dbo].[Departman] ([DepartmanID])
GO
ALTER TABLE [dbo].[Departman] CHECK CONSTRAINT [FK_Departman_Departman]
GO
ALTER TABLE [dbo].[Izinler]  WITH CHECK ADD  CONSTRAINT [FK_Izınler_KayitliPersonel] FOREIGN KEY([KId])
REFERENCES [dogantestdb].[KayitliPersonel] ([KId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Izinler] CHECK CONSTRAINT [FK_Izınler_KayitliPersonel]
GO
ALTER TABLE [dogantestdb].[Hareketler]  WITH CHECK ADD  CONSTRAINT [FK_Hareketler_KayitliPersonel] FOREIGN KEY([KayitId])
REFERENCES [dogantestdb].[KayitliPersonel] ([KId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dogantestdb].[Hareketler] CHECK CONSTRAINT [FK_Hareketler_KayitliPersonel]
GO
ALTER TABLE [dogantestdb].[KayitliPersonel]  WITH CHECK ADD  CONSTRAINT [FK_KayitliPersonel_Departman] FOREIGN KEY([DepartmanID])
REFERENCES [dbo].[Departman] ([DepartmanID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dogantestdb].[KayitliPersonel] CHECK CONSTRAINT [FK_KayitliPersonel_Departman]
GO
ALTER TABLE [dogantestdb].[Notlar]  WITH CHECK ADD  CONSTRAINT [FK_Notlar_Notlar] FOREIGN KEY([KayitId])
REFERENCES [dogantestdb].[KayitliPersonel] ([KId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dogantestdb].[Notlar] CHECK CONSTRAINT [FK_Notlar_Notlar]
GO
USE [master]
GO
ALTER DATABASE [PersonnelTrackingDb] SET  READ_WRITE 
GO
