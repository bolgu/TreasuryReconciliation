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
print 'Starting Script: ' +'04.Load_PaymentRequestResponse_UnwrapPPI.sql'
print 'Script start time  ' + convert(char(30),getdate())

set xact_abort on
begin transaction

;with xmlnamespaces
(	'http://ncogroup.com/ws' as ns1,
	'http://www.ePayments.com/PPI' as ns2,
	'http://schemas.xmlsoap.org/soap/envelope/' as soap,
	'http://www.ePayments.com/PPIPayment' as ns3
)
insert dbo.T_PaymentRequestResponse
(
	PPIAuditLogId
	,PaymentID
	,TransactionTime
	,RequestStartTime
	,RequestEndTime
	,ResultText
	,SegAmount
	,SegFeeAmount
)
select 
	PPIAuditLogId
	, response.value('(/soap:Envelope/soap:Body/ns1:SubmitPPIPaymentResponse/ns1:SubmitPPIPaymentResult/ns2:PaymentID)[1]', 'varchar(100)') PaymentID
	, response.value('(/soap:Envelope/soap:Body/ns1:SubmitPPIPaymentResponse/ns1:SubmitPPIPaymentResult/ns2:TransactionTime)[1]', 'varchar(50)') TransactionTime
	, RequestStartTime
	, RequestEndTime
	, response.value('(/soap:Envelope/soap:Body/ns1:SubmitPPIPaymentResponse/ns1:SubmitPPIPaymentResult/ns2:ResultText)[1]', 'varchar(4000)') ResultText
	, AdditionalInfo.value('(/ns2:AdditionalInfo/ns2:PaymentSegment/ns3:Amount)[1]', 'money') SegAmount
	, AdditionalInfo.value('(/ns2:AdditionalInfo/ns2:Fees/ns3:Amount)[1]', 'money') FeeAmount

 from (
		select 
			CONVERT(xml, convert(varchar(max), l.response)) response
			,CONVERT(xml, convert(varchar(max), l.request)).query('(/soap:Envelope/soap:Body/ns1:SubmitPPIPayment/payment/ns2:AdditionalInfo)') AdditionalInfo
			,l.PPIAuditLogId
			,l.RequestStartTime
			,l.RequestEndTime
		from dbo.T_STG_PPIAuditLog l
		where l.ReqtRespsTransMatch_Id is null
      )a
     
      ;

commit transaction
--- rollback
set xact_abort off

goto Complete

Error_Exit:
print 'Error has occured, ' + @ErrorMsg

Complete:
print 'SQL Server:  ' + @@Servername +'  Ending Script: ' + @@Servername + '  ' +  
'04.Load_PaymentRequestResponse_UnwrapPPI.sql' + ' End time:  ' + convert(char(30),getdate())

