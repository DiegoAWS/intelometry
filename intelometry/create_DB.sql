USE master
GO
IF NOT EXISTS (SELECT [name]
FROM sys.databases
WHERE [name] = N'intelometry_test_db')
CREATE DATABASE intelometry_test_db
GO

USE intelometry_test_db

CREATE TABLE electricity_market_data
(
    id INT IDENTITY (1, 1) PRIMARY KEY,
    Pricehub VARCHAR (255) NOT NULL,
    Tradedate VARCHAR (255) NOT NULL,
    Deliverystartdate VARCHAR (255) NOT NULL,
    Deliveryenddate VARCHAR (255) NOT NULL,
    HighpriceMWh VARCHAR (255) NOT NULL,
    LowpriceMWh VARCHAR (255) NOT NULL,
    WtdavgpriceMWh VARCHAR (255) NOT NULL,
    Change VARCHAR (255) NOT NULL,
    DailyvolumeMWh VARCHAR (255) NOT NULL,
)
GO
