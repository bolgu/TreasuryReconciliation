select * from dbo.T_PaymentTransactions
select * from dbo.T_CybersourceGatewayTransactions 
select * from dbo.T_CybersourceGatewayLog 
select * from dbo.T_GlobalCashTransactions 


select * from dbo.T_PaymentTransactions
where TransactionId not in 
(
select tr.TransactionId 
from  dbo.T_PaymentTransactions tr
inner join dbo.T_CybersourceGatewayLog  tl
on tr.AuthorizationRequestId = tl.AuthorizationRequestId 
inner join dbo.T_CybersourceGatewayTransactions ct
on ct.RequestId = tl.RequestId 
)
and originaltransactionid is null order by TransactionId 


select * from dbo.T_STG_CybersourceGatewayAuditLog

--<?xml version="1.0" encoding="utf-16"?><RequestMessage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><merchantReferenceCode xmlns="urn:schemas-cybersource-com:transaction-data-1.42">7e9651d5-f4f4-4bcc-9de7-9c588b67ed91</merchantReferenceCode><clientLibrary xmlns="urn:schemas-cybersource-com:transaction-data-1.42">.NET C# WSE</clientLibrary><clientLibraryVersion xmlns="urn:schemas-cybersource-com:transaction-data-1.42">2.0</clientLibraryVersion><clientEnvironment xmlns="urn:schemas-cybersource-com:transaction-data-1.42">Win32NT5.2.3790.131072-CLR2.0.50727.1873</clientEnvironment><billTo xmlns="urn:schemas-cybersource-com:transaction-data-1.42"><firstName>MARYGRACE</firstName><lastName>MAYO</lastName><street1>30265 GREENVIEW PKWY</street1><city>WESTLAKE</city><state>OH</state><postalCode>44145</postalCode><country>US</country><email>null@cybersource.com</email></billTo><item xmlns="urn:schemas-cybersource-com:transaction-data-1.42"><unitPrice>286.13</unitPrice></item><purchaseTotals xmlns="urn:schemas-cybersource-com:transaction-data-1.42"><currency>USD</currency></purchaseTotals><card xmlns="urn:schemas-cybersource-com:transaction-data-1.42" /><decisionManager xmlns="urn:schemas-cybersource-com:transaction-data-1.42"><enabled>false</enabled></decisionManager><merchantDefinedData xmlns="urn:schemas-cybersource-com:transaction-data-1.42"><field2>INTEGRATEDPORTAL</field2><field3>1310</field3><field4>110027125670</field4></merchantDefinedData><ccAuthService run="true" xmlns="urn:schemas-cybersource-com:transaction-data-1.42"><commerceIndicator>internet</commerceIndicator></ccAuthService><ccCaptureService run="true" xmlns="urn:schemas-cybersource-com:transaction-data-1.42" /></RequestMessage>
--<?xml version="1.0" encoding="utf-16"?><ReplyMessage xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><merchantReferenceCode xmlns="urn:schemas-cybersource-com:transaction-data-1.42">7e9651d5-f4f4-4bcc-9de7-9c588b67ed91</merchantReferenceCode><requestID xmlns="urn:schemas-cybersource-com:transaction-data-1.42">2885841186710008284280</requestID><decision xmlns="urn:schemas-cybersource-com:transaction-data-1.42">ACCEPT</decision><reasonCode xmlns="urn:schemas-cybersource-com:transaction-data-1.42">100</reasonCode><requestToken xmlns="urn:schemas-cybersource-com:transaction-data-1.42">Ahj//wSROOIu6vETjYjwVCGjNm1ZNHLFPmLGeVYAFPmLGeVYNIH78UEwhk0kyro9JVdkMI8iccRd1eInGxHgFHZr</requestToken><purchaseTotals xmlns="urn:schemas-cybersource-com:transaction-data-1.42"><currency>USD</currency></purchaseTotals><ccAuthReply xmlns="urn:schemas-cybersource-com:transaction-data-1.42"><reasonCode>100</reasonCode><amount>286.13</amount><authorizationCode>514406</authorizationCode><avsCode>N</avsCode><avsCodeRaw>N</avsCodeRaw><cvCode>M</cvCode><cvCodeRaw>M</cvCodeRaw><authorizedDateTime>2010-11-01T04:01:59Z</authorizedDateTime><processorResponse>000</processorResponse></ccAuthReply><ccCaptureReply xmlns="urn:schemas-cybersource-com:transaction-data-1.42"><reasonCode>100</reasonCode><requestDateTime>2010-11-01T04:01:59Z</requestDateTime><amount>286.13</amount><reconciliationID>43352491</reconciliationID></ccCaptureReply></ReplyMessage>