/*********************************************************************************
 * Скрипт формирует сырые данные для таблицы фактов [dbo].[tblTransactionFacts]  *
 * Edit by valex                                                                 *
 *********************************************************************************/

 SET NOCOUNT ON;

Declare @nCountRow int = 1000
Declare @nCurrentRow int = 0

Truncate table [tblTransactionFacts]

-- 1 Формируем список клиентов
Create table #tmpCustomers( Id bigint identity(1,1),  CustomerName varchar(255), CustomerInn varchar(50))
Create table #tmpCustomersPreffix(Preffix varchar(50))

Insert into #tmpCustomersPreffix(Preffix)
Values
('ООО'),
('ЗАО'),
('ПАО'),
('ИП'),
('АО')

Declare @nCountPreffix as int
Select @nCountPreffix = Count(*)  from #tmpCustomersPreffix

Print 'Формируем временную таблицу с произвольными именами клиентов'


While(@nCurrentRow <= @nCountRow)
Begin
	-- Случайным числом определим преффикс к названию организации
	Declare @nRandomPreffixNumber as int 
	Select @nRandomPreffixNumber = 1 + floor(5 * RAND(convert(varbinary, newid())))


	Declare @strPreffix as varchar(50) = ''
	Select Top 1 @strPreffix = Preffix
	from
		(
			Select Preffix, Max(RowNumber) as RowNumber from
			(
				Select Top (@nRandomPreffixNumber ) Preffix, ROW_NUMBER() OVER ( Order by (Select 1) ) AS RowNumber  
				from #tmpCustomersPreffix
			) as tt
			Group by Preffix
		) as t1
	

	-- Получить случайное наименование организации
	Declare @strRandomName as varchar(MaX)
	Select @strRandomName = LEFT(REPLACE(CAST(NEWID ()  as varchar(max)),'-',''),6)


	-- Получить случайное ИНН
	Declare @strRandomINN as varchar(10)
	Set @strRandomINN = 
		LEFT(
			LEFT(REPLACE(CAST(RAND()  as varchar(max)),'0.','') ,5)
	       + LEFT(REPLACE(CAST(RAND()  as varchar(max)),'0.','') ,5), 10)

	-- Добавляем в таблицу		
	Insert into #tmpCustomers(CustomerName, CustomerInn)
	Values(@strPreffix + ' ' + @strRandomName, @strRandomINN)
	
	Set @nCurrentRow = @nCurrentRow + 1
End

-- Категория клиента
Create table #tmpBaseIndex(Id bigint identity(1,1), Category varchar(50))
Insert into #tmpBaseIndex(Category)
Values
('FF'),
('AA'),
('AB'),
('AC'),
('FA'),
('BB'),
('BC'),
('TC'),
('*')


Print 'Формируем таблицу с исходными транзакциями'
Set @nCurrentRow = 0
Set @nCountRow = 10000000

-- 2 Формируем транзакции
While(@nCurrentRow <= @nCountRow)
Begin

	-- Уникальный номер транзакции
	Declare @strTransactionNumber  as varchar(max) 
	Set @strTransactionNumber =  CAST(NEWID ()  as varchar(max))

	-- Уникальный номер счета
	Declare @strAccountNumber as varchar(20) 
	Set @strAccountNumber = 
		LEFT(
			 LEFT(REPLACE(CAST(RAND()  as varchar(max)),'0.','') ,5)
	       + LEFT(REPLACE(CAST(RAND()  as varchar(max)),'0.','') ,5)
		   + LEFT(REPLACE(CAST(RAND()  as varchar(max)),'0.','') ,5)
		   + LEFT(REPLACE(CAST(RAND()  as varchar(max)),'0.','') ,5), 20)


	Declare @strCustomerName as varchar(255), @strCustomerInn as varchar(50)
	Declare @nCustomerCount bigint = 0
	Select @nCustomerCount = Count(*) from #tmpCustomers
	Declare @nCustomerRandomNumber as bigint 
	Select @nCustomerRandomNumber = 1 + floor(@nCustomerCount * RAND(convert(varbinary, newid())))
	if exists(Select 1 from #tmpCustomers where Id = @nCustomerRandomNumber)
	Begin
		-- Наименование и ИНН клиента полученное случайным числом
		Select @strCustomerName = CustomerName , @strCustomerInn = CustomerInn
		from #tmpCustomers
		where Id = @nCustomerRandomNumber


		-- Номер контракта
		Declare @strContractNumber as varchar(100)
		Set @strContractNumber =  CAST(NEWID ()  as varchar(max))


		-- Сумма сделки
		Declare @nAmount as numeric(18,2) = 0
		Select @nAmount = 1 + floor(1000 * RAND(convert(varbinary, newid())))

		-- Множитель (1 / -1)
		Declare @nFactor int
		Select  @nFactor = Case when floor(2 * RAND(convert(varbinary, newid()))) = 0 then -1 else 1 end

		-- Период транзации
		Declare @nRandomDays int
		Set @nRandomDays = 1 + floor(500 * RAND(convert(varbinary, newid())))
		Declare @nRandomPeriod dateTime
		Set @nRandomPeriod = DateAdd(DAY, @nRandomDays *@nFactor , '2020-01-01')

		-- Категория
		Declare @strCategory as varchar(50) = ''
		Declare @nRandomCategoryNumber int
		Set @nRandomCategoryNumber = 1 + floor(10 * RAND(convert(varbinary, newid())))
		Select @strCategory = Category from #tmpBaseIndex where Id = @nRandomCategoryNumber


		-- Вставляем транзацию
		Insert into [dbo].[tblTransactionFacts] (Period, TransactionNumber, AccountNumber, CustomerNumber, CustomerInn, ContractNumber, Amount, Category)
		Values(@nRandomPeriod, @strTransactionNumber, @strAccountNumber, @strCustomerName, @strCustomerInn, @strContractNumber, @nAmount, @strCategory)
		
	End


	Set @nCurrentRow = @nCurrentRow  +1
End