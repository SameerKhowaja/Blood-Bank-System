-- Database: bloodbank
CREATE DATABASE bloodbank1;

-- Donor Entry table
CREATE TABLE bloodbank1.dbo.donors_table(
	[donor_id] [int] NOT NULL PRIMARY KEY IDENTITY (1, 1),
	[firstname] [nchar](15) NOT NULL,
	[lastname] [nchar](15) NOT NULL,
	[dob] [date] NOT NULL,
	[gender] [nchar](7) NOT NULL,
	[blood_type] [nchar](4) NOT NULL,
	[last_date_donation] [date] NOT NULL,
	[contact_number] [nchar](15) NOT NULL,
	[email_id] [nchar](25) NULL,
	[city] [nchar](20) NOT NULL,
	[address] [nchar](100) NULL,
);

-- Donor Donated Blood Entry table
CREATE TABLE bloodbank1.dbo.donorDonatedBlood_table(
	[blood_id] [int] NOT NULL PRIMARY KEY IDENTITY (1, 1),
	[donor_id] [int] NULL FOREIGN KEY REFERENCES bloodbank1.dbo.donors_table(donor_id) ON DELETE SET NULL,
	[blood_type] [nchar](4) NOT NULL,
	[unit_of_blood] [int] NOT NULL,
	[date_donated] [date] NOT NULL,
	[expiry_date] [date] NOT NULL,
	[CheckDonate] [int] NOT NULL DEFAULT 0,
);

-- Bloodbank Blood Stock table
CREATE TABLE bloodbank1.dbo.bloodbankStock_table(
	[blood_type] [nchar](4) NOT NULL PRIMARY KEY,
	[total_units] [int] NOT NULL,
);

-- Blood Requestor table
CREATE TABLE bloodbank1.dbo.requestor_table(
	[request_id] [int] NOT NULL PRIMARY KEY IDENTITY (1, 1),
	[fullname] [nchar](25) NOT NULL,
	[dob] [date] NOT NULL,
	[gender] [nchar](7) NOT NULL,
	[contact_number] [nchar](15) NOT NULL,
	[city] [nchar](20) NOT NULL,
	[blood_id] [int] NULL FOREIGN KEY REFERENCES bloodbank1.dbo.donorDonatedBlood_table(blood_id) ON DELETE SET NULL,
	[date_recieved] [date] NOT NULL,
);

CREATE TABLE bloodbank1.dbo.requestor_donor_table(
	[donor_id] [int] NULL FOREIGN KEY REFERENCES bloodbank1.dbo.donors_table(donor_id) ON DELETE SET NULL,
	[request_id] [int] NULL FOREIGN KEY REFERENCES bloodbank1.dbo.requestor_table(request_id) ON DELETE SET NULL,
);


INSERT INTO bloodbank1.dbo.bloodbankStock_table VALUES ('O+', 0);
INSERT INTO bloodbank1.dbo.bloodbankStock_table VALUES ('O-', 0);
INSERT INTO bloodbank1.dbo.bloodbankStock_table VALUES ('O+', 0);
INSERT INTO bloodbank1.dbo.bloodbankStock_table VALUES ('A+', 0);
INSERT INTO bloodbank1.dbo.bloodbankStock_table VALUES ('A-', 0);
INSERT INTO bloodbank1.dbo.bloodbankStock_table VALUES ('B+', 0);
INSERT INTO bloodbank1.dbo.bloodbankStock_table VALUES ('B-', 0);
INSERT INTO bloodbank1.dbo.bloodbankStock_table VALUES ('AB+', 0);
INSERT INTO bloodbank1.dbo.bloodbankStock_table VALUES ('AB-', 0);
