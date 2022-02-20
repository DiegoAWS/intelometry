USE master
GO
IF NOT EXISTS (SELECT [name]
FROM sys.databases
WHERE [name] = N'intelometry_test_db')
CREATE DATABASE intelometry_test_db
GO

USE intelometry_test_db

CREATE TABLE price_hub_table
(
    PriceHub_id INT IDENTITY (1, 1),
    PriceHubName VARCHAR (255) NOT NULL,
    
    PRIMARY KEY (PriceHub_id)
)
GO

CREATE TABLE electricity_market_table
(
    id INT IDENTITY (1, 1),
    PriceHub_id INT NOT NULL,
    Tradedate DATE NOT NULL,
    Deliverystartdate DATE NOT NULL,
    Deliveryenddate DATE NOT NULL,
    HighpriceMWh VARCHAR (255) NOT NULL,
    LowpriceMWh VARCHAR (255) NOT NULL,
    WtdavgpriceMWh VARCHAR (255) NOT NULL,
    Change VARCHAR (255) NOT NULL,
    DailyvolumeMWh VARCHAR (255) NOT NULL,

    FOREIGN KEY(PriceHub_id) 
     REFERENCES price_hub_table(PriceHub_id)
     ON DELETE CASCADE
     ON UPDATE CASCADE,
    PRIMARY KEY(id),
)
GO



