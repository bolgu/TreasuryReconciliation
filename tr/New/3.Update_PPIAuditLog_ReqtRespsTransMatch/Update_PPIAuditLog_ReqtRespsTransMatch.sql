use Treasury
go

declare	@Server		varchar(128)
declare	@Target		varchar(128)
declare	@ErrorMsg	varchar(128)

set	@Target = 'CORDEVSQL27'
set @Server = @@servername

if @Server <> @Target begin
	set @ErrorMsg = 'This execution is on the incorrect server'
	goto Error_Exit
end

print 'SQL Server ' + @@servername + ' run by user: ' + suser_sname()
print 'Starting Script: ' +'03_Update_PPIAuditLog_ReqtRespsTransMatch.sql'
print 'Script start time  ' + convert(char(30),getdate())

set xact_abort on
begin transaction


update al
set al.ReqtRespsTransMatch_Id = tm.ReqtRespsTransMatch_Id
from dbo.T_STG_PPIAuditLog al
inner join dbo.T_Match_ReqtRespsTransMatch tm
on al.PPIAuditLogId = tm.PPIAuditLogId
where al.ReqtRespsTransMatch_Id is null


commit transaction
--- rollback
set xact_abort off

goto Complete

Error_Exit:
print 'Error has occured, ' + @ErrorMsg

Complete:
print 'SQL Server:  ' + @@Servername +'  Ending Script: ' + @@Servername + '  ' +  
'03_Update_PPIAuditLog_ReqtRespsTransMatch.sql' + ' End time:  ' + convert(char(30),getdate())

