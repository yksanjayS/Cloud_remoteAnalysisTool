SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE addReport
 (
  @ID int ,
  @EmpID varchar(50),
  @ReportFile varchar(50)
  
  )
AS BEGIN
  BEGIN TRY
    BEGIN TRANSACTION;
   Update AnalysisReportData set EmpID =@EmpID ,ReportFile = @ReportFile ,UploadStatus='True' where FileID = @ID
    COMMIT TRANSACTION;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
  END CATCH
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[addUploadData]
 (
  @UserName varchar(50),
  @InstrumentUsed varchar(50),
  @FileName varchar(100),
  @Description varchar(max),
  @UploadDate Datetime,
  @Machine varchar(max),
  @RPM varchar(max),
  @Coupling1 varchar(max),
  @RPMWINGS1 varchar(max),
  @Coupling2 varchar(max),
  @RPMWINGS2 varchar(max),
  @Coupling3 varchar(max),
  @RPMWINGS3 varchar(max),
  @TotalCost float,
  @PointCount varchar(50),
  @FileType varchar(20),
  @AnalysisType varchar(50)
  )
AS BEGIN
  BEGIN TRY
    BEGIN TRANSACTION;
    DECLARE @id [int];
      INSERT INTO UploadData(UserName,InstrumentUsed,FileName,Description,UploadDate,Machine,RPM,Coupling1,RPMWINGS1,Coupling2,RPMWINGS2,Coupling3,RPMWINGS3,PointCount,AnalysisType,FileType)
      VALUES(@UserName,@InstrumentUsed,@FileName,@Description,@UploadDate,@Machine,@RPM,@Coupling1,@RPMWINGS1,@Coupling2,@RPMWINGS2,@Coupling3,@RPMWINGS3,@PointCount,@AnalysisType,@FileType)
      SELECT @id = SCOPE_IDENTITY();
      INSERT INTO AnalysisReportData(UserName,EmpID,FileID,ReportFile,UploadStatus)
      VALUES(@UserName,'',@id,'','False')
      INSERT INTO PaymentDetail(UserName,FileID,FileName,Amount,Status)
      VALUES(@UserName,@id,@FileName,@TotalCost,'False')
    COMMIT TRANSACTION;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
  END CATCH
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[CheckUser]
( 
@username varchar(50),
@userpass varchar(50),
@statuschk int output
)
as begin
begin tran
declare @uname varchar(50), @upass varchar(50)
select @uname=UserName,@upass=uPassword from UserProfile
if(@username=@uname and @userpass=@upass)
begin
set @statuschk = 1
end
else
begin
set @statuschk = 0
end 
commit tran
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllClient]
AS BEGIN
  BEGIN TRY
    BEGIN TRANSACTION;
      select up.ID ,up.UserName,ud.uMobile_No,ud.uCompanyName,ud.Email_ID,up.FileName,up.UploadDate from UserDetail ud left join  UploadData up on ud.UserName = up.UserName
    COMMIT TRANSACTION;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
  END CATCH
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[getCityByState]
(
@StateID int
)
AS BEGIN
  BEGIN TRY
    BEGIN TRANSACTION;
  select * from Cities where StateID = @StateID 
    COMMIT TRANSACTION;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
  END CATCH
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE GetClientByID
 (
  @ID int
  )
AS BEGIN
  BEGIN TRY
    BEGIN TRANSACTION;
      SELECT * FROM UploadData WHERE ID = @ID
    COMMIT TRANSACTION;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
  END CATCH
END;

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[getCountry]
AS BEGIN
  BEGIN TRY
    BEGIN TRANSACTION;
  select * from Countries
    COMMIT TRANSACTION;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
  END CATCH
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Batch submitted through debugger: SQLQuery5.sql|7|0|C:\Users\rohit_000\AppData\Local\Temp\~vs3E51.sql

CREATE PROCEDURE [dbo].[getFileWithStatus]
(
@UserRole varchar(50),
@UserName varchar(50)
)
AS BEGIN
BEGIN TRY
BEGIN TRANSACTION
      IF @UserRole='Client'
      BEGIN
      select up.ID ,ud.UserRole,up.UserName,ud.uMobile_No,ud.uCompanyName,ud.Email_ID,up.FileName,up.UploadDate, pd.Status ,pd.Amount,rd.UploadStatus from UploadData up left join UserDetail ud on ud.UserName = up.UserName left join PaymentDetail pd on up.ID = pd.FileID left join AnalysisReportData rd on up.ID=rd.FileID where up.UserName =@UserName
      END
      ELSE 
      BEGIN
       select up.ID ,ud.UserRole,up.UserName,ud.uMobile_No,ud.uCompanyName,ud.Email_ID,up.FileName,up.UploadDate, pd.Status ,pd.Amount,rd.UploadStatus from UploadData up left join UserDetail ud on ud.UserName = up.UserName left join PaymentDetail pd on up.ID = pd.FileID left join AnalysisReportData rd on up.ID=rd.FileID
      END

COMMIT TRANSACTION
END TRY
BEGIN CATCH
  IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
END CATCH
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure getMachineList
As Begin
begin tran
begin try
select * from MachineList
end try
begin catch
rollback
end catch
commit
end; 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[getStateByCountry]
(
@CountryID int
)
AS BEGIN
  BEGIN TRY
    BEGIN TRANSACTION;
  select * from States where CountryID =@CountryID
    COMMIT TRANSACTION;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
  END CATCH
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE getUserByRole
 (
  @UserRole varchar(50),
  @UserName varchar(50)
  )
AS BEGIN
      IF @UserRole='Client'
      BEGIN
     Select * from UserDetail where UserName=@UserName
      END
      ELSE 
      BEGIN
      Select * from Emp_Detail where LoginID=@UserName
      END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc getUserRole
( 
@username varchar(50)

)
as begin
begin tran
select UserRole  from Emp_Detail where LoginID = @username  union Select UserRole from UserDetail where UserName =@username 
commit tran
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spAddEmployee]
(
      @Emp_Name VARCHAR(50),
      @Designation VARCHAR(50),
      @Department VARCHAR(50),
      @EmailId VARCHAR(70),
      @LoginID VARCHAR(50),
	  @Password VARCHAR(50),
	  @UserRole varchar(20)
)
AS
BEGIN
 
BEGIN TRANSACTION
      DECLARE @Rows VARCHAR(12),@Department1 VARCHAR(50),@EmpID VARCHAR(50),@id INT
      SELECT @id=AutoId FROM Emp_Detail
IF @id IS NULL
      BEGIN
      SET @id=1
      SELECT @id=AutoId FROM Emp_Detail
      SET @Rows=1
 END
 SET @Rows = (CONVERT(VARCHAR(12), (SELECT COUNT(AutoId) FROM Emp_Detail) + 1))
 SET @Department1=@Department
 
      IF @Department1='IT'
      BEGIN
      SET @EmpID= 'EIT00'+@Rows
      END
      ELSE IF @Department1='HR'
      BEGIN
      SET @EmpID= 'EHR00'+ @Rows
      END
      ELSE IF @Department1='SALES AND MARKETING'
      BEGIN
      SET @EmpID= 'ESM00'+ @Rows
      END
      ELSE IF @Department1='ACCOUNT'
      BEGIN
      SET @EmpID= 'EAC00'+ @Rows
      END
	  ELSE IF @Department1='ADMIN'
      BEGIN
      SET @EmpID= 'EAD00'+ @Rows
	  END
 
SELECT @Department=@Department1 from Emp_Detail
INSERT INTO Emp_Detail(EmpID,Emp_Name,Designation,Department,EmailId,LoginID,Password,UserRole)
             SELECT @EmpID,@Emp_Name,@Designation,@Department,@EmailId,@LoginID,@Password,@UserRole
 COMMIT TRANSACTION
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[spAddUser]
(
@UserName varchar(50),
@Password VARCHAR(50),
@Name VARCHAR(50),
@EmailID VARCHAR(50),
@CompanyName VARCHAR(100),
@ContactNo VARCHAR(50),
@UserRole varchar(20)
)
as begin
insert into UserDetail(UserName,uPassword,uName,Email_ID,uCompanyName,uMobile_No,UserRole)
values(@UserName,@Password,@Name,@EmailID,@CompanyName,@ContactNo,@UserRole)
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[spFindServiceByID]
@serviceid varchar(50)
as begin
select * from Services where ServiceId=@serviceid
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[spFindUserByID]
@userID varchar(50)
,@uPass varchar(50)
as begin
select * from UserProfile where UserName=@userID and uPassword=@uPass
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[sppaymentdetail]
@userid varchar(50),
@serviceid varchar(50),
@amount float,
@status char(20),
@accountdeptconfirm varchar(50),
@adminremork varchar(200)
as begin
insert into PaymentDetail(UserName,ServiceId,Amount,Status,AccountDeptConfirm,AdminRemark)
 values(@userid,@serviceid,@amount,@status,@accountdeptconfirm,@adminremork)
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc sppaymentServiceselect
@serviceid varchar(50)
as begin
select * from ProgressReport where ServiceId=@serviceid
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------------------------------------------------------------
CREATE proc [dbo].[sppaymentUserselect]
@userid varchar(50)
as begin
select * from ProgressReport where UserName=@userid
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc spprogresServiceselect
@serviceid varchar(50)
as begin
select * from ProgressReport where ServiceId=@serviceid
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[spprogressreport]
@serviceid varchar(50),
@userid varchar(50),
@assignempid varchar(50),
@currentstatus char(20),
@remark varchar(200),
@paymentstatus char(20),
@contactperson varchar(50)
as begin
insert into ProgressReport(ServiceId,UserName,AssignEmpId,StartDate,CurrentStatus,Remark,PaymentStatus,ContactPerson)
 values(@serviceid,@userid,@assignempid,getdate(),@currentstatus,@remark,@paymentstatus,@contactperson)
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[spprogresUserselect]
@userid varchar(50)
as begin
select * from ProgressReport where UserName=@userid
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC spservices2
(
      @servicename VARCHAR(50),
      @servicedept VARCHAR(50),
      @servicetype VARCHAR(50),
      @servicecost FLOAT,
      @serviceduration VARCHAR(50),
      @isenabled VARCHAR(50)
)
AS
BEGIN
 
BEGIN TRANSACTION
      DECLARE @Rows VARCHAR(12),@servicetype1 VARCHAR(50),@serviceid1 VARCHAR(50),@id INT
      SELECT @id=id FROM Services
IF @id IS NULL
      BEGIN
      SET @id=1
      SELECT @id=id FROM Services
      SET @Rows=1
 END
 SET @Rows = (CONVERT(VARCHAR(12), (SELECT COUNT(Id) FROM Services) + 1))
 SET @servicetype1=@servicetype
 
      IF @servicetype1='Vibration'
      BEGIN
      SET @serviceid1= 'SVB'+@Rows
      END
      ELSE IF @servicetype1='Thermoghaphy'
      BEGIN
      SET @serviceid1= 'STH'+ @Rows
      END
      ELSE IF @servicetype1='Leak Detection'
      BEGIN
      SET @serviceid1= 'SLD'+ @Rows
      END
 
--SELECT ServiceId=@serviceid1 from Services
INSERT INTO Services(ServiceId,ServiceName,ServiceDepartment,ServiceType,ServiceCost,ServiceDuration,IsEnabled)
                   SELECT @serviceid1,@servicename,@servicedept,@servicetype,@servicecost,@serviceduration,@isenabled
 COMMIT TRANSACTION
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[updateUploadFile]
 (
  @ID int,
  @FileName varchar(50),
  @InstrumentUsed varchar(50),
  @Description varchar(max),
  @UploadDate Datetime,
  @AnalysisCost float,
  @PointCount varchar(50),
  @AnalysisType varchar(50),
  @FileType varchar(10)
  )
AS BEGIN
  BEGIN TRY
    BEGIN TRANSACTION;
     Update UploadData set FileName=@FileName, InstrumentUsed=@InstrumentUsed, Description=@Description, AnalysisType=@AnalysisType,UploadDate= @UploadDate , PointCount= @PointCount ,FileType= @FileType where ID=@ID
     Update PaymentDetail set Amount=@AnalysisCost where FileID = @ID
    COMMIT TRANSACTION;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
  END CATCH
END;
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AnalysisCost](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceType] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CostPerPoint] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CostPerGraphOrImage] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_AnalysisCost] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AnalysisFields](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceId] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[AnalysisName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IsEnabled] [bit] NOT NULL,
 CONSTRAINT [PK_AnalysisFields] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AnalysisReportData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[EmpID] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[FileID] [int] NOT NULL,
	[ReportFile] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[UploadStatus] [bit] NOT NULL,
 CONSTRAINT [PK_AnalysisReportDate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cities](
	[CityID] [int] NOT NULL,
	[CityName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[StateID] [int] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[CountryID] [int] IDENTITY(1,1) NOT NULL,
	[NameID] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CountryName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[CountryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CountryList](
	[CountryID] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_CountryList] PRIMARY KEY CLUSTERED 
(
	[CountryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Emp_Detail](
	[AutoId] [int] IDENTITY(1,1) NOT NULL,
	[EmpID] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Emp_Name] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Designation] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Department] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[EmailId] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[LoginID] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Password] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[UserRole] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_Emp_Detail] PRIMARY KEY CLUSTERED 
(
	[EmpID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MachineDetail](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FileID] [int] NOT NULL,
	[UserName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[MachineName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[RPMDetail] [float] NOT NULL,
 CONSTRAINT [PK_MachineDetail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MachineList](
	[MachineID] [int] IDENTITY(1,1) NOT NULL,
	[MachineName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[RequiredDetail] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_MachineNameInfo] PRIMARY KEY CLUSTERED 
(
	[MachineID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[FileID] [int] NOT NULL,
	[FileName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Amount] [float] NOT NULL,
	[Status] [bit] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProgressReport](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServiceId] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[UserName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[AssignEmpId] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[StartDate] [date] NULL,
	[CurrentStatus] [char](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Remark] [varchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PaymentStatus] [char](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ContactPerson] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Services](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServiceId] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ServiceName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ServiceType] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ServiceCost] [float] NULL,
	[ServiceDuration] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UrgentDuration] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IsEnabled] [bit] NULL,
 CONSTRAINT [PK_Services] PRIMARY KEY CLUSTERED 
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StateList](
	[StateID] [int] IDENTITY(1,1) NOT NULL,
	[StateName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CountryID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[StateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[States](
	[id] [int] NULL,
	[StateName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CountryID] [int] NULL
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UploadData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[InstrumentUsed] [varchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[FileName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Description] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[UploadDate] [datetime] NOT NULL,
	[Machine] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[RPM] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Coupling1] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[RPMWINGS1] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Coupling2] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[RPMWINGS2] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Coupling3] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[RPMWINGS3] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PointCount] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AnalysisType] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[FileType] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_UploadData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserDetail](
	[AutoId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[uPassword] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[uName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Email_ID] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[uCompanyName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[uCompanyAddress] [varchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[uCity] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[uState] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[uCountry] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[uFax_No] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[uMobile_No] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[uCompany_Website] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UserRole] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_UserProfile] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](56) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Membership](
	[UserId] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[ConfirmationToken] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IsConfirmed] [bit] NULL,
	[LastPasswordFailureDate] [datetime] NULL,
	[PasswordFailuresSinceLastSuccess] [int] NOT NULL,
	[Password] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PasswordChangedDate] [datetime] NULL,
	[PasswordSalt] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PasswordVerificationToken] [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PasswordVerificationTokenExpirationDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_OAuthMembership](
	[Provider] [nvarchar](30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ProviderUserId] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[UserId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Provider] ASC,
	[ProviderUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_UsersInRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET IDENTITY_INSERT [dbo].[AnalysisCost] ON 

GO
INSERT [dbo].[AnalysisCost] ([ID], [ServiceType], [CostPerPoint], [CostPerGraphOrImage]) VALUES (1, N'Interpretor Analysis', N'100', N'100')
GO
INSERT [dbo].[AnalysisCost] ([ID], [ServiceType], [CostPerPoint], [CostPerGraphOrImage]) VALUES (2, N'VibAnalyst Analysis', N'200', N'200')
GO
INSERT [dbo].[AnalysisCost] ([ID], [ServiceType], [CostPerPoint], [CostPerGraphOrImage]) VALUES (3, N'Expert Analysis', N'300', N'300')
GO
SET IDENTITY_INSERT [dbo].[AnalysisCost] OFF
GO
SET IDENTITY_INSERT [dbo].[AnalysisReportData] ON 

GO
INSERT [dbo].[AnalysisReportData] ([ID], [UserName], [EmpID], [FileID], [ReportFile], [UploadStatus]) VALUES (1, N'vimalkyadav', N'deepakbhat001', 1, N'CShapFile1.pdf', 1)
GO
INSERT [dbo].[AnalysisReportData] ([ID], [UserName], [EmpID], [FileID], [ReportFile], [UploadStatus]) VALUES (7, N'vimalkyadav', N'deepakbhat001', 7, N'jquery_tutorial.pdf', 1)
GO
INSERT [dbo].[AnalysisReportData] ([ID], [UserName], [EmpID], [FileID], [ReportFile], [UploadStatus]) VALUES (9, N'vimalkyadav', N'', 9, N'', 0)
GO
INSERT [dbo].[AnalysisReportData] ([ID], [UserName], [EmpID], [FileID], [ReportFile], [UploadStatus]) VALUES (10, N'vimalkyadav', N'', 10, N'', 0)
GO
SET IDENTITY_INSERT [dbo].[AnalysisReportData] OFF
GO
SET IDENTITY_INSERT [dbo].[Emp_Detail] ON 

GO
INSERT [dbo].[Emp_Detail] ([AutoId], [EmpID], [Emp_Name], [Designation], [Department], [EmailId], [LoginID], [Password], [UserRole]) VALUES (1, N'EAD001', N'Deepak Bhat', N'Manager', N'Admin', N'deepakbhat@iadeptmarketing.com', N'deepakbhat001', N'deepak001', N'Admin')
GO
SET IDENTITY_INSERT [dbo].[Emp_Detail] OFF
GO
SET IDENTITY_INSERT [dbo].[PaymentDetail] ON 

GO
INSERT [dbo].[PaymentDetail] ([Id], [UserName], [FileID], [FileName], [Amount], [Status]) VALUES (1, N'vimalkyadav', 1, N'6july.sdf', 3000, 1)
GO
INSERT [dbo].[PaymentDetail] ([Id], [UserName], [FileID], [FileName], [Amount], [Status]) VALUES (7, N'vimalkyadav', 7, N'Analysis.docx', 2000, 1)
GO
INSERT [dbo].[PaymentDetail] ([Id], [UserName], [FileID], [FileName], [Amount], [Status]) VALUES (10, N'vimalkyadav', 10, N'5july.sdf', 2000, 0)
GO
INSERT [dbo].[PaymentDetail] ([Id], [UserName], [FileID], [FileName], [Amount], [Status]) VALUES (9, N'vimalkyadav', 9, N'Anuj Joshi salary slip.xlsx', 2000, 0)
GO
SET IDENTITY_INSERT [dbo].[PaymentDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[UploadData] ON 

GO
INSERT [dbo].[UploadData] ([ID], [UserName], [InstrumentUsed], [FileName], [Description], [UploadDate], [Machine], [RPM], [Coupling1], [RPMWINGS1], [Coupling2], [RPMWINGS2], [Coupling3], [RPMWINGS3], [PointCount], [AnalysisType], [FileType]) VALUES (1, N'vimalkyadav', N'IMPAQ-ELITE', N'Route216.sdf', N'Hi, actually i want to know the current position of my machine.And also suggest if any problem you found.', CAST(0x0000A5A2010346C7 AS DateTime), N'Motor', N'5000', N'GearBox', N'2500', N'Pump', N'1500', N'Centrifugal Pump', N'5', N'10', N'Expert Analysis', N'Data')
GO
INSERT [dbo].[UploadData] ([ID], [UserName], [InstrumentUsed], [FileName], [Description], [UploadDate], [Machine], [RPM], [Coupling1], [RPMWINGS1], [Coupling2], [RPMWINGS2], [Coupling3], [RPMWINGS3], [PointCount], [AnalysisType], [FileType]) VALUES (7, N'vimalkyadav', N'C911', N'Analysis.docx', N'bfghgyjyjy', CAST(0x0000A5A600000000 AS DateTime), N'Motor', N'5000', N'GearBox', N'2500', N'', N'', N'', N'', N'10', N'VibAnalyst Analysis', N'Data')
GO
INSERT [dbo].[UploadData] ([ID], [UserName], [InstrumentUsed], [FileName], [Description], [UploadDate], [Machine], [RPM], [Coupling1], [RPMWINGS1], [Coupling2], [RPMWINGS2], [Coupling3], [RPMWINGS3], [PointCount], [AnalysisType], [FileType]) VALUES (9, N'vimalkyadav', N'VIBXPERT', N'bootstrap.css', N'vgh', CAST(0x0000A5A700000000 AS DateTime), N'Motor', N'2000', N'Pump', N'150', N'coupling', N'50000', N'truyt', N'5000', N'10', N'VibAnalyst Analysis', N'Data')
GO
INSERT [dbo].[UploadData] ([ID], [UserName], [InstrumentUsed], [FileName], [Description], [UploadDate], [Machine], [RPM], [Coupling1], [RPMWINGS1], [Coupling2], [RPMWINGS2], [Coupling3], [RPMWINGS3], [PointCount], [AnalysisType], [FileType]) VALUES (10, N'vimalkyadav', N'IMPAQ-ELITE', N'5july.sdf', N'hiiiii', CAST(0x0000A5B000000000 AS DateTime), N'Motor', N'5000', N'GearBox', N'1500', N'Pump', N'1500', N'Centrifugal Pump', N'5', N'10', N'VibAnalyst Analysis', N'Data')
GO
SET IDENTITY_INSERT [dbo].[UploadData] OFF
GO
SET IDENTITY_INSERT [dbo].[UserDetail] ON 

GO
INSERT [dbo].[UserDetail] ([AutoId], [UserName], [uPassword], [uName], [Email_ID], [uCompanyName], [uCompanyAddress], [uCity], [uState], [uCountry], [uFax_No], [uMobile_No], [uCompany_Website], [UserRole]) VALUES (2, N'anuj123', N'anuj123', N'anuj joshi', N'anujjoshi77@gmail.com', N'Iadept Marketing', NULL, NULL, NULL, NULL, NULL, N'9158325685', NULL, N'Client')
GO
INSERT [dbo].[UserDetail] ([AutoId], [UserName], [uPassword], [uName], [Email_ID], [uCompanyName], [uCompanyAddress], [uCity], [uState], [uCountry], [uFax_No], [uMobile_No], [uCompany_Website], [UserRole]) VALUES (3, N'iadept05', N'iadept05', N'Chandra Pal', N'stores@iadept.in', N'iAdept Marketing', NULL, NULL, NULL, NULL, NULL, N'9871125850', NULL, N'Client')
GO
INSERT [dbo].[UserDetail] ([AutoId], [UserName], [uPassword], [uName], [Email_ID], [uCompanyName], [uCompanyAddress], [uCity], [uState], [uCountry], [uFax_No], [uMobile_No], [uCompany_Website], [UserRole]) VALUES (1, N'vimalkyadav', N'Chirag123', N'Vimal Yadav', N'vimalkyadav@gmail.com', N'Iadept Marketing Pvt. Ltd.', NULL, NULL, NULL, NULL, NULL, N'08506058447', NULL, N'Client')
GO
SET IDENTITY_INSERT [dbo].[UserDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[UserProfile] ON 

GO
INSERT [dbo].[UserProfile] ([UserId], [UserName]) VALUES (2, N'anuj123')
GO
INSERT [dbo].[UserProfile] ([UserId], [UserName]) VALUES (3, N'deepakbhat001')
GO
INSERT [dbo].[UserProfile] ([UserId], [UserName]) VALUES (4, N'iadept05')
GO
INSERT [dbo].[UserProfile] ([UserId], [UserName]) VALUES (1, N'vimalkyadav')
GO
SET IDENTITY_INSERT [dbo].[UserProfile] OFF
GO
INSERT [dbo].[webpages_Membership] ([UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess], [Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate]) VALUES (1, CAST(0x0000A5A20051D3BD AS DateTime), NULL, 1, NULL, 0, N'ALMwRRdQcKhk2J77uKbA/MlK5949cDluCu49Z9HZL6P6x+hfCoidRQQL/5EnUAN+FQ==', CAST(0x0000A5A20051D3BD AS DateTime), N'', NULL, NULL)
GO
INSERT [dbo].[webpages_Membership] ([UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess], [Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate]) VALUES (2, CAST(0x0000A5A2005DF61C AS DateTime), NULL, 1, NULL, 0, N'ABS5cnAqqgPLBQX0GGtP7LZYllkfX9xVvOdpMAGY4LbdD0CLEfmRfbseZGOvgX3MCA==', CAST(0x0000A5A2005DF61C AS DateTime), N'', NULL, NULL)
GO
INSERT [dbo].[webpages_Membership] ([UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess], [Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate]) VALUES (3, CAST(0x0000A5A20061E3E6 AS DateTime), NULL, 1, NULL, 0, N'AH3uITbsBZgAsKEv+K4o/ul9b22PyTxvJlmI2os4MJcaC02XKLQZXfS3rx9CU6xCtA==', CAST(0x0000A5A20061E3E6 AS DateTime), N'', NULL, NULL)
GO
INSERT [dbo].[webpages_Membership] ([UserId], [CreateDate], [ConfirmationToken], [IsConfirmed], [LastPasswordFailureDate], [PasswordFailuresSinceLastSuccess], [Password], [PasswordChangedDate], [PasswordSalt], [PasswordVerificationToken], [PasswordVerificationTokenExpirationDate]) VALUES (4, CAST(0x0000A5A400A66203 AS DateTime), NULL, 1, NULL, 0, N'AOzxnC4FDgtKvEOygaTcyUImQKZwqphiU5nRHSVDm3VTPTIcBpuq3WuDxKPyS4kA+A==', CAST(0x0000A5A400A66203 AS DateTime), N'', NULL, NULL)
GO
SET ANSI_PADDING ON

GO
ALTER TABLE [dbo].[Services] ADD  CONSTRAINT [IX_Services] UNIQUE NONCLUSTERED 
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
ALTER TABLE [dbo].[UserProfile] ADD UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
SET ANSI_PADDING ON

GO
ALTER TABLE [dbo].[webpages_Roles] ADD UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
ALTER TABLE [dbo].[webpages_Membership] ADD  DEFAULT ((0)) FOR [IsConfirmed]
GO
ALTER TABLE [dbo].[webpages_Membership] ADD  DEFAULT ((0)) FOR [PasswordFailuresSinceLastSuccess]
GO
ALTER TABLE [dbo].[AnalysisReportData]  WITH NOCHECK ADD  CONSTRAINT [FK_AnalysisReportData_UploadData] FOREIGN KEY([FileID])
REFERENCES [dbo].[UploadData] ([ID])
GO
ALTER TABLE [dbo].[AnalysisReportData] CHECK CONSTRAINT [FK_AnalysisReportData_UploadData]
GO
ALTER TABLE [dbo].[MachineDetail]  WITH NOCHECK ADD  CONSTRAINT [FK_MachineDetail_UploadData] FOREIGN KEY([FileID])
REFERENCES [dbo].[UploadData] ([ID])
GO
ALTER TABLE [dbo].[MachineDetail] CHECK CONSTRAINT [FK_MachineDetail_UploadData]
GO
ALTER TABLE [dbo].[StateList]  WITH NOCHECK ADD FOREIGN KEY([CountryID])
REFERENCES [dbo].[CountryList] ([CountryID])
GO
ALTER TABLE [dbo].[States]  WITH NOCHECK ADD FOREIGN KEY([CountryID])
REFERENCES [dbo].[Countries] ([CountryID])
GO
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH NOCHECK ADD  CONSTRAINT [fk_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[webpages_Roles] ([RoleId])
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [fk_RoleId]
GO
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH NOCHECK ADD  CONSTRAINT [fk_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[UserProfile] ([UserId])
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [fk_UserId]
GO
