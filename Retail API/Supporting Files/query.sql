use [InvitaECMDB]

select * from ezfileupload where status='Export'
select *,[Tin Number] from eZCA_3_9_stage where [RIM Number] ='1000004' itemid=821368
select *,[Tin Number] from eZCA_3_9_stage where [tin number] like '%-%'
update eZCA_3_9_items set [RIM Number] ='1000004',[RIM Type]='PERSONAL' where itemid=2785
select * from eZTemplateField
select * from eZCabinet
select * from eZXmlCreateCondition
select * from eZAPICallHistory where callhistoryid in (22398,22399,22400) order by callhistoryid desc --22400
select top 1 RefNumber from eZAPICallHistory where CallHistoryId!=4 order by CallHistoryId desc
select top 3  * from eZAPICallHistory where itemid in( select itemid from eZCA_3_9_items where ifilename!='')

update eZAPICallHistory set Remarks='Error code: 2_4 - Cabinet Name must be in ''Corporate''',[RIM Number]='', UpdatedOnAPI='22-Nov-2022 05:14:43 PM' where CallHistoryId='22405'


select us.*,c.CabinetName 'Cabinet Name',t.TemplateName 'Template Name',dbo.udf_LoginName(us.CreatedBy) as LoginName,'ezca_'+cast(us.CabinetId as nvarchar)+'_'+cast(us.TemplateId as nvarchar)+'_items' 'Table Name' from eZAPICallHistory us left join ezcabinet c on c.CabinetID=us.CabinetId left join eztemplate t on t.TemplateId=us.TemplateId   where us.isdeleted=0   order by CallHistoryId desc 

select * from ezca_3_9_items where isdeleted=0  and [RIM Number]='1000004' and [RIM Type]='PERSONAL'
Select  isnull([dbo].udf_Templateidbytempname('BANK'),'') as TemplateId
update eZXmlCreateCondition set templateid=9
truncate table eZAPICallHistory
insert into eZAPICallHistory (Template,CabinetId ,TemplateId ,Status,Remarks,[RIM Number],ItemId,ParentCallId ,APIFunction,XmlFileName,CreatedOn,UpdatedOn,CreatedBy,UpdatedBy,Isdeleted ) values ('Corporate',0,0,'Process','','',0,0,'Upload','','18-Oct-2022 12:06:59 PM','',0,0,0)

select * from ezca_3_9_items where isdeleted=0  and [RIM Number]='1000004' and [RIM Type]='PERSONAL'

select us.*,c.CabinetName 'Cabinet Name',t.TemplateName 'Template Name',dbo.udf_LoginName(us.CreatedBy) as LoginName,'ezca_'+cast(us.CabinetId as nvarchar)+'_'+cast(us.TemplateId as nvarchar)+'_items' 'Table Name' from eZAPICallHistory us left join ezcabinet c on c.CabinetID=us.CabinetId left join eztemplate t on t.TemplateId=us.TemplateId   where us.isdeleted=0   order by CallHistoryId desc 
select us.Status,us.[Rim Number],'' [Tin Number],c.CabinetName 'Cabinet Name',t.TemplateName 'Template Name',dbo.udf_LoginName(us.CreatedBy) as LoginName,'ezca_'+cast(us.CabinetId as nvarchar)+'_'+cast(us.TemplateId as nvarchar)+'_items' 'Table Name', from eZAPICallHistory us left join ezcabinet c on c.CabinetID=us.CabinetId left join eztemplate t on t.TemplateId=us.TemplateId   where us.isdeleted=0   order by CallHistoryId desc 
1)Status
2)RimNumber
3)TinNumber
4)InitiatedAT
5)CompletedAt
6)FileName
7)NoofPages
8)CallDuration
9)Corporate ---yes/No
10)Retail -- Yes/No

drop table eZAPICallHistory
CREATE TABLE [dbo].[eZAPICallHistory](
	[CallHistoryId] [int] IDENTITY(1,1) NOT NULL,
	[Template] [nvarchar](100) NULL,
	[CabinetId] [int] NULL,
	[TemplateId] [int] NULL,
	[Status] [nvarchar](500) NULL,
	[Remarks] [nvarchar](max) NULL,
	[RIM Number] [nvarchar](500) NULL,
	[XmlFileName] [nvarchar](500) NULL,
	[ItemId] [int] NULL,
	[ParentCallId] [int] NULL,
	[APIFunction] [nvarchar](500) NULL,
	[CreatedOn] [nvarchar](100) NULL,
	[UpdatedOn] [nvarchar](100) NULL,
	[CreatedBy] [int] NOT NULL DEFAULT ((0)),
	[UpdatedBy] [int] NOT NULL DEFAULT ((0)),
	[Isdeleted] [bit] NOT NULL DEFAULT ((0))
	)
	
	select *   from eZAPICallHistory_Bk_live where TemplateId=18 order by CallHistoryId desc
	update eZAPICallHistory set TemplateId=9 where TemplateId=18

	select *,[RIM Number],[TIN Number] from eZCA_3_9_items
	drop table eZAPICallHistory
select * into eZAPICallHistory from eZAPICallHistory_Bk_live
select CabinetId,TemplateId,[dbo].[udf_Cabinet](CabinetId) CabinetName,[dbo].[udf_Template](TemplateId) TemplateName from eztemplate where cabinetId=3 and templateName='RETAIL' and isdeleted=0


select * from ezadministration.dbo.ezcurrentplaninfo where Tenantid=204
[EZOFISCLOUDDB]

select  * from ezwflowtransation_Completed
