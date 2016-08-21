
SQL*Plus: Release 11.2.0.2.0 Production on Mon Oct 19 19:42:41 2015

Copyright (c) 1982, 2010, Oracle.  All rights reserved.

connect / as sysdba
create user USERNAME identified by USERNAME;
grant create session to USERNAME;
grant connect to USERNAME;
grant create table to USERNAME;
grant unlimited tablespace to USERNAME;
grant create sequence to USERNAME;




