create database NASE_DB_W;

use NASE_DB_W;

create table Producer(
	Id int identity primary key not null,
	ProducerName nvarchar(150) not null unique
);

create table Reception(
	Id int identity primary key not null,
	Variety nvarchar(150) not null,
	ReceivedFromField float not null,
	FieldName nvarchar(150) not null,
	CarRegistration nvarchar(50),
	EntryDate DateTime not null,
	IssueDate DateTime not null,
	HeatHoursDtrying float,
	HumidityPercent float,
	Observations nvarchar(500),
	ProducerId int not null,
	GrillId int not null
);

create table Remission(
	Id int identity not null primary key,
	Cultivation nvarchar(150) not null,
	Batch nvarchar(150) not null,
	Quantity float not null,
	Butler nvarchar(150) not null,
	TransportNumber int not null,
	Driver nvarchar(150) not null,
	Elaborate nvarchar(150) not null,
	ReceptionId int not null
);

create table Sampling(
	Id int identity primary key not null,
	DateCapture DateTime not null,
	SampleWeight float not null,
	HumidityPercent float not null,
	WalnutNumber int not null,
	Performance float not null,
	TotalWeightOfEdibleNuts float not null
);

create table Grill(
	Id int identity primary key not null,
	DateCapture DateTime not null,
	ReceptionId int not null,
	Size int not null,
	Sacks int not null,
	Kilos float not null
);

create table Cylinder(
	Id int identity primary key not null,
	CylinderName nvarchar(20) not null,
	Active bit not null default 0
);

create table Humidity(
	Id int identity primary key not null,
	DateCapture DateTime not null,
	HumidityPercent float not null,
	CylinderId int not null,
	ReceptionId int not null
);

create table Cylinder_Reception
(
  receptionId int,
  cylinderId int,
  CONSTRAINT cylinder_reception_pk PRIMARY KEY (receptionId, cylinderId),
  CONSTRAINT FK_reception
      FOREIGN KEY (receptionId) REFERENCES Reception (Id) on delete cascade,
  CONSTRAINT FK_technicalUser 
      FOREIGN KEY (cylinderId) REFERENCES Cylinder (Id) on delete cascade
);

ALTER TABLE Reception ADD CONSTRAINT fk_reception_producer FOREIGN KEY (ProducerId) REFERENCES Producer(Id) on delete cascade;
ALTER TABLE Remission ADD CONSTRAINT fk_remission_reception FOREIGN KEY (ReceptionId) REFERENCES Reception(Id) on delete cascade;
ALTER TABLE Reception ADD CONSTRAINT fk_reception_grill FOREIGN KEY (GrillId) REFERENCES Grill(Id) on delete cascade;
ALTER TABLE Cylinder_Reception ADD CONSTRAINT fk_cylinderReception_cylinder FOREIGN KEY (CylinderId) REFERENCES Cylinder(Id) on delete cascade;
ALTER TABLE Humidity ADD CONSTRAINT fk_humidity_cylinder FOREIGN KEY (CylinderId) REFERENCES Cylinder(Id) on delete cascade;
ALTER TABLE Humidity ADD CONSTRAINT fk_humidity_reception FOREIGN KEY (ReceptionId) REFERENCES Reception(Id) on delete cascade;
ALTER TABLE Sampling ADD CONSTRAINT fk_sampling_grill FOREIGN KEY (Id) REFERENCES Grill(Id);
ALTER TABLE Cylinder_Reception ADD CONSTRAINT fk_cylinderReception_reception FOREIGN KEY (ReceptionId) REFERENCES Reception(Id) on delete cascade;
/*create table Receipts(
);

create table Issues(
	
);*/