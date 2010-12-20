/*
This code will work against a table of comma separated values
and output a table where the data is split into columns.
The assumption here is that you know in advance how may columns
will be in the final table. 

The recursive CTE will work for a dataset of indeterminate levels (up to maxrecursion)
The final result set is dependent upon the coder knowing how many levels of 
data exist in the cte data.

It seems to work well with small data sets, Large data sets get rather slow.
*/


declare @trans table(	
						Rid varchar(40)	,
						filePathName varchar(2000), 
						col1 varchar(2000),
						col2 varchar(2000),
						col3 varchar(2000),
						col4 varchar(2000),
						col5 varchar(2000),
						col6 varchar(2000),
						col7 varchar(2000),
						col8 varchar(2000),
						col9 varchar(2000),
						col10 varchar(2000),
						col11 varchar(2000),
						col12 varchar(2000),
						col13 varchar(2000),
						col14 varchar(2000),
						col15 varchar(2000),
						col16 varchar(2000),
						col17 varchar(2000),
						col18 varchar(2000),
						col19 varchar(2000),
						col20 varchar(2000),
						col21 varchar(2000),
						col22 varchar(2000),
						col23 varchar(2000),
						col24 varchar(2000),
						col25 varchar(2000),
						col26 varchar(2000),
						col27 varchar(2000),
						col28 varchar(2000),
						col29 varchar(2000),
						col30 varchar(2000),
						col31 varchar(2000),
						col32 varchar(2000),
						col33 varchar(2000),
						col34 varchar(2000),
						col35 varchar(2000),
						col36 varchar(2000),
						col37 varchar(2000),
						col38 varchar(2000),
						col39 varchar(2000),
						col40 varchar(2000),
						col41 varchar(2000),
						col42 varchar(2000),
						col43 varchar(2000),
						col44 varchar(2000),
						col45 varchar(2000),
						col46 varchar(2000),
						col47 varchar(2000),
						col48 varchar(2000),
						col49 varchar(2000),
						col50 varchar(2000),
						col51 varchar(2000),
						col52 varchar(2000),
						col53 varchar(2000),
						col54 varchar(2000),
						col55 varchar(2000),
						col56 varchar(2000),
						col57 varchar(2000),
						col58 varchar(2000),
						col59 varchar(2000),
						col60 varchar(2000),
						col61 varchar(2000),
						col62 varchar(2000),
						col63 varchar(2000),
						col64 varchar(2000),
						col65 varchar(2000),
						col66 varchar(2000),
						col67 varchar(2000),
						col68 varchar(2000),
						col69 varchar(2000),
						col70 varchar(2000),
						col71 varchar(2000),
						col72 varchar(2000),
						col73 varchar(2000),
						col74 varchar(2000),
						col75 varchar(2000),
						col76 varchar(2000),
						col77 varchar(2000),
						col78 varchar(2000),
						col79 varchar(2000),
						col80 varchar(2000),
						col81 varchar(2000),
						col82 varchar(2000),
						col83 varchar(2000),
						col84 varchar(2000),
						col85 varchar(2000),
						col86 varchar(2000),
						col87 varchar(2000),
						col88 varchar(2000),
						col89 varchar(2000),
						col90 varchar(2000),
						col91 varchar(2000),
						col92 varchar(2000),
						col93 varchar(2000),
						col94 varchar(2000),
						col95 varchar(2000),
						col96 varchar(2000),
						col97 varchar(2000),
						col98 varchar(2000),
						col99 varchar(2000),
						col100 varchar(2000),
						col101 varchar(2000),
						col102 varchar(2000),
						col103 varchar(2000),
						col104 varchar(2000),
						col105 varchar(2000),
						col106 varchar(2000),
						col107 varchar(2000),
						col108 varchar(2000),
						col109 varchar(2000),
						col110 varchar(2000),
						col111 varchar(2000),
						col112 varchar(2000),
						col113 varchar(2000),
						col114 varchar(2000),
						col115 varchar(2000),
						col116 varchar(2000),
						col117 varchar(2000),
						col118 varchar(2000),
						col119 varchar(2000),
						col120 varchar(2000),
						col121 varchar(2000),
						col122 varchar(2000),
						col123 varchar(2000),
						col124 varchar(2000),
						col125 varchar(2000),
						col126 varchar(2000),
						col127 varchar(2000),
						col128 varchar(2000),
						col129 varchar(2000),
						col130 varchar(2000)
						
						);

;with A (RID, SubRData, CommaLoc, NLevel,FilePathName)
as (
	select	STG_CybersourceTransactions_Id,
			left(Row_descriptor,charindex(',', Row_descriptor, 1) - 1),
			charindex(',', Row_descriptor, 1),
			1,
			c.FileName
	from dbo.T_STG_CybersourceTransactions c
	--where datalength(replace(Row_Descriptor, ',', '**')) - datalength(Row_Descriptor)=127
	
	union all			-- this identifies the recursive member of the CTE
	
	select	t.STG_CybersourceTransactions_Id,
			substring(t.Row_descriptor,a.CommaLoc + 1,(charindex(',', t.Row_descriptor, a.CommaLoc + 1) - (a.CommaLoc + 1))),
			charindex(',', t.Row_descriptor, a.CommaLoc + 1),
			NLevel + 1,
			t.FileName		
	from A a
		inner join dbo.T_STG_CybersourceTransactions t 
		on t.STG_CybersourceTransactions_Id = a.RID
		--and datalength(replace(Row_Descriptor, ',', '**')) - datalength(Row_Descriptor)=127
	where charindex(',', t.Row_descriptor, a.CommaLoc + 1) > a.CommaLoc	
	 
),
B (RID, SubRData, NLevel,FilePathName)
as (
	select	RID,
			SubRData,
			NLevel,
			FilePathName
	from A		
)
insert 
	@trans
select * 
from B
	pivot (
			min(SubRData) for NLevel in (
					[1],[2],[3],[4],[5],[6],[7],[8],[9],[10],
					[11],[12],[13],[14],[15],[16],[17],[18],[19],
					[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],
					[30],[31],[32],[33],[34], [35], [36], [37], [38], [39], 
					[40], [41], [42], [43], [44], [45], [46], [47], [48], [49],	
					[50], [51], [52], [53], [54], [55], [56], [57], [58], [59], 
					[60], [61], [62], [63], [64], [65], [66], [67], [68], [69], 
					[70], [71], [72], [73], [74], [75], [76], [77], [78], [79], 
					[80], [81], [82], [83], [84], [85], [86], [87], [88], [89], 
					[90], [91], [92], [93], [94], [95], [96], [97], [98], [99], 
					[100], [101], [102], [103], [104], [105], [106], [107], [108], [109], 
					[110], [111], [112], [113], [114], [115], [116], [117], [118], [119], 
					[120], [121], [122], [123], [124], [125], [126], [127], [128], [129],[130] )  
)
as p
option (maxrecursion 130)


Insert dbo.T_CybersourceGatewayTransactions 
(
	RequestId
	,MerchantId
	,AuthorizationRequestId
	,AuthCode
	,MerchantReferenceCode
	,AccountSuffix
	,TransactionDate
	,AuthMessage
	,BillMessage
	,Amount
)
select 
	col2	as RequestId
	,col5	as MerchantId
	,col4	as AuthorizationRequestId
	,col98	as AuthCode
	,col4	as MerchantReferenceCode
	,col88  as AccountSuffix
	,dateadd(mm, -cast(substring(col3,CHARINDEX('-',col3,15)+4,2) as int) ,dateadd(hh, -cast(substring(col3,CHARINDEX('-',col3,15)+1,2) as int), cast(substring(replace(col3,'T', ' '),0,20) as datetime))) as TransactionDate
	,col9  as AuthMessage
	,col18  as BillMessage
	,cast(col95 as money) as Amount
	
 from @trans
 where col126 like ('%API%')
 and col2 not in ( 
						select distinct RequestId from  dbo.T_CybersourceGatewayTransactions
 )
 


Insert  dbo.T_CybersourceGatewayTransactions
(
	RequestId
	,MerchantId
	,AuthorizationRequestId
	,AuthCode
	,MerchantReferenceCode
	,AccountSuffix
	,TransactionDate
	,AuthMessage
	,BillMessage
	,Amount
)
select 
	col2	as RequestId
	,col5	as MerchantId
	,col4	as AuthorizationRequestId 
	,col99	as AuthCode
	,col4	as MerchantReferenceCode
	,col89  as AccountSuffix
	,dateadd(mm, -cast(substring(col3,CHARINDEX('-',col3,15)+4,2) as int) ,dateadd(hh, -cast(substring(col3,CHARINDEX('-',col3,15)+1,2) as int), cast(substring(replace(col3,'T', ' '),0,20) as datetime))) as TransactionDate
	,col10  as AuthMessage
	,col16  as BillMessage
	,cast(col96 as money) as Amount
	
 from @trans
 where col127 like ('%API%')
 and col2 not in ( 
						select distinct RequestId from  dbo.T_CybersourceGatewayTransactions
 )




Insert  dbo.T_CybersourceGatewayTransactions
(
	RequestId
	,MerchantId
	,AuthorizationRequestId
	,AuthCode
	,MerchantReferenceCode
	,AccountSuffix
	,TransactionDate
	,AuthMessage
	,BillMessage
	,Amount
)
select 
	col2	as RequestId
	,col5	as MerchantId
	,col4	as AuthorizationRequestId 
	,col100	as AuthCode
	,col4	as MerchantReferenceCode
	,col90  as AccountSuffix
	,dateadd(mm, -cast(substring(col3,CHARINDEX('-',col3,15)+4,2) as int) ,dateadd(hh, -cast(substring(col3,CHARINDEX('-',col3,15)+1,2) as int), cast(substring(replace(col3,'T', ' '),0,20) as datetime))) as TransactionDate
	,col10  as AuthMessage
	,col16  as BillMessage
	,cast(col97 as money) as Amount
	
 from @trans
 where col128 like ('%API%')
and col2 not in ( 
						select distinct RequestId from  dbo.T_CybersourceGatewayTransactions
 )


Insert  dbo.T_CybersourceGatewayTransactions
(
	RequestId
	,MerchantId
	,AuthorizationRequestId
	,AuthCode
	,MerchantReferenceCode
	,AccountSuffix
	,TransactionDate
	,AuthMessage
	,BillMessage
	,Amount
)
select 
	col2	as RequestId
	,col5	as MerchantId
	,col4	as AuthorizationRequestId 
	,col101	as AuthCode
	,col4	as MerchantReferenceCode
	,col91  as AccountSuffix
	,dateadd(mm, -cast(substring(col3,CHARINDEX('-',col3,15)+4,2) as int) ,dateadd(hh, -cast(substring(col3,CHARINDEX('-',col3,15)+1,2) as int), cast(substring(replace(col3,'T', ' '),0,20) as datetime))) as TransactionDate
	,col10  as AuthMessage
	,col12  as BillMessage
	,cast(col98 as money) as Amount
 from @trans
 where col129 like ('%API%')
and col2 not in ( 
						select distinct RequestId from  dbo.T_CybersourceGatewayTransactions
 )


Insert  dbo.T_CybersourceGatewayTransactions
(
	RequestId
	,MerchantId
	,AuthorizationRequestId
	,AuthCode
	,MerchantReferenceCode
	,AccountSuffix
	,TransactionDate
	,AuthMessage
	,BillMessage
	,Amount
)
select 
	col2	as RequestId
	,col5	as MerchantId
	,col4	as AuthorizationRequestId 
	,col102	as AuthCode
	,col4	as MerchantReferenceCode
	,col92  as AccountSuffix
	,dateadd(mm, -cast(substring(col3,CHARINDEX('-',col3,15)+4,2) as int) ,dateadd(hh, -cast(substring(col3,CHARINDEX('-',col3,15)+1,2) as int), cast(substring(replace(col3,'T', ' '),0,20) as datetime))) as TransactionDate
	,col10  as AuthMessage
	,col16  as BillMessage
	,cast(col99 as money) as Amount
 from @trans
 where
   col130 like ('%API%')
and col2 not in ( 
						select distinct RequestId from  dbo.T_CybersourceGatewayTransactions
 )


--select * from T_CybersourceTransactions 
--where (col128 is not null) or (col129 is not null) or (col130 is not null)

