
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
where TIMESHEETID = 47393
--delete from [OSUSR_KWM_TIMESHEETLINE3]
--where ID = 96464

select * from [dbo].[OSUSR_KWM_TIMESHEETLINE3] order by 1 desc

select * from [dbo].[OSUSR_KWM_TIMESHEETITEM3]
where TIMESHEETLINEID = 96469


select * from [OSUSR_KWM_TIMESHEET3] as sheet3
inner join OSUSR_KWM_TIMESHEETLINE3 as line3 on line3.TIMESHEETID = sheet3.ID 
inner join OSUSR_KWM_TIMESHEETITEM3 as item3 on item3.TIMESHEETLINEID = line3.ID
where sheet3.EMPLOYEEID = 11531 --and STARTDATE = '2018-08-31'
order by 1 desc
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
@date datetime
as

declare @count int;
set @count = 0;
while @count < 7
begin
	if(@count<5)
		insert into OSUSR_KWM_TIMESHEETITEM3 values(@timesheetlineid,@date,8.00000000,0.00000000)
	else
		insert into OSUSR_KWM_TIMESHEETITEM3 values(@timesheetlineid,@date,0.00000000,0.00000000)
	set @date = DATEADD(DAY, 1, @date) 
	set @count = @count + 1
end

go
--drop proc insert_OSUSR_KWM_TIMESHEETITEM3


select * from  OSUSR_KWM_TIMESHEET3 where EMPLOYEEID = 11531 and STARTDATE = '2012-05-28'
select ID from OSUSR_KWM_TIMESHEETLINE3 where TIMESHEETID = 5336 and projectid = 2 and tasktypeid = 9
select * from  OSUSR_KWM_TIMESHEETITEM3 where DATE = '2012-05-28'


SELECT DATEADD(DAY, 1, '2017-08-25') AS DateAdd;