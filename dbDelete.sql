drop Sequence UsersSeq;
drop Sequence GroupsSeq;
drop Sequence UserMembershipSeq;
drop Sequence MailSeq;
drop Sequence EmailAdressSeq;
drop Sequence MailRecipientSeq;
drop Sequence MailAttachmentSeq;
drop Sequence VerificationKeySeq;
delete constraint UserMembershipFk0;
delete constraint UserMembershipFk1;
delete constraint EmailAdressFk0;
delete constraint MailFk0;
delete constraint MailRecipientFk0;
delete constraint MailRecipientFk1;
delete constraint MailAttachmentFk0;
delete constraint VerificationKeyFk0;
drop TABLE UserMembership;
drop TABLE MailRecipient;
drop TABLE MailAttachment;
drop TABLE VerificationKey;
drop TABLE Groups;
drop TABLE Mail;
drop TABLE EmailAdress;
drop TABLE Users;




