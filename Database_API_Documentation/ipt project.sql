use BloodBank2;
go
CREATE View BloodBank2View as 
select 
bd.FullName as DonorName,
br.FullName as RecipientName,
bd.DateOfBirth as DonorDateOfBirth,
br.DateOfBirth as RecipientDateOfBirth,
bd.Gender as DonorGender,
br.Gender as RecipientGender,
bd.BloodType as DonorBloodType,
br.BloodType as RecipientBloodType,
bd.LastDonatedAt as DonorLasteDonatedAt,
br.LastReceivedAt as RecipientLastReceivedAt,
bd.PhoneNumber as  DonorPhoneNumber,
br.PhoneNumber as RecipientPhoneNumber,
bd.EmailAddress as DonorEmailAddress,
br.EmailAddress as RecipientEmailAddress,
bd.Address as DonorAddress,
br.Address as RecipientAddress,
bd.CreatedAt as DonorCreatedAt,
br.CreatedAt as RecipientCreatedAt,
dr.Units as DonorUnits,
rr.Units as RecipientUnits,
dr.DonatedAt as DonorDonatedAt,
rr.ReceivedAt as RecipientReceivedAt,
dr.DonatedById as DonorDonatedById,
rr.ReceivedById as RecipientReceivedById
from dbo.BloodDonors bd 
join dbo.DonationRecords dr on bd.Id=dr.Id 
join dbo.ReceptionRecords rr on rr.ReceivedById=dr.DonatedById 
join dbo.BloodRecipients br on br.Id=rr.Id;
go
select * from BloodBank2View;

use bloodbank1;
go
create View bloodbank1View 
as
select 
db.firstname as Donor_firstname,
tr.fullname as requestor_fullname,
db.lastname as Donor_lastname,
db.dob as Donor_dob,
tr.dob as requestor_dob,
db.gender as Donor_gender,
tr.gender as requestor_gender,
db.blood_type as Donor_blood_type ,
db.last_date_donation as Donor_last_date_donation,
tr.date_recieved as requestor_date_recieved,
db.contact_number as Donor_contact_number,
tr.contact_number as requestor_number ,
db.email_id as Donor_email_id,
db.city as Donor_city,
tr.city as requestor_city,
db.address as Donor_address
from dbo.donors_table db join dbo.donorDonatedBlood_table ddb on db.donor_id=ddb.donor_id join dbo.requestor_table tr on tr.blood_id=ddb.blood_id;
go


select * from bloodbank1View ;
