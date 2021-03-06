if object_id('ProjectInvoiceDetail') is not null 
begin
	drop table ProjectInvoiceDetail
end
go

if object_id('ProjectInvoice') is not null 
begin
	drop table ProjectInvoice
end
go

if object_id('ProjectData') is not null 
begin
	drop table ProjectData
end
go

if object_id('ProjectDetail') is not null 
begin
	drop table ProjectDetail
end
go

if object_id('Project') is not null 
begin
	drop table Project
end
go

if object_id('Employee') is not null 
begin
	drop table Employee
end
go

create table Employee(
	Id bigint not null,
	Code varchar(100) not null,
	[Name] varchar(100) not null,
	Email varchar(100) null,
	[Rank] varchar(100) null,
	RankCode varchar(100) null,
	constraint PK_Employee primary key (Id),
	constraint UQ_Code unique (Code)
)

create table Project(
	Id bigint not null,
	Code varchar(250) not null,
	EmployeeId bigint null, 
	EngagementPartnerName varchar(100) null,
	EngagementPartnerEmail varchar(100) null,
	EngagementParnerOfficeAddress varchar(255) null,
	EngagementCode bigint null,
	EngagementManager varchar(100) null,
	EngagementManagerEmail varchar(100) null,
	PaceNumber bigint null,
	LocalProjectId bigint null,
	LocalActivityCodeName varchar(10) null,
	StartDate datetime null,
	EndDate datetime null,
	constraint PK_Project primary key (Id),
	constraint FK_Project_Employee_Manager foreign key (EmployeeId) references Employee (Id),
	constraint UQ_Project_Code unique (Code)
)
go

create unique index UQ_Project_Id on Project (LocalProjectId) where LocalProjectId is not null
go

create table ProjectDetail(
	Id bigint not null,
	EmployeeId bigint null,
	ProjectId bigint not null,
	[Rank] varchar(100) not null,
	[Role] varchar(100) not null,
	Utilization float not null,
	Rate float not null,
	StartDate datetime not null,
	EndDate datetime not null,
	constraint PK_ProjectDetail primary key (Id),
	constraint FK_ProjectDetail_Project foreign key (EmployeeId) references Project (Id)
)

create table ProjectData(
	Id bigint not null,
	ProjectId bigint not null,
	EmployeeId bigint null,
	ClientName varchar(255),
	ClientId bigint,
	EngagementName varchar(255),
	EngagementId bigint,
	WeekEndDate datetime,
	TransactionDate datetime,
	ProcessedDate datetime,
	EmployeeName varchar(255),
	EmployeeCode varchar(255),
	[Rank] varchar(255),
	HoursCharged float,
	BillRate float,
	SER float,
	NER float,
	ERP float,
	CostRate float,
	ActivityName varchar(255),
	ActivityCode varchar(255),
	RecievedFlag varchar(255),
	CategoryCode varchar(255),
	ExpenseAmount float,
	IsExpense bit,
	CategoryDescription varchar(255),
	[Description] varchar(2500),
	SubCategoryDescription varchar(255),
	constraint PK_ProjectData primary key (Id),
	constraint FK_ProjectData_Project foreign key (ProjectId) references Project (Id),
	constraint FK_ProjectData_Employee foreign key (EmployeeId) references Employee (Id),
	--constraint UQ_ProjectData unique (IsExpense, EmployeeCode, TransactionDate, ClientName, EngagementName)
)
go

create table ProjectInvoice(
	Id bigint not null,
	ProjectId bigint not null,
	InvoiceCode varchar(255) not null,
	StartDate datetime not null,
	EndDate datetime not null,
	constraint PK_ProjectInvoice primary key (Id),
	constraint FK_ProjectInvoice_Project foreign key (ProjectId) references Project (Id)
)
go

create table ProjectInvoiceDetail(
	Id bigint not null,
	ProjectInvoiceId bigint not null,
	ProjectDataId bigint not null,
	constraint FK_ProjectInvoiceDetail_Project foreign key (ProjectInvoiceId) references Project (Id),
	constraint FK_ProjectInvoiceDetail_ProjectData foreign key (ProjectDataId) references ProjectData (Id)
)