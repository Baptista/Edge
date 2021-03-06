
host 192.168.210.100
user outdev
pass outdev

data ini
dat fim
projec
Emp
tasktype


select * from [dbo].[OSUSR_KWM_TIMESHEET3] 
where EMPLOYEEID = 11531
order by 1 desc
--delete from [OSUSR_KWM_TIMESHEET3]
--where ID = 47387

select * from [dbo].[OSUSR_KWM_TIMESHEETLINE3]
where TIMESHEETID = 47399
--delete from [OSUSR_KWM_TIMESHEETLINE3]
--where ID = 96464

select * from [dbo].[OSUSR_KWM_TIMESHEETLINE3] order by 1 desc

select * from [dbo].[OSUSR_KWM_TIMESHEETITEM3]
--where TIMESHEETLINEID = 96474
order by 1 desc

select * from [OSUSR_KWM_TIMESHEET3] as sheet3
inner join OSUSR_KWM_TIMESHEETLINE3 as line3 on line3.TIMESHEETID = sheet3.ID 
inner join OSUSR_KWM_TIMESHEETITEM3 as item3 on item3.TIMESHEETLINEID = line3.ID
where sheet3.EMPLOYEEID = 11531 --and STARTDATE = '2018-08-31'
order by 1 desc
go

SELECT DATEPART(weekday, '2018-08-26')
SELECT DATENAME(weekday, '2018-08-26')

go
create proc insert_OSUSR_KWM_TIMESHEET3
@employeeid int,
@startdate datetime,
@statuid int
as

insert into OSUSR_KWM_TIMESHEET3 values (@employeeid,@startdate,@statuid, '','')

go



create proc insert_OSUSR_KWM_TIMESHEETLINE3
@timesheetid int,
@projectid int,
@tasktypeid int
as



insert into OSUSR_KWM_TIMESHEETLINE3 values (@timesheetid,@projectid,@tasktypeid, 
8.00,8.00,8.00,8.00,8.00,0.00,0.00,
0.00000000,0.00000000,0.00000000,0.00000000,0.00000000,0.00000000,0.00000000
,null)

go
--drop proc insert_OSUSR_KWM_TIMESHEETLINE3

create proc insert_OSUSR_KWM_TIMESHEETITEM3
@timesheetlineid int,
@date datetime,
@numberofdays int
as

declare @count int;
declare @isweek int;
set @count = 0;
while @numberofdays >= 0
begin
	if(DATEPART(weekday, @date) = 1 or DATEPART(weekday, @date) = 7)
		insert into OSUSR_KWM_TIMESHEETITEM3 values(@timesheetlineid,@date,0.00000000,0.00000000)		
	else
		insert into OSUSR_KWM_TIMESHEETITEM3 values(@timesheetlineid,@date,8.00000000,0.00000000)

	set @date = DATEADD(DAY, 1, @date) 
	set @numberofdays = @numberofdays - 1
end

go
--drop proc insert_OSUSR_KWM_TIMESHEETITEM3


select * from  OSUSR_KWM_TIMESHEET3 where EMPLOYEEID = 11531 and STARTDATE = '2012-05-28'
select ID from OSUSR_KWM_TIMESHEETLINE3 where TIMESHEETID = 5336 and projectid = 2 and tasktypeid = 9
select * from  OSUSR_KWM_TIMESHEETITEM3 where DATE = '2012-05-28'


SELECT DATEADD(DAY, 1, '2017-08-25') AS DateAdd;

go

create proc insert_EmployeeHours
@employeeid int,
@startdate datetime,
@statuid int,
@projectid int,
@tasktypeid int,
@numberofdays int
as


begin try

begin tran

declare @timesheetid int;
declare @timesheetlineid int;
set @timesheetid = 0; 
set @timesheetlineid = 0;

insert into OSUSR_KWM_TIMESHEET3 values (@employeeid,@startdate,@statuid, '','');

-------------------

set @timesheetid = (select top 1 ID from  OSUSR_KWM_TIMESHEET3 where EMPLOYEEID = @employeeid and STARTDATE = @startdate order by 1 desc)

insert into OSUSR_KWM_TIMESHEETLINE3 values (@timesheetid,@projectid,@tasktypeid, 
8.00,8.00,8.00,8.00,8.00,0.00,0.00,
0.00000000,0.00000000,0.00000000,0.00000000,0.00000000,0.00000000,0.00000000
,null)

--------------------

set @timesheetlineid = (select top 1 ID from OSUSR_KWM_TIMESHEETLINE3 where TIMESHEETID = @timesheetid and projectid = @projectid and tasktypeid = @tasktypeid order by 1 desc)
declare @count int;
declare @isweek int;
set @count = 0;
while @numberofdays >= 0
begin
	if(DATEPART(weekday, @startdate) = 1 or DATEPART(weekday, @startdate) = 7)
		insert into OSUSR_KWM_TIMESHEETITEM3 values(@timesheetlineid,@startdate,0.00000000,0.00000000)		
	else
		insert into OSUSR_KWM_TIMESHEETITEM3 values(@timesheetlineid,@startdate,8.00000000,0.00000000)

	set @startdate = DATEADD(DAY, 1, @startdate) 
	set @numberofdays = @numberofdays - 1
end

commit
end try
begin catch
	rollback
end catch