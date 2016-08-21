CREATE sequence UsersSeq
  INCREMENT BY 1 START WITH 0 MINVALUE 0 NOCACHE;
  
CREATE sequence GroupsSeq
  INCREMENT BY 1 START WITH 0 MINVALUE 0 NOCACHE;
  
CREATE sequence UsersingroupsSeq
  INCREMENT BY 1 START WITH 0 MINVALUE 0 NOCACHE;

CREATE sequence MailSeq
  INCREMENT BY 1 START WITH 0 MINVALUE 0 NOCACHE;

CREATE sequence EmailAdressSeq
  INCREMENT BY 1 START WITH 0 MINVALUE 0 NOCACHE;
  
CREATE sequence MailRecipientSeq
  INCREMENT BY 1 START WITH 0 MINVALUE 0 NOCACHE;
  
CREATE sequence VerificationSeq  
  INCREMENT BY 1 START WITH 0 MINVALUE 0 NOCACHE;
  
CREATE sequence MailAttachmentSeq
  INCREMENT BY 1 START WITH 0 MINVALUE 0 NOCACHE;
  

CREATE TABLE Users 
(
	id int,
	login varchar2(20),
	pass varchar2(100),
	admin number(1),
	constraint UsersPk PRIMARY KEY (id) enable,
	constraint UsersCheck check (admin in (0,1)) enable
);
  
CREATE TABLE Groups (
	id int,
	groupName varchar2(20),
	addr varchar2(100),
	constraint GroupsPk PRIMARY KEY (id) enable
  );
  
CREATE TABLE UserMembership (
	idUser int,
	idGroup int
  );

CREATE TABLE EmailAdress (
	id int,
	idUser int,
	address varchar2(100),
	verified number(1),
	constraint EmailadressPk PRIMARY KEY (id) enable,
	constraint EmailAdressCheck check (verified in (0,1)) enable
  );
  
CREATE TABLE VerificationKey
(
	id int,
	idEmail int,
	key varchar2(32),
	status varchar(20),
	constraint VerivicationkeyPk PRIMARY KEY (id) enable
);

CREATE TABLE Mail (
	id int,
	idSender int,
	subject varchar2(50),
	body varchar2(4000),
	constraint MailPk PRIMARY KEY (id) enable
  );

CREATE TABLE MailRecipient (
	idMail int,
	idUser int,
	field varchar(3),
	constraint MailRecipientCheck check (verified in ("To","CC","Bcc")) enable
  );

CREATE TABLE MailAttachment (
	id int,
	idMail int,
	attachment bfile,
	constraint MailattachmentPk PRIMARY KEY (id) enable
  );
 
ALTER TABLE UserMembership ADD CONSTRAINT UserMembershipFk0 FOREIGN KEY (idUser) REFERENCES Users(id);
ALTER TABLE UserMembership ADD CONSTRAINT UserMembershipFk1 FOREIGN KEY (idGroup) REFERENCES Groups(id);  
ALTER TABLE EmailAdress ADD CONSTRAINT EmailAdressFk0 FOREIGN KEY (idUser) REFERENCES Users(id);  
ALTER TABLE VerificationKey ADD CONSTRAINT VerificationKeyFk0 FOREIGN KEY (idEmail) REFERENCES EmailAdress(id); 
ALTER TABLE Mail ADD CONSTRAINT MailFk0 FOREIGN KEY (idSender) REFERENCES EmailAdress(id);
ALTER TABLE MailRecipient ADD CONSTRAINT MailRecipientFk0 FOREIGN KEY (idMail) REFERENCES Mail(id);
ALTER TABLE MailRecipient ADD CONSTRAINT MailRecipientFk1 FOREIGN KEY (idUser) REFERENCES EmailAdress(id);
ALTER TABLE MailAttachment ADD CONSTRAINT MailAttachmentFk0 FOREIGN KEY (idMail) REFERENCES Mail(id);

insert into Users values(0,'root','root',1);
